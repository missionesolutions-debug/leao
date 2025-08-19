from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

class ProjectMeasureItemResponse(BaseModel):
    """
    Pydantic model equivalent to the C# ProjectMeasureItemResponse DTO.
    Represents the response body for ProjectMeasureItemResponse data.
    """
    id: int = Field(alias='Id') # Corresponds to public int Id
    name: str = Field(alias='Name') # Corresponds to public string Name
    width: str = Field(alias='Width') # Corresponds to public string Width
    height: str = Field(alias='Height') # Corresponds to public string Height
    length: str = Field(alias='Length') # Corresponds to public string Length
    weight: str = Field(alias='Weight') # Corresponds to public string Weight
    name_editable: bool = Field(alias='NameEditable') # Corresponds to public bool NameEditable
    # Add other properties if they exist in the full C# file

