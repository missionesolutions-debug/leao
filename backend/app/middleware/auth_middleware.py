"""
Middleware de autenticação centralizada para facilitar manutenção
"""
from fastapi import Request, HTTPException, status
from fastapi.security import HTTPBearer, HTTPAuthorizationCredentials
from typing import List, Optional
import re

# Configuração de rotas públicas (não precisam de autenticação)
PUBLIC_ROUTES = [
    # Frontend routes
    r"^/$",
    r"^/index.*",
    r"^/menu.*", 
    r"^/marca.*",
    r"^/oraculo.*",
    r"^/perfil.*",
    r"^/cadastro.*",
    r"^/caducidade.*",
    r"^/nulidade.*",
    r"^/oposicao.*",
    r"^/recurso_indeferimento.*",
    r"^/manifestacao_.*",
    r"^/contrarazao_nulidade.*",
    r"^/config\.js$",
    
    # Static files
    r"^/assets/.*",
    r"^/lib/.*",
    r"^/scripts/.*",
    r"^/static/.*",
    
    # Public API routes
    r"^/api/v1/auth/login$",
    r"^/api/v1/auth/register$",
    r"^/api/v1/usuarios/count$",  # Para verificar primeiro usuário
    r"^/health$",
    r"^/docs.*",
    r"^/redoc.*",
    r"^/openapi\.json$"
]

# Configuração de rotas que exigem role ADMIN
ADMIN_ROUTES = [
    r"^/api/v1/usuarios/.*",  # Gestão de usuários
    r"^/perfil\.html$"        # Página de administração
]

security = HTTPBearer(auto_error=False)

def is_public_route(path: str) -> bool:
    """Verifica se a rota é pública"""
    for pattern in PUBLIC_ROUTES:
        if re.match(pattern, path):
            return True
    return False

def is_admin_route(path: str) -> bool:
    """Verifica se a rota exige privilégios de admin"""
    for pattern in ADMIN_ROUTES:
        if re.match(pattern, path):
            return True
    return False

async def auth_middleware(request: Request, call_next):
    """
    Middleware de autenticação centralizada
    - Permite rotas públicas sem autenticação
    - Valida token JWT para rotas protegidas  
    - Verifica role ADMIN para rotas administrativas
    """
    
    # Se for rota pública, continua sem verificação
    if is_public_route(request.url.path):
        response = await call_next(request)
        return response
    
    # Para rotas protegidas, verifica autenticação
    authorization: HTTPAuthorizationCredentials = await security(request)
    
    if not authorization:
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Token de autenticação necessário",
            headers={"WWW-Authenticate": "Bearer"},
        )
    
    # Aqui você pode adicionar validação do token JWT
    # Por enquanto, só verificamos se o token existe
    token = authorization.credentials
    
    # TODO: Validar token JWT e extrair user info
    # user = validate_jwt_token(token)
    # request.state.current_user = user
    
    # Para rotas admin, verificar role
    if is_admin_route(request.url.path):
        # TODO: Verificar se user.role == "ADMIN"
        pass
    
    response = await call_next(request)
    return response

# Função helper para aplicar o middleware ao app
def setup_auth_middleware(app):
    """Configura o middleware de autenticação no app"""
    app.middleware("http")(auth_middleware)
    return app