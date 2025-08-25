from typing import List, Optional
from sqlalchemy.orm import Session
from sqlalchemy import func
from models import Blog, Categoria, Imagem  # Seus modelos SQLAlchemy
from repositories import BlogRepository, CategoriaRepository, ImagemRepository  # Repositórios equivalentes
from services import ItemService  # Serviço que cria DTOs semelhantes ao _srvItem
from dtos import BlogPage, BlogPageDetail, Item, Pagination  # Seus DTOs equivalentes
from math import ceil

class BlogService:
    def __init__(
        self,
        db: Session,
        item_service: ItemService,
        blog_repo: BlogRepository,
        categoria_repo: CategoriaRepository,
        imagem_repo: ImagemRepository
    ):
        self._db = db
        self._srvItem = item_service
        self._faBlog = blog_repo
        self._faCategoria = categoria_repo
        self._faImagem = imagem_repo

    def list(self, category: Optional[str], search: Optional[str], tags: Optional[str], page: Optional[int] = 1) -> BlogPage:
        page_size = 12
        page_number = page if page and page > 0 else 1

        # Filtrar blogs
        if category is None:
            blogs = self._faBlog.get_all_ativo_and_category_active()
        elif category == "GetAll":
            blogs = self._faBlog.get_all_ativo()
        else:
            blogs = self._faBlog.get_all_ativo_by_category(category)

        # Filtro de busca
        if search:
            search_lower = search.lower()
            blogs = [b for b in blogs if (b.titulo and search_lower in b.titulo.lower()) or (b.descricao and search_lower in b.descricao.lower())]

        # Obter todas as tags únicas
        all_tags = set(tag for b in blogs if b.tags for tag in b.tags.split(";"))

        # Montar DTO
        blogDTO = BlogPage()
        blogDTO.page.page_title = "Blog"
        blogDTO.page.tags = ";".join(all_tags)

        # Categorias
        categorias = self._faCategoria.get_all_ativo_having_blogs()
        blogDTO.categorias = [self._srvItem.build(c) for c in categorias]

        # Destaques
        destaques = self._faBlog.get_destaque()
        for d in destaques:
            d.descricao = ""
            d.categoria = None
        blogDTO.destaques = [self._srvItem.build(d) for d in destaques]

        # Ordenar
        blogs = sorted(blogs, key=lambda b: b.id, reverse=True)

        # Paginação
        total_items = len(blogs)
        start_index = (page_number - 1) * page_size
        end_index = start_index + page_size
        paginated_blogs = blogs[start_index:end_index]
        blogDTO.posts = [self._srvItem.build(b) for b in paginated_blogs]

        # Montar info de paginação
        total_pages = ceil(total_items / page_size)
        pagination = Pagination(
            page_size=page_size,
            page_total=total_items,
            page_number=page_number,
            is_last_page=page_number >= total_pages,
            is_first_page=page_number == 1,
            has_previous_page=page_number > 1,
            has_next_page=page_number < total_pages,
            start_page=1,
            end_page=total_pages,
            total_pages=total_pages,
            total_items=len(blogDTO.posts),
            pages=list(range(1, total_pages + 1))
        )
        blogDTO.pagination = pagination

        return blogDTO

    def detail(self, url: str) -> BlogPageDetail:
        blog = self._faBlog.get_by_url_only_post(url)
        if not blog:
            raise Exception("404NotFound")

        model = BlogPageDetail()
        model.page = self._srvItem.build(blog)

        # Categorias
        categorias = self._faCategoria.get_all_ativo_having_blogs()
        model.categorias = [self._srvItem.build(c) for c in categorias]

        # Posts relacionados
        posts = self._faBlog.get_blogs_by_categoria_id(blog.categoria_id)
        posts = [p for p in posts if p.id != blog.id]
        for p in posts:
            p.descricao = ""
            p.categoria = None
        model.posts = [self._srvItem.build(p) for p in posts]

        # Imagens
        imagens = self._faImagem.get_imagens_by_table_action_and_table_id("Blogs", blog.id)
        model.page.imagens = [self._srvItem.build(img) for img in imagens]

        return model

    def list_categorias(self) -> List[Item]:
        categorias = self._faCategoria.get_all_ativo_having_blogs()
        return [self._srvItem.build(c) for c in categorias]
