# 📋 **Documentação das Rotas - LegalAI v3.1**

## 🎯 **Estrutura Organizada para Fácil Manutenção**

### 📁 **Organização dos Arquivos**

```
backend/app/
├── main.py                    # ✅ LIMPO - Apenas configurações essenciais
├── api/routes.py              # ✅ CENTRAL - Todas as rotas organizadas
├── api/
│   ├── auth_routes.py         # 🔐 Autenticação (login/register)
│   ├── user_routes.py         # 👥 Gestão de usuários
│   ├── brand_routes.py        # 🏷️ Gestão de marcas
│   ├── oracle_routes.py       # 🤖 IA/ChatBot
│   ├── frontend_routes.py     # 🌐 Páginas HTML
│   └── [petições]_routes.py   # 📄 Rotas específicas de petições
├── controllers/
│   └── peticao_controller.py  # 📄 Controller unificado de petições
└── middleware/
    └── auth_middleware.py     # 🔒 Autenticação centralizada
```

---

## 🌐 **Mapa Completo das Rotas**

### 🏠 **Frontend (Páginas HTML)**

| URL                           | Arquivo                    | Função              |
| ----------------------------- | -------------------------- | ------------------- |
| `/`                           | menu.html                  | Dashboard principal |
| `/index.html`                 | index.html                 | Login               |
| `/menu.html`                  | menu.html                  | Dashboard           |
| `/marca.html`                 | marca.html                 | Gestão de marcas    |
| `/oraculo.html`               | oraculo.html               | IA Assistant        |
| `/perfil.html`                | perfil.html                | Admin usuários      |
| `/caducidade.html`            | caducidade.html            | Caducidade          |
| `/nulidade.html`              | nulidade.html              | Nulidade            |
| `/oposicao.html`              | oposicao.html              | Oposição            |
| `/recurso_indeferimento.html` | recurso_indeferimento.html | Recursos            |
| `/manifestacao_oposicao.html` | manifestacao_oposicao.html | Manifestações       |
| `/manifestacao_recurso.html`  | manifestacao_recurso.html  | Manifestações       |
| `/contrarazao_nulidade.html`  | contrarazao_nulidade.html  | Contrarrazões       |
| `/cadastro.html`              | cadastro.html              | Cadastro usuários   |
| `/config.js`                  | config.js                  | Configuração JS     |

### 🔐 **API - Autenticação**

| URL              | Método | Função      | Auth       |
| ---------------- | ------ | ----------- | ---------- |
| `/auth/login`    | POST   | Login + JWT | ❌ Público |
| `/auth/register` | POST   | Registro    | ❌ Público |

### 👥 **API - Usuários**

| URL               | Método | Função                 | Auth       |
| ----------------- | ------ | ---------------------- | ---------- |
| `/usuarios/`      | POST   | Criar usuário          | 🔒 Privado |
| `/usuarios/me`    | GET    | Dados do usuário atual | 🔒 Privado |
| `/usuarios/{id}`  | GET    | Buscar usuário         | 🔒 Privado |
| `/usuarios/`      | GET    | Listar usuários        | 👑 Admin   |
| `/usuarios/{id}`  | PUT    | Atualizar usuário      | 🔒 Privado |
| `/usuarios/{id}`  | DELETE | Excluir usuário        | 👑 Admin   |
| `/usuarios/count` | GET    | Contar usuários        | ❌ Público |

### 🏷️ **API - Marcas**

| URL            | Método | Função          | Auth       |
| -------------- | ------ | --------------- | ---------- |
| `/brands/`     | POST   | Criar marca     | 🔒 Privado |
| `/brands/{id}` | GET    | Buscar marca    | 🔒 Privado |
| `/brands/`     | GET    | Listar marcas   | 🔒 Privado |
| `/brands/{id}` | PUT    | Atualizar marca | 🔒 Privado |
| `/brands/{id}` | DELETE | Excluir marca   | 🔒 Privado |

### 🤖 **API - Oracle IA**

| URL             | Método | Função      | Auth       |
| --------------- | ------ | ----------- | ---------- |
| `/oracle/query` | POST   | Consulta IA | 🔒 Privado |

### 📄 **API - Petições (Controller Unificado)**

