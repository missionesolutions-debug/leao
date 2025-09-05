from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from app.api.routes import router as main_router
from app.api.auth_routes import router as auth_router
from app.config.settings import settings
from app.controllers.user_controller import router as user_router

app = FastAPI()

# CORS middleware
app.add_middleware(
    CORSMiddleware,
    allow_origins=settings.ALLOW_ORIGINS,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Include routers
app.include_router(main_router)
app.include_router(auth_router)
app.include_router(user_router)

@app.get("/")
def read_root():
    return {"message": "Welcome to the Leão Adv API v3 !"}