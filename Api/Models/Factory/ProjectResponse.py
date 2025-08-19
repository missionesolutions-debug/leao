# from .UserResponse import UserResponse
# from .ProjectItemResponse import ProjectItemResponse
from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

class ProjectResponse(BaseModel):
    """
    Pydantic model equivalent to the C# ProjectResponse DTO.
    Represents the response body for ProjectResponse data.
    """
    id: int = Field(alias='Id') # Corresponds to public int Id
    guid: str = Field(alias='Guid') # Corresponds to public string Guid
    name: str = Field(alias='Name') # Corresponds to public string Name
    created_at: datetime = Field(alias='CreatedAt') # Corresponds to public DateTime CreatedAt
    finished_at: Optional[datetime] = Field(alias='FinishedAt') # Corresponds to public DateTime FinishedAt
    date_previsioned: Optional[datetime] = Field(alias='DatePrevisioned') # Corresponds to public DateTime DatePrevisioned
    observations: str = Field(alias='Observations') # Corresponds to public string Observations
    pdf_report_total: str = Field(alias='PdfReportTotal') # Corresponds to public string PdfReportTotal
    client: ClientResponse = Field(alias='Client') # Corresponds to public ClientResponse Client
    supplier: SupplierResponse = Field(alias='Supplier') # Corresponds to public SupplierResponse Supplier
    project_status: ProjectStatusResponse = Field(alias='ProjectStatus') # Corresponds to public ProjectStatusResponse ProjectStatus
    items: List[ProjectItemResponse] = Field(alias='Items', Field(default_factory=list)) # Corresponds to public List<ProjectItemResponse> Items
    assigned_users: List[UserResponse] = Field(alias='AssignedUsers', Field(default_factory=list)) # Corresponds to public List<UserResponse> AssignedUsers
    # Add other properties if they exist in the full C# file

