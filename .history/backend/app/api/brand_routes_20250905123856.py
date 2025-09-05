from fastapi import APIRouter, HTTPException, Depends
from sqlalchemy.orm import Session
from app.repositories.brand_repository import BrandRepository
from app.schemas.brand_schema import BrandCreate, BrandUpdate, BrandResponse
from app.config.database import get_db

router = APIRouter()

@router.post("/", response_model=BrandResponse)
async def create_brand(brand: BrandCreate, db: Session = Depends(get_db)):
    brand_repository = BrandRepository(db)
    created_brand = brand_repository.create_brand(brand)
    return created_brand

@router.get("/{brand_id}", response_model=BrandResponse)
async def get_brand(brand_id: int, db: Session = Depends(get_db)):
    brand_repository = BrandRepository(db)
    brand = brand_repository.get_by_id(brand_id)
    if not brand:
        raise HTTPException(status_code=404, detail="Brand not found")
    return brand

@router.put("/{brand_id}", response_model=BrandResponse)
async def update_brand(brand_id: int, brand: BrandUpdate, db: Session = Depends(get_db)):
    brand_repository = BrandRepository(db)
    db_brand = brand_repository.get_brand(brand_id)
    if not db_brand:
        raise HTTPException(status_code=404, detail="Brand not found")
    update_data = brand.dict(exclude_unset=True)
    for key, value in update_data.items():
        setattr(db_brand, key, value)
    db.commit()
    db.refresh(db_brand)
    return db_brand

@router.delete("/{brand_id}", response_model=dict)
async def delete_brand(brand_id: int, db: Session = Depends(get_db)):
    brand_repository = BrandRepository(db)
    db_brand = brand_repository.get_brand(brand_id)
    if not db_brand:
        raise HTTPException(status_code=404, detail="Brand not found")
    db.delete(db_brand)
    db.commit()
    return {"detail": "Brand deleted successfully"}