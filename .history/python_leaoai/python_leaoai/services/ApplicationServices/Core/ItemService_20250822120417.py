from typing import Any, List, Dict

class Item:
    def __init__(self):
        self.fields: Dict[str, Any] = {}

    def __setattr__(self, name, value):
        if name in ["fields"]:
            super().__setattr__(name, value)
        else:
            self.__dict__[name] = value


class ItemService:
    CamposIgnorados = [
        "DataEdicao",
        "Ativo",
        "Excluido",
        "Password",
        "Senha",
        "FileType",
        "FileSize",
        "PlaceReceived",
        "TableId",
        "TableAction"
    ]

    def build(self, obj: Any, ignore_fields: List[str] = None) -> Item:
        item = Item()
        campos_ignorados = set(self.CamposIgnorados)
        if ignore_fields:
            campos_ignorados.update(ignore_fields)

        for attr in dir(obj):
            # Ignora métodos e campos privados/dunder
            if attr.startswith("_") or callable(getattr(obj, attr)):
                continue

            if attr in campos_ignorados:
                continue

            value = getattr(obj, attr)
            if value is None:
                continue

            # Tenta definir como atributo direto
            if hasattr(item, attr):
                setattr(item, attr, value)
            else:
                # Coloca no dicionário fields
                field_name = attr[0].upper() + attr[1:]
                if field_name not in campos_ignorados:
                    item.fields[field_name] = value

        return item
