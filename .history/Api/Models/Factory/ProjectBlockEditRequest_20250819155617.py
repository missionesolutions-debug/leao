# from .IFormFile import IFormFile
# from .ProjectMeasureItemRequest import ProjectMeasureItemRequest
from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

class ProjectBlockEditRequest(BaseModel):
    """
    Pydantic model equivalent to the C# ProjectBlockEditRequest DTO.
    Represents the response body for ProjectBlockEditRequest data.
    """
    code: Optional[str] = Field(alias='Code') # Corresponds to public string Code
    usuario_id: Optional[int] = Field(alias='UsuarioId') # Corresponds to public int UsuarioId
    box_type: Optional[str] = Field(alias='BoxType') # Corresponds to public string BoxType
    card_board_type: Optional[str] = Field(alias='CardBoardType') # Corresponds to public string CardBoardType
    closure_type: Optional[str] = Field(alias='ClosureType') # Corresponds to public string ClosureType
    observation: Optional[str] = Field(alias='Observation') # Corresponds to public string Observation
    observations_enabled: Optional[bool] = Field(alias='ObservationsEnabled') # Corresponds to public bool ObservationsEnabled
    version: Optional[str] = Field(alias='Version') # Corresponds to public string Version
    measure_items: List[ProjectMeasureItemRequest] = Field(default_factory=list, alias='MeasureItems') # Corresponds to public List<ProjectMeasureItemRequest> MeasureItems
    files: List[IFormFile] = Field(default_factory=list, alias='Files') # Corresponds to public List<IFormFile> Files
    # Add other properties if they exist in the full C# file

