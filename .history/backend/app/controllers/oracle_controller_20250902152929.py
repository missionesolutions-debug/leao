from fastapi import APIRouter, HTTPException, Depends
from src.schemas.oracle_schema import OracleRequest, OracleResponse
from src.services.oracle_service import OracleService
from src.utils.auth import get_current_user

router = APIRouter()

class OracleController:
    def __init__(self):
        self.oracle_service = OracleService()

    @router.post("/oracle/query", response_model=OracleResponse)
    async def query_oracle(
        request: OracleRequest, 
        current_user: str = Depends(get_current_user)
    ):
        try:
            response = await self.oracle_service.query_openai(request)
            return response
        except Exception as e:
            raise HTTPException(status_code=500, detail=str(e))