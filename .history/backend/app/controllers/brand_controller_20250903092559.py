from fastapi import APIRouter, HTTPException, Depends
from app.schemas.brand_schema import BrandCreate, BrandUpdate, BrandResponse
from app.services.brand_service import BrandService

router = APIRouter()
brand_service = BrandService()

@router.post("/", response_model=BrandResponse)
async def create_brand(brand: BrandCreate):
    return await brand_service.create_brand(brand)

@router.get("/{brand_id}", response_model=BrandResponse)
async def get_brand(brand_id: int):
    brand = await brand_service.get_brand(brand_id)
    if not brand:
        raise HTTPException(status_code=404, detail="Brand not found")
    return brand

@router.put("/{brand_id}", response_model=BrandResponse)
async def update_brand(brand_id: int, brand: BrandUpdate):
    updated_brand = await brand_service.update_brand(brand_id, brand)
    if not updated_brand:
        raise HTTPException(status_code=404, detail="Brand not found")
    return updated_brand

@router.delete("/{brand_id}", response_model=dict)
async def delete_brand(brand_id: int):
    success = await brand_service.delete_brand(brand_id)
    if not success:
        raise HTTPException(status_code=404, detail="Brand not found")
    return {"detail": "Brand deleted successfully"}