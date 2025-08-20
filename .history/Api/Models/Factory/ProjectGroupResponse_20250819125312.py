# from .ProjectBlockResponse import ProjectBlockResponse
from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

class ProjectGroupResponse(BaseModel):
    """
    Pydantic model equivalent to the C# ProjectGroupResponse DTO.
    Represents the response body for ProjectGroupResponse data.
    """
    id: int = Field(alias='Id') # Corresponds to public int Id
    name: str = Field(alias='Name') # Corresponds to public string Name
    blocks: List[ProjectBlockResponse] = Field(alias='Blocks', Field(default_factory=list)) # Corresponds to public List<ProjectBlockResponse> Blocks
    # Add other properties if they exist in the full C# file

