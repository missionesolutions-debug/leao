from fastapi import APIRouter, Depends
from app.services.oracle_service import OracleService
from app.schemas.oracle_schema import OracleRequest, OracleResponse
from app.utils.auth import get_current_user
from app.integrations.openai_client import OpenAIClient

router = APIRouter()

openai_client = OpenAIClient()
oracle_service = OracleService(openai_client)

@router.post("/query", response_model=OracleResponse)
async def query_oracle(request: OracleRequest, current_user: str = Depends(get_current_user)):
    """
    Endpoint to query the Oracle AI for information related to intellectual property.
    """
    response = await oracle_controller.query_oracle(request)
    return response