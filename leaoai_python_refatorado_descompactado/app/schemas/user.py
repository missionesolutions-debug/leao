# Cole o código Pydantic para o modelo User aqui:
# from pydantic import BaseModel
#
# class UserBase(BaseModel):
#     Nome: str
#     Email: str
#     RoleGate: str
#     Avatar: str | None = None
#
# class UserCreate(UserBase):
#     Password: str
#
# class User(UserBase):
#     Id: int
#
#     class Config:
#         orm_mode = True
