from fastapi import APIRouter
from .auth_routes import router as auth_router
from .brand_routes import router as brand_router
from .oracle_routes import router as oracle_router
from .user_routes import router as user_router
from .frontend_routes import router as frontend_router
from .caducidade_routes import router as caducidade_router
from .nulidade_routes import router as nulidade_router
from .oposicao_routes import router as oposicao_router
from .recurso_indeferimento_routes import router as recurso_router
from .manifestacao_oposicao_routes import router as manifestacao_oposicao_router
from .manifestacao_recurso_routes import router as manifestacao_recurso_router
from .contrarazao_nulidade_routes import router as contrarazao_router
from .peticao_caducidade_routes import router as peticao_caducidade_router
from ..controllers.peticao_controller import router as peticao_controller_router

router = APIRouter()

# Core System Routes (diretas, sem versionamento)
router.include_router(auth_router, prefix="/auth", tags=["Authentication"])
router.include_router(user_router, prefix="/usuarios", tags=["Users"])
router.include_router(brand_router, prefix="/brands", tags=["Brands"])
router.include_router(oracle_router, prefix="/oracle", tags=["AI Oracle"])

# Petições Routes (controller unificado)
router.include_router(peticao_controller_router, prefix="/peticoes", tags=["Petições"])

# Petições Específicas (rotas individuais)
router.include_router(caducidade_router, prefix="/caducidades", tags=["Caducidade"])
router.include_router(nulidade_router, prefix="/nulidades", tags=["Nulidade"])
router.include_router(oposicao_router, prefix="/oposicoes", tags=["Oposição"])
router.include_router(recurso_router, prefix="/recursos-indeferimento", tags=["Recursos"])
router.include_router(manifestacao_oposicao_router, prefix="/manifestacoes-oposicao", tags=["Manifestações Oposição"])
router.include_router(manifestacao_recurso_router, prefix="/manifestacoes-recurso", tags=["Manifestações Recurso"])
router.include_router(contrarazao_router, prefix="/contrarazoes-nulidade", tags=["Contrarrazões"])
router.include_router(peticao_caducidade_router, prefix="/peticoes-caducidade", tags=["Petições Caducidade"])

# Frontend Routes (páginas)
router.include_router(frontend_router, tags=["Frontend"])