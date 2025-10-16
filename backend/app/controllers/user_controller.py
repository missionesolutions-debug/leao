from fastapi import APIRouter, Depends, HTTPException
from sqlalchemy.orm import Session
from app.services.user_service import UserService
from app.repositories.user_repository import UserRepository
from app.schemas.user_schema import UserCreate, UserUpdate, UserResponse
from app.config.database import get_db
from app.utils.auth import require_admin_role, require_user_access, get_current_user_from_db
from app.models.user_model import User

router = APIRouter()

@router.get("/usuarios/me")
def get_current_user_profile(current_user: User = Depends(get_current_user_from_db)):
    """Retorna o perfil do usuário atual"""
    return {
        "id": current_user.id,
        "username": current_user.username,
        "email": current_user.email,
        "role": current_user.role.value if hasattr(current_user.role, 'value') else current_user.role
    }

@router.get("/usuarios/count")
def count_users(db: Session = Depends(get_db)):
    """Contar total de usuários (público para verificar primeiro usuário)"""
    count = db.query(User).count()
    return {"count": count}

@router.get("/usuarios/")
def list_users(db: Session = Depends(get_db), current_user: User = Depends(get_current_user_from_db)):
    """
    Lista usuários baseado no perfil:
    - Admin: vê todos os usuários
    - Usuário comum: vê apenas seus próprios dados
    """
    user_service = UserService(db)
    
    # Normalizar role para comparação
    user_role = current_user.role.value if hasattr(current_user.role, 'value') else str(current_user.role)
    
    if user_role == "admin":
        # Admin vê todos os usuários
        users = user_service.get_all_users()
        return [
            {
                "id": user.id,
                "username": user.username,
                "email": user.email,
                "role": user.role.value if hasattr(user.role, 'value') else str(user.role)
            } for user in users
        ]
    else:
        # Usuário comum vê apenas seus próprios dados
        return [
            {
                "id": current_user.id,
                "username": current_user.username,
                "email": current_user.email,
                "role": user_role
            }
        ]

@router.post("/usuarios/", response_model=UserResponse)
def create_user(user: UserCreate, db: Session = Depends(get_db), current_user: User = Depends(require_admin_role)):
    """Apenas admins podem criar novos usuários"""
    user_service = UserService(db)
    return user_service.create_user(user)

@router.get("/usuarios/{user_id}", response_model=UserResponse)
def get_user(user_id: int, db: Session = Depends(get_db), current_user: User = Depends(require_user_access)):
    """Admins podem ver qualquer usuário, usuários comuns apenas seus próprios dados"""
    user_service = UserService(db)
    user = user_service.get_user_by_id(user_id)
    return user

@router.put("/usuarios/{user_id}", response_model=UserResponse)
def update_user(user_id: int, user_update: UserUpdate, db: Session = Depends(get_db), current_user: User = Depends(get_current_user_from_db)):
    """
    Atualiza usuário com controles de acesso:
    - Admins podem atualizar qualquer usuário
    - Usuários comuns só podem atualizar seus próprios dados
    - Usuários comuns não podem alterar o próprio role
    """
    
    # Normalizar role para verificação
    user_role = current_user.role.value if hasattr(current_user.role, 'value') else str(current_user.role)
    
    # Verificar permissões
    if user_role != "admin" and current_user.id != user_id:
        raise HTTPException(
            status_code=403,
            detail="Acesso negado. Você só pode editar seus próprios dados."
        )
    
    # Usuários comuns não podem alterar o role
    if user_role != "admin" and user_update.role is not None:
        raise HTTPException(
            status_code=403,
            detail="Apenas administradores podem alterar perfis de usuário."
        )
    
    user_service = UserService(db)
    user = user_service.update_user(user_id, user_update)
    return user

@router.delete("/usuarios/{user_id}")
def delete_user(user_id: int, db: Session = Depends(get_db), current_user: User = Depends(require_admin_role)):
    """Apenas admins podem deletar usuários"""
    
    # Evitar que admin delete a si mesmo
    if current_user.id == user_id:
        raise HTTPException(
            status_code=400,
            detail="Você não pode deletar sua própria conta."
        )
    
    user_service = UserService(db)
    result = user_service.delete_user(user_id)
    return {"detail": "Usuário deletado com sucesso"}