from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from fastapi.staticfiles import StaticFiles
from fastapi.responses import FileResponse
from app.api.routes import router as main_router
import os
from pathlib import Path

app = FastAPI(
    title="Leão Adv API",
    description="Sistema de Gestão de Marcas e Propriedade Intelectual",
    version="3.0.0"
)

# CORS middleware
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Include routers
app.include_router(main_router)

# Serve static files from frontend directory
frontend_path = os.path.join(os.path.dirname(os.path.dirname(os.path.dirname(__file__))), "frontend")
if os.path.exists(frontend_path):
    app.mount("/assets", StaticFiles(directory=os.path.join(frontend_path, "assets")), name="assets")
    app.mount("/lib", StaticFiles(directory=os.path.join(frontend_path, "lib")), name="lib")
    app.mount("/scripts", StaticFiles(directory=os.path.join(frontend_path, "scripts")), name="scripts")
    app.mount("/static", StaticFiles(directory=frontend_path), name="static")

# Rotas básicas para as páginas HTML
@app.get("/")
async def index():
    return FileResponse(os.path.join(frontend_path, "menu.html"))

@app.get("/menu")
@app.get("/menu.html")
async def menu():
    return FileResponse(os.path.join(frontend_path, "menu.html"))

@app.get("/marca")
@app.get("/marca.html")
async def marca():
    return FileResponse(os.path.join(frontend_path, "marca.html"))

@app.get("/oraculo")
@app.get("/oraculo.html")
async def oraculo():
    return FileResponse(os.path.join(frontend_path, "oraculo.html"))

@app.get("/perfil")
@app.get("/perfil.html")
async def perfil():
    return FileResponse(os.path.join(frontend_path, "perfil.html"))

@app.get("/config.js")
async def config_js():
    return FileResponse(os.path.join(frontend_path, "config.js"), media_type="application/javascript")

@app.get("/health")
def health_check():
    return {"status": "healthy", "version": "3.0.0"}