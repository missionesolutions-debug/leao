
from pydantic import BaseModel

class LoginRequest(BaseModel):
    Email: str
    Password: str

class AuthenticateResponse(BaseModel):
    Token: str
