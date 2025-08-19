
from pydantic import BaseModel
from typing import Optional

class UserDto(BaseModel):
    """
    Pydantic model equivalent to the C# UserDto.
    Represents user data, likely for authentication or profile.
    """
    token: str
    # Add other properties if they exist in the full C# file
