from backend.app.config.database import engine
from backend.app.models.user_model import Base

Base.metadata.create_all(bind=engine)