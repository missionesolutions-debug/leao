using Data;
using Framework.Data.Models.Factory;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Repositories.Implementations.Factory
{
    public class EmbalagemTypeFactory
    {
        private readonly ApplicationDbContext _context;

        public EmbalagemTypeFactory(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Crud
        public EmbalagemType GetObj(int id)
        {
            return _context.EmbalagemTypes.FirstOrDefault(x => x.Id == id);
        }

        public EmbalagemType SaveObj(EmbalagemType obj)
        {
            _context.EmbalagemTypes.Add(obj);
            _context.SaveChanges();
            return obj;
        }

        public EmbalagemType UpdateObj(EmbalagemType obj)
        {
            _context.EmbalagemTypes.Update(obj);
            _context.SaveChanges();
            return obj;
        }

        public List<EmbalagemType> GetAll()
        {
            return _context.EmbalagemTypes.ToList();
        }
        public List<EmbalagemType> GetListActives()
        {
            return _context.EmbalagemTypes.Where(x => x.Ativo).ToList();
        }
        public List<EmbalagemType> GetList()
        {
            return _context.EmbalagemTypes.Where(x => !x.Excluido).ToList();
        }

        public bool DeleteObj(int id)
        {
            var obj = GetObj(id);
            if (obj == null)
                return false;
            try
            {
                obj.Ativo = false;
                obj.Excluido = true;
                _context.EmbalagemTypes.Update(obj);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
