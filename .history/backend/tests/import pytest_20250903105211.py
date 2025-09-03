import pytest
from fastapi.testclient import TestClient
from app.api.auth_routes import router
from fastapi import FastAPI, status

app = FastAPI()
app.include_router(router)

@pytest.fixture
def client():
    return TestClient(app)

def test_register_success(mocker, client):
    mock_user_service = mocker.patch("app.services.user_service.UserService")
    instance = mock_user_service.return_value
    instance.get_user_by_email.return_value = None
    instance.create_user.return_value = {"id": 1, "email": "test@example.com", "username": "testuser"}

    response = client.post("/register", json={
        "email": "test@example.com",
        "username": "testuser",
        "password": "123456"
    })
    assert response.status_code == status.HTTP_200_OK
    assert response.json()["email"] == "test@example.com"

def test_register_existing_user(mocker, client):
    mock_user_service = mocker.patch("app.services.user_service.UserService")
    instance = mock_user_service.return_value
    instance.get_user_by_email.return_value = {"id": 1, "email": "test@example.com"}

    response = client.post("/register", json={
        "email": "test@example.com",
        "username": "testuser",
        "password": "123456"
    })
    assert response.status_code == status.HTTP_400_BAD_REQUEST
    assert response.json()["detail"] == "Email already registered"

def test_login_success(mocker, client):
    mock_user_service = mocker.patch("app.services.user_service.UserService")
    instance = mock_user_service.return_value
    instance.authenticate_user.return_value = type("User", (), {"email": "test@example.com"})
    mocker.patch("app.utils.auth.create_access_token", return_value="token123")

    response = client.post("/login", json={
        "email": "test@example.com",
        "username": "testuser",
        "password": "123456"
    })
    assert response.status_code == status.HTTP_200_OK
    assert response.json()["access_token"] == "token123"

def test_login_invalid_credentials(mocker, client):
    mock_user_service = mocker.patch("app.services.user_service.UserService")
    instance = mock_user_service.return_value
    instance.authenticate_user.return_value = None

    response = client.post("/login", json={
        "email": "wrong@example.com",
        "username": "testuser",
        "password": "wrongpass"
    })
    assert response.status_code == status.HTTP_401_UNAUTHORIZED
    assert response.json()["detail"] == "Invalid credentials"