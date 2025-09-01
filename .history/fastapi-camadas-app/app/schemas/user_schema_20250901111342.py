from pydantic import BaseModel

class UserSchema(BaseModel):
    id: int
    username: str
    email: str
    full_name: str = None

    class Config:
        orm_mode = True

class UserCreateSchema(BaseModel):
    username: str
    email: str
    full_name: str = None

class UserUpdateSchema(BaseModel):
    username: str = None
    email: str = None
    full_name: str = None