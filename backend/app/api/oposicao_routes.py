from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from typing import List

from app.config.database import get_db
from app.models.oposicao_model import Oposicao
from app.schemas.oposicao_schema import (
    OposicaoCreate,
    OposicaoUpdate,
    OposicaoResponse,
)

router = APIRouter()

@router.post("/", response_model=OposicaoResponse)
def criar_oposicao(
    oposicao: OposicaoCreate, db: Session = Depends(get_db)
):
    db_obj = Oposicao(**oposicao.dict())
    db.add(db_obj)
    db.commit()
    db.refresh(db_obj)
    return db_obj

@router.get("/", response_model=List[OposicaoResponse])
def listar_oposicoes(db: Session = Depends(get_db)):
    return db.query(Oposicao).all()

@router.get("/{oposicao_id}", response_model=OposicaoResponse)
def obter_oposicao(oposicao_id: int, db: Session = Depends(get_db)):
    obj = db.query(Oposicao).filter(Oposicao.id == oposicao_id).first()
    if not obj:
        raise HTTPException(status_code=404, detail="Oposição não encontrada")
    return obj

@router.put("/{oposicao_id}", response_model=OposicaoResponse)
def atualizar_oposicao(
    oposicao_id: int,
    oposicao: OposicaoUpdate,
    db: Session = Depends(get_db),
):
    db_obj = db.query(Oposicao).filter(Oposicao.id == oposicao_id).first()
    if not db_obj:
        raise HTTPException(status_code=404, detail="Oposição não encontrada")
    for key, value in oposicao.dict(exclude_unset=True).items():
        setattr(db_obj, key, value)
    db.commit()
    db.refresh(db_obj)
    return db_obj

@router.delete("/{oposicao_id}")
def deletar_oposicao(oposicao_id: int, db: Session = Depends(get_db)):
    db_obj = db.query(Oposicao).filter(Oposicao.id == oposicao_id).first()
    if not db_obj:
        raise HTTPException(status_code=404, detail="Oposição não encontrada")
    db.delete(db_obj)
    db.commit()
    return {"detail": "Oposição excluída com sucesso"}