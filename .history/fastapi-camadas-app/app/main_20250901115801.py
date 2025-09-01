from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from app.api.routes import router as api_router
#from app.config.settings import settings

#app = FastAPI(title=settings.PROJECT_NAME)

# app.add_middleware(
#     CORSMiddleware,
#     allow_origins=settings.ALLOW_ORIGINS,
#     allow_credentials=True,
#     allow_methods=["*"],
#     allow_headers=["*"],
# )

app.include_router(api_router)

# @app.get("/")
# def read_root():
#     return {"message": "Welcome to the FastAPI application!"}