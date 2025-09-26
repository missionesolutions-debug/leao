from fastapi import APIRouter, Depends, HTTPException, Request
from sqlalchemy.orm import Session
from slowapi import Limiter, _rate_limit_exceeded_handler
from slowapi.util import get_remote_address
from app.schemas.caducidade_request_schema import CaducidadeRequest, CaducidadeRepostRequest
from app.schemas.oposicao_request_schema import OposicaoRequest, OposicaoRepostRequest
from app.schemas.nulidade_request_schema import NulidadeRequest, NulidadeRepostRequest
from app.schemas.manifestacao_oposicao_request_schema import ManifestacaoOposicaoRequest, ManifestacaoOposicaoRepostRequest
from app.schemas.manifestacao_recurso_request_schema import ManifestacaoRecursoRequest, ManifestacaoRecursoRepostRequest
from app.schemas.recurso_indeferimento_request_schema import RecursoIndeferimentoRequest, RecursoIndeferimentoRepostRequest
from app.schemas.peticao_caducidade_request_schema import PeticaoCaducidadeRequest, PeticaoCaducidadeRepostRequest
from app.schemas.contrarazao_nulidade_request_schema import ContrarazaoNulidadeRequest, ContrarazaoNulidadeRepostRequest
from app.services.peticao_service import PeticaoService
from app.repositories.peticao_repository import PeticaoRepository
from app.config.database import get_db
from app.integrations.openai_client import OpenAIClient

router = APIRouter()
openai_client = OpenAIClient()

# Rate Limiter para petições
limiter = Limiter(key_func=get_remote_address)

@router.get("/caducidade/mock")
def get_caducidade_mock():
    # Retorna um exemplo pronto (mock)
    return CaducidadeRequest(
        registro_requerida="123456789",
        marca_requerida="Marca Exemplo",
        classe_requerida="35",
        especificacoes_requerida="Serviços de marketing, consultoria e publicidade.",
        titular_requerida="Titular S/A",
        processo_requerida="987654321",
        nome_cliente="Cliente Exemplo Ltda.",
        marca_cliente="Marca Cliente",
        data_deposito_cliente="01/09/2024",
        classe_cliente="35"
    )

@router.post("/caducidade")
@limiter.limit("5/minute")  # Máximo 5 petições por minuto por usuário
def criar_peticao_caducidade(
    request: Request,
    peticao: CaducidadeRequest,
    db: Session = Depends(get_db)
):
    service = PeticaoService(db, openai_client)
    resultado = service.gerar_caducidade(peticao)
    return resultado  # {"id": ..., "peticao": ...}

@router.post("/caducidade/repost")
def reenviar_peticao_caducidade(
    repost: CaducidadeRepostRequest,
    db: Session = Depends(get_db)
):
    service = PeticaoService(db, openai_client)
    resposta = service.reenviar_caducidade(repost.chat_id, repost.prompt_anterior, repost.observacoes)
    return {"nova_resposta": resposta}

# ===== OPOSIÇÃO =====
@router.post("/oposicao")
def criar_peticao_oposicao(
    peticao: OposicaoRequest,
    db: Session = Depends(get_db)
):
    service = PeticaoService(db, openai_client)
    resultado = service.gerar_oposicao(peticao)
    return resultado

@router.post("/oposicao/repost")
def reenviar_peticao_oposicao(
    repost: OposicaoRepostRequest,
    db: Session = Depends(get_db)
):
    service = PeticaoService(db, openai_client)
    resposta = service.reenviar_oposicao(repost.chat_id, repost.prompt_anterior, repost.observacoes)
    return {"nova_resposta": resposta}

# ===== NULIDADE =====
@router.post("/nulidade")
def criar_peticao_nulidade(
    peticao: NulidadeRequest,
    db: Session = Depends(get_db)
):
    service = PeticaoService(db, openai_client)
    resultado = service.gerar_nulidade(peticao)
    return resultado

