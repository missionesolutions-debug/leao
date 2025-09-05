using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Framework.Repositories
{
    public interface IRepository<T> where T : class
    {
        T GetObj(int id);
        T SaveObj(T obj);
        T UpdateObj(T obj);
        Task<T> SaveObjAsync(T obj);
        Task<T> UpdateObjAsync(T obj);
        List<T> GetAll();
        List<T> GetAllAtivo(); 
        List<T> GetAllAtivoWithCategoria();
        Task<List<T>> GetAllAtivoAsync();
        IQueryable<T> GetAllQueryableAtivo();
        List<T> GetAllExcluido();
        bool RemoveObj(T obj);
        Task<bool> RemoveObjAsync(T obj);
        bool DeleteObj(int id);
        T GetByUrl(string url);
        IEnumerable<T> GetByCondition(Expression<Func<T, bool>> expression);
        T GetById(int id);
        IEnumerable<T> List();
        IEnumerable<T> List(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
		IEnumerable<T> GetAllByCategoriaUrl(string category);
	}

}
