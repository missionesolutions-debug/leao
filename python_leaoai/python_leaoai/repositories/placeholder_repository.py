from sqlalchemy.orm import Session
from models.placeholder_model import Placeholder

class PlaceholderRepository:
    def __init__(self, db: Session):
        self.db = db

    def create_placeholder(self, name: str, description: str):
        db_placeholder = Placeholder(name=name, description=description)
        self.db.add(db_placeholder)
        self.db.commit()
        self.db.refresh(db_placeholder)
        return db_placeholder

    def get_placeholder(self, placeholder_id: int):
        return self.db.query(Placeholder).filter(Placeholder.id == placeholder_id).first()

    def get_all_placeholders(self):
        return self.db.query(Placeholder).all()

    def update_placeholder(self, placeholder_id: int, name: str = None, description: str = None):
        db_placeholder = self.get_placeholder(placeholder_id)
        if db_placeholder:
            if name is not None:
                db_placeholder.name = name
            if description is not None:
                db_placeholder.description = description
            self.db.commit()
            self.db.refresh(db_placeholder)
        return db_placeholder

    def delete_placeholder(self, placeholder_id: int):
        db_placeholder = self.get_placeholder(placeholder_id)
        if db_placeholder:
            self.db.delete(db_placeholder)
            self.db.commit()
            return True
        return False

print('Placeholder repository defined.')
