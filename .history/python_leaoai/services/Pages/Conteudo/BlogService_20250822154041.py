# from sqlalchemy.orm import Session, joinedload
# from sqlalchemy import or_
# from typing import List, Optional
# from pydantic import BaseModel
# from models import Blog, Categoria, BlogImagem  # seus modelos ORM
# from datetime import datetime

# # ----------------------------
# # DTOs
# # ----------------------------
# class BlogImagemDto(BaseModel):
#     id: int
#     url: str

# class BlogDetalheDto(BaseModel):
#     id: int
#     titulo: str
#     url: str
#     categoria_id: int
#     categoria_title: str
#     descricao: Optional[str]
#     subtitulo: Optional[str]
#     imagem: Optional[str]
#     imagens: List[BlogImagemDto] = []

# class BlogPageDto(BaseModel):
#     posts: List[BlogDetalheDto]
#     page_number: int
#     page_size: int
#     total_count: int
#     total_pages: int

# class CategoriaDetailPage(BaseModel):
#     id: int
#     titulo: str
#     url: str
#     blogs: List[BlogDetalheDto]

# # ----------------------------
# # Serviços auxiliares simplificados
# # ----------------------------
# class ItemService:
#     def build(self, blog: Blog) -> BlogDetalheDto:
#         return BlogDetalheDto(
#             id=blog.id,
#             titulo=blog.titulo,
#             url=blog.url,
#             categoria_id=blog.categoria_id,
#             categoria_title=blog.categoria.titulo if blog.categoria else "",
#             descricao=blog.descricao,
#             subtitulo=blog.subtitulo,
#             imagem=blog.imagem,
#             imagens=[BlogImagemDto(id=i.id, url=i.url) for i in getattr(blog, "imagens", [])]
#         )

# class HeadService:
#     def build(self, blog: Blog):
#         return {"id": blog.id, "titulo": blog.titulo}

# class BodyService:
#     def build(self, blog: Blog):
#         return {"descricao": blog.descricao, "subtitulo": blog.subtitulo}


# # ----------------------------
# # BlogService
# # ----------------------------
# class BlogService:
#     def __init__(self, db: Session):
#         self.db = db
#         self.item_service = ItemService()
#         self.head_service = HeadService()
#         self.body_service = BodyService()

#     def listar(self, categoria_url: Optional[str] = None, search: Optional[str] = None,
#                page: int = 1, page_size: int = 10) -> BlogPageDto:

#         query = self.db.query(Blog).options(
#             joinedload(Blog.categoria),
#             joinedload(Blog.imagens)
#         ).filter(Blog.ativo == True, Blog.excluido == False)

#         if categoria_url and categoria_url != "GetAll":
#             query = query.join(Categoria).filter(Categoria.url == categoria_url)

#         if search:
#             query = query.filter(Blog.titulo.ilike(f"%{search}%"))

#         total_count = query.count()
#         blogs = query.offset((page - 1) * page_size).limit(page_size).all()
#         posts = [self.item_service.build(b) for b in blogs]

#         return BlogPageDto(
#             posts=posts,
#             page_number=page,
#             page_size=page_size,
#             total_count=total_count,
#             total_pages=(total_count + page_size - 1) // page_size
#         )

#     def detalhe(self, url: str) -> Optional[BlogDetalheDto]:
#         blog = self.db.query(Blog).options(
#             joinedload(Blog.categoria),
#             joinedload(Blog.imagens)
#         ).filter(Blog.url == url, Blog.ativo == True, Blog.excluido == False).first()

#         if not blog:
#             return None

#         dto = self.item_service.build(blog)
#         dto.head = self.head_service.build(blog)
#         dto.body = self.body_service.build(blog)
#         return dto

#     def get_destaque(self) -> List[BlogDetalheDto]:
#         blogs = self.db.query(Blog).filter(Blog.destaque == True).all()
#         return [self._min_blog(b) for b in blogs]

#     def get_all_categories(self) -> List[CategoriaDetailPage]:
#         categorias = self.db.query(Categoria).filter(Categoria.ativo == True).all()
#         result = []
#         for cat in categorias:
#             result.append(CategoriaDetailPage(
#                 id=cat.id,
#                 titulo=cat.titulo,
#                 url=cat.url,
#                 blogs=[self._min_blog(b) for b in cat.blogs]  # assume relacionamento back_populates="categoria"
#             ))
#         return result

#     def _min_blog(self, blog: Blog) -> BlogDetalheDto:
#         return BlogDetalheDto(
#             id=blog.id,
#             titulo=blog.titulo,
#             url=blog.url,
#             categoria_id=blog.categoria_id,
#             categoria_title=blog.categoria.titulo if blog.categoria else "",
#             descricao=blog.descricao,
#             subtitulo=blog.subtitulo,
#             imagem=blog.imagem
#         )
