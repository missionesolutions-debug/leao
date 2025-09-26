from fastapi import APIRouter
from .auth_routes import router as auth_router
from .brand_routes import router as brand_router
from .oracle_routes import router as oracle_router
# from .user_routes import router as user_router  # Removido - usando controller direto
from .caducidade_routes import router as caducidade_router 
from .contrarazao_nulidade_routes import router as contrarazao_router
from .nulidade_routes import router as nulidade_router
from .recurso_indeferimento_routes import router as recurso_indeferimento_router
from .manifestacao_oposicao_routes import router as manifestacao_oposicao_router
from .manifestacao_recurso_routes import router as manifestacao_recurso_router
from .oposicao_routes import router as oposicao_router
from app.controllers.peticao_controller import router as peticao_controller_router


router = APIRouter()
router.include_router(auth_router, prefix="/auth")
router.include_router(brand_router, prefix="/brand")
router.include_router(oracle_router, prefix="/oracle")
# router.include_router(user_router, prefix="/usuarios")  # Removido - usando controller direto

# Rotas individuais para compatibilidade com frontend
router.include_router(caducidade_router, prefix="/caducidade")
router.include_router(contrarazao_router, prefix="/contrarazao_nulidade")
router.include_router(recurso_indeferimento_router, prefix="/recurso_indeferimento")
router.include_router(nulidade_router, prefix="/nulidade")
router.include_router(manifestacao_oposicao_router, prefix="/manifestacao_oposicao")
router.include_router(manifestacao_recurso_router, prefix="/manifestacao_recurso")
router.include_router(oposicao_router, prefix="/oposicao")

# Controller de petições com IA (novas funcionalidades)
router.include_router(peticao_controller_router, prefix="/peticoes")