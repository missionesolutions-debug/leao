from fastapi import APIRouter, HTTPException, Depends
from app.controllers.brand_controller import BrandController
from app.schemas.brand_schema import BrandCreate, BrandUpdate, BrandResponse

router = APIRouter()
brand_controller = BrandController()

@router.post("/brands/", response_model=BrandResponse)
async def create_brand(brand: BrandCreate):
    return await brand_controller.create_brand(brand)

@router.get("/brands/{brand_id}", response_model=BrandResponse)
async def get_brand(brand_id: int):
    brand = await brand_controller.get_brand(brand_id)
    if not brand:
        raise HTTPException(status_code=404, detail="Brand not found")
    return brand

@router.put("/brands/{brand_id}", response_model=BrandResponse)
async def update_brand(brand_id: int, brand: BrandUpdate):
    updated_brand = await brand_controller.update_brand(brand_id, brand)
    if not updated_brand:
        raise HTTPException(status_code=404, detail="Brand not found")
    return updated_brand

@router.delete("/brands/{brand_id}", response_model=dict)
async def delete_brand(brand_id: int):
    success = await brand_controller.delete_brand(brand_id)
    if not success:
        raise HTTPException(status_code=404, detail="Brand not found")
    return {"detail": "Brand deleted successfully"}