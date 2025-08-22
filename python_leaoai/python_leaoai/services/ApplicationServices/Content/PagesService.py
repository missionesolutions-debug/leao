from typing import List, Optional, Type, TypeVar
from sqlalchemy.orm import Session
from math import ceil
from models import Pagina, PaginaContent, Product, Servico, Artigo, Produto, Foto, Curso, Categoria  # Seus modelos SQLAlchemy
from dtos import PageDTO, ListPageDTO, Item, Pagination
from repositories import Repository  # Repositórios genéricos
from services import ItemService, PageSectionService, PageSectionItemService, ImagemFactory

T = TypeVar('T')

class PagesService:
    def __init__(
        self,
        db: Session,
        item_service: ItemService,
        pagina_repo: Repository,
        pagina_content_repo: Repository,
        banner_repo: Repository,
        categoria_repo: Repository,
        equipe_repo: Repository,
        cliente_repo: Repository,
        marca_repo: Repository,
        depoimento_repo: Repository,
        artigo_repo: Repository,
        blog_repo: Repository,
        product_repo: Repository,
        imagem_repo: ImagemFactory,
        page_section_service: PageSectionService,
        page_section_item_service: PageSectionItemService,
        servico_repo: Repository,
        produto_repo: Repository,
        foto_repo: Repository,
        curso_repo: Repository
    ):
        self._db = db
        self._srvItem = item_service
        self._pagina_repo = pagina_repo
        self._pagina_content_repo = pagina_content_repo
        self._imagem_repo = imagem_repo
        self._page_section_service = page_section_service
        self._page_section_item_service = page_section_item_service
        self._product_repo = product_repo
        self._servico_repo = servico_repo
        self._artigo_repo = artigo_repo
        self._produto_repo = produto_repo
        self._foto_repo = foto_repo
        self._curso_repo = curso_repo
        self._categoria_repo = categoria_repo

        # Map de content builders
        self._content_builders = {
            "Artigo": lambda chave, pagina_id, selector, ignore_fields: self.build_content(artigo_repo, chave, pagina_id, selector, ignore_fields),
            "Servico": lambda chave, pagina_id, selector, ignore_fields: self.build_content(servico_repo, chave, pagina_id, selector, ignore_fields),
            "Banner": lambda chave, pagina_id, selector, ignore_fields: self.build_content(banner_repo, chave, pagina_id, selector, ignore_fields),
            "Blog": lambda chave, pagina_id, selector, ignore_fields: self.build_content(blog_repo, chave, pagina_id, selector, ignore_fields),
            "Depoimento": lambda chave, pagina_id, selector, ignore_fields: self.build_content(depoimento_repo, chave, pagina_id, selector, ignore_fields),
            "Equipe": lambda chave, pagina_id, selector, ignore_fields: self.build_content(equipe_repo, chave, pagina_id, selector, ignore_fields),
            "Marca": lambda chave, pagina_id, selector, ignore_fields: self.build_content(marca_repo, chave, pagina_id, selector, ignore_fields),
            "Cliente": lambda chave, pagina_id, selector, ignore_fields: self.build_content(cliente_repo, chave, pagina_id, selector, ignore_fields),
            "Product": lambda chave, pagina_id, selector, ignore_fields: self.build_content(product_repo, chave, pagina_id, selector, ignore_fields),
            "Produto": lambda chave, pagina_id, selector, ignore_fields: self.build_content(produto_repo, chave, pagina_id, selector, ignore_fields),
            "Foto": lambda chave, pagina_id, selector, ignore_fields: self.build_content(foto_repo, chave, pagina_id, selector, ignore_fields),
            "Curso": lambda chave, pagina_id, selector, ignore_fields: self.build_content(curso_repo, chave, pagina_id, selector, ignore_fields)
        }

    def build_page(self, url: str) -> PageDTO:
        dto = PageDTO()
        pagina = self._pagina_repo.get_by_url(url)
        if not pagina:
            raise Exception("Page not found")

        dto.page = self._srvItem.build(pagina)

        contents = [c for c in self._pagina_content_repo.get_all() if c.pagina_id == pagina.id]
        for content in contents:
            builder = self._content_builders.get(content.table_action)
            if builder:
                built_content = builder(content.chave, content.pagina_id, content.selector, content.ignore_fields)
                setattr(dto, content.table_action.lower() + 's', built_content)

        dto.sections = self.get_page_sections(pagina.id, "Pagina")
        return dto

    def get_group(self, repo: Repository, category: Optional[str], search: Optional[str], tags: Optional[str],
                  page: Optional[int], destaque: Optional[bool], destaque_vitrine: Optional[bool], page_size: int = 12) -> ListPageDTO:
        entities = repo.get_all_ativo() if category in (None, "GetAll") else repo.get_all_by_categoria_url(category)

        if search:
            search_lower = search.lower()
            entities = [e for e in entities if search_lower in (getattr(e, "titulo", "") or "").lower() or
                        search_lower in (getattr(e, "descricao", "") or "").lower()]

        if destaque:
            entities = [e for e in entities if getattr(e, "destaque", False)]
        if destaque_vitrine:
            entities = [e for e in entities if getattr(e, "destaque_vitrine", False)]

        # Tags
        all_tags = set(tag for e in entities if getattr(e, "descricao", None) for tag in getattr(e, "descricao", "").split(';'))
        dto = ListPageDTO()
        dto.page.page_title = "Group"
        dto.page.tags = ";".join(all_tags)

        # Categorias
        categorias = {getattr(e, "categoria") for e in entities if getattr(e, "categoria", None)}
        dto.categorias = [Item(titulo=c.titulo, url=c.url) for c in categorias]

        # Ordenação
        entities = sorted(entities, key=lambda e: getattr(e, "id", 0), reverse=True)

        # Pagination
        page_number = page or 1
        total_items = len(entities)
        start = (page_number - 1) * page_size
        end = start + page_size
        dto.pages = [self._srvItem.build(e) for e in entities[start:end]]

        total_pages = ceil(total_items / page_size)
        dto.pagination = Pagination(
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
            total_items=len(dto.pages),
            pages=list(range(1, total_pages + 1))
        )
        return dto

    def build_content(self, repo: Repository, chave: str, pagina_id: int, selector: str, ignore_fields: Optional[str] = None) -> List[Item]:
        items = []
        entities = repo.get_all_ativo()
        for e in entities:
            add = False
            if selector == "GetAll":
                add = True
            elif selector == "GetAllWithKey" and chave:
                add = getattr(e, "chave", None) == chave
            elif selector == "GetByPaginaId" and getattr(e, "pagina_id", None) == pagina_id:
                add = True
            elif selector == "GetByPaginaIdAndChave" and chave and getattr(e, "pagina_id", None) == pagina_id:
                add = getattr(e, "chave", None) == chave

            if add:
                obj = self._srvItem.build(e, ignore_fields.split(';') if ignore_fields else None)
                # Imagens
                imagens = self._imagem_repo.get_imagens_by_table_action_and_table_id(type(e).__name__ + "s", obj.id)
                obj.imagens = [self._srvItem.build(img) for img in imagens]
                items.append(obj)
        return items

    def get_page_sections(self, id_: int, owner_type: str) -> List[Item]:
        sections = self._page_section_service.get_page_sections_by_owner(id_, owner_type)
        section_items = []
        for s in sections:
            section_item = Item(
                ref=s.ref,
                titulo=s.titulo,
                subtitulo=s.subtitulo,
                descricao=s.descricao,
                imagem=s.imagem,
                arquivo=s.arquivo,
                thumbnail=s.thumbnail
            )
            for i in self._page_section_item_service.get_page_section_items(s.id):
                item_obj = Item(
                    ref=i.ref,
                    titulo=i.titulo,
                    subtitulo=i.subtitulo,
                    descricao=i.descricao,
                    imagem=i.imagem,
                    arquivo=i.arquivo,
                    thumbnail=s.thumbnail
                )
                section_item.items.append(item_obj)
            section_items.append(section_item)
        return section_items
