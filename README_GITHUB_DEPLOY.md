# 🚀 Deploy Automático - GitHub → Digital Ocean

## ✅ TUDO CONFIGURADO PARA DEPLOY VIA GITHUB!

<!-- Deploy test 26/09/2025 -->

### 🎯 **Como funciona:**

1. **Você faz push** para branch `NewPython-only-front`
2. **GitHub Actions** executa automaticamente
3. **Deploy acontece** no Digital Ocean
4. **Site fica online** em www.leaoia.com.br

---

## 🛠️ SETUP INICIAL (FAZER APENAS UMA VEZ)

### **Passo 1: Configurar Digital Ocean**

#### **Opção A: App Platform (Recomendado)**

1. Acesse [Digital Ocean Apps](https://cloud.digitalocean.com/apps)
2. **Create App** → **GitHub** → Selecione este repositório
3. **Import** o arquivo `.do/app.yaml`
4. **Configure variáveis de ambiente:**
   ```env
   DATABASE_URL=postgresql://user:pass@host:port/db
   OPENAI_API_KEY=sk-your-key-here
   SECRET_KEY=your-jwt-secret-here
   ```
5. **Add Domain:** `www.leaoai.com.br`

#### **Opção B: Droplet + Actions**

1. **Criar Droplet** Ubuntu 22.04 (2GB RAM)
2. **Configurar GitHub Secrets:**
   ```bash
   python setup_github_secrets.py
   ```
3. **Ou configurar manualmente:**
   - `DO_HOST`: IP do servidor
   - `DO_USERNAME`: root
   - `DO_SSH_KEY`: conteúdo da chave SSH privada
   - `DO_PORT`: 22

### **Passo 2: Configurar DNS**

No seu provedor de domínio:

```dns
www.leaoai.com.br    CNAME    your-app.ondigitalocean.app
@.leaoai.com.br      A        IP-DO-SERVIDOR
```

---

## 🚀 PROCESSO DE DEPLOY

### **Deploy é automático!** Basta:

```bash
# 1. Fazer suas alterações
git add .
git commit -m "Nova funcionalidade adicionada"

# 2. Push para NewPython-only-front (dispara deploy automaticamente)
git push origin NewPython-only-front

# 3. Acompanhar deploy
# GitHub → Actions → Ver progresso do deploy
```

### **Status do Deploy:**

- ✅ **Sucesso:** Site atualizado em www.leaoai.com.br
- ❌ **Erro:** Verificar logs em GitHub Actions

---

## 📊 MONITORAMENTO

### **URLs de Verificação:**

- **🌐 Site:** https://www.leaoai.com.br
- **⚡ API:** https://www.leaoai.com.br/api
- **📋 Docs:** https://www.leaoai.com.br/api/docs
- **💚 Health:** https://www.leaoai.com.br/api/

### **Logs e Status:**

```bash
# SSH no servidor (se usando Droplet)
ssh root@your-server-ip

# Ver logs da aplicação
journalctl -u leaoai -f

# Status dos serviços
systemctl status leaoai nginx
```

---

## 🔧 CONFIGURAÇÕES

### **Arquivos importantes:**

- **`.github/workflows/deploy.yml`** - Configuração do GitHub Actions
- **`.do/app.yaml`** - Configuração do Digital Ocean App Platform
- **`backend/.env.production`** - Template das variáveis de ambiente
- **`DIGITAL_OCEAN_DEPLOY.md`** - Guia completo de deploy

### **Otimizações ativas:**

- ✅ **Rate Limiting:** 5 petições/min por usuário
- ✅ **Connection Pool:** 20 + 30 overflow
- ✅ **Multiple Workers:** 4 processos paralelos
- ✅ **Retry Logic:** 3 tentativas automáticas
- ✅ **CORS:** Configurado para www.leaoai.com.br
- ✅ **SSL:** Automático no App Platform

---

## 🆘 TROUBLESHOOTING

### **Deploy falhou?**

1. **Verificar GitHub Actions:** Repository → Actions → Ver erro
2. **Problemas comuns:**
   - Secrets não configurados
   - Erro na conexão SSH
   - Dependências não instaladas
   - Banco de dados não acessível

### **Site não carrega?**

1. **Verificar DNS:** `nslookup www.leaoai.com.br`
2. **Verificar SSL:** Pode demorar alguns minutos
3. **Verificar logs:** SSH no servidor e ver logs

### **API não responde?**

1. **Verificar backend:** https://www.leaoai.com.br/api
2. **Verificar variáveis de ambiente**
3. **Verificar banco de dados**

---

## 🎉 RESULTADO FINAL

**Seu sistema está configurado para:**

- ✅ **Deploy automático** via GitHub push
- ✅ **SSL/HTTPS** automático
- ✅ **Scaling** conforme necessário
- ✅ **Monitoramento** integrado
- ✅ **Backup** e rollback fácil

**Basta fazer push que o deploy acontece automaticamente!** 🚀

---

## 📞 COMANDOS RÁPIDOS

```bash
# Deploy manual (emergência)
git add . && git commit -m "Fix: urgent update" && git push origin NewPython-only-front

# Rollback para commit anterior
git reset --hard HEAD~1 && git push origin NewPython-only-front --force

# Ver status do último deploy
gh run list --limit 1
```

**Deploy via GitHub configurado com sucesso!** 🎯
