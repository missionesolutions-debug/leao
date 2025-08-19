from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

class ImagesGalleryResponse(BaseModel):
    """
    Pydantic model equivalent to the C# ImagesGalleryResponse DTO.
    Represents the response body for ImagesGalleryResponse data.
    """
    id: int = Field(alias='Id') # Corresponds to public int Id
    url_source: str = Field(alias='UrlSource') # Corresponds to public string UrlSource
    title: str # Corresponds to public string title
    description: str # Corresponds to public string description
    # Add other properties if they exist in the full C# file

