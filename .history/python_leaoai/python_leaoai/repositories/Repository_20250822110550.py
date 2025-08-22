# framework/repositories/repository.py

from typing import TypeVar, Generic, List, Optional, Callable, Any
from sqlalchemy.ext.asyncio import AsyncSession
from sqlalchemy.future import select
from sqlalchemy.orm import selectinload
from sqlalchemy import update, delete
from sqlalchemy.exc import SQLAlchemyError

T = TypeVar("T")


class Repository(Generic[T]):
    def __init__(self, session: AsyncSession, entity: Any):
        self.session = session
        self.entity = entity

    # ------------------ CRUD ------------------
    async def get_obj(self, id: int) -> Optional[T]:
        return await self.session.get(self.entity, id)

    async def save_obj(self, obj: T) -> T:
        self.session.add(obj)
        await self.session.commit()
        await self.session.refresh(obj)
        return obj

    async def update_obj(self, obj: T) -> T:
        if obj is None:
            raise ValueError("obj cannot be None")

        self.session.add(obj)
        await self.session.commit()
        await self.session.refresh(obj)
        return obj

    async def get_all(self) -> List[T]:
        result = await self.session.execute(select(self.entity))
        return result.scalars().all()

    async def get_by_condition(self, condition: Callable) -> List[T]:
        result = await self.session.execute(select(self.entity).where(condition))
        return result.scalars().all()

    async def get_all_ativo(self) -> List[T]:
        result = await self.session.execute(
            select(self.entity).where(self.entity.ativo == True)  # exige col Ativo
        )
        return result.scalars().all()

    async def get_all_excluido(self) -> List[T]:
        result = await self.session.execute(
            select(self.entity).where(self.entity.excluido == True)  # exige col Excluido
        )
        return result.scalars().all()

    async def remove_obj(self, obj: T) -> bool:
        try:
            await self.session.delete(obj)
            await self.session.commit()
            return True
        except SQLAlchemyError:
            return False

    async def delete_obj(self, id: int) -> bool:
        try:
            obj = await self.session.get(self.entity, id)
            if not obj:
                return False

            # tenta setar os atributos se existirem
            if hasattr(obj, "ativo") and hasattr(obj, "excluido"):
                setattr(obj, "ativo", False)
                setattr(obj, "excluido", True)
                self.session.add(obj)
                await self.session.commit()
                return True

            return False
        except SQLAlchemyError:
            return False

    async def get_by_url(self, url: str) -> Optional[T]:
        result = await self.session.execute(
            select(self.entity).where(
                getattr(self.entity, "ativo") == True,
                getattr(self.entity, "url") == url
            )
        )
        return result.scalars().first()

    # ------------------ Métodos adicionais ------------------
    async def list(self) -> List[T]:
        result = await self.session.execute(select(self.entity))
        return result.scalars().all()

    async def add(self, entity: T) -> None:
        self.session.add(entity)
        await self.session.commit()

    async def update(self, entity: T) -> None:
        self.session.add(entity)
        await self.session.commit()

    async def delete(self, entity: T) -> None:
        await self.session.delete(entity)
        await self.session.commit()
