from fastapi import APIRouter, Request
from fastapi.responses import FileResponse, RedirectResponse
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
async def dashboard():
    """Dashboard principal"""
    return FileResponse(frontend_path / "menu.html")

@router.get("/marca")
async def marcas():
    """Hub de marcas"""
    return FileResponse(frontend_path / "marca.html")

@router.get("/oraculo")
async def oraculo():
    """Assistente IA"""
    return FileResponse(frontend_path / "oraculo.html")

@router.get("/perfil")
async def perfil():
    """Administração de usuários"""
    return FileResponse(frontend_path / "perfil.html")

@router.get("/caducidade")
async def caducidade():
    """Processos de caducidade"""
    return FileResponse(frontend_path / "caducidade.html")

@router.get("/nulidade")
async def nulidade():
    """Processos de nulidade"""
    return FileResponse(frontend_path / "nulidade.html")

@router.get("/oposicao")
async def oposicao():
    """Processos de oposição"""
    return FileResponse(frontend_path / "oposicao.html")

@router.get("/recurso_indeferimento")
async def recurso_indeferimento():
    """Recursos de indeferimento"""
    return FileResponse(frontend_path / "recurso_indeferimento.html")

@router.get("/manifestacao_oposicao")
async def manifestacao_oposicao():
    """Manifestações de oposição"""
    return FileResponse(frontend_path / "manifestacao_oposicao.html")

@router.get("/manifestacao_recurso")
async def manifestacao_recurso():
    """Manifestações de recurso"""
    return FileResponse(frontend_path / "manifestacao_recurso.html")

@router.get("/contrarazao_nulidade")
async def contrarazao_nulidade():
    """Contrarrazões de nulidade"""
    return FileResponse(frontend_path / "contrarazao_nulidade.html")

@router.get("/cadastro")
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

# Redirecionamentos para compatibilidade com links antigos
from fastapi.responses import RedirectResponse

@router.get("/marca.html")
async def marca_html_redirect():
    """Redirecionamento de marca.html para marca (compatibilidade)"""
    return RedirectResponse(url="/marca", status_code=301)

@router.get("/menu.html")
async def menu_html_redirect():
    """Redirecionamento de menu.html para menu (compatibilidade)"""
    return RedirectResponse(url="/menu", status_code=301)

@router.get("/oraculo.html")
async def oraculo_html_redirect():
    """Redirecionamento de oraculo.html para oraculo (compatibilidade)"""
    return RedirectResponse(url="/oraculo", status_code=301)

@router.get("/perfil.html")
async def perfil_html_redirect():
    """Redirecionamento de perfil.html para perfil (compatibilidade)"""
    return RedirectResponse(url="/perfil", status_code=301)

@router.get("/caducidade.html")
async def caducidade_html_redirect():
    """Redirecionamento de caducidade.html para caducidade (compatibilidade)"""
    return RedirectResponse(url="/caducidade", status_code=301)

@router.get("/nulidade.html")
async def nulidade_html_redirect():
    """Redirecionamento de nulidade.html para nulidade (compatibilidade)"""
    return RedirectResponse(url="/nulidade", status_code=301)

@router.get("/oposicao.html")
async def oposicao_html_redirect():
    """Redirecionamento de oposicao.html para oposicao (compatibilidade)"""
    return RedirectResponse(url="/oposicao", status_code=301)

@router.get("/recurso_indeferimento.html")
async def recurso_indeferimento_html_redirect():
    """Redirecionamento de recurso_indeferimento.html para recurso_indeferimento (compatibilidade)"""
    return RedirectResponse(url="/recurso_indeferimento", status_code=301)

@router.get("/manifestacao_oposicao.html")
async def manifestacao_oposicao_html_redirect():
    """Redirecionamento de manifestacao_oposicao.html para manifestacao_oposicao (compatibilidade)"""
    return RedirectResponse(url="/manifestacao_oposicao", status_code=301)

@router.get("/manifestacao_recurso.html")
async def manifestacao_recurso_html_redirect():
    """Redirecionamento de manifestacao_recurso.html para manifestacao_recurso (compatibilidade)"""
    return RedirectResponse(url="/manifestacao_recurso", status_code=301)

@router.get("/contrarazao_nulidade.html")
async def contrarazao_nulidade_html_redirect():
    """Redirecionamento de contrarazao_nulidade.html para contrarazao_nulidade (compatibilidade)"""
    return RedirectResponse(url="/contrarazao_nulidade", status_code=301)

@router.get("/cadastro.html")
async def cadastro_html_redirect():
    """Redirecionamento de cadastro.html para cadastro (compatibilidade)"""
    return RedirectResponse(url="/cadastro", status_code=301)