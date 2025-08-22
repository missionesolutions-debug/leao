from typing import Type, TypeVar
from .generic_mapper import GenericMapper  # supondo que esteja no mesmo módulo

TSource = TypeVar("TSource")
TDestination = TypeVar("TDestination")

def map_to(source: TSource, destination_type: Type[TDestination]) -> TDestination:
    """
    Função utilitária para mapear um objeto para outro tipo,
    usando GenericMapper internamente.
    """
    mapper = GenericMapper(destination_type)
    return mapper.map(source)
