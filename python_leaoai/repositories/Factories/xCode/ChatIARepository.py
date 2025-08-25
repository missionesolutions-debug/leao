# framework/repositories/factories/xcode/chat_ia_repository.py

from sqlalchemy.ext.asyncio import AsyncSession
from sqlalchemy.future import select
from sqlalchemy.orm import selectinload
from sqlalchemy import desc
from typing import List, Optional

from data.models.xcode.chat_ia import ChatIA
from data.models.xcode.chat_ia_item import ChatIAItem


class ChatIARepository:
    def __init__(self, session: AsyncSession):
        self.session = session

    async def create_chat_ia(self, chat_ia: ChatIA) -> ChatIA:
        self.session.add(chat_ia)
        await self.session.commit()
        await self.session.refresh(chat_ia)
        return chat_ia

    async def update_chat_ia(self, chat_ia: ChatIA) -> ChatIA:
        self.session.add(chat_ia)  # Update é só add em SQLAlchemy
        await self.session.commit()
        await self.session.refresh(chat_ia)
        return chat_ia

    async def get_chat_ia_by_id(self, chat_ia_id: int) -> Optional[ChatIA]:
        result = await self.session.execute(
            select(ChatIA)
            .options(selectinload(ChatIA.chat_ia_items))
            .where(ChatIA.id == chat_ia_id)
        )
        return result.scalars().first()

    async def add_chat_ia_item(self, chat_ia_item: ChatIAItem) -> None:
        self.session.add(chat_ia_item)
        await self.session.commit()

    async def get_all_chats(self) -> List[ChatIA]:
        result = await self.session.execute(
            select(ChatIA)
            .options(selectinload(ChatIA.chat_ia_items))
            .order_by(desc(ChatIA.data_criacao))
        )
        return result.scalars().all()

    async def get_all_chats_by_user_id(self, user_id: int) -> List[ChatIA]:
        result = await self.session.execute(
            select(ChatIA)
            .options(selectinload(ChatIA.chat_ia_items))
            .where(ChatIA.usuario_id == user_id)
            .order_by(desc(ChatIA.data_criacao))
        )
        return result.scalars().all()

    async def get_all_chats_by_user_id_and_typed(self, user_id: int, typed: str) -> List[ChatIA]:
        result = await self.session.execute(
            select(ChatIA)
            .options(selectinload(ChatIA.chat_ia_items))
            .where(ChatIA.usuario_id == user_id, ChatIA.typed == typed)
            .order_by(desc(ChatIA.data_criacao))
        )
        return result.scalars().all()
