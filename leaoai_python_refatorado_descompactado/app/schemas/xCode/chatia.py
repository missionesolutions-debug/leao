from pydantic import BaseModel
from typing import Optional

class ChatIABase(BaseModel):
    Id: int

class ChatIACreate(ChatIABase):
    pass # Add specific fields for creation if needed

class ChatIA(ChatIABase):
    Id: int

    class Config:
        from_attributes = True
