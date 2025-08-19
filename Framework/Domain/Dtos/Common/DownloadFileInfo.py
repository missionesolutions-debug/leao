
from pydantic import BaseModel
from typing import Optional

class DownloadFileInfo(BaseModel):
    """
    Pydantic model equivalent to the C# DownloadFileInfo DTO.
    Represents file information for download.
    """
    file_path: str
    content_type: str
    file_name: str
    # Add other properties if they exist in the full C# file
