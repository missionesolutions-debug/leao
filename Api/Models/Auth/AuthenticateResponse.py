
from pydantic import BaseModel

class AuthenticateResponse(BaseModel):
    """
    Pydantic model equivalent to the C# AuthenticateResponse DTO.
    Represents the response body for authentication.
    """
    token: str
    # Add other properties if they exist in the full C# file
