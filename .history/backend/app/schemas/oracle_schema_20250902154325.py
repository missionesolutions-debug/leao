from pydantic import BaseModel
from typing import List, Optional

class OracleRequest(BaseModel):
    question: str

class OracleResponse(BaseModel):
    answer: str

class OracleErrorSchema(BaseModel):
    error: str
    message: str