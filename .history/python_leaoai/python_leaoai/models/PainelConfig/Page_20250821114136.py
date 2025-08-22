# models/page.py

from sqlalchemy import Column, Integer, String, Boolean, ForeignKey
from sqlalchemy.orm import relationship, Mapped, mapped_column
from database import Base
from .menu import Menu  # assumindo que o modelo Menu já existe

class Page(Base):
    __tablename__ = "page"

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    menu_id: Mapped[int | None] = mapped_column(Integer, ForeignKey("menu.id"), nullable=True)
    menu: Mapped[Menu] = relationship("Menu", back_populates="page")

    svg_icon: Mapped[str | None] = mapped_column(String, nullable=True)
    role_gate: Mapped[str | None] = mapped_column(String, nullable=True)
    nome: Mapped[str | None] = mapped_column(String, nullable=True)
    route: Mapped[str | None] = mapped_column(String, nullable=True)
    delete_route: Mapped[str | None] = mapped_column(String, nullable=True)
    detail_route: Mapped[str | None] = mapped_column(String, nullable=True)
    post: Mapped[str | None] = mapped_column(String, nullable=True)
    tela: Mapped[str | None] = mapped_column(String, nullable=True)
    backplace: Mapped[str | None] = mapped_column(String, nullable=True)
    obs: Mapped[str | None] = mapped_column(String, nullable=True)
    table_action: Mapped[str | None] = mapped_column(String, nullable=True)

    nav_tabs: Mapped[bool] = mapped_column(Boolean, default=False)
    ac_ordem: Mapped[bool] = mapped_column(Boolean, default=False)
    ac_ativo: Mapped[bool] = mapped_column(Boolean, default=False)
    ac_destaque: Mapped[bool] = mapped_column(Boolean, default=False)
    ac_nome: Mapped[bool] = mapped_column(Boolean, default=False)
    ac_titulo: Mapped[bool] = mapped_column(Boolean, default=False)
    ac_subtitulo: Mapped[bool] = mapped_column(Boolean, default=False)
    ac_descricao: Mapped[bool] = mapped_column(Boolean, default=False)
    ac_imagem: Mapped[bool] = mapped_column(Boolean, default=False)
    ac_thumbnail: Mapped[bool] = mapped_column(Boolean, default=False)
    ac_imagem_alt: Mapped[bool] = mapped_column(Boolean, default=False)
    ac_thumbnail_alt: Mapped[bool] = mapped_column(Boolean, default=False)
    ac_tags: Mapped[bool] = mapped_column(Boolean, default=False)
    ac_page_title: Mapped[bool] = mapped_column(Boolean, default=False)
    ac_meta_description: Mapped[bool] = mapped_column(Boolean, default=False)
    ac_meta_image: Mapped[bool] = mapped_column(Boolean, default=False)
    ac_url: Mapped[bool] = mapped_column(Boolean, default=False)
    ac_slug: Mapped[bool] = mapped_column(Boolean, default=False)
    ac_exclusivo: Mapped[bool] = mapped_column(Boolean, default=False)
    ac_tipo_categoria: Mapped[bool] = mapped_column(Boolean, default=False)
    ac_categoria: Mapped[bool] = mapped_column(Boolean, default=False)
    ac_sub_categoria: Mapped[bool] = mapped_column(Boolean, default=False)

    show_job_task_item: Mapped[bool] = mapped_column(Boolean, default=False)
    show_users_job_task: Mapped[bool] = mapped_column(Boolean, default=False)
    show_content_detail_item: Mapped[bool] = mapped_column(Boolean, default=False)
    show_content: Mapped[bool] = mapped_column(Boolean, default=False)
    show_file_item: Mapped[bool] = mapped_column(Boolean, default=False)
    show_image_item: Mapped[bool] = mapped_column(Boolean, default=False)
    show_pieces: Mapped[bool] = mapped_column(Boolean, default=False)

    is_site_url_active: Mapped[bool | None] = mapped_column(Boolean, nullable=True)
    entity_name: Mapped[str] = mapped_column(String, default="", nullable=False)
    entity_route: Mapped[str] = mapped_column(String, default="", nullable=False)
    priority: Mapped[str] = mapped_column(String, default="", nullable=False)
    guid: Mapped[str | None] = mapped_column(String, nullable=True)
