# models/__init__.py

# Importação dos modelos do módulo xCode
from .chat_ia import ChatIA
from .chat_ia_item import ChatIAItem

# Se futuramente tiver outros módulos, como Factory, Core, Conteudo, etc.,
# basta adicionar os imports aqui também:
# from .factory import Project, ProjectItem, ProjectBlock, ...
# from .core import User, Arquivo, Imagem, ...

# Lista opcional para facilitar iteração (como ao criar tabelas)
all_models = [
    ChatIA,
    ChatIAItem,
    # Adicione aqui os demais modelos
]
