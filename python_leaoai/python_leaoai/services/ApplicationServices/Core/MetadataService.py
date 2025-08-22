import uuid
from datetime import datetime
from typing import Optional

class MetadataService:
    def __init__(self, repository, blob_service, section_repository):
        self._repository = repository
        self._blob_service = blob_service
        self._section_repository = section_repository

    async def process_file_async(self, file, entity_id: str, entity_type: str):
        # Faz o upload do arquivo para o BlobStorage
        file_uri = await self._blob_service.upload_blob_async(file)
        if not file_uri:
            raise Exception("Erro ao fazer upload do arquivo.")

        section_id: Optional[int] = None
        metadata_ref: Optional[str] = None

        # Verifica se o EntityId está associado a uma Section
        try:
            section_id_int = int(entity_id)
            section = self._section_repository.get_obj(section_id_int)
            if section:
                section_id = section.id
        except ValueError:
            pass

        # Se não encontrar Section, tenta buscar Metadata pelo Guid
        if section_id is None:
            parent_metadata = self._repository.get_obj(entity_id)
            if parent_metadata:
                metadata_ref = str(parent_metadata.id).upper()

        # Gera o GUID para o novo Metadata
        new_guid = str(uuid.uuid4())

        # Cria o objeto Metadata
        metadata = {
            "url": str(file_uri),
            "file_name": file.filename,
            "file_length": file.size,
            "file_type": file.content_type,
            "entity_type": entity_type,
            "section_id": section_id,
            "metadata_ref": metadata_ref,
            "guid": new_guid,
            "is_main": False,
            "position": 0,
            "created_at": datetime.utcnow(),
            "section": None
        }

        # Salva o Metadata no banco de dados
        self._repository.save_obj(metadata)

        return metadata

    def delete_metadata(self, id: str) -> bool:
        # Busca o Metadata no banco de dados
        metadata = self._repository.get_obj(id)
        if not metadata:
            return False

        # Deleta o arquivo do blob storage (descomentando se implementar)
        # deleted_from_blob = await self._blob_service.delete_blob_async(metadata['url'])
        # if not deleted_from_blob:
        #     raise Exception("Falha ao deletar o arquivo no Blob Storage.")

        # Remove o Metadata do banco de dados
        self._repository.remove_obj(metadata)
        return True
