from fastapi import APIRouter, HTTPException
from app.services.oracle_service import OracleService
from app.schemas.oracle_schema import OracleRequest, OracleResponse
from app.integrations.openai_client import OpenAIClient

router = APIRouter()
openai_client = OpenAIClient()
oracle_service = OracleService(openai_client)

@router.post("/oracle/query", response_model=OracleResponse)
def query_oracle(request: OracleRequest):
    try:
        # Resposta de teste primeiro
        test_response = f"Resposta de teste para: {request.query}"
        return OracleResponse(response=test_response, confidence=0.9)
        
        # Depois implementamos a chamada real à OpenAI
        # response = oracle_service.query_openai(request.query)
        # return OracleResponse(response=response, confidence=0.9)
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))

@router.get("/oracle/test")
def test_oracle():
    return {"message": "Oracle endpoint is working!"}