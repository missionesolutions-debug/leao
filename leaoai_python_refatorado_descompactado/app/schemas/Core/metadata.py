from pydantic import BaseModel
from typing import Optional

class MetadataBase(BaseModel):
    Id: int

class MetadataCreate(MetadataBase):
    pass # Add specific fields for creation if needed

class Metadata(MetadataBase):
    Id: int

    class Config:
        from_attributes = True
