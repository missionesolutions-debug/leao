from pydantic import BaseModel
from typing import Optional

class BrandBase(BaseModel):
    name: str
    description: Optional[str] = None

class BrandCreate(BrandBase):
    pass

class BrandUpdate(BrandBase):
    pass

class Brand(BrandBase):
    id: int

    class Config:
        orm_mode = True