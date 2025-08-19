from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

class ProjectAssignUsersRequest(BaseModel):
    """
    Pydantic model equivalent to the C# ProjectAssignUsersRequest DTO.
    Represents the response body for ProjectAssignUsersRequest data.
    """
    project_id: int = Field(alias='ProjectId') # Corresponds to public int ProjectId
    user_ids: List[int] = Field(alias='UserIds', Field(default_factory=list)) # Corresponds to public List<int> UserIds
    # Add other properties if they exist in the full C# file

