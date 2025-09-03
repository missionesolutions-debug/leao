from app.config.database import engine
from app.models.user_model import Base

Base.metadata.create_all(bind=engine)