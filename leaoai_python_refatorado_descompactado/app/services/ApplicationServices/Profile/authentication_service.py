
import os
import sys

# Change directory to the project root to ensure 'app' is in the path for imports
# This is for notebook execution environment. In a standard application,
# the project structure and PYTHONPATH would be handled differently.
project_root = '/content/leaoai_python_refatorado_descompactado'
os.chdir(project_root)
# Add the project root to sys.path just in case
if project_root not in sys.path:
    sys.path.insert(0, project_root)


import jwt
from datetime import datetime, timedelta, timezone
from typing import Optional

# Assuming SQLAlchemy model is in app/models/Pessoa/usuario.py
from app.models.Pessoa.usuario import Usuario as DBUsuario
# Assuming Pydantic schemas are in app/schemas/Auth/authenticate.py
from app.schemas.Auth.authenticate import LoginRequest
from app.schemas.Auth.authenticate import AuthenticateResponse
# Assuming settings is in app.core.config
from app.core.config import settings


from sqlalchemy.orm import Session

class AuthenticationService:
    def __init__(self, db: Session):
        self.db = db

    def authenticate_user(self, request: LoginRequest) -> Optional[AuthenticateResponse]:
        # Authenticate the user – adjust your logic as needed
        # This is a simplified example; proper password hashing should be used
        user = self.db.query(DBUsuario).filter(
            DBUsuario.Email == request.Email,
            DBUsuario.Password == request.Password # In a real app, compare hashed passwords
        ).first()

        if user is None:
            return None # Authentication failed

        # Generate JWT token
        # Assuming SECRET_KEY is added to settings in app.core.config
        try:
            SECRET_KEY = settings.SECRET_KEY.encode('utf-8') # Ensure SECRET_KEY is bytes
        except AttributeError:
            print("Error: SECRET_KEY not found in settings. Add SECRET_KEY to your .env file and app/core/config.py")
            return None # Cannot generate token without key


        ALGORITHM = "HS256"
        ACCESS_TOKEN_EXPIRE_MINUTES = 120 # 2 hours expiry

        to_encode = {"sub": user.Email}
        # Add claims from C# code
        to_encode.update({"UsuarioId": user.Id, "Role": user.RoleGate})

        expire = datetime.now(timezone.utc) + timedelta(minutes=ACCESS_TOKEN_EXPIRE_MINUTES)
        to_encode.update({"exp": expire})

        encoded_jwt = jwt.encode(to_encode, SECRET_KEY, algorithm=ALGORITHM)

        return AuthenticateResponse(Token=encoded_jwt)