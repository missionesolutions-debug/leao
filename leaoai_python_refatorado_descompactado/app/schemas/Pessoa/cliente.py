from pydantic import BaseModel
from typing import Optional

class ClienteBase(BaseModel):
    Id: int

class ClienteCreate(ClienteBase):
    pass # Add specific fields for creation if needed

class Cliente(ClienteBase):
    Id: int

    class Config:
        from_attributes = True
