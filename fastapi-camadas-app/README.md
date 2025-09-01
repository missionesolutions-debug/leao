# FastAPI Camadas App

Este projeto é uma aplicação web desenvolvida com FastAPI, estruturada em camadas para promover uma arquitetura limpa e organizada. A aplicação é focada na gestão de usuários, permitindo operações como criação, leitura, atualização e exclusão de usuários.

## Estrutura do Projeto

```
fastapi-camadas-app
├── src
│   ├── main.py                # Ponto de entrada da aplicação
│   ├── api
│   │   └── routes.py          # Definição das rotas da API
│   ├── controllers
│   │   └── user_controller.py  # Controlador de usuários
│   ├── services
│   │   └── user_service.py     # Lógica de negócios para usuários
│   ├── repositories
│   │   └── user_repository.py   # Interação com a base de dados
│   ├── models
│   │   └── user_model.py        # Modelo de dados do usuário
│   ├── schemas
│   │   └── user_schema.py       # Esquemas de validação de dados
│   └── config
│       └── settings.py          # Configurações da aplicação
├── requirements.txt             # Dependências do projeto
└── README.md                    # Documentação do projeto
```

## Instalação

1. Clone o repositório:
   ```
   git clone <URL_DO_REPOSITORIO>
   cd fastapi-camadas-app
   ```

2. Crie um ambiente virtual e ative-o:
   ```
   python -m venv venv
   source venv/bin/activate  # Para Linux/Mac
   venv\Scripts\activate     # Para Windows
   ```

3. Instale as dependências:
   ```
   pip install -r requirements.txt
   ```

## Uso

Para iniciar a aplicação, execute o seguinte comando:

```
uvicorn src.main:app --reload
```

A aplicação estará disponível em `http://127.0.0.1:8000`.

## Documentação da API

A documentação interativa da API pode ser acessada em `http://127.0.0.1:8000/docs`.

## Contribuição

Contribuições são bem-vindas! Sinta-se à vontade para abrir issues ou pull requests.