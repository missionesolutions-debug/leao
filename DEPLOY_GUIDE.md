# 🌐 CONFIGURAÇÃO PARA SERVIDOR PRODUÇÃO - www.leaoai.com.br

## 📋 CHECKLIST DE DEPLOY

### 1. 🔧 Configurações do Servidor

**Nginx Configuration (nginx.conf ou sites-available):**

```nginx
server {
    listen 80;
    server_name www.leaoai.com.br leaoai.com.br;

    # Redirect to HTTPS
    return 301 https://$server_name$request_uri;
}

server {
    listen 443 ssl http2;
    server_name www.leaoai.com.br leaoai.com.br;

    # SSL Certificate
    ssl_certificate /path/to/ssl/certificate.crt;
    ssl_certificate_key /path/to/ssl/private.key;

    # Frontend - Servir arquivos estáticos
    location / {
        root /path/to/LeaoPy/frontend;
        index index.html;
        try_files $uri $uri/ /index.html;
    }

    # Backend API - Proxy para FastAPI
    location /api/ {
        proxy_pass http://127.0.0.1:8000/;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;

        # CORS headers para API
        add_header Access-Control-Allow-Origin "https://www.leaoai.com.br" always;
        add_header Access-Control-Allow-Methods "GET, POST, PUT, DELETE, OPTIONS" always;
        add_header Access-Control-Allow-Headers "Authorization, Content-Type" always;

        # Handle preflight requests
        if ($request_method = 'OPTIONS') {
            add_header Access-Control-Allow-Origin "https://www.leaoai.com.br";
            add_header Access-Control-Allow-Methods "GET, POST, PUT, DELETE, OPTIONS";
            add_header Access-Control-Allow-Headers "Authorization, Content-Type";
            add_header Content-Length 0;
            add_header Content-Type text/plain;
            return 204;
        }
    }
}
```

### 2. 🐳 SystemD Service (Opcional)

**Arquivo: `/etc/systemd/system/leaoai.service`**

```ini
[Unit]
Description=LeaoAI FastAPI Application
After=network.target

[Service]
Type=exec
User=www-data
Group=www-data
WorkingDirectory=/path/to/LeaoPy/backend
Environment=PATH=/path/to/LeaoPy/backend/venv/bin
ExecStart=/path/to/LeaoPy/backend/venv/bin/python start_production.py
Restart=always
RestartSec=3

[Install]
WantedBy=multi-user.target
```

### 3. 🔐 Variáveis de Ambiente

**Arquivo: `.env` (backend/app/.env)**

```env
DATABASE_URL=postgresql://user:password@localhost:5432/leaoai_prod
OPENAI_API_KEY=sk-your-openai-api-key-here
SECRET_KEY=your-super-secret-jwt-key-here
DEBUG=False
```

### 4. 📦 Deploy Steps

```bash
# 1. Clone o repositório no servidor
git clone https://github.com/your-repo/LeaoPy.git
cd LeaoPy

# 2. Configurar backend
cd backend
python -m venv venv
source venv/bin/activate  # Linux/Mac
pip install -r requirements.txt

# 3. Configurar banco de dados
createdb leaoai_prod
python -c "from app.config.database import engine, Base; Base.metadata.create_all(bind=engine)"

# 4. Testar aplicação
python start_production.py

# 5. Configurar Nginx (usando os configs acima)
sudo nginx -t
sudo systemctl reload nginx

# 6. Configurar SSL (Let's Encrypt recomendado)
sudo certbot --nginx -d www.leaoai.com.br
```

### 5. 🔍 URLs de Teste

**Frontend:** https://www.leaoai.com.br
**API:** https://www.leaoai.com.br/api
**Health Check:** https://www.leaoai.com.br/api/

### 6. 📊 Monitoramento

```bash
# Verificar logs da aplicação
journalctl -u leaoai.service -f

# Verificar status do serviço
systemctl status leaoai.service

# Verificar logs do Nginx
tail -f /var/log/nginx/access.log
tail -f /var/log/nginx/error.log

# Verificar processos Python
ps aux | grep python
```

### 7. ⚙️ Otimizações de Produção

- ✅ **Rate Limiting:** 5 petições/min por usuário
- ✅ **Connection Pool:** 20 conexões + 30 overflow
- ✅ **Multiple Workers:** 4 processos paralelos
- ✅ **Retry Logic:** 3 tentativas automáticas
- ✅ **CORS Configurado:** Para www.leaoai.com.br
- ✅ **Config Automática:** Detecta ambiente automaticamente

### 8. 🚨 Troubleshooting

**Se o frontend não carrega:**

- Verificar se Nginx está servindo os arquivos estáticos
- Verificar permissões dos arquivos (755 para diretórios, 644 para arquivos)

**Se a API não responde:**

- Verificar se FastAPI está rodando na porta 8000
- Verificar logs: `journalctl -u leaoai.service -f`
- Testar diretamente: `curl http://127.0.0.1:8000/`

**Se há erros de CORS:**

- Verificar configuração do Nginx
- Verificar se domínio está correto em `settings.py`

**Se petições falham:**

- Verificar se OpenAI API key está configurada
- Verificar se banco de dados está acessível
- Verificar logs de rate limiting
