from typing import Type, TypeVar, Generic, List, Optional, Any, Callable
from sqlalchemy.orm import Session, joinedload
from sqlalchemy.future import select
from sqlalchemy import update as sql_update
from sqlalchemy.exc import SQLAlchemyError
from sqlalchemy.ext.asyncio import AsyncSession
from models import Categoria  # Importe seus modelos aqui

T = TypeVar('T')

class Repository(Generic[T]):
    def __init__(self, model: Type[T], db: Session):
        self.model = model
        self.db = db

    # -------------------------
    # CRUD Síncrono
    # -------------------------
    def get_obj(self, id: int) -> Optional[T]:
        return self.db.query(self.model).get(id)

    def save_obj(self, obj: T) -> T:
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def update_obj(self, obj: T) -> T:
        if obj is None:
            raise ValueError("Objeto não pode ser None")
        self.db.add(obj)
        self.db.commit()
        self.db.refresh(obj)
        return obj

    def delete_obj(self, id: int) -> bool:
        obj = self.get_obj(id)
        if not obj:
            return False
        try:
            if hasattr(obj, 'ativo'):
                setattr(obj, 'ativo', False)
            if hasattr(obj, 'excluido'):
                setattr(obj, 'excluido', True)
            self.db.add(obj)
            self.db.commit()
            return True
        except SQLAlchemyError:
            self.db.rollback()
            return False

    def remove_obj(self, obj: T) -> bool:
        try:
            self.db.delete(obj)
            self.db.commit()
            return True
        except SQLAlchemyError:
            self.db.rollback()
            return False

    def get_all(self) -> List[T]:
        return self.db.query(self.model).all()

    def get_all_ativo(self) -> List[T]:
        return self.db.query(self.model).filter(getattr(self.model, 'ativo') == True).all()

    def get_all_excluido(self) -> List[T]:
        return self.db.query(self.model).filter(getattr(self.model, 'excluido') == True).all()

    def get_by_url(self, url: str) -> Optional[T]:
        return self.db.query(self.model).filter(
            getattr(self.model, 'ativo') == True,
            getattr(self.model, 'url') == url
        ).first()

    # -------------------------
    # Consultas com Categoria
    # -------------------------
    def get_all_with_categoria_ativo(self) -> List[T]:
        items = self.db.query(self.model).all()
        result = []
        for item in items:
            if getattr(item, 'ativo', False) and getattr(item, 'categoria', None):
                result.append(item)
        return result

    def get_all_by_categoria_url(self, category_url: str) -> Optional[List[T]]:
        categoria = self.db.query(Categoria).filter(
            Categoria.ativo == True,
            Categoria.url == category_url
        ).first()
        if categoria:
            items = self.db.query(self.model).all()
            result = []
            for item in items:
                if getattr(item, 'ativo', False) and getattr(item, 'categoria_id', None) == categoria.id:
                    result.append(item)
            return result
        return None

    def get_all_by_categoria_ativo(self, category_name: str) -> List[T]:
        items = self.db.query(self.model).all()
        result = []
        for item in items:
            if getattr(item, 'ativo', False) and getattr(item, 'categoria', None) == category_name:
                result.append(item)
        return result

    def get_all_categorias(self) -> List[Any]:
        items = self.db.query(self.model).all()
        categorias = set()
        for item in items:
            categoria = getattr(item, 'categoria', None)
            if categoria:
                categorias.add(categoria)
        return list(categorias)

    # -------------------------
    # Métodos Assíncronos
    # -------------------------
    async def save_obj_async(self, obj: T, async_db: AsyncSession) -> T:
        async_db.add(obj)
        await async_db.commit()
        await async_db.refresh(obj)
        return obj

    async def update_obj_async(self, obj: T, async_db: AsyncSession) -> T:
        if obj is None:
            raise ValueError("Objeto não pode ser None")
        async_db.add(obj)
        await async_db.commit()
        await async_db.refresh(obj)
        return obj

    async def remove_obj_async(self, obj: T, async_db: AsyncSession) -> bool:
        try:
            await async_db.delete(obj)
            await async_db.commit()
            return True
        except SQLAlchemyError:
            await async_db.rollback()
            return False
