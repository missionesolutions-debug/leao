from typing import TypeVar, Generic, Type
from dataclasses import fields, is_dataclass
from datetime import datetime

TSource = TypeVar("TSource")
TDestination = TypeVar("TDestination")

class GenericMapper(Generic[TSource, TDestination]):
    def __init__(self, destination_type: Type[TDestination]):
        self.destination_type = destination_type

    def map(self, source: TSource) -> TDestination:
        # Cria instância do destino
        if is_dataclass(self.destination_type):
            destination = self.destination_type()  # dataclass inicializada
            destination_fields = {f.name: f for f in fields(self.destination_type)}
        else:
            destination = self.destination_type()
            destination_fields = destination.__dict__.keys()

        # Itera sobre atributos do source
        for attr, value in vars(source).items():
            if attr in destination_fields and value is not None:
                try:
                    # Se for dataclass, conseguimos inferir tipo esperado
                    if is_dataclass(self.destination_type):
                        expected_type = destination_fields[attr].type
                        setattr(destination, attr, self._convert_value(value, expected_type))
                    else:
                        setattr(destination, attr, value)
                except Exception:
                    # Ignora se não conseguir converter
                    pass

        return destination

    def _convert_value(self, value, expected_type):
        """Tenta converter o valor para o tipo esperado"""
        try:
            if expected_type == datetime and isinstance(value, str):
                # Exemplo: converter string -> datetime
                return datetime.fromisoformat(value)
            return expected_type(value)
        except Exception:
            return value
