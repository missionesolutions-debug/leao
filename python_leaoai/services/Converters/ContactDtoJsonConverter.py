import json
from dataclasses import dataclass, field
from typing import Dict, Any

@dataclass
class ContactDTO:
    Name: str = ""
    Email: str = ""
    Phone: str = ""
    Subject: str = ""
    Message: str = ""
    AdditionalFields: Dict[str, Any] = field(default_factory=dict)

class ContactDtoJsonConverter:

    @staticmethod
    def read(json_str: str) -> ContactDTO:
        """
        Converte JSON string para ContactDTO, incluindo campos adicionais.
        """
        data = json.loads(json_str)
        dto = ContactDTO()

        for key, value in data.items():
            lower_key = key.lower()
            if lower_key == "name":
                dto.Name = value
            elif lower_key == "email":
                dto.Email = value
            elif lower_key == "phone":
                dto.Phone = value
            elif lower_key == "subject":
                dto.Subject = value
            elif lower_key == "message":
                dto.Message = value
            else:
                dto.AdditionalFields[key] = value

        return dto

    @staticmethod
    def write(dto: ContactDTO) -> str:
        """
        Converte ContactDTO em JSON string, incluindo campos adicionais.
        """
        data = {
            "name": dto.Name,
            "email": dto.Email,
            "phone": dto.Phone,
            "subject": dto.Subject,
            "message": dto.Message
        }

        # Adiciona os campos adicionais
        data.update(dto.AdditionalFields)

        return json.dumps(data, ensure_ascii=False)
