
from pydantic import BaseModel
from typing import Dict, Optional

class ErrorResult(BaseModel):
    """
    Pydantic model equivalent to the C# ErrorResult DTO.
    Represents a common error result structure.
    """
    status: int
    message: Optional[str] = None
    validation_errors: Optional[Dict[str, str]] = {}
    # If ErrorKeyString is a complex model, replace str with the Pydantic model type
