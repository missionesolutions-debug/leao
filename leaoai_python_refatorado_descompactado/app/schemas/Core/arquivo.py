from pydantic import BaseModel
from typing import Optional

class ArquivoBase(BaseModel):
    Id: int

class ArquivoCreate(ArquivoBase):
    pass # Add specific fields for creation if needed

class Arquivo(ArquivoBase):
    Id: int

    class Config:
        from_attributes = True
