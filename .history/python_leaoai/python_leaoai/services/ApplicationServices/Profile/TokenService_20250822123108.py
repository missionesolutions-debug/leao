import jwt
from datetime import datetime, timedelta
from typing import List
from dataclasses import dataclass

# Supondo que você tenha definido a chave secreta em Settings
class Settings:
    SECRET = "SUA_CHAVE_SECRETA_AQUI"

# --- DTOs / Classes ---
@dataclass
class Usuario:
    Id: int
    Email: str
    Nome: str
    RoleGate: str

@dataclass
class Claim:
    type: str
    value: str

# --- Token Service ---
class TokenService:

    @staticmethod
    async def generate_token_async(usuario: Usuario, account_service) -> str:
        """
        Gera token JWT assíncrono incluindo claims adicionais do usuário.
        """
        # Claims básicas
        claims = {
            "nameid": str(usuario.Id),
            "email": usuario.Email,
            "name": usuario.Nome or "Usuário",
            "role": usuario.RoleGate
        }

        # Recupera claims adicionais do serviço de conta
        additional_claims = account_service.get_user_claims(usuario.Id)
        for claim in additional_claims:
            claims[claim.type] = claim.value

        expiration = datetime.utcnow() + timedelta(hours=2)

        token = jwt.encode(
            payload={**claims, "exp": expiration},
            key=Settings.SECRET,
            algorithm="HS256"
        )

        return token

    @staticmethod
    def generate_token(usuario: Usuario) -> str:
        """
        Gera token JWT síncrono com validade de 1 mês.
        """
        expiration = datetime.utcnow() + timedelta(days=30)
        my_issuer = "https://codie.com.br"
        my_audience = "https://codie.com.br"

        claims = {
            "email": usuario.Email,
            "role": usuario.RoleGate,
            "id": str(usuario.Id),
            "iss": my_issuer,
            "aud": my_audience,
            "exp": expiration
        }

        token = jwt.encode(
            payload=claims,
            key=Settings.SECRET,
            algorithm="HS256"
        )
        return token

    @staticmethod
    def get_user_id_from_token(token: str) -> int:
        """
        Lê o token JWT e extrai o ID do usuário.
        """
        if not token:
            raise ValueError("Token inválido.")

        token = token.split(" ").pop()  # Remove 'Bearer' se houver
        decoded = jwt.decode(token, Settings.SECRET, algorithms=["HS256"], options={"verify_exp": True})

        user_id_str = decoded.get("id")
        if not user_id_str or not user_id_str.isdigit():
            raise ValueError("ID do usuário ausente ou inválido no token.")

        return int(user_id_str)
