using Data.Models.Conteudo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Repositories.Interface
{
    public interface IPaginaRepository
    {
        Task<IEnumerable<Pagina>> GetActivePagesAsync();
    }
}
