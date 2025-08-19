
from pydantic import BaseModel
from typing import Optional

class UpdateAccountResponse(BaseModel):
    """
    Pydantic model equivalent to the C# UpdateAccountResponse DTO.
    Represents the response data after updating an account.
    """
    email: str
    nome: str
    login: str
    password: str
    role_gate: str
    avatar: Optional[str] = None
    imagem: Optional[str] = None
    # Add other properties if they exist in the full C# file
