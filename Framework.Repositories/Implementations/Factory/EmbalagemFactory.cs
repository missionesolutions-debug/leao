using Data;
using Framework.Data.Models.Factory;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Repositories.Implementations.Factory
{
    public class EmbalagemFactory
    {
        private readonly ApplicationDbContext _context;

        public EmbalagemFactory(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Crud
        public Embalagem GetObj(int id)
        {
            return _context.Embalagems.FirstOrDefault(x => x.Id == id);
        }

        public Embalagem SaveObj(Embalagem obj)
        {
            _context.Embalagems.Add(obj);
            _context.SaveChanges();
            return obj;
        }

        public Embalagem UpdateObj(Embalagem obj)
        {
            _context.Embalagems.Update(obj);
            _context.SaveChanges();
            return obj;
        }

        public List<Embalagem> GetAll()
        {
            return _context.Embalagems.ToList();
        }
        public List<Embalagem> GetListActives()
        {
            return _context.Embalagems.Where(x => x.Ativo).ToList();
        }
        public List<Embalagem> GetList()
        {
            return _context.Embalagems.Where(x => !x.Excluido).Include(b=>b.EmbalagemType).ToList();
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
                _context.Embalagems.Update(obj);
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