@router.post("/nulidade/repost")
def reenviar_peticao_nulidade(
    repost: NulidadeRepostRequest,
    db: Session = Depends(get_db)
):
    service = PeticaoService(db, openai_client)
    resposta = service.reenviar_nulidade(repost.chat_id, repost.prompt_anterior, repost.observacoes)
    return {"nova_resposta": resposta}

# ===== MANIFESTAÇÃO À OPOSIÇÃO =====
@router.post("/manifestacao-oposicao")
def criar_manifestacao_oposicao(
    peticao: ManifestacaoOposicaoRequest,
    db: Session = Depends(get_db)
):
    service = PeticaoService(db, openai_client)
    resultado = service.gerar_manifestacao_oposicao(peticao)
    return resultado

@router.post("/manifestacao-oposicao/repost")
def reenviar_manifestacao_oposicao(
    repost: ManifestacaoOposicaoRepostRequest,
    db: Session = Depends(get_db)
):
    service = PeticaoService(db, openai_client)
    resposta = service.reenviar_manifestacao_oposicao(repost.chat_id, repost.prompt_anterior, repost.observacoes)
    return {"nova_resposta": resposta}

# ===== MANIFESTAÇÃO AO RECURSO =====
@router.post("/manifestacao-recurso")
def criar_manifestacao_recurso(
    peticao: ManifestacaoRecursoRequest,
    db: Session = Depends(get_db)
):
    service = PeticaoService(db, openai_client)
    resultado = service.gerar_manifestacao_recurso(peticao)
    return resultado

@router.post("/manifestacao-recurso/repost")
def reenviar_manifestacao_recurso(
    repost: ManifestacaoRecursoRepostRequest,
    db: Session = Depends(get_db)
):
    service = PeticaoService(db, openai_client)
    resposta = service.reenviar_manifestacao_recurso(repost.chat_id, repost.prompt_anterior, repost.observacoes)
    return {"nova_resposta": resposta}

# ===== RECURSO DE INDEFERIMENTO =====
@router.post("/recurso-indeferimento")
def criar_recurso_indeferimento(
    peticao: RecursoIndeferimentoRequest,
    db: Session = Depends(get_db)
):
    service = PeticaoService(db, openai_client)
    resultado = service.gerar_recurso_indeferimento(peticao)
    return resultado

@router.post("/recurso-indeferimento/repost")
def reenviar_recurso_indeferimento(
    repost: RecursoIndeferimentoRepostRequest,
    db: Session = Depends(get_db)
):
    service = PeticaoService(db, openai_client)
    resposta = service.reenviar_recurso_indeferimento(repost.chat_id, repost.prompt_anterior, repost.observacoes)
    return {"nova_resposta": resposta}

# ===== PETIÇÃO DE CADUCIDADE =====
@router.post("/peticao-caducidade")
def criar_peticao_caducidade_completa(
    peticao: PeticaoCaducidadeRequest,
    db: Session = Depends(get_db)
):
    service = PeticaoService(db, openai_client)
    resultado = service.gerar_peticao_caducidade(peticao)
    return resultado

@router.post("/peticao-caducidade/repost")
def reenviar_peticao_caducidade_completa(
    repost: PeticaoCaducidadeRepostRequest,
    db: Session = Depends(get_db)
):
    service = PeticaoService(db, openai_client)
    resposta = service.reenviar_peticao_caducidade(repost.chat_id, repost.prompt_anterior, repost.observacoes)
    return {"nova_resposta": resposta}

# ===== CONTRARAZÃO DE NULIDADE =====
@router.post("/contrarazao-nulidade")
def criar_contrarazao_nulidade(
    peticao: ContrarazaoNulidadeRequest,
    db: Session = Depends(get_db)
):
    service = PeticaoService(db, openai_client)
    resultado = service.gerar_contrarazao_nulidade(peticao)
    return resultado

@router.post("/contrarazao-nulidade/repost")
def reenviar_contrarazao_nulidade(
    repost: ContrarazaoNulidadeRepostRequest,
    db: Session = Depends(get_db)
):
    service = PeticaoService(db, openai_client)
    resposta = service.reenviar_contrarazao_nulidade(repost.chat_id, repost.prompt_anterior, repost.observacoes)
    return {"nova_resposta": resposta}

