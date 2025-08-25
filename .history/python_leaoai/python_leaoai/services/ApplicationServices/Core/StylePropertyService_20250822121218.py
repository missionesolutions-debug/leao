from typing import List

class StylePropertyService:
    def __init__(self, style_property_repository):
        self._style_property_repository = style_property_repository

    def get_style_properties(self, owner_id: int, owner_type: str) -> List:
        return self._style_property_repository.list(
            lambda sp: sp.owner_id == owner_id and sp.owner_type == owner_type
        )

    def add_style_property(self, style_property):
        self._style_property_repository.add(style_property)

    def update_style_property(self, style_property):
        self._style_property_repository.update(style_property)

    def delete_style_property(self, id: int):
        style_property = self._style_property_repository.get_by_id(id)
        if style_property:
            self._style_property_repository.delete(style_property)
