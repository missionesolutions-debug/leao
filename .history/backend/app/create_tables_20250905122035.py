from app.config.database import engine
from app.models.user_model import Base
from app.models.brand_model import Brand

Base.metadata.create_all(bind=engine)