from sqlalchemy.orm import Session
from sqlalchemy.exc import SQLAlchemyError
from sqlalchemy.future import select
from typing import List, Optional
import uuid

from data.database import ApplicationDbContext
from framework.data.models.core.metadata import Metadata


class MetadataRepository:
    def __init__(self, context: ApplicationDbContext):
        self._context: Session = context.session

    # -----------------------------
    # Crud
    # -----------------------------
    def GetObj(self, id: str) -> Optional[Metadata]:
        try:
            # tenta converter para UUID
            guid_id = uuid.UUID(id)
            return self._context.query(Metadata).filter(Metadata.Id == guid_id).first()
        except (ValueError, TypeError):
            return None

    async def GetObjByGuidAsync(self, id: str) -> Optional[Metadata]:
        stmt = select(Metadata).where(Metadata.Guid == id)
        result = await self._context.execute(stmt)
        return result.scalars().first()

    def GetObjByGuid(self, id: str) -> Optional[Metadata]:
        return self._context.query(Metadata).filter(Metadata.Guid == id).first()

    def GetObjsByGuid(self, id: str) -> List[Metadata]:
        return self._context.query(Metadata).filter(Metadata.Guid == id).all()

    def GetObjsByRefGuid(self, referenc: str) -> List[Metadata]:
        return (
            self._context.query(Metadata)
            .filter(Metadata.MetadataRef.ilike(referenc))
            .all()
        )

    def SaveObj(self, obj: Metadata) -> Metadata:
        self._context.add(obj)
        self._context.commit()
        return obj

    def UpdateObj(self, obj: Metadata) -> Metadata:
        self._context.merge(obj)
        self._context.commit()
        return obj

    def GetAll(self) -> List[Metadata]:
        return self._context.query(Metadata).all()

    def RemoveObj(self, obj: Metadata) -> bool:
        try:
            self._context.delete(obj)
            self._context.commit()
            return True
        except SQLAlchemyError:
            self._context.rollback()
            return False

    def DeleteObj(self, Id: str) -> bool:
        try:
            obj = self.GetObj(Id)
            if obj:
                self._context.merge(obj)
                self._context.commit()
                return True
            return False
        except SQLAlchemyError:
            self._context.rollback()
            return False
