from sqlalchemy.orm import Session
from sqlalchemy.future import select
from sqlalchemy.exc import SQLAlchemyError
from datetime import datetime
from typing import List, Optional
import asyncio

from data.models.pessoa import Usuario


class UsuarioFactory:
    def __init__(self, db: Session):
        self._db = db

    # ---------------------------
    # CRUD
    # ---------------------------
    def get_obj(self, id: int) -> Optional[Usuario]:
        return self._db.query(Usuario).filter(Usuario.id == id).first()

    def save_obj(self, obj: Usuario) -> Usuario:
        self._db.add(obj)
        self._db.commit()
        self._db.refresh(obj)
        return obj

    def update_obj(self, obj: Usuario) -> Usuario:
        self._db.merge(obj)
        self._db.commit()
        return obj

    def get_all(self) -> List[Usuario]:
        return self._db.query(Usuario).all()

    def get_all_ativo(self) -> List[Usuario]:
        return self._db.query(Usuario).filter(Usuario.ativo.is_(True)).all()

    def get_all_excluido(self) -> List[Usuario]:
        return self._db.query(Usuario).filter(Usuario.excluido.is_(True)).all()

    def remove_obj(self, obj: Usuario) -> bool:
        try:
            self._db.delete(obj)
            self._db.commit()
            return True
        except SQLAlchemyError:
            self._db.rollback()
            return False

    def delete_obj(self, id: int) -> bool:
        try:
            obj = self._db.query(Usuario).filter(Usuario.id == id).first()
            if not obj:
                return False
            obj.ativo = False
            obj.excluido = True
            self._db.commit()
            return True
        except SQLAlchemyError:
            self._db.rollback()
            return False

    # ---------------------------
    # Consultas específicas
    # ---------------------------
    def get_obj_by_login(self, login: str) -> Optional[Usuario]:
        return self._db.query(Usuario).filter(Usuario.login == login).first()

    def get_by_email(self, email: str) -> Optional[Usuario]:
        return self._db.query(Usuario).filter(Usuario.email == email).first()

    def get_user(self, email: str, cpf: Optional[str] = None) -> Optional[Usuario]:
        query = self._db.query(Usuario).filter(Usuario.email == email)
        if cpf:
            query = query.filter(Usuario.cpf == cpf)
        return query.first()

    def get_user_student(self, email: str, cpf: str) -> Optional[Usuario]:
        return (
            self._db.query(Usuario)
            .filter(Usuario.email == email, Usuario.role_gate == "User")
            .first()
        )

    # ---------------------------
    # Assíncronos (async)
    # ---------------------------
    async def add_usuario_async(self, usuario: Usuario, async_session) -> Usuario:
        async with async_session() as session:
            async with session.begin():
                session.add(usuario)
            await session.commit()
            return usuario

    async def get_user_async(self, email: str, async_session) -> Optional[Usuario]:
        async with async_session() as session:
            result = await session.execute(
                select(Usuario).where(Usuario.email == email)
            )
            return result.scalars().first()

    async def update_obj_async(self, usuario: Usuario, async_session) -> Usuario:
        async with async_session() as session:
            async with session.begin():
                session.merge(usuario)
            await session.commit()
            return usuario

    async def get_by_password_reset_guid_async(self, guid, async_session) -> Optional[Usuario]:
        async with async_session() as session:
            result = await session.execute(
                select(Usuario).where(
                    Usuario.password_token == guid,
                    Usuario.password_token_expiry >= datetime.now()
                )
            )
            return result.scalars().first()
