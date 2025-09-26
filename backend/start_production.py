#!/usr/bin/env python3
"""
Script de inicialização para produção com múltiplos workers
Otimizado para 10+ usuários simultâneos
"""

import subprocess
import sys
import os

def start_production_server():
    """
    Inicia o servidor FastAPI com configurações otimizadas para produção
    """
    
    # Configurações otimizadas para 10+ usuários simultâneos
    workers = 4  # 4 workers para handling concurrent requests
    host = "0.0.0.0"
    port = 8000
    
    # Comando uvicorn otimizado
    cmd = [
        "uvicorn",
        "app.main:app",
        f"--host={host}",
        f"--port={port}",
        f"--workers={workers}",
        "--worker-class=uvicorn.workers.UvicornWorker",
        "--timeout-keep-alive=30",
        "--access-log",
        "--reload=False"  # Desabilitar reload em produção
    ]
    
    print("🚀 Iniciando servidor em modo PRODUÇÃO...")
    print(f"📊 Workers: {workers}")
    print(f"🌐 Host: {host}:{port}")
    print(f"👥 Otimizado para: 10+ usuários simultâneos")
    print(f"⚡ Rate Limiting: 5 petições/min por usuário")
    print(f"🗄️ Connection Pool: 20 conexões ativas + 30 overflow")
    print("-" * 50)
    
    try:
        subprocess.run(cmd, check=True)
    except KeyboardInterrupt:
        print("\n🛑 Servidor interrompido pelo usuário")
    except subprocess.CalledProcessError as e:
        print(f"❌ Erro ao iniciar servidor: {e}")
        sys.exit(1)

def start_development_server():
    """
    Inicia servidor em modo desenvolvimento (single worker)
    """
    cmd = [
        "uvicorn",
        "app.main:app",
        "--host=127.0.0.1",
        "--port=8000",
        "--reload",
        "--access-log"
    ]
    
    print("🔧 Iniciando servidor em modo DESENVOLVIMENTO...")
    print("🌐 Host: 127.0.0.1:8000")
    print("🔄 Hot reload: ATIVADO")
    print("-" * 50)
    
    try:
        subprocess.run(cmd, check=True)
    except KeyboardInterrupt:
        print("\n🛑 Servidor interrompido pelo usuário")

if __name__ == "__main__":
    # Verifica argumentos da linha de comando
    if len(sys.argv) > 1 and sys.argv[1] == "--dev":
        start_development_server()
    else:
        start_production_server()