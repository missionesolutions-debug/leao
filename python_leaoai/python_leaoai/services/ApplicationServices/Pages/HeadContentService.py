from typing import TypeVar, Optional

T = TypeVar('T')

# --- Model equivalente ---
class Head:
    def __init__(self, page_title: Optional[str] = None,
                 meta_description: Optional[str] = None,
                 image_open_graph: Optional[str] = None):
        self.page_title = page_title
        self.meta_description = meta_description
        self.image_open_graph = image_open_graph

class HeadContentService:
    def build(self, obj: T) -> Head:
        """
        Constrói um Head a partir de qualquer objeto que tenha as propriedades 'page_title', 'meta_description' e 'meta_image'.
        """
        page_title = self.get_property_value(obj, "page_title")
        meta_description = self.get_property_value(obj, "meta_description")
        meta_image = self.get_property_value(obj, "meta_image")
        image_open_graph = meta_image  # Mantendo lógica do C#

        return Head(
            page_title=page_title,
            meta_description=meta_description,
            image_open_graph=image_open_graph
        )

    def get_property_value(self, obj: T, property_name: str):
        """
        Retorna o valor de uma propriedade do objeto ou None se não existir.
        """
        return getattr(obj, property_name, None)
