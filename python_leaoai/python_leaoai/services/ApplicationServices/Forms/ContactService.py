from http import HTTPStatus
from sqlalchemy.exc import SQLAlchemyError

# --- DTOs e Models já devem existir ---
# class ContactDTO: ...
# class Contact: ...
# class User: ...

class ContactService:
    def __init__(self, db_session, item_service, contact_repository, user_repository, contact_mapper):
        self._db = db_session
        self._srv_item = item_service
        self._repository = contact_repository
        self._user_repository = user_repository
        self._contact_mapper = contact_mapper

    # --- Criar/Atualizar contato ---
    def set_contact(self, model):
        try:
            # Verifica se é ContactDTO ou User
            if isinstance(model, ContactDTO):
                contact = self._contact_mapper.map_to_contact(model)
                self._repository.save_obj(contact)
            elif isinstance(model, User):
                self._user_repository.save_obj(model)
            else:
                return HTTPStatus.BAD_REQUEST

            return HTTPStatus.OK
        except SQLAlchemyError:
            self._db.rollback()
            return HTTPStatus.BAD_REQUEST
        except Exception:
            return HTTPStatus.BAD_REQUEST

    # --- Newsletter ---
    def set_newsletter(self, model: 'ContactDTO'):
        try:
            contact = self._contact_mapper.map_to_contact(model)
            self._repository.save_obj(contact)
            return HTTPStatus.OK
        except SQLAlchemyError:
            self._db.rollback()
            return HTTPStatus.BAD_REQUEST
        except Exception:
            return HTTPStatus.BAD_REQUEST

    # --- Work ---
    def set_work(self, model: 'ContactDTO'):
        try:
            contact = self._contact_mapper.map_to_contact(model)
            self._repository.save_obj(contact)
            return HTTPStatus.OK
        except SQLAlchemyError:
            self._db.rollback()
            return HTTPStatus.BAD_REQUEST
        except Exception:
            return HTTPStatus.BAD_REQUEST
