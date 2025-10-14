# 🌐 CONFIGURAÇÃO SERVIDOR - www.leaoia.com.br

## ✅ CONFIGURAÇÕES IMPLEMENTADAS

### 1. 🔧 **Detecção Automática de Ambiente**

```javascript
// frontend/assets/js/config.js
window.APP_CONFIG = {
  API_URL:
    window.location.hostname === "localhost" ||
    window.location.hostname === "127.0.0.1"
      ? "http://localhost:8000" // 🔧 Desenvolvimento
      : "https://www.leaoia.com.br/api", // 🌐 Produção
};
```

### 2. 🛡️ **CORS Configurado**

```python
# backend/app/main.py
allow_origins=[
    "http://localhost:3000",        # Desenvolvimento frontend
    "http://127.0.0.1:3000",        # Desenvolvimento local
    "https://www.leaoia.com.br",    # Produção HTTPS
    "http://www.leaoia.com.br",     # Produção HTTP (fallback)
]
```

### 3. 🏗️ **Build Automatizado**

- **Windows:** `build.bat`
- **Linux/Mac:** `python build_production.py`
- **Output:** Pasta `dist/` pronta para deploy

### 4. 🚀 **URLs de Produção**

- **Frontend:** https://www.leaoia.com.br
- **API:** https://www.leaoia.com.br/api
- **Health Check:** https://www.leaoia.com.br/api/

## 🎯 COMO FAZER O DEPLOY

### Opção 1: Build Automático

```cmd
# No Windows
build.bat

# No Linux/Mac
python build_production.py
```

### Opção 2: Manual

```bash
# 1. Configurar frontend
# Arquivo: frontend/assets/js/config.js já está configurado!

# 2. Configurar backend CORS (já feito!)

# 3. Subir arquivos para servidor
# Frontend: /var/www/html/
# Backend: /var/www/api/
```

## 🌐 CONFIGURAÇÃO DO NGINX

```nginx
server {
    listen 443 ssl;
    server_name www.leaoia.com.br;

    # Frontend
    location / {
        root /var/www/html;
        try_files $uri $uri/ /index.html;
    }

    # API Backend
    location /api/ {
        proxy_pass http://127.0.0.1:8000/;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

## 📋 CHECKLIST DE DEPLOY

- [x] ✅ **Configuração automática de ambiente**
- [x] ✅ **CORS configurado para www.leaoia.com.br**
- [x] ✅ **Build script criado**
- [x] ✅ **Guia de deploy completo**
- [ ] 🔲 **Certificado SSL configurado**
- [ ] 🔲 **Banco de dados PostgreSQL configurado**
- [ ] 🔲 **Variáveis de ambiente (.env) configuradas**
- [ ] 🔲 **Nginx configurado**

## 🚨 PRÓXIMOS PASSOS

1. **Execute o build:** `build.bat`
2. **Envie pasta `dist/` para servidor**
3. **Configure Nginx** (use exemplo acima)
4. **Configure SSL** com Let's Encrypt
5. **Configure .env** com DATABASE_URL e OPENAI_API_KEY
6. **Inicie o backend:** `./start_server.sh`

## 🎉 RESULTADO FINAL

Seu sistema Leão Adv estará disponível em:

- **🌐 Site:** https://www.leaoia.com.br
- **⚡ API:** https://www.leaoia.com.br/api
- **👥 Suporte:** 10+ usuários simultâneos
- **🔒 Rate Limiting:** 5 petições/min por usuário
- **🚀 Performance:** 4 workers paralelos
