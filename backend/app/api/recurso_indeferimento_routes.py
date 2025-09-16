from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from typing import List

from app.config.database import get_db
from app.models.recurso_indeferimento_model import RecursoIndeferimento
from app.schemas.recurso_indeferimento_schema import (
    RecursoIndeferimentoCreate,
    RecursoIndeferimentoUpdate,
    RecursoIndeferimentoResponse,
)

router = APIRouter()

@router.post("/", response_model=RecursoIndeferimentoResponse)
def criar_recurso_indeferimento(
    recurso: RecursoIndeferimentoCreate, db: Session = Depends(get_db)
):
    db_obj = RecursoIndeferimento(**recurso.dict())
    db.add(db_obj)
    db.commit()
    db.refresh(db_obj)
    return db_obj

@router.get("/", response_model=List[RecursoIndeferimentoResponse])
def listar_recursos_indeferimento(db: Session = Depends(get_db)):
    return db.query(RecursoIndeferimento).all()

@router.get("/{recurso_id}", response_model=RecursoIndeferimentoResponse)
def obter_recurso_indeferimento(recurso_id: int, db: Session = Depends(get_db)):
    obj = db.query(RecursoIndeferimento).filter(RecursoIndeferimento.id == recurso_id).first()
    if not obj:
        raise HTTPException(status_code=404, detail="Recurso não encontrado")
    return obj

@router.put("/{recurso_id}", response_model=RecursoIndeferimentoResponse)
def atualizar_recurso_indeferimento(
    recurso_id: int,
    recurso: RecursoIndeferimentoUpdate,
    db: Session = Depends(get_db),
):
    db_obj = db.query(RecursoIndeferimento).filter(RecursoIndeferimento.id == recurso_id).first()
    if not db_obj:
        raise HTTPException(status_code=404, detail="Recurso não encontrado")
    for key, value in recurso.dict(exclude_unset=True).items():
        setattr(db_obj, key, value)
    db.commit()
    db.refresh(db_obj)
    return db_obj

@router.delete("/{recurso_id}")
def deletar_recurso_indeferimento(recurso_id: int, db: Session = Depends(get_db)):
    db_obj = db.query(RecursoIndeferimento).filter(RecursoIndeferimento.id == recurso_id).first()
    if not db_obj:
        raise HTTPException(status_code=404, detail="Recurso não encontrado")
    db.delete(db_obj)
    db.commit()
    return {"detail": "Recurso excluído com sucesso"}