from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

class CompleteProjectBlockRequest(BaseModel):
    """
    Pydantic model equivalent to the C# CompleteProjectBlockRequest DTO.
    Represents the response body for CompleteProjectBlockRequest data.
    """
    code: str = Field(alias='Code') # Corresponds to public string Code
    # Add other properties if they exist in the full C# file

