from fastapi import APIRouter, Request
from fastapi.responses import FileResponse
from fastapi.templating import Jinja2Templates
import os
from pathlib import Path

router = APIRouter()

# Caminho para o frontend
frontend_path = Path(__file__).parent.parent.parent.parent / "frontend"

@router.get("/")
async def index():
    """Página inicial - redireciona para dashboard"""
    return FileResponse(frontend_path / "menu.html")

@router.get("/menu")
@router.get("/menu.html")
async def dashboard():
    """Dashboard principal"""
    return FileResponse(frontend_path / "menu.html")

@router.get("/marca")
@router.get("/marca.html") 
async def marcas():
    """Hub de marcas"""
    return FileResponse(frontend_path / "marca.html")

@router.get("/oraculo")
@router.get("/oraculo.html")
async def oraculo():
    """Assistente IA"""
    return FileResponse(frontend_path / "oraculo.html")

@router.get("/perfil")
@router.get("/perfil.html")
async def perfil():
    """Administração de usuários"""
    return FileResponse(frontend_path / "perfil.html")

@router.get("/caducidade")
@router.get("/caducidade.html")
async def caducidade():
    """Processos de caducidade"""
    return FileResponse(frontend_path / "caducidade.html")

@router.get("/nulidade")
@router.get("/nulidade.html")
async def nulidade():
    """Processos de nulidade"""
    return FileResponse(frontend_path / "nulidade.html")

@router.get("/oposicao")
@router.get("/oposicao.html")
async def oposicao():
    """Processos de oposição"""
    return FileResponse(frontend_path / "oposicao.html")

@router.get("/recurso_indeferimento")
@router.get("/recurso_indeferimento.html")
async def recurso_indeferimento():
    """Recursos de indeferimento"""
    return FileResponse(frontend_path / "recurso_indeferimento.html")

@router.get("/manifestacao_oposicao")
@router.get("/manifestacao_oposicao.html")
async def manifestacao_oposicao():
    """Manifestações de oposição"""
    return FileResponse(frontend_path / "manifestacao_oposicao.html")

@router.get("/manifestacao_recurso")
@router.get("/manifestacao_recurso.html")
async def manifestacao_recurso():
    """Manifestações de recurso"""
    return FileResponse(frontend_path / "manifestacao_recurso.html")

@router.get("/contrarazao_nulidade")
@router.get("/contrarazao_nulidade.html")
async def contrarazao_nulidade():
    """Contrarrazões de nulidade"""
    return FileResponse(frontend_path / "contrarazao_nulidade.html")

@router.get("/cadastro")
@router.get("/cadastro.html")
async def cadastro():
    """Cadastro de usuários"""
    return FileResponse(frontend_path / "cadastro.html")

@router.get("/index")
@router.get("/index.html")
async def login():
    """Página de login"""
    return FileResponse(frontend_path / "index.html")

@router.get("/config.js")
async def config_js():
    """Arquivo de configuração JavaScript"""
    return FileResponse(frontend_path / "config.js", media_type="application/javascript")