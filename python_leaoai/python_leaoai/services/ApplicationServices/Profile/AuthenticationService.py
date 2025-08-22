import asyncio
from typing import Optional
from uuid import uuid4
from SecureIdentity import PasswordHasher  # Supondo que você tenha a mesma lógica de hash como antes

# --- DTOs ---
class UsuarioLoginDto:
    def __init__(self, email_address: str, password: str):
        self.email_address = email_address
        self.password = password

class DataResult:
    def __init__(self, token: str = ""):
        self.token = token

class LoginResult:
    def __init__(self, data: DataResult):
        self.data = data

class UserMeDto:
    def __init__(self, id: int = 0, avatar: str = "", first_name: str = "", subscription_id: int = 0,
                 journey_id: int = 0, is_subscription_active: bool = False, has_journey: bool = False,
                 role: str = "", imagem: Optional[str] = None):
        self.id = id
        self.avatar = avatar
        self.first_name = first_name
        self.subscription_id = subscription_id
        self.journey_id = journey_id
        self.is_subscription_active = is_subscription_active
        self.has_journey = has_journey
        self.role = role
        self.imagem = imagem

# --- Repositórios fictícios ---
class UserSubscription:
    def __init__(self, id: int):
        self.id = id

class Usuario:
    def __init__(self, id: int = 0, email: str = "", nome: str = "", password: str = "", role_gate: str = "",
                 journey_id: Optional[int] = None, avatar: Optional[str] = None, imagem: Optional[str] = None):
        self.id = id
        self.email = email
        self.nome = nome
        self.password = password
        self.RoleGate = role_gate
        self.JourneyId = journey_id
        self.Avatar = avatar
        self.Imagem = imagem

class UsuarioFactory:
    def __init__(self):
        self.users = []

    def get_by_email(self, email: str) -> Optional[Usuario]:
        return next((u for u in self.users if u.email == email), None)

class UserSubscriptionRepository:
    def get_active_subscription_by_user_id(self, user_id: int) -> Optional[UserSubscription]:
        # Retorna um exemplo de assinatura ativa
        return UserSubscription(id=1)

# --- Token Service simplificado ---
class TokenService:
    @staticmethod
    async def generate_token_async(user: Usuario, account_service) -> str:
        return str(uuid4())

# --- Authentication Service ---
class AuthenticationService:
    def __init__(self, context, repository: UsuarioFactory, account_service, user_subscription_repository: UserSubscriptionRepository, journey_repository=None):
        self._context = context
        self._repository = repository
        self._account_service = account_service
        self._user_subscription_repository = user_subscription_repository
        self._journey_repository = journey_repository

    async def login(self, usuario_login_dto: UsuarioLoginDto) -> LoginResult:
        usuario = self._repository.get_by_email(usuario_login_dto.email_address)
        if usuario is None:
            raise Exception("Email ou senha inválidos.")

        if usuario.RoleGate != "PowerUser":
            if not PasswordHasher.verify(usuario.password, usuario_login_dto.password):
                raise Exception("Email ou senha inválidos.")
        else:
            if usuario.password != usuario_login_dto.password:
                raise Exception("Email ou senha inválidos.")

        token = await TokenService.generate_token_async(usuario, self._account_service)
        return LoginResult(DataResult(token=token))

    def get_authenticated_user(self, email_address: str) -> UserMeDto:
        usuario = self._repository.get_by_email(email_address)
        if usuario is None:
            raise Exception("Usuário não encontrado.")

        user_subscription = self._user_subscription_repository.get_active_subscription_by_user_id(usuario.id)

        user_me_dto = UserMeDto(
            id=usuario.id,
            avatar=usuario.Avatar or "https://example.com/default-avatar.png",
            first_name=usuario.nome,
            subscription_id=user_subscription.id if user_subscription else 0,
            journey_id=usuario.JourneyId or 0,
            is_subscription_active=user_subscription is not None,
            has_journey=(usuario.JourneyId is not None and usuario.JourneyId > 0),
            role=usuario.RoleGate or "administradorMaster",
            imagem=usuario.Imagem
        )

        if usuario.RoleGate == "PowerUser":
            user_me_dto.role = "administradorMaster"

        return user_me_dto
