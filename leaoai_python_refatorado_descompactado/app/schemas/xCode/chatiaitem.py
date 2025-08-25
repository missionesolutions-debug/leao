from pydantic import BaseModel
from typing import Optional

class ChatIAItemBase(BaseModel):
    Id: int
    ChatIAId: int

class ChatIAItemCreate(ChatIAItemBase):
    pass # Add specific fields for creation if needed

class ChatIAItem(ChatIAItemBase):
    Id: int

    class Config:
        from_attributes = True
