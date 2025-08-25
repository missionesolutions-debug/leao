from pydantic import BaseModel
from typing import Optional

class UsuarioBase(BaseModel):
    Nome: str
    Email: str
    Password: str
    RoleGate: str
    Avatar: Optional[str] = None

class UsuarioCreate(UsuarioBase):
    pass  # campos específicos para criação, se necessário

class UsuarioSchema(UsuarioBase):
    Id: int

    class Config:
        orm_mode = True  # permite compatibilidade com SQLAlchemy
