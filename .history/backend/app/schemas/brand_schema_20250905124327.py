from pydantic import BaseModel
from typing import Optional

class BrandBase(BaseModel):
    name: str
    description: Optional[str] = None

class BrandCreate(BrandBase):
    name: str
    pass

class BrandUpdate(BrandBase):
    name: str
    description: Optional[str] = None
    pass

class BrandResponse(BaseModel):
    id: int
    name: str
    description: Optional[str] = None
    
class Brand(BrandBase):
    id: int

    class Config:
        from_attributes = True