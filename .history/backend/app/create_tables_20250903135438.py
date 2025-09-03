from config.database import engine
from models.user_model import Base

Base.metadata.create_all(bind=engine)