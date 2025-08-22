import asyncio
from datetime import datetime
from typing import List, Optional
from uuid import uuid4
import hashlib

# --- Models / DTOs ---
class Usuario:
    def __init__(self, id: int = 0, email: str = "", nome: str = "", password: str = "",
                 cpf: str = "", password_token: str = "", password_token_expiry: Optional[datetime] = None):
        self.id = id
        self.email = email
        self.nome = nome
        self.password = password
        self.cpf = cpf
        self.password_token = password_token
        self.password_token_expiry = password_token_expiry

class UserDto:
    def __init__(self, token: str = ""):
        self.token = token

class CreateAccountResponse:
    def __init__(self, email: str, password: str, cpf: str):
        self.email = email
        self.password = password
        self.cpf = cpf

class CreateAccountResult:
    def __init__(self, data: UserDto, message: str):
        self.data = data
        self.message = message

class UpdateAccountResponse:
    def __init__(self, email: str, nome: Optional[str] = None):
        self.email = email
        self.nome = nome

class UpdateAccountResult:
    def __init__(self, success: bool, message: str):
        self.success = success
        self.message = message

class ChangePasswordResponse:
    def __init__(self, current_password: str, new_password: str):
        self.current_password = current_password
        self.new_password = new_password

class ChangePasswordByForgotResponse:
    def __init__(self, guid: str, new_password: str):
        self.guid = guid
        self.new_password = new_password

class UserAlreadyExistsException(Exception): pass
class UserNotFoundException(Exception): pass

# --- Hashing simples ---
class PasswordHasher:
    @staticmethod
    def hash(password: str) -> str:
        return hashlib.sha256(password.encode()).hexdigest()

    @staticmethod
    def verify(hashed: str, password: str) -> bool:
        return hashed == PasswordHasher.hash(password)

# --- Token service simplificado ---
class TokenService:
    @staticmethod
    async def generate_token_async(user: Usuario, service) -> str:
        return str(uuid4())

# --- Repositories fictícios ---
class UsuarioFactory:
    def __init__(self):
        self.users = []

    async def add_usuario_async(self, user: Usuario):
        user.id = len(self.users) + 1
        self.users.append(user)

    def get_user(self, email: str, cpf: str) -> Optional[Usuario]:
        return next((u for u in self.users if u.email == email or u.cpf == cpf), None)

    async def get_user_async(self, email: str) -> Optional[Usuario]:
        return self.get_user(email, "")

    async def update_obj_async(self, user: Usuario):
        for i, u in enumerate(self.users):
            if u.id == user.id:
                self.users[i] = user
                break

    async def get_by_password_reset_guid_async(self, guid: str) -> Optional[Usuario]:
        return next((u for u in self.users if u.password_token == guid), None)

    def get_by_email(self, email: str) -> Optional[Usuario]:
        return next((u for u in self.users if u.email == email), None)

class UserClaim:
    def __init__(self, usuario_id: int, claim_type: str, claim_value: str):
        self.usuario_id = usuario_id
        self.claim_type = claim_type
        self.claim_value = claim_value

class IUserClaimRepository:
    def __init__(self):
        self.claims: List[UserClaim] = []

    async def get_user_claim_async(self, usuario_id: int, claim_type: str):
        return next((c for c in self.claims if c.usuario_id == usuario_id and c.claim_type == claim_type), None)

    async def add_claim_async(self, claim: UserClaim):
        existing = await self.get_user_claim_async(claim.usuario_id, claim.claim_type)
        if existing:
            existing.claim_value = claim.claim_value
        else:
            self.claims.append(claim)

    async def remove_claim_async(self, claim: UserClaim):
        self.claims = [c for c in self.claims if not (c.usuario_id == claim.usuario_id and c.claim_type == claim.claim_type)]

    async def get_user_claims_async(self, usuario_id: int):
        return [c for c in self.claims if c.usuario_id == usuario_id]

    def get_user_claims(self, usuario_id: int):
        return [c for c in self.claims if c.usuario_id == usuario_id]

# --- Serviço de e-mail fictício ---
class IEmailService:
    async def send_password_reset_email_async(self, email, token, *args):
        print(f"Email sent to {email} with token {token}")

