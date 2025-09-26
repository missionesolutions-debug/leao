# 🌊 DEPLOY NO DIGITAL OCEAN - www.leaoia.com.br

## 🚀 CONFIGURAÇÃO COMPLETA PARA DIGITAL OCEAN + GITHUB

### 📋 OPÇÕES DE DEPLOY

#### **Opção 1: Digital Ocean App Platform (Recomendado)**

- ✅ **Deploy automático** via GitHub
- ✅ **SSL automático** (certificados gerenciados)
- ✅ **Scaling automático**
- ✅ **Monitoramento integrado**

#### **Opção 2: Droplet + GitHub Actions**

- ✅ **Controle total** do servidor
- ✅ **Mais barato** para long-term
- ✅ **Customização completa**

---

## 🎯 OPÇÃO 1: APP PLATFORM (MAIS FÁCIL)

### 1. **Configurar no GitHub**

```bash
# 1. Push seu código para GitHub
git add .
git commit -m "Deploy ready for Digital Ocean"
git push origin NewPython-only-front
```

### 2. **Criar App no Digital Ocean**

1. Acesse [Digital Ocean Apps](https://cloud.digitalocean.com/apps)
2. Clique em **"Create App"**
3. Selecione **GitHub** como source
4. Escolha o repositório: `missionesolutions-debug/leao`
5. Branch: `NewPython-only-front`
6. **Importe o arquivo de configuração:**

```yaml
# Copie o conteúdo de .do/app.yaml para a configuração
```

### 3. **Configurar Variáveis de Ambiente**

```env
DATABASE_URL=postgresql://username:password@host:port/database
OPENAI_API_KEY=sk-your-openai-key-here
SECRET_KEY=your-super-secret-jwt-key-here
DEBUG=False
```

### 4. **Configurar Domínio**

1. Na aba **"Settings"** da App
2. **"Domains"** → **"Add Domain"**
3. Adicione: `www.leaoai.com.br`
4. Configure DNS:
   - **CNAME**: `www` → `your-app.ondigitalocean.app`
   - **A Record**: `@` → IP do App Platform

---

## 🛠️ OPÇÃO 2: DROPLET + GITHUB ACTIONS

### 1. **Criar Droplet**

```bash
# Criar no Digital Ocean:
# - Ubuntu 22.04 LTS
# - 2GB RAM / 1 vCPU (mínimo)
# - Adicionar sua SSH key
```

### 2. **Configurar Secrets no GitHub**

Repository → Settings → Secrets and Variables → Actions:

```env
DO_HOST=your-server-ip
DO_USERNAME=root
DO_SSH_KEY=your-private-ssh-key-content
DO_PORT=22
```

### 3. **Preparar Servidor (SSH no droplet)**

```bash
# Conectar via SSH
ssh root@your-server-ip

# Atualizar sistema
apt update && apt upgrade -y

# Instalar dependências
apt install -y python3 python3-pip python3-venv nginx postgresql postgresql-contrib git certbot python3-certbot-nginx

# Configurar PostgreSQL
sudo -u postgres createuser --interactive --pwprompt leaoai
sudo -u postgres createdb -O leaoai leaoai_prod

# Configurar firewall
ufw allow OpenSSH
ufw allow 'Nginx Full'
ufw enable
```

### 4. **Push para GitHub (Deploy automático)**

```bash
git add .
git commit -m "Deploy to Digital Ocean"
git push origin NewPython-only-front
# GitHub Actions executará automaticamente!
```

---

## 🔧 CONFIGURAÇÕES ADICIONAIS

### **SSL com Let's Encrypt**

```bash
# No servidor (se usando Droplet)
certbot --nginx -d www.leaoai.com.br -d leaoai.com.br
```

### **Monitoramento**

```bash
# Verificar logs da aplicação
journalctl -u leaoai -f

# Verificar status dos serviços
systemctl status leaoai
systemctl status nginx
systemctl status postgresql
```

### **Backup Automático**

```bash
# Adicionar ao crontab
crontab -e

# Adicionar linha:
0 2 * * * pg_dump leaoai_prod > /var/backups/leaoai-$(date +\%Y\%m\%d).sql
```

---

## 🎯 RESULTADO FINAL

### **URLs de Produção:**

- **🌐 Site Principal:** https://www.leaoai.com.br
- **⚡ API Endpoint:** https://www.leaoai.com.br/api
- **🔍 Health Check:** https://www.leaoai.com.br/api/docs

### **Capacidade:**

- **👥 Usuários simultâneos:** 10+
- **⚡ Rate limiting:** 5 petições/min
- **🗄️ Connection pool:** 20 + 30 overflow
- **🚀 Workers:** 4 processos paralelos

---

## 📞 COMANDOS ÚTEIS

### **Deploy Manual (se necessário):**

```bash
# SSH no servidor
ssh root@your-server-ip

# Atualizar código
cd /var/www/leaoai
git pull origin NewPython-only-front

# Reiniciar serviços
systemctl restart leaoai
systemctl reload nginx
```

### **Troubleshooting:**

```bash
# Verificar logs de erro
tail -f /var/log/nginx/error.log
journalctl -u leaoai -f --since "1 hour ago"

# Verificar status dos serviços
systemctl status leaoai nginx postgresql

# Testar configuração do Nginx
nginx -t
```

---

## ✅ CHECKLIST DE DEPLOY

### **Pré-Deploy:**

- [x] ✅ Código commitado no GitHub
- [x] ✅ Secrets configurados no GitHub
- [x] ✅ Arquivo `.do/app.yaml` criado
- [x] ✅ Workflow `.github/workflows/deploy.yml` criado

### **Digital Ocean:**

- [ ] 🔲 App Platform criada OU Droplet configurado
- [ ] 🔲 Variáveis de ambiente configuradas
- [ ] 🔲 Banco PostgreSQL configurado
- [ ] 🔲 Domínio `www.leaoai.com.br` apontado
- [ ] 🔲 SSL certificado configurado

### **Testes:**

- [ ] 🔲 Site carrega: https://www.leaoai.com.br
- [ ] 🔲 API responde: https://www.leaoai.com.br/api
- [ ] 🔲 Formulários funcionando
- [ ] 🔲 Rate limiting ativo
- [ ] 🔲 Histórico de chat funcional

**Deploy no Digital Ocean via GitHub configurado com sucesso!** 🎉
