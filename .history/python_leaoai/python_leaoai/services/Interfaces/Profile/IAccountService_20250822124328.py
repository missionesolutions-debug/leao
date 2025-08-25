from abc import ABC, abstractmethod
from typing import List
from fastapi import Depends
from pydantic import BaseModel
from jose import jwt
from starlette.requests import Request


class IAccountService(ABC):
    @abstractmethod
    async def get_user_claims_async(self, user_id: int) -> List[dict]:
        """
        Recupera as Claims do usuário de forma assíncrona.
        :param user_id: ID do usuário
        :return: Lista de claims em formato dict (equivalente a Claim do .NET)
        """
        pass

    @abstractmethod
    def get_user_claims(self, user_id: int) -> List[dict]:
        """
        Recupera as Claims do usuário de forma síncrona.
        :param user_id: ID do usuário
        :return: Lista de claims em formato dict
        """
        pass
