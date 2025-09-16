from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from typing import List

from app.config.database import get_db
from app.models.nulidade_model import Nulidade
from app.schemas.nulidade_schema import (
    NulidadeCreate,
    NulidadeUpdate,
    NulidadeResponse,
)

router = APIRouter()

@router.post("/", response_model=NulidadeResponse)
def criar_nulidade(
    nulidade: NulidadeCreate, db: Session = Depends(get_db)
):
    db_obj = Nulidade(**nulidade.dict())
    db.add(db_obj)
    db.commit()
    db.refresh(db_obj)
    return db_obj

@router.get("/", response_model=List[NulidadeResponse])
def listar_nulidades(db: Session = Depends(get_db)):
    return db.query(Nulidade).all()

@router.get("/{nulidade_id}", response_model=NulidadeResponse)
def obter_nulidade(nulidade_id: int, db: Session = Depends(get_db)):
    obj = db.query(Nulidade).filter(Nulidade.id == nulidade_id).first()
    if not obj:
        raise HTTPException(status_code=404, detail="Nulidade não encontrada")
    return obj

@router.put("/{nulidade_id}", response_model=NulidadeResponse)
def atualizar_nulidade(
    nulidade_id: int,
    nulidade: NulidadeUpdate,
    db: Session = Depends(get_db),
):
    db_obj = db.query(Nulidade).filter(Nulidade.id == nulidade_id).first()
    if not db_obj:
        raise HTTPException(status_code=404, detail="Nulidade não encontrada")
    for key, value in nulidade.dict(exclude_unset=True).items():
        setattr(db_obj, key, value)
    db.commit()
    db.refresh(db_obj)
    return db_obj

@router.delete("/{nulidade_id}")
def deletar_nulidade(nulidade_id: int, db: Session = Depends(get_db)):
    db_obj = db.query(Nulidade).filter(Nulidade.id == nulidade_id).first()
    if not db_obj:
        raise HTTPException(status_code=404, detail="Nulidade não encontrada")
    db.delete(db_obj)
    db.commit()
    return {"detail": "Nulidade excluída com sucesso"}