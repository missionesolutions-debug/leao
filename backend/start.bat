@echo off
echo 🚀 Iniciando servidor LeaoPy em modo PRODUCAO...
echo 📊 Configuracoes otimizadas para 10+ usuarios simultaneos
echo ⚡ Rate Limiting: 5 peticoes/min por usuario
echo 🗄️ Connection Pool: 20 conexoes ativas + 30 overflow
echo 👥 Workers: 4 processos paralelos
echo ---------------------------------------------------------

cd /d "%~dp0"
uvicorn app.main:app --host 0.0.0.0 --port 8000 --workers 4 --worker-class uvicorn.workers.UvicornWorker --timeout-keep-alive 30 --access-log

pause