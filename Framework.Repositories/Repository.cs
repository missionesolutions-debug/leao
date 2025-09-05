using Data;
using Data.Models.Propriedade;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Framework.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
	{
        protected readonly ApplicationDbContext _context;
        private readonly DbSet<T> _entities;

        public Repository(ApplicationDbContext context)
		{
			_context = context;
            _entities = context.Set<T>();
        }

		#region Crud
		public T GetObj(int id)
		{
			return _context.Set<T>().Find(id);
		}

		public T SaveObj(T obj)
		{
			_context.Add(obj);
			_context.SaveChanges();

			return obj;
		}

		public T UpdateObj(T obj)
		{
			if (obj == null)
				throw new ArgumentNullException(nameof(obj));

			_context.Update(obj);
			_context.SaveChanges();

			return obj;
		}

        public IEnumerable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression).AsEnumerable();
        }

        public async Task<T> SaveObjAsync(T obj)
        {
            await _context.AddAsync(obj);
            await _context.SaveChangesAsync();

            return obj;
        }

        public async Task<T> UpdateObjAsync(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            _context.Update(obj);

            await _context.SaveChangesAsync();

            return obj;
        }

        public List<T> GetAll()
		{
			return _context.Set<T>().ToList();
		}

        public List<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.ToList();
        }

        public async Task<List<T>> GetAllAtivoAsync()
        {
            return await _context.Set<T>().Where(b => EF.Property<bool>(b, "Ativo") == true).ToListAsync();
        }
        public List<T> GetAllAtivo()
		{
			return _context.Set<T>().Where(b => EF.Property<bool>(b, "Ativo") == true).ToList();
		}

        public List<T> GetAllAtivoWithCategoria()
        {
            List<Categoria> categorias = _context.Categoria.Where(b => b.Ativo == true).ToList();

            return _context.Set<T>().Where(b => EF.Property<bool>(b, "Ativo") == true).ToList();
        }

        

        public IQueryable<T> GetAllQueryableAtivo()
        {
            return _context.Set<T>().Where(b => EF.Property<bool>(b, "Ativo") == true);
        }

        public List<T> GetAllExcluido()
		{
			return _context.Set<T>().Where(b => EF.Property<bool>(b, "Excluido") == true).ToList();
		}

        public async Task<bool> RemoveObjAsync(T obj)
        {
            try
            {
                _context.Set<T>().Remove(obj);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveObj(T obj)
		{
			try
			{
				_context.Set<T>().Remove(obj);
				_context.SaveChanges();

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		public bool DeleteObj(int id)
		{
			try
			{
				var obj = _context.Set<T>().Find(id);

				if (obj == null)
					return false;

				// Assuming obj has properties Ativo and Excluido
				var propertyAtivo = obj.GetType().GetProperty("Ativo");
				var propertyExcluido = obj.GetType().GetProperty("Excluido");

				if (propertyAtivo != null && propertyExcluido != null)
				{
					propertyAtivo.SetValue(obj, false);
					propertyExcluido.SetValue(obj, true);

					_context.Update(obj);
					_context.SaveChanges();
				}

				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

        public T GetByUrl(string url)
        {
            return _context.Set<T>().FirstOrDefault(b => EF.Property<bool>(b, "Ativo") == true && EF.Property<string>(b, "Url") == url);
        }
        #endregion

      
        public T GetById(int id)
        {
            return _entities.Find(id);
        }

        public IEnumerable<T> List()
        {
            return _entities.ToList();
        }

        public IEnumerable<T> List(Expression<Func<T, bool>> predicate)
        {
            return _entities.Where(predicate).ToList();
        }

        public void Add(T entity)
        {
            _entities.Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _entities.Remove(entity);
            _context.SaveChanges();
        }


        public IEnumerable<T> GetAllWithCategoriaAtivo()
        {
            // Implementação: Retorna todos os itens ativos que possuem uma categoria
            var allEntities = _entities.ToList(); // Carrega todos os itens na memória para evitar problemas de reflexão no LINQ to Entities.

            return allEntities.Where(e =>
            {
                var categoriaProperty = typeof(T).GetProperty("Categoria");
                var ativoProperty = typeof(T).GetProperty("Ativo");

                bool hasCategoria = categoriaProperty != null && categoriaProperty.GetValue(e) != null;
                bool isActive = ativoProperty != null && (bool?)ativoProperty.GetValue(e) == true;

                return hasCategoria && isActive;
            }).ToList();
        }

		//public IEnumerable<T> GetAllAtivo()
		//{
		//    // Implementação: Retorna todos os itens ativos
		//    var allEntities = _entities.ToList(); // Carrega todos os itens na memória

		//    return allEntities.Where(e =>
		//    {
		//        var ativoProperty = typeof(T).GetProperty("Ativo");

		//        return ativoProperty != null && (bool?)ativoProperty.GetValue(e) == true;
		//    }).ToList();
		//}


		public IEnumerable<T> GetAllByCategoriaUrl(string category)
		{

            Categoria categoria = _context.Categoria.Where(b => b.Ativo == true && b.Url == category).FirstOrDefault();


            if(categoria is not null)
            {
				// Implementação: Retorna todos os itens ativos filtrados por categoria
				var allEntities = _entities.ToList(); // Carrega todos os itens na memória

				return allEntities.Where(e =>
				{
					// Obtém as propriedades "Categoria" e "Ativo" da entidade
					var categoriaProperty = typeof(T).GetProperty("CategoriaId");
					var ativoProperty = typeof(T).GetProperty("Ativo");

					// Verifica se o item corresponde ao Id da categoria e se está ativo
					bool matchesCategory = categoriaProperty != null &&
										   categoriaProperty.GetValue(e)?.ToString() == categoria.Id.ToString();
					bool isActive = ativoProperty != null &&
									(bool?)ativoProperty.GetValue(e) == true;

					return matchesCategory && isActive;
				}).ToList();
			}

            return null;
		}



		public IEnumerable<T> GetAllByCategoriaAtivo(string category)
        {
            // Implementação: Retorna todos os itens ativos filtrados por categoria
            var allEntities = _entities.ToList(); // Carrega todos os itens na memória

            return allEntities.Where(e =>
            {
                var categoriaProperty = typeof(T).GetProperty("Categoria");
                var ativoProperty = typeof(T).GetProperty("Ativo");

                bool matchesCategory = categoriaProperty != null && categoriaProperty.GetValue(e)?.ToString() == category;
                bool isActive = ativoProperty != null && (bool?)ativoProperty.GetValue(e) == true;

                return matchesCategory && isActive;
            }).ToList();
        }

        public IEnumerable<object> GetAllCategorias()
        {
            // Implementação: Retorna todas as categorias distintas dos itens
            var allEntities = _entities.ToList(); // Carrega todos os itens na memória

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