| Categoria              | Endpoints Base                      | Auth       |
| ---------------------- | ----------------------------------- | ---------- |
| **Caducidade**         | `/peticoes/caducidade/*`            | 🔒 Privado |
| **Oposição**           | `/peticoes/oposicao/*`              | 🔒 Privado |
| **Nulidade**           | `/peticoes/nulidade/*`              | 🔒 Privado |
| **Manifestações**      | `/peticoes/manifestacao-*/*`        | 🔒 Privado |
| **Recursos**           | `/peticoes/recurso-indeferimento/*` | 🔒 Privado |
| **Petição Caducidade** | `/peticoes/peticao-caducidade/*`    | 🔒 Privado |
| **Contrarrazão**       | `/peticoes/contrarazao-nulidade/*`  | 🔒 Privado |
| **Listagem**           | `/peticoes/`                        | 🔒 Privado |
| **Por Tipo**           | `/peticoes/{tipo}`                  | 🔒 Privado |
| **Histórico**          | `/peticoes/{id}/historico`          | 🔒 Privado |

### 📄 **API - Petições (Rotas Específicas)**

| Categoria                  | Base URL                   | CRUD Completo                   |
| -------------------------- | -------------------------- | ------------------------------- |
| **Caducidades**            | `/caducidades/`            | ✅ Create, Read, Update, Delete |
| **Nulidades**              | `/nulidades/`              | ✅ Create, Read, Update, Delete |
| **Oposições**              | `/oposicoes/`              | ✅ Create, Read, Update, Delete |
| **Recursos**               | `/recursos-indeferimento/` | ✅ Create, Read, Update, Delete |
| **Manifestações Oposição** | `/manifestacoes-oposicao/` | ✅ Create, Read, Update, Delete |
| **Manifestações Recurso**  | `/manifestacoes-recurso/`  | ✅ Create, Read, Update, Delete |
| **Contrarrazões**          | `/contrarazoes-nulidade/`  | ✅ Create, Read, Update, Delete |
| **Petições Caducidade**    | `/peticoes-caducidade/`    | ✅ Create, Read, Update, Delete |

---

## 🔒 **Sistema de Autenticação**

### ❌ **Rotas Públicas (sem autenticação)**

- Todas as páginas HTML (`*.html`)
- Assets estáticos (`/assets/*`, `/lib/*`, `/scripts/*`)
- Login/registro (`/auth/*`)
- Health check (`/health`)
- Documentação (`/docs`, `/redoc`)

### 🔒 **Rotas Privadas (requer token JWT)**

- Todas as APIs de negócio
- Gestão de marcas, petições, oracle

### 👑 **Rotas Admin (requer role ADMIN)**

- Listagem de usuários (`/usuarios/`)
- Exclusão de usuários
- Página de administração (`/perfil.html`)

---

## 🛠️ **Como Fazer Manutenção**

### ➕ **Adicionar Nova Rota API**

1. Criar arquivo `nova_rota_routes.py` em `/app/api/`
2. Implementar router com padrão CRUD
3. Adicionar import em `/app/api/routes.py`
4. Incluir no `api_router` com prefix e tags apropriadas

### 🔧 **Modificar Autenticação**

1. Editar `/app/middleware/auth_middleware.py`
2. Atualizar listas `PUBLIC_ROUTES` ou `ADMIN_ROUTES`
3. Modificar lógica de validação conforme necessário

### 🌐 **Adicionar Nova Página Frontend**

1. Criar arquivo HTML em `/frontend/`
2. Adicionar rota em `/app/api/frontend_routes.py`
3. Seguir padrão existente com decoradores duplos

### 🏷️ **Adicionar Nova Tag/Categoria**

1. Atualizar `tags=["Nova Categoria"]` no routes.py
2. Documentação automática no Swagger será atualizada

---

## 🎯 **Benefícios da Estrutura Atual**

✅ **Manutenção Simples**: Um arquivo central (routes.py) para todas as rotas
✅ **Versionamento**: API v1 preparada para futuras versões (v2, v3...)
✅ **Organização Clara**: Separação entre frontend e API
✅ **Documentação Automática**: Swagger gerado automaticamente
✅ **Autenticação Centralizada**: Um arquivo controla toda a segurança
✅ **Escalabilidade**: Fácil adicionar novos módulos

---

## 📞 **Suporte**

Para dúvidas sobre manutenção, consulte:

1. Este arquivo de documentação
2. Comentários no código
3. Swagger UI em `/docs` quando servidor rodando
