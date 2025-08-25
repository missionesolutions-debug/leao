# from .ProjectGroupResponse import ProjectGroupResponse
from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

class ProjectPhaseResponse(BaseModel):
    """
    Pydantic model equivalent to the C# ProjectPhaseResponse DTO.
    Represents the response body for ProjectPhaseResponse data.
    """
    id: int = Field(alias='Id') # Corresponds to public int Id
    name: str = Field(alias='Name') # Corresponds to public string Name
    is_aproved: bool = Field(alias='IsAproved') # Corresponds to public bool IsAproved
    is_taken: bool = Field(alias='IsTaken') # Corresponds to public bool IsTaken
    groups: List[ProjectGroupResponse] = Field(alias='Groups', Field(default_factory=list)) # Corresponds to public List<ProjectGroupResponse> Groups
    assigned_user: UserResponse = Field(alias='AssignedUser') # Corresponds to public UserResponse AssignedUser
    # Add other properties if they exist in the full C# file

