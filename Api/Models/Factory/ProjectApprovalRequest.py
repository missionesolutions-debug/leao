from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

class ProjectApprovalRequest(BaseModel):
    """
    Pydantic model equivalent to the C# ProjectApprovalRequest DTO.
    Represents the response body for ProjectApprovalRequest data.
    """
    project_id: int = Field(alias='ProjectId') # Corresponds to public int ProjectId
    approved: bool = Field(alias='Approved') # Corresponds to public bool Approved
    # Add other properties if they exist in the full C# file

