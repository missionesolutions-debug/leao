from fastapi import FastAPI, Request, HTTPException
from fastapi.middleware.cors import CORSMiddleware
from fastapi.staticfiles import StaticFiles
from slowapi import Limiter, _rate_limit_exceeded_handler
from slowapi.util import get_remote_address
from slowapi.errors import RateLimitExceeded
from app.api.routes import router as main_router
from app.config.settings import settings
from app.controllers.user_controller import router as user_router
import os
import logging

app = FastAPI(
    title="Leão Adv API",
    description="Sistema de Gestão de Marcas e Propriedade Intelectual",
    version="3.0.0",
    docs_url="/docs",  # Habilitar docs para desenvolvimento
    redoc_url="/redoc"  # Habilitar redoc para desenvolvimento
)

# Configurar logging para monitorar ataques
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

# Rate Limiter setup
limiter = Limiter(key_func=get_remote_address)
app.state.limiter = limiter
app.add_exception_handler(RateLimitExceeded, _rate_limit_exceeded_handler)

# Middleware de segurança para bloquear tentativas suspeitas
@app.middleware("http")
async def security_middleware(request: Request, call_next):
    # Lista de paths suspeitos para monitorar
    suspicious_paths = [
        ".env", "config", "admin", "wp-admin", "phpmyadmin", 
        "session/properties", "sonicos", "v1/pods", "api/v1",
        ".git", "backup", "database", "sql"
    ]
    
    path = request.url.path.lower()
    
    # Log tentativas suspeitas
    if any(suspicious in path for suspicious in suspicious_paths):
        client_ip = request.client.host if request.client else "unknown"
        logger.warning(f"Tentativa suspeita de acesso de {client_ip} para {path}")
        # Retornar 403 Forbidden para desencorajar atacantes
        raise HTTPException(status_code=403, detail="Access denied")
    
    response = await call_next(request)
    return response

# CORS middleware - Configurado para produção
app.add_middleware(
    CORSMiddleware,
    allow_origins=[
        "http://localhost:3000",        # Desenvolvimento frontend
        "http://127.0.0.1:3000",        # Desenvolvimento local
        "http://localhost:8080",        # Desenvolvimento frontend (porta 8080)
        "http://127.0.0.1:8080",        # Desenvolvimento local (porta 8080)
        "https://www.leaoia.com.br",    # Produção
        "http://www.leaoia.com.br",     # Produção sem HTTPS (fallback)
        "https://leaoia.com.br",        # Produção sem www
        "http://leaoia.com.br",         # Produção sem www e HTTPS (fallback)
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

@app.get("/health")
def health_check():
    """Endpoint para verificação de saúde da aplicação"""
    return {
        "status": "healthy",
        "version": "3.0.0",
        "service": "leao-adv-api"
    }