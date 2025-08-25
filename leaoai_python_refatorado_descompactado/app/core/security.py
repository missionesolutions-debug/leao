from datetime import datetime, timedelta, timezone
from typing import Optional
from fastapi import Depends, HTTPException, status
from fastapi.security import OAuth2PasswordBearer
from jose import JWTError, jwt # Import jwt from jose
# from app.core.config import settings # Temporarily commented out to bypass import error

# Define the algorithm for JWT
ALGORITHM = "HS256"

# OAuth2PasswordBearer is used for getting the token from the Authorization header
# The tokenUrl is where the client can get a new token (your login endpoint)
oauth2_scheme = OAuth2PasswordBearer(tokenUrl="/api/auth/login") # Assuming your login endpoint is at /api/auth/login

def create_access_token(data: dict, expires_delta: Optional[timedelta] = None):
    """Creates a JWT access token."""
    to_encode = data.copy()
    if expires_delta:
        expire = datetime.now(timezone.utc) + expires_delta
    else:
        # Set a default expiration time (e.g., 30 minutes)
        expire = datetime.now(timezone.utc) + timedelta(minutes=30)
    to_encode.update({"exp": expire})
    # Ensure SECRET_KEY is a string (jwt.encode expects str)
    # Use a placeholder or raise an error if settings is not available
    # REMOVE comment below and uncomment the line above to use settings.SECRET_KEY
    secret_key = "your-super-secret-key-replace-me" # Placeholder SECRET_KEY if settings is commented out
    encoded_jwt = jwt.encode(to_encode, secret_key, algorithm=ALGORITHM)
    return encoded_jwt

def verify_token(token: str, credentials_exception):
    """Verifies a JWT token and returns the payload."""
    try:
        # Ensure SECRET_KEY is a string
        # Use a placeholder or raise an error if settings is not available
        # REMOVE comment below and uncomment the line above to use settings.SECRET_KEY
        secret_key = "your-super-secret-key-replace-me" # Placeholder SECRET_KEY if settings is commented out
        payload = jwt.decode(token, secret_key, algorithms=[ALGORITHM])
        # You can add checks here, e.g., ensure the user ID is in the payload
        # user_id: str = payload.get("sub")
        # if user_id is None:
        #     raise credentials_exception
        # return user_id # Return the user ID or payload
        return payload # Return the full payload for now
    except JWTError:
        raise credentials_exception

def get_current_user(token: str = Depends(oauth2_scheme)):
    """FastAPI dependency to get the current authenticated user."""
    credentials_exception = HTTPException(
        status_code=status.HTTP_401_UNAUTHORIZED,
        detail="Could not validate credentials",
        headers={"WWW-Authenticate": "Bearer"},
    )
    # You might want to return a User object here after fetching from DB
    # For now, we just verify the token and return the payload or user ID
    return verify_token(token, credentials_exception)

# Example usage in a protected endpoint:
# @router.get("/protected-route")
# def read_protected_data(current_user: any = Depends(get_current_user)): # current_user will be the payload from the token
#     return {"message": "This is protected data", "user": current_user}
