from pydantic import BaseModel, Field
from typing import Optional, List
from datetime import datetime

class ClientResponse(BaseModel):
    """
    Pydantic model equivalent to the C# ClientResponse DTO.
    Represents the response body for ClientResponse data.
    """
    id: int = Field(alias='Id') # Corresponds to public int Id
    nome: str = Field(alias='Nome') # Corresponds to public string Nome
    c_n_p_j: str = Field(alias='CNPJ') # Corresponds to public string CNPJ
    # Add other properties if they exist in the full C# file

