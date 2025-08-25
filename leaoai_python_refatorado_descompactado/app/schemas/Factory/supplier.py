from pydantic import BaseModel
from typing import Optional

class SupplierBase(BaseModel):
    Id: int
    Nome: str
    CNPJ: Optional[str] = None
    Ativo: bool
    Excluido: bool

class SupplierCreate(SupplierBase):
    pass # Add specific fields for creation if needed

class Supplier(SupplierBase):
    Id: int

    class Config:
        from_attributes = True
