from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

class StartProjectPhaseRequest(BaseModel):
    """
    Pydantic model equivalent to the C# StartProjectPhaseRequest DTO.
    Represents the response body for StartProjectPhaseRequest data.
    """
    project_phase_id: int = Field(alias='ProjectPhaseId') # Corresponds to public int ProjectPhaseId
    # Add other properties if they exist in the full C# file

