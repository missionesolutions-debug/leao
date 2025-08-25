from pydantic import BaseModel

class PlaceholderRequestModel(BaseModel):
    field1: str = None
    field2: int = None

class PlaceholderResponseModel(BaseModel):
    message: str
    status: str = 'success'

print('Placeholder schemas file created.')
