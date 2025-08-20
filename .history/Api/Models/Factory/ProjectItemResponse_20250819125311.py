# from .ProjectPhaseResponse import ProjectPhaseResponse
from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

class ProjectItemResponse(BaseModel):
    """
    Pydantic model equivalent to the C# ProjectItemResponse DTO.
    Represents the response body for ProjectItemResponse data.
    """
    id: int = Field(alias='Id') # Corresponds to public int Id
    name: str = Field(alias='Name') # Corresponds to public string Name
    phases: List[ProjectPhaseResponse] = Field(alias='Phases', Field(default_factory=list)) # Corresponds to public List<ProjectPhaseResponse> Phases
    # Add other properties if they exist in the full C# file

