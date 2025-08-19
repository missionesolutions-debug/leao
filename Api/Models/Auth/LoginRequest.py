
from pydantic import BaseModel

class LoginRequest(BaseModel):
    """
    Pydantic model equivalent to the C# LoginRequest DTO.
    Represents the request body for login.
    """
    email: str
    password: str
    # Add other properties if they exist in the full C# file
