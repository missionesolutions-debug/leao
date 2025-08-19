
from pydantic import BaseModel
from typing import Optional

# Translate ForgotPasswordResponse
class ForgotPasswordResponse(BaseModel):
    """
    Pydantic model equivalent to the C# ForgotPasswordResponse DTO.
    """
    email: str

# Translate ForgotPasswordResult
class ForgotPasswordResult(BaseModel):
    """
    Pydantic model equivalent to the C# ForgotPasswordResult DTO.
    """
    message: str

# Translate ValidateTokenPasswordResponse
class ValidateTokenPasswordResponse(BaseModel):
    """
    Pydantic model equivalent to the C# ValidateTokenPasswordResponse DTO.
    """
    token: str

# Translate ValidateTokenPasswordResult
class ValidateTokenPasswordResult(BaseModel):
    """
    Pydantic model equivalent to the C# ValidateTokenPasswordResult DTO.
    """
    email: str

# Translate ChangePasswordResponse (based on the snippet, it seems to have a string property, assuming Message)
class ChangePasswordResponse(BaseModel):
    """
    Pydantic model equivalent to the C# ChangePasswordResponse DTO.
    """
    message: str
