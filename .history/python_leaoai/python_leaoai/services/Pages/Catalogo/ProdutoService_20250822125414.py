from sqlalchemy.orm import Session, joinedload
from sqlalchemy import or_
from typing import List, Optional
from datetime import datetime
from database import get_db  # sua função de sessão SQLAlchemy
from models import Produto, Categoria, ProdutoImagem  # modelos ORM
from pydantic import BaseModel


# ----------------------------
# DTOs
# ----------------------------
class ProdutoImagemDto(BaseModel):
    id: int
    url: str


class ProdutoDetalheDto(BaseModel):
    id: int
    nome: str
    descricao: Optional[str]
    preco: float
    categoria: str
    imagens: List[ProdutoImagemDto]


class PagedResult(BaseModel):
    items: List[ProdutoDetalheDto]
    page_number: int
    page_size: int
    total_count: int
    total_pages: int


# ----------------------------
# Serviços auxiliares
# ----------------------------
class ItemService:
    def __init__(self, db: Session):
        self.db = db

    def buscar_itens(self, produto_id: int):
        return self.db.query(Produto).filter(Produto.id == produto_id).all()


class HeadService:
    def __init__(self, db: Session):
        self.db = db

    def gerar_cabecalho(self, produto: Produto):
        return {"id": produto.id, "nome": produto.nome}


class BodyService:
    def __init__(self, db: Session):
        self.db = db

    def gerar_corpo(self, produto: Produto):
        return {"descricao": produto.descricao, "preco": produto.preco}


class ProdutoFactory:
    @staticmethod
    def criar(produto: Produto) -> ProdutoDetalheDto:
        return ProdutoDetalheDto(
            id=produto.id,
            nome=produto.nome,
            descricao=produto.descricao,
            preco=produto.preco,
            categoria=produto.categoria.nome if produto.categoria else None,
            imagens=[
                ProdutoImagemDto(id=img.id, url=img.url)
                for img in produto.imagens
            ]
        )


# ----------------------------
# ProdutoService (principal)
# ----------------------------
class ProdutoService:
    def __init__(self, db: Session):
        self.db = db
        self.item_service = ItemService(db)
        self.head_service = HeadService(db)
        self.body_service = BodyService(db)

    def listar(self, search: Optional[str] = None, categoria_id: Optional[int] = None,
               page: int = 1, page_size: int = 10) -> PagedResult:

        query = self.db.query(Produto).options(
            joinedload(Produto.categoria),
            joinedload(Produto.imagens)
        ).filter(Produto.ativo == True, Produto.excluido == False)

        if search:
            query = query.filter(
                or_(
                    Produto.nome.ilike(f"%{search}%"),
                    Produto.descricao.ilike(f"%{search}%")
                )
            )

        if categoria_id:
            query = query.filter(Produto.categoria_id == categoria_id)

        total_count = query.count()
        produtos = query.offset((page - 1) * page_size).limit(page_size).all()

        items = [ProdutoFactory.criar(p) for p in produtos]

        return PagedResult(
            items=items,
            page_number=page,
            page_size=page_size,
            total_count=total_count,
            total_pages=(total_count + page_size - 1) // page_size
        )

    def detalhe(self, produto_id: int) -> Optional[ProdutoDetalheDto]:
        produto = self.db.query(Produto).options(
            joinedload(Produto.categoria),
            joinedload(Produto.imagens)
        ).filter(Produto.id == produto_id, Produto.ativo == True, Produto.excluido == False).first()

        if not produto:
            return None

        return ProdutoFactory.criar(produto)
