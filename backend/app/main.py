from fastapi import FastAPI, Request
from fastapi.middleware.cors import CORSMiddleware
from fastapi.staticfiles import StaticFiles
from slowapi import Limiter, _rate_limit_exceeded_handler
from slowapi.util import get_remote_address
from slowapi.errors import RateLimitExceeded
from app.api.routes import router as main_router
from app.config.settings import settings
from app.controllers.user_controller import router as user_router
import os

app = FastAPI()

# Rate Limiter setup
limiter = Limiter(key_func=get_remote_address)
app.state.limiter = limiter
app.add_exception_handler(RateLimitExceeded, _rate_limit_exceeded_handler)

# CORS middleware - Configurado para produção
app.add_middleware(
    CORSMiddleware,
    allow_origins=[
        "http://localhost:3000",        # Desenvolvimento frontend
        "http://127.0.0.1:3000",        # Desenvolvimento local
        "https://www.leaoai.com.br",    # Produção
        "http://www.leaoai.com.br",     # Produção sem HTTPS (fallback)
    ],
    allow_credentials=True,
    allow_methods=["GET", "POST", "PUT", "DELETE", "OPTIONS"],
    allow_headers=["*"],
)

# Include routers
app.include_router(main_router)
app.include_router(user_router)

# Serve static files from frontend directory
frontend_path = os.path.join(os.path.dirname(os.path.dirname(os.path.dirname(__file__))), "frontend")
if os.path.exists(frontend_path):
    app.mount("/static", StaticFiles(directory=frontend_path), name="static")

@app.get("/")
def read_root():
    return {"message": "Welcome to the Leão Adv API v3 !"}