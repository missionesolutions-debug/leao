from fastapi import APIRouter, Depends
from app.controllers.oracle_controller import OracleController
from app.schemas.oracle_schema import OracleRequest, OracleResponse
from app.utils.auth import get_current_user

router = APIRouter()

oracle_controller = OracleController()

@router.post("/oracle/query", response_model=OracleResponse)
async def query_oracle(request: OracleRequest, current_user: str = Depends(get_current_user)):
    """
    Endpoint to query the Oracle AI for information related to intellectual property.
    """
    response = await oracle_controller.query_oracle(request)
    return response