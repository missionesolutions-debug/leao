from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

class InsertProjectImageRequest(BaseModel):
    """
    Pydantic model equivalent to the C# InsertProjectImageRequest DTO.
    Represents the response body for InsertProjectImageRequest data.
    """
    project_image_id: int = Field(alias='ProjectImageId') # Corresponds to public int ProjectImageId
    project_id: int = Field(alias='ProjectId') # Corresponds to public int ProjectId
    project_block_id: int = Field(alias='ProjectBlockId') # Corresponds to public int ProjectBlockId
    file: Optional[IFormFile] = Field(alias='File') # Corresponds to public IFormFile File
    description: Optional[str] = Field(alias='Description') # Corresponds to public string Description
    enable_on_report: Optional[bool] = Field(alias='EnableOnReport') # Corresponds to public bool EnableOnReport
    # Add other properties if they exist in the full C# file

