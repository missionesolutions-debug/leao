from fastapi import APIRouter
from app.services.oracle_service import OracleService
from app.schemas.oracle_schema import OracleRequest, OracleResponse
from app.integrations.openai_client import OpenAIClient

router = APIRouter()

openai_client = OpenAIClient()
oracle_service = OracleService(openai_client)

@router.post("/query", response_model=OracleResponse)
async def query_oracle(request: OracleRequest):
    """
    Endpoint to query the Oracle AI for information related to intellectual property.
    """
    try:
        # Chamada real à OpenAI
        answer = oracle_service.query_openai(request.query)
        return OracleResponse(response=answer, confidence=0.9)
    except Exception as e:
        # Em caso de erro, retorna uma mensagem amigável
        error_message = f"Desculpe, não foi possível processar sua pergunta no momento. Erro: {str(e)}"
        return OracleResponse(response=error_message, confidence=0.1)