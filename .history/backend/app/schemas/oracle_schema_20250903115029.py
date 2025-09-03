from pydantic import BaseModel
from typing import List, Optional

class OracleRequest(BaseModel):
    query: str
    parameters: Optional[List[str]] = None

class OracleResponse(BaseModel):
    response: str
    confidence: float

class OracleErrorSchema(BaseModel):
    error: str
    code: int