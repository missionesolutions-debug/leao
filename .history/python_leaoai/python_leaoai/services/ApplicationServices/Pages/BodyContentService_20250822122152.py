from typing import TypeVar, Generic, Optional

T = TypeVar('T')

# --- Model equivalente ---
class Body:
    def __init__(self, body_scripts: Optional[str] = None):
        self.body_scripts = body_scripts

class BodyContentService:
    def build(self, obj: T) -> Body:
        """
        Constrói um Body a partir de qualquer objeto que tenha a propriedade 'body_scripts'.
        """
        body_scripts = self.get_property_value(obj, "body_scripts")
        return Body(body_scripts=body_scripts)

    def get_property_value(self, obj: T, property_name: str):
        """
        Retorna o valor de uma propriedade do objeto ou None se não existir.
        """
        return getattr(obj, property_name, None)
