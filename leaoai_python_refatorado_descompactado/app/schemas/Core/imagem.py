from pydantic import BaseModel
from typing import Optional

class ImagemBase(BaseModel):
    Id: int

class ImagemCreate(ImagemBase):
    pass # Add specific fields for creation if needed

class Imagem(ImagemBase):
    Id: int

    class Config:
        from_attributes = True
