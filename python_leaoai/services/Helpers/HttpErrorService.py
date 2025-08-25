from http import HTTPStatus

class HttpErrorService:

    @staticmethod
    def get_error(ex: Exception) -> str | None:
        """
        Retorna a mensagem da exceção interna baseada no tipo de erro.
        """
        inner = getattr(ex, '__cause__', None)  # inner exception equivalente
        if inner is not None:
            mapping = {
                "404NotFound": str(inner),
                "409Conflict": str(inner),
                "403Forbidden": str(inner),
                "204NoContent": str(inner),
                "400BadRequest": str(inner),
                "500InternalServerError": str(inner)
            }
            return mapping.get(str(ex), None)
        return None

    @staticmethod
    def get_error_code(ex: Exception) -> int:
        """
        Retorna o código HTTP equivalente baseado na mensagem da exceção.
        """
        inner = getattr(ex, '__cause__', None)
        if inner is not None:
            mapping = {
                "404NotFound": HTTPStatus.NOT_FOUND,
                "409Conflict": HTTPStatus.CONFLICT,
                "403Forbidden": HTTPStatus.FORBIDDEN,
                "204NoContent": HTTPStatus.NO_CONTENT,
                "400BadRequest": HTTPStatus.BAD_REQUEST,
                "500InternalServerError": HTTPStatus.INTERNAL_SERVER_ERROR
            }
            return mapping.get(str(ex), HTTPStatus.OK).value
        return HTTPStatus.INTERNAL_SERVER_ERROR.value
