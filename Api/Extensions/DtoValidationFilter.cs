using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace xCodie.API.Extensions
{
    public class DTOValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Verifica se o ModelState é inválido
            if (!context.ModelState.IsValid)
            {
                var validationErrors = context.ModelState
                    .Where(m => m.Value.Errors.Count > 0)
                    .Select(m => new
                    {
                        Field = m.Key,
                        Errors = m.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    });

                var errorResponse = new
                {
                    Message = "Erro de validação",
                    Errors = validationErrors
                };

                context.Result = new ObjectResult(errorResponse)
                {
                    StatusCode = StatusCodes.Status422UnprocessableEntity
                };
            }
            // Se não houver erros de validação, continua normalmente
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
