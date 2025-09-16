from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from typing import List

from app.config.database import get_db
from app.models.manifestacao_oposicao_model import ManifestacaoOposicao
from app.schemas.manifestacao_oposicao_schema import (
    ManifestacaoOposicaoCreate,
    ManifestacaoOposicaoUpdate,
    ManifestacaoOposicaoResponse,
)

router = APIRouter()

@router.post("/", response_model=ManifestacaoOposicaoResponse)
def criar_manifestacao_oposicao(
    manifestacao: ManifestacaoOposicaoCreate, db: Session = Depends(get_db)
):
    db_obj = ManifestacaoOposicao(**manifestacao.dict())
    db.add(db_obj)
    db.commit()
    db.refresh(db_obj)
    return db_obj

@router.get("/", response_model=List[ManifestacaoOposicaoResponse])
def listar_manifestacoes_oposicao(db: Session = Depends(get_db)):
    return db.query(ManifestacaoOposicao).all()

@router.get("/{manifestacao_id}", response_model=ManifestacaoOposicaoResponse)
def obter_manifestacao_oposicao(manifestacao_id: int, db: Session = Depends(get_db)):
    obj = db.query(ManifestacaoOposicao).filter(ManifestacaoOposicao.id == manifestacao_id).first()
    if not obj:
        raise HTTPException(status_code=404, detail="Manifestação não encontrada")
    return obj

@router.put("/{manifestacao_id}", response_model=ManifestacaoOposicaoResponse)
def atualizar_manifestacao_oposicao(
    manifestacao_id: int,
    manifestacao: ManifestacaoOposicaoUpdate,
    db: Session = Depends(get_db),
):
    db_obj = db.query(ManifestacaoOposicao).filter(ManifestacaoOposicao.id == manifestacao_id).first()
    if not db_obj:
        raise HTTPException(status_code=404, detail="Manifestação não encontrada")
    for key, value in manifestacao.dict(exclude_unset=True).items():
        setattr(db_obj, key, value)
    db.commit()
    db.refresh(db_obj)
    return db_obj

@router.delete("/{manifestacao_id}")
def deletar_manifestacao_oposicao(manifestacao_id: int, db: Session = Depends(get_db)):
    db_obj = db.query(ManifestacaoOposicao).filter(ManifestacaoOposicao.id == manifestacao_id).first()
    if not db_obj:
        raise HTTPException(status_code=404, detail="Manifestação não encontrada")
    db.delete(db_obj)
    db.commit()
    return {"detail": "Manifestação excluída com sucesso"}