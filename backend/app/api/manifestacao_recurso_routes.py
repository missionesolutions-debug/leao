from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from typing import List

from app.config.database import get_db
from app.models.manifestacao_recurso_model import ManifestacaoRecurso
from app.schemas.manifestacao_recurso_schema import (
    ManifestacaoRecursoCreate,
    ManifestacaoRecursoUpdate,
    ManifestacaoRecursoResponse,
)

router = APIRouter()

@router.post("/", response_model=ManifestacaoRecursoResponse)
def criar_manifestacao_recurso(
    manifestacao: ManifestacaoRecursoCreate, db: Session = Depends(get_db)
):
    db_obj = ManifestacaoRecurso(**manifestacao.dict())
    db.add(db_obj)
    db.commit()
    db.refresh(db_obj)
    return db_obj

@router.get("/", response_model=List[ManifestacaoRecursoResponse])
def listar_manifestacoes_recurso(db: Session = Depends(get_db)):
    return db.query(ManifestacaoRecurso).all()

@router.get("/{manifestacao_id}", response_model=ManifestacaoRecursoResponse)
def obter_manifestacao_recurso(manifestacao_id: int, db: Session = Depends(get_db)):
    obj = db.query(ManifestacaoRecurso).filter(ManifestacaoRecurso.id == manifestacao_id).first()
    if not obj:
        raise HTTPException(status_code=404, detail="Manifestação não encontrada")
    return obj

@router.put("/{manifestacao_id}", response_model=ManifestacaoRecursoResponse)
def atualizar_manifestacao_recurso(
    manifestacao_id: int,
    manifestacao: ManifestacaoRecursoUpdate,
    db: Session = Depends(get_db),
):
    db_obj = db.query(ManifestacaoRecurso).filter(ManifestacaoRecurso.id == manifestacao_id).first()
    if not db_obj:
        raise HTTPException(status_code=404, detail="Manifestação não encontrada")
    for key, value in manifestacao.dict(exclude_unset=True).items():
        setattr(db_obj, key, value)
    db.commit()
    db.refresh(db_obj)
    return db_obj

@router.delete("/{manifestacao_id}")
def deletar_manifestacao_recurso(manifestacao_id: int, db: Session = Depends(get_db)):
    db_obj = db.query(ManifestacaoRecurso).filter(ManifestacaoRecurso.id == manifestacao_id).first()
    if not db_obj:
        raise HTTPException(status_code=404, detail="Manifestação não encontrada")
    db.delete(db_obj)
    db.commit()
    return {"detail": "Manifestação excluída com sucesso"}