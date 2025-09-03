from sqlalchemy.orm import Session
from app.models.brand_model import Brand
from app.schemas.brand_schema import BrandCreate, BrandUpdate

class BrandRepository:
    def __init__(self, db: Session):
        self.db = db

    def create_brand(self, brand: BrandCreate) -> Brand:
        db_brand = Brand(**brand.dict())
        self.db.add(db_brand)
        self.db.commit()
        self.db.refresh(db_brand)
        return db_brand

    def get_brand(self, brand_id: int) -> Brand:
        return self.db.query(Brand).filter(Brand.id == brand_id).first()

    def get_all_brands(self) -> list[Brand]:
        return self.db.query(Brand).all()

    def update_brand(self, brand_id: int, brand: BrandUpdate) -> Brand:
        db_brand = self.get_brand(brand_id)
        if db_brand:
            for key, value in brand.dict(exclude_unset=True).items():
                setattr(db_brand, key, value)
            self.db.commit()
            self.db.refresh(db_brand)
        return db_brand

    def delete_brand(self, brand_id: int) -> bool:
        db_brand = self.get_brand(brand_id)
        if db_brand:
            self.db.delete(db_brand)
            self.db.commit()
            return True
        return False