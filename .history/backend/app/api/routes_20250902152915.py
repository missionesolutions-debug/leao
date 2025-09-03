from fastapi import APIRouter
from src.controllers.user_controller import UserController
from src.controllers.oracle_controller import OracleController

router = APIRouter()

user_controller = UserController()
oracle_controller = OracleController()

# User routes
router.post("/users/", response_model=user_controller.create_user)
router.get("/users/{user_id}", response_model=user_controller.get_user)

# Oracle routes
router.post("/oracle/query", response_model=oracle_controller.query_openai)