# --- Account Service ---
class AccountService:
    def __init__(self, context, repository: UsuarioFactory, user_claim_repository: IUserClaimRepository, email_service: IEmailService):
        self._context = context
        self._repository = repository
        self._user_claim_repository = user_claim_repository
        self._email_service = email_service

    async def create_account(self, model: CreateAccountResponse):
        usuario = self._repository.get_user(model.email, model.cpf)
        if usuario and usuario.id > 0:
            raise UserAlreadyExistsException("Usuário já existe")
        model.password = PasswordHasher.hash(model.password)
        usuario = Usuario(email=model.email, cpf=model.cpf, password=model.password)
        await self._repository.add_usuario_async(usuario)
        token = await TokenService.generate_token_async(usuario, self)
        return CreateAccountResult(data=UserDto(token=token), message="Usuário criado com sucesso")

    async def update_account(self, model: UpdateAccountResponse):
        usuario = await self._repository.get_user_async(model.email)
        if not usuario:
            raise UserNotFoundException("Usuário não encontrado")
        if model.nome:
            usuario.nome = model.nome
        await self._repository.update_obj_async(usuario)
        return UpdateAccountResult(success=True, message="Conta atualizada com sucesso")

    # --- Password ---
    async def forgot_password_async(self, email: str):
        user = await self._repository.get_user_async(email)
        if not user:
            raise Exception("User with the specified email does not exist")
        user.password_token_expiry = datetime.utcnow()
        user.password_token = str(uuid4())
        await self._email_service.send_password_reset_email_async(user.email, user.password_token, "", user.nome, "")

    async def check_token_return_email_async(self, token: str) -> str:
        user = await self._repository.get_by_password_reset_guid_async(token)
        if not user:
            raise Exception("Invalid or expired token")
        return user.email

    async def check_if_password_equals_then_edit_async(self, model: ChangePasswordResponse, user_email: str):
        user = await self._repository.get_user_async(user_email)
        if not user:
            raise Exception("User not found")
        if not PasswordHasher.verify(user.password, model.current_password):
            raise Exception("Current password is incorrect")
        user.password = PasswordHasher.hash(model.new_password)
        await self._repository.update_obj_async(user)

    async def check_if_password_equals_and_guid_async(self, model: ChangePasswordByForgotResponse):
        user = await self._repository.get_by_password_reset_guid_async(model.guid)
        if not user:
            raise Exception("Invalid or expired password reset link")
        if PasswordHasher.verify(user.password, model.new_password):
            raise Exception("The new password must be different from the current password")
        user.password = PasswordHasher.hash(model.new_password)
        await self._repository.update_obj_async(user)

    # --- Claims ---
    async def add_claim_to_user_async(self, usuario_id: int, claim_type: str, claim_value: str):
        existing_claim = await self._user_claim_repository.get_user_claim_async(usuario_id, claim_type)
        if existing_claim:
            existing_claim.claim_value = claim_value
            await self._user_claim_repository.add_claim_async(existing_claim)
        else:
            user_claim = UserClaim(usuario_id, claim_type, claim_value)
            await self._user_claim_repository.add_claim_async(user_claim)

    async def remove_claim_from_user_async(self, usuario_id: int, claim_type: str):
        user_claim = await self._user_claim_repository.get_user_claim_async(usuario_id, claim_type)
        if user_claim:
            await self._user_claim_repository.remove_claim_async(user_claim)

    async def get_user_claims_async(self, usuario_id: int) -> List[dict]:
        claims = await self._user_claim_repository.get_user_claims_async(usuario_id)
        return [{"type": c.claim_type, "value": c.claim_value} for c in claims]

    def get_user_claims(self, usuario_id: int) -> List[dict]:
        claims = self._user_claim_repository.get_user_claims(usuario_id)
        return [{"type": c.claim_type, "value": c.claim_value} for c in claims]

    def get_user_by_email(self, email: str) -> Optional[Usuario]:
        return self._repository.get_by_email(email)

    def encrypt_password(self, password: str):
        return {"hash": PasswordHasher.hash(password)}
