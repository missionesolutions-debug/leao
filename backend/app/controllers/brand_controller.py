from fastapi import APIRouter, HTTPException, Depends
from sqlalchemy.orm import Session
from app.schemas.brand_schema import BrandCreate, BrandUpdate, BrandResponse
from app.services.brand_service import BrandService
from app.config.database import get_db

router = APIRouter()

@router.post("/", response_model=BrandResponse)
async def create_brand(brand: BrandCreate, db: Session = Depends(get_db)):
    brand_service = BrandService(db)
    return brand_service.create_brand(brand)

@router.get("/{brand_id}", response_model=BrandResponse)
async def get_brand(brand_id: int, db: Session = Depends(get_db)):
    brand_service = BrandService(db)
    brand = brand_service.get_brand(brand_id)
    if not brand:
        raise HTTPException(status_code=404, detail="Brand not found")
    return brand

@router.put("/{brand_id}", response_model=BrandResponse)
async def update_brand(brand_id: int, brand: BrandUpdate, db: Session = Depends(get_db)):
    brand_service = BrandService(db)
    updated_brand = brand_service.update_brand(brand_id, brand)
    if not updated_brand:
        raise HTTPException(status_code=404, detail="Brand not found")
    return updated_brand

@router.delete("/{brand_id}", response_model=dict)
async def delete_brand(brand_id: int, db: Session = Depends(get_db)):
    brand_service = BrandService(db)
    success = brand_service.delete_brand(brand_id)
    if not success:
        raise HTTPException(status_code=404, detail="Brand not found")
    return {"detail": "Brand deleted successfully"}