from pydantic import BaseModel
from typing import Optional

class ContactBase(BaseModel):
    Id: int

class ContactCreate(ContactBase):
    pass # Add specific fields for creation if needed

class Contact(ContactBase):
    Id: int

    class Config:
        from_attributes = True
