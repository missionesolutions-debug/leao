from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from fastapi.staticfiles import StaticFiles
from app.api.routes import router as main_router
import os
from pathlib import Path

app = FastAPI(
    title="LegalAI - Sistema de Propriedade Intelectual",
    description="Sistema Genérico de Gestão de Marcas e Propriedade Intelectual",
    version="3.1.0"
)

# CORS middleware
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Include all routes
app.include_router(main_router)

# Serve static files from frontend directory
frontend_path = os.path.join(os.path.dirname(os.path.dirname(os.path.dirname(__file__))), "frontend")
if os.path.exists(frontend_path):
    app.mount("/assets", StaticFiles(directory=os.path.join(frontend_path, "assets")), name="assets")
    app.mount("/lib", StaticFiles(directory=os.path.join(frontend_path, "lib")), name="lib")
    app.mount("/scripts", StaticFiles(directory=os.path.join(frontend_path, "scripts")), name="scripts")
    app.mount("/static", StaticFiles(directory=frontend_path), name="static")

@app.get("/health")
def health_check():
    return {
        "status": "healthy", 
        "version": "3.1.0",
        "description": "LegalAI - Sistema Otimizado"
    }