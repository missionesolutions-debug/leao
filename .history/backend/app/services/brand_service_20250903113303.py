from app.repositories.brand_repository import BrandRepository
from app.schemas.brand_schema import BrandCreate, BrandUpdate, BrandResponse

class BrandService:
    def __init__(self, db):
        self.brand_repository = BrandRepository()

    def create_brand(self, brand_data: BrandCreate) -> BrandResponse:
        brand = self.brand_repository.create(brand_data)
        return BrandResponse.from_orm(brand)

    def get_brand(self, brand_id: int) -> BrandResponse:
        brand = self.brand_repository.get(brand_id)
        return BrandResponse.from_orm(brand)

    def update_brand(self, brand_id: int, brand_data: BrandUpdate) -> BrandResponse:
        brand = self.brand_repository.update(brand_id, brand_data)
        return BrandResponse.from_orm(brand)

    def delete_brand(self, brand_id: int) -> None:
        self.brand_repository.delete(brand_id)

    def list_brands(self) -> list[BrandResponse]:
        brands = self.brand_repository.list_all()
        return [BrandResponse.from_orm(brand) for brand in brands]