
from pydantic import BaseModel
from typing import Optional

class UserResponse(BaseModel):
    """
    Pydantic model equivalent to the C# UserResponse DTO.
    Represents the response body for user data.
    """
    id: int
    nome: str
    email: str
    role: str
    avatar: Optional[str] = None
    # Add other properties if they exist in the full C# file
