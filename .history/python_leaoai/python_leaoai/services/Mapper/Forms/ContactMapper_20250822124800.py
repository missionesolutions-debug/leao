from typing import Dict, Any
from datetime import datetime


class Contact:
    def __init__(self,
                 id: int = None,
                 ativo: bool = True,
                 excluido: bool = False,
                 data_criacao: datetime = None,
                 data_edicao: datetime = None,
                 tipo_contact_id: int = None,
                 produto_id: int = None,
                 name: str = None,
                 email: str = None,
                 phone: str = None,
                 subject: str = None,
                 message: str = None,
                 curriculum_file: str = None,
                 curriculum_url: str = None,
                 curriculum_file_name: str = None,
                 **kwargs):
        self.id = id
        self.ativo = ativo
        self.excluido = excluido
        self.data_criacao = data_criacao
        self.data_edicao = data_edicao
        self.tipo_contact_id = tipo_contact_id
        self.produto_id = produto_id
        self.name = name
        self.email = email
        self.phone = phone
        self.subject = subject
        self.message = message
        self.curriculum_file = curriculum_file
        self.curriculum_url = curriculum_url
        self.curriculum_file_name = curriculum_file_name

        # Qualquer campo adicional pode ser armazenado dinamicamente
        for key, value in kwargs.items():
            setattr(self, key, value)


class ContactDTO:
    def __init__(self,
                 name: str = None,
                 email: str = None,
                 phone: str = None,
                 subject: str = None,
                 message: str = None,
                 additional_fields: Dict[str, Any] = None):
        self.name = name
        self.email = email
        self.phone = phone
        self.subject = subject
        self.message = message
        self.additional_fields = additional_fields or {}


class ContactMapper:
    FIXED_PROPERTIES = {
        "id", "ativo", "excluido", "data_criacao", "data_edicao",
        "tipo_contact_id", "produto_id", "name", "email", "phone",
        "subject", "message", "curriculum_file", "curriculum_url", "curriculum_file_name"
    }

    @staticmethod
    def map_to_contact(dto: ContactDTO) -> Contact:
        # Inicializa com campos fixos
        kwargs = {
            "name": dto.name,
            "email": dto.email,
            "phone": dto.phone,
            "subject": dto.subject,
            "message": dto.message,
        }

        # Adiciona campos adicionais
        kwargs.update(dto.additional_fields)

        return Contact(**kwargs)

    @staticmethod
    def map_to_dto(contact: Contact) -> ContactDTO:
        dto = ContactDTO(
            name=contact.name,
            email=contact.email,
            phone=contact.phone,
            subject=contact.subject,
            message=contact.message
        )

        # Percorrer atributos de Contact e adicionar os que não são fixos
        for prop, value in vars(contact).items():
            if value is not None and prop.lower() not in ContactMapper.FIXED_PROPERTIES:
                dto.additional_fields[prop] = value

        return dto
