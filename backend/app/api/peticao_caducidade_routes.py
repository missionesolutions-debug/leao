from fastapi import APIRouter, Depends
from sqlalchemy.orm import Session
from app.schemas.caducidade_request_schema import CaducidadeRequest
from app.services.peticao_service import PeticaoService
from app.config.database import get_db
from app.integrations.openai_client import OpenAIClient

router = APIRouter()
openai_client = OpenAIClient()

@router.post("/peticao/caducidade")
def gerar_peticao_caducidade(
    peticao: CaducidadeRequest,
    db: Session = Depends(get_db)
):
    service = PeticaoService(db, openai_client)
    resposta = service.gerar_caducidade(peticao)
    return {"peticao": resposta}