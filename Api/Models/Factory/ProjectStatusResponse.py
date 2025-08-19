from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

class ProjectStatusResponse(BaseModel):
    """
    Pydantic model equivalent to the C# ProjectStatusResponse DTO.
    Represents the response body for ProjectStatusResponse data.
    """
    id: int = Field(alias='Id') # Corresponds to public int Id
    nome: str = Field(alias='Nome') # Corresponds to public string Nome
    # Add other properties if they exist in the full C# file