# ===== ENDPOINTS AUXILIARES =====
@router.get("/peticao/{peticao_id}/historico")
def buscar_historico_chat(
    peticao_id: int,
    db: Session = Depends(get_db)
):
    """Busca o histórico de conversas de uma petição específica"""
    service = PeticaoService(db, openai_client)
    try:
        historico = service.repository.buscar_historico_chat(peticao_id)
        return {
            "peticao_id": peticao_id,
            "historico": [
                {
                    "id": h.id,
                    "prompt_usuario": h.prompt_usuario,
                    "resposta_ia": h.resposta_ia,
                    "created_at": h.created_at.isoformat()
                }
                for h in historico
            ]
        }
    except Exception as e:
        raise HTTPException(status_code=404, detail=f"Erro ao buscar histórico: {str(e)}")

@router.get("/peticao/{peticao_id}")
def buscar_peticao(
    peticao_id: int,
    db: Session = Depends(get_db)
):
    """Busca uma petição específica com seus detalhes"""
    service = PeticaoService(db, openai_client)
    try:
        peticao = service.repository.buscar_peticao_por_id(peticao_id)
        if not peticao:
            raise HTTPException(status_code=404, detail="Petição não encontrada")
        
        return {
            "id": peticao.id,
            "tipo": peticao.tipo,
            "processo_contestado": peticao.processo_contestado,
            "marca_contestada": peticao.marca_contestada,
            "texto_peticao": peticao.texto_peticao
        }
    except Exception as e:
        raise HTTPException(status_code=404, detail=f"Erro ao buscar petição: {str(e)}")

# ===== LISTAGEM DE PETIÇÕES =====
@router.get("/")
def listar_todas_peticoes(db: Session = Depends(get_db)):
    """Lista todas as petições criadas, agrupadas por tipo"""
    try:
        repository = PeticaoRepository(db)
        peticoes = repository.listar_todas_peticoes()
        
        # Agrupar por tipo
        resultado = {}
        for peticao in peticoes:
            tipo = peticao.tipo
            if tipo not in resultado:
                resultado[tipo] = []
            
            resultado[tipo].append({
                "id": peticao.id,
                "processo_contestado": peticao.processo_contestado,
                "marca_contestada": peticao.marca_contestada,
                "created_at": peticao.created_at.isoformat() if peticao.created_at else None,
                "texto_preview": peticao.texto_peticao[:100] + "..." if len(peticao.texto_peticao) > 100 else peticao.texto_peticao
            })
        
        return resultado
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Erro ao listar petições: {str(e)}")

@router.get("/{tipo}")
def listar_peticoes_por_tipo(tipo: str, db: Session = Depends(get_db)):
    """Lista petições de um tipo específico"""
    try:
        repository = PeticaoRepository(db)
        peticoes = repository.listar_por_tipo(tipo)
        
        resultado = []
        for peticao in peticoes:
            resultado.append({
                "id": peticao.id,
                "processo_contestado": peticao.processo_contestado,
                "marca_contestada": peticao.marca_contestada,
                "created_at": peticao.created_at.isoformat() if peticao.created_at else None,
                "texto_preview": peticao.texto_peticao[:100] + "..." if len(peticao.texto_peticao) > 100 else peticao.texto_peticao
            })
        
        return resultado
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Erro ao listar petições do tipo {tipo}: {str(e)}")

@router.delete("/{peticao_id}")
def excluir_peticao(peticao_id: int, db: Session = Depends(get_db)):
    """Exclui uma petição específica"""
    try:
        repository = PeticaoRepository(db)
        sucesso = repository.excluir_peticao(peticao_id)
        
        if sucesso:
            return {"message": "Petição excluída com sucesso"}
        else:
            raise HTTPException(status_code=404, detail="Petição não encontrada")
    except Exception as e:
        raise HTTPException(status_code=500, detail=f"Erro ao excluir petição: {str(e)}")