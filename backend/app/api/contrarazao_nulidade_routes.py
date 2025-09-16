from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from typing import List

from app.config.database import get_db
from app.models.contrarazao_nulidade_model import ContrarazaoNulidade
from app.schemas.contrarazao_nulidade_schema import (
    ContrarazaoNulidadeCreate,
    ContrarazaoNulidadeUpdate,
    ContrarazaoNulidadeResponse,
)

router = APIRouter()

@router.post("/", response_model=ContrarazaoNulidadeResponse)
def criar_contrarrazao_nulidade(
    contrarrazao: ContrarazaoNulidadeCreate, db: Session = Depends(get_db)
):
    db_obj = ContrarazaoNulidade(**contrarrazao.dict())
    db.add(db_obj)
    db.commit()
    db.refresh(db_obj)
    return db_obj

@router.get("/", response_model=List[ContrarazaoNulidadeResponse])
def listar_contrarrazoes_nulidade(db: Session = Depends(get_db)):
    return db.query(ContrarazaoNulidade).all()

@router.get("/{contrarrazao_id}", response_model=ContrarazaoNulidadeResponse)
def obter_contrarrazao_nulidade(contrarrazao_id: int, db: Session = Depends(get_db)):
    obj = db.query(ContrarazaoNulidade).filter(ContrarazaoNulidade.id == contrarrazao_id).first()
    if not obj:
        raise HTTPException(status_code=404, detail="Contrarrazão não encontrada")
    return obj

@router.put("/{contrarrazao_id}", response_model=ContrarazaoNulidadeResponse)
def atualizar_contrarrazao_nulidade(
    contrarrazao_id: int,
    contrarrazao: ContrarazaoNulidadeUpdate,
    db: Session = Depends(get_db),
):
    db_obj = db.query(ContrarazaoNulidade).filter(ContrarazaoNulidade.id == contrarrazao_id).first()
    if not db_obj:
        raise HTTPException(status_code=404, detail="Contrarrazão não encontrada")
    for key, value in contrarrazao.dict(exclude_unset=True).items():
        setattr(db_obj, key, value)
    db.commit()
    db.refresh(db_obj)
    return db_obj

@router.delete("/{contrarrazao_id}")
def deletar_contrarrazao_nulidade(contrarrazao_id: int, db: Session = Depends(get_db)):
    db_obj = db.query(ContrarazaoNulidade).filter(ContrarazaoNulidade.id == contrarrazao_id).first()
    if not db_obj:
        raise HTTPException(status_code=404, detail="Contrarrazão não encontrada")
    db.delete(db_obj)
    db.commit()
    return {"detail": "Contrarrazão excluída com sucesso"}