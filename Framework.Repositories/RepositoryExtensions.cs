using System.Collections.Generic;
using System.Linq;

namespace Framework.Repositories
{
    public static class RepositoryExtensions
    {
        public static IEnumerable<T> GetAllWithCategoriaAtivo<T>(this IRepository<T> repository) where T : class
        {
            var allEntities = repository.GetAllAtivoWithCategoria().ToList(); // Assume que GetAllAtivo() traga os dados da base
            return allEntities.Where(e =>
            {
                var categoriaProperty = typeof(T).GetProperty("Categoria");
                var ativoProperty = typeof(T).GetProperty("Ativo");

                bool hasCategoria = categoriaProperty != null && categoriaProperty.GetValue(e) != null;
                bool isActive = ativoProperty != null && (bool?)ativoProperty.GetValue(e) == true;

                return hasCategoria && isActive;
            }).ToList();
        }

        public static IEnumerable<T> GetAllByCategoriaAtivo<T>(this IRepository<T> repository, string category) where T : class
        {
            var allEntities = repository.GetAllAtivo().ToList();
            return allEntities.Where(e =>
            {
                var categoriaProperty = typeof(T).GetProperty("Categoria");
                var ativoProperty = typeof(T).GetProperty("Ativo");

                bool matchesCategory = categoriaProperty != null && categoriaProperty.GetValue(e)?.ToString() == category;
                bool isActive = ativoProperty != null && (bool?)ativoProperty.GetValue(e) == true;

                return matchesCategory && isActive;
            }).ToList();
        }

        public static IEnumerable<object> GetAllCategorias<T>(this IRepository<T> repository) where T : class
        {
            var allEntities = repository.GetAllAtivo().ToList();
            return allEntities.Select(e =>
            {
                var categoriaProperty = typeof(T).GetProperty("Categoria");
                return categoriaProperty != null ? categoriaProperty.GetValue(e) : null;
            })
            .Where(c => c != null)
            .Distinct()
            .ToList();
        }
    }
}