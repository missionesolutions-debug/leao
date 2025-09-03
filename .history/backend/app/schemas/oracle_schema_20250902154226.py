from pydantic import BaseModel
from typing import List, Optional

class OracleRequestSchema(BaseModel):
    prompt: str
    max_tokens: Optional[int] = 150
    temperature: Optional[float] = 0.7
    question: str

class OracleResponseSchema(BaseModel):
    id: str
    object: str
    created: int
    model: str
    choices: List[dict]
    answer: str

class OracleErrorSchema(BaseModel):
    error: str
    message: str