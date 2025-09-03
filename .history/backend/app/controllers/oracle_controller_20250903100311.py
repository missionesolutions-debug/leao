from fastapi import APIRouter, HTTPException, Depends
from app.services.oracle_service import OracleService
from app.schemas.oracle_schema import OracleRequest, OracleResponse
from app.utils.auth import get_current_user

router = APIRouter()
oracle_service = OracleService()

@router.post("/oracle/query", response_model=OracleResponse)
def query_oracle(request: OracleRequest, user=Depends(get_current_user)):
    try:
        response = oracle_service.query_openai(request.query)
        return OracleResponse(answer=response)
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))