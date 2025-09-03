from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from api.routes import router as main_router
from api.auth_routes import router as auth_router
from config.settings import settings

app = FastAPI()

# CORS middleware
app.add_middleware(
    CORSMiddleware,
    allow_origins=settings.ALLOW_ORIGINS,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Include routers
app.include_router(main_router)
app.include_router(auth_router)

@app.get("/")
def read_root():
    return {"message": "Welcome to the Leão Adv API!"}