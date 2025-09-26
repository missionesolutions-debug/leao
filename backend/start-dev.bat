@echo off
echo 🔧 Iniciando servidor LeaoPy em modo DESENVOLVIMENTO...
echo 🌐 Host: 127.0.0.1:8000
echo 🔄 Hot reload: ATIVADO
echo ---------------------------------------------------------

cd /d "%~dp0"
uvicorn app.main:app --host 127.0.0.1 --port 8000 --reload --access-log

pause