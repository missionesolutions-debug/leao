from sqlalchemy import Column, Integer, String
from database import Base

class Placeholder(Base):
    __tablename__ = 'placeholders'

    id = Column(Integer, primary_key=True, index=True)
    name = Column(String)
    description = Column(String)

print('Placeholder model defined.')
