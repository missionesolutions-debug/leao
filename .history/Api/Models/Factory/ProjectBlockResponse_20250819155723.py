# from .ImagesGalleryResponse import ImagesGalleryResponse
# from .ProjectImageResponse import ProjectImageResponse
# from .ProjectMeasureItemResponse import ProjectMeasureItemResponse
from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

class ProjectBlockResponse(BaseModel):
    """
    Pydantic model equivalent to the C# ProjectBlockResponse DTO.
    Represents the response body for ProjectBlockResponse data.
    """
    id: int = Field(alias='Id') # Corresponds to public int Id
    name: str = Field(alias='Name') # Corresponds to public string Name
    min_images_amount: int = Field(alias='MinImagesAmount') # Corresponds to public int MinImagesAmount
    max_images_amount: int = Field(alias='MaxImagesAmount') # Corresponds to public int MaxImagesAmount
    observations_enabled: bool = Field(alias='ObservationsEnabled') # Corresponds to public bool ObservationsEnabled
    instructions: str = Field(alias='Instructions') # Corresponds to public string Instructions
    position: Optional[int] = Field(alias='Position') # Corresponds to public int Position
    code: str = Field(alias='Code') # Corresponds to public string Code
    box_type_active: bool = Field(alias='BoxTypeActive') # Corresponds to public bool BoxTypeActive
    card_board_type_active: bool = Field(alias='CardBoardTypeActive') # Corresponds to public bool CardBoardTypeActive
    closure_type_active: bool = Field(alias='ClosureTypeActive') # Corresponds to public bool ClosureTypeActive
    version_active: bool = Field(alias='VersionActive') # Corresponds to public bool VersionActive
    version_info: str = Field(alias='VersionInfo') # Corresponds to public string VersionInfo
    codigo_barra_info: str = Field(alias='CodigoBarraInfo') # Corresponds to public string CodigoBarraInfo
    block_for_conclude: bool = Field(alias='BlockForConclude') # Corresponds to public bool BlockForConclude
    action_text: str = Field(alias='ActionText') # Corresponds to public string ActionText
    images_label: str = Field(alias='ImagesLabel') # Corresponds to public string ImagesLabel
    is_completed: bool = Field(alias='IsCompleted') # Corresponds to public bool IsCompleted
    observation: str = Field(alias='Observation') # Corresponds to public string Observation
    version: str = Field(alias='Version') # Corresponds to public string Version
    box_type: str = Field(alias='BoxType') # Corresponds to public string BoxType
    card_board_type: str = Field(alias='CardBoardType') # Corresponds to public string CardBoardType
    closure_type: str = Field(alias='ClosureType') # Corresponds to public string ClosureType
    measure_items: List[ProjectMeasureItemResponse] = Field(default_factory=list, alias='MeasureItems') # Corresponds to public List<ProjectMeasureItemResponse> MeasureItems
    images: List[ProjectImageResponse] = Field(default_factory=list, alias='Images') # Corresponds to public List<ProjectImageResponse> Images
    images_gallery: List[ImagesGalleryResponse] = Field(default_factory=list, alias='ImagesGallery') # Corresponds to public List<ImagesGalleryResponse> ImagesGallery
    # Add other properties if they exist in the full C# file

