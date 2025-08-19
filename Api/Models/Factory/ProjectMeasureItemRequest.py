from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

class ProjectMeasureItemRequest(BaseModel):
    """
    Pydantic model equivalent to the C# ProjectMeasureItemRequest DTO.
    Represents the response body for ProjectMeasureItemRequest data.
    """
    id: int = Field(alias='Id') # Corresponds to public int Id
    project_block_id: int = Field(alias='ProjectBlockId') # Corresponds to public int ProjectBlockId
    name: str = Field(alias='Name') # Corresponds to public string Name
    width: str = Field(alias='Width') # Corresponds to public string Width
    height: str = Field(alias='Height') # Corresponds to public string Height
    length: str = Field(alias='Length') # Corresponds to public string Length
    weight: str = Field(alias='Weight') # Corresponds to public string Weight
    name_editable: bool = Field(alias='NameEditable') # Corresponds to public bool NameEditable
    # Add other properties if they exist in the full C# file

