from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

class ProjectPhaseApprovalRequest(BaseModel):
    """
    Pydantic model equivalent to the C# ProjectPhaseApprovalRequest DTO.
    Represents the response body for ProjectPhaseApprovalRequest data.
    """
    project_phase_id: int = Field(alias='ProjectPhaseId') # Corresponds to public int ProjectPhaseId
    approved: bool = Field(alias='Approved') # Corresponds to public bool Approved
    # Add other properties if they exist in the full C# file

