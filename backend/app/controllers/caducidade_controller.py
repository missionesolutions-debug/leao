from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from typing import List

from app.config.database import get_db
from app.models.caducidade_model import Caducidade
from app.schemas.caducidade_schema import CaducidadeCreate, CaducidadeResponse, CaducidadeUpdate

router = APIRouter(prefix="/caducidade", tags=["Caducidade"])

@router.post("/", response_model=CaducidadeResponse)
def criar_caducidade(caducidade: CaducidadeCreate, db: Session = Depends(get_db)):
    db_obj = Caducidade(**caducidade.dict())
    db.add(db_obj)
    db.commit()
    db.refresh(db_obj)
    return db_obj

@router.get("/", response_model=List[CaducidadeResponse])
def listar_caducidades(db: Session = Depends(get_db)):
    return db.query(Caducidade).all()

@router.get("/{caducidade_id}", response_model=CaducidadeResponse)
def obter_caducidade(caducidade_id: int, db: Session = Depends(get_db)):
    caducidade = db.query(Caducidade).filter(Caducidade.id == caducidade_id).first()
    if not caducidade:
        raise HTTPException(status_code=404, detail="Caducidade não encontrada")
    return caducidade

@router.put("/{caducidade_id}", response_model=CaducidadeResponse)
def atualizar_caducidade(caducidade_id: int, caducidade: CaducidadeUpdate, db: Session = Depends(get_db)):
    db_obj = db.query(Caducidade).filter(Caducidade.id == caducidade_id).first()
    if not db_obj:
        raise HTTPException(status_code=404, detail="Caducidade não encontrada")
    for key, value in caducidade.dict(exclude_unset=True).items():
        setattr(db_obj, key, value)
    db.commit()
    db.refresh(db_obj)
    return db_obj

@router.delete("/{caducidade_id}")
def deletar_caducidade(caducidade_id: int, db: Session = Depends(get_db)):
    db_obj = db.query(Caducidade).filter(Caducidade.id == caducidade_id).first()
    if not db_obj:
        raise HTTPException(status_code=404, detail="Caducidade não encontrada")
    db.delete(db_obj)
    db.commit()
    return {"detail": "Caducidade excluída com sucesso"}