from fastapi import APIRouter
from .auth_routes import router as auth_router
from .brand_routes import router as brand_router
from .oracle_routes import router as oracle_router
from .user_routes import router as user_router

router = APIRouter()
router.include_router(auth_router, prefix="/auth")
router.include_router(brand_router, prefix="/brand")
router.include_router(oracle_router, prefix="/oracle")
router.include_router(user_router, prefix="/user")
