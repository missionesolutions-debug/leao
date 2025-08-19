from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

class ProjectImageResponse(BaseModel):
    """
    Pydantic model equivalent to the C# ProjectImageResponse DTO.
    Represents the response body for ProjectImageResponse data.
    """
    id: int = Field(alias='Id') # Corresponds to public int Id
    url_source: str = Field(alias='UrlSource') # Corresponds to public string UrlSource
    project_block_id: int = Field(alias='ProjectBlockId') # Corresponds to public int ProjectBlockId
    data_cadastro: datetime = Field(alias='DataCadastro') # Corresponds to public DateTime DataCadastro
    description: str # Corresponds to public string description
    enable_on_report: bool = Field(alias='enableOnReport') # Corresponds to public bool enableOnReport
    # Add other properties if they exist in the full C# file

