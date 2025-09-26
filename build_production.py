#!/usr/bin/env python3
"""
Script para preparar o projeto para deploy em produção
"""

import os
import shutil
import subprocess
import json
from pathlib import Path

def create_production_build():
    """
    Cria uma build otimizada para produção
    """
    print("🚀 Preparando build para produção...")
    
    # Diretório base
    base_dir = Path(__file__).parent
    frontend_dir = base_dir / "frontend"
    backend_dir = base_dir / "backend"
    build_dir = base_dir / "dist"
    
    # Limpar build anterior
    if build_dir.exists():
        shutil.rmtree(build_dir)
    
    build_dir.mkdir()
    
    print("📁 Copiando arquivos frontend...")
    
    # Copiar frontend
    frontend_build = build_dir / "frontend"
    shutil.copytree(frontend_dir, frontend_build, ignore=shutil.ignore_patterns(
        '*.pyc', '__pycache__', '.git', '.gitignore', 'node_modules'
    ))
    
    print("📁 Copiando arquivos backend...")
    
    # Copiar backend
    backend_build = build_dir / "backend"
    shutil.copytree(backend_dir, backend_build, ignore=shutil.ignore_patterns(
        '*.pyc', '__pycache__', '.git', '.gitignore', 'venv', '.env'
    ))
    
    # Criar arquivo de configuração de produção
    print("⚙️ Criando configurações de produção...")
    
    # Config para frontend
    config_js = frontend_build / "assets" / "js" / "config.js"
    with open(config_js, 'w', encoding='utf-8') as f:
        f.write("""// Configuração de PRODUÇÃO - www.leaoai.com.br
window.APP_CONFIG = {
  API_URL: "https://www.leaoai.com.br/api"
};
""")
    
    # Criar .env template para backend
    env_template = backend_build / ".env.template"
    with open(env_template, 'w', encoding='utf-8') as f:
        f.write("""# Configurações de Produção - LeaoAI
DATABASE_URL=postgresql://user:password@localhost:5432/leaoai_prod
OPENAI_API_KEY=sk-your-openai-api-key-here
SECRET_KEY=your-super-secret-jwt-key-minimum-32-characters-long
DEBUG=False
""")
    
    # Criar script de start para produção
    start_script = build_dir / "start_server.sh"
    with open(start_script, 'w', encoding='utf-8') as f:
        f.write("""#!/bin/bash
# Script para iniciar LeaoAI em produção

echo "🚀 Iniciando LeaoAI em modo PRODUÇÃO..."
echo "🌐 Domínio: www.leaoai.com.br"
echo "⚡ Workers: 4 processos paralelos"
echo "🗄️ Connection Pool: Otimizado para 10+ usuários"
echo "----------------------------------------------------"

cd backend

# Verificar se .env existe
if [ ! -f "app/.env" ]; then
    echo "❌ Arquivo .env não encontrado!"
    echo "📝 Copie .env.template para app/.env e configure as variáveis"
    exit 1
fi

# Instalar dependências se necessário
if [ ! -d "venv" ]; then
    echo "📦 Criando ambiente virtual..."
    python3 -m venv venv
    source venv/bin/activate
    pip install -r requirements.txt
else
    source venv/bin/activate
fi

# Iniciar servidor
echo "🌟 Iniciando FastAPI..."
uvicorn app.main:app \\
    --host 0.0.0.0 \\
    --port 8000 \\
    --workers 4 \\
    --worker-class uvicorn.workers.UvicornWorker \\
    --timeout-keep-alive 30 \\
    --access-log \\
    --no-reload
""")
    
    # Tornar executável
    os.chmod(start_script, 0o755)
    
    # Criar arquivo README para deploy
    readme_deploy = build_dir / "README_DEPLOY.md"
    with open(readme_deploy, 'w', encoding='utf-8') as f:
        f.write("""# 🚀 LeaoAI - Deploy em Produção

## 📦 Este é o build de produção do LeaoAI

### 🎯 Configurado para:
- **Domínio:** www.leaoai.com.br
- **API Endpoint:** https://www.leaoai.com.br/api
- **Workers:** 4 processos paralelos
- **Rate Limiting:** 5 petições/min por usuário
- **Connection Pool:** 20 + 30 overflow

### 🛠️ Como fazer deploy:

1. **Copie os arquivos para o servidor:**
   ```bash
   scp -r dist/* user@server:/var/www/leaoai/
   ```

2. **Configure o backend:**
   ```bash
   cd /var/www/leaoai/backend
   cp .env.template app/.env
   nano app/.env  # Configure DATABASE_URL e OPENAI_API_KEY
   ```

3. **Configure o Nginx:**
   - Frontend: `/var/www/leaoai/frontend`
   - API Proxy: `http://127.0.0.1:8000`

4. **Inicie o servidor:**
   ```bash
   ./start_server.sh
   ```

### 🔗 URLs de teste:
- **Frontend:** https://www.leaoai.com.br
- **API:** https://www.leaoai.com.br/api
- **Health:** https://www.leaoai.com.br/api/

### 📞 Suporte:
Consulte o arquivo DEPLOY_GUIDE.md para instruções detalhadas.
""")
    
    print("✅ Build de produção criada com sucesso!")
    print(f"📁 Localização: {build_dir.absolute()}")
    print(f"🌐 Configurado para: www.leaoai.com.br")
    print(f"📝 Leia o README_DEPLOY.md para instruções de deploy")

if __name__ == "__main__":
    create_production_build()