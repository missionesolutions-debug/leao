using Data;
using Framework.Data.Models.Factory;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Repositories.Implementations.Factory
{
    public class AcabamentoFactory
    {
        private readonly ApplicationDbContext _context;

        public AcabamentoFactory(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Crud
        public Acabamento GetObj(int id)
        {
            return _context.Acabamentos.FirstOrDefault(x => x.Id == id);
        }

        public Acabamento SaveObj(Acabamento obj)
        {
            _context.Acabamentos.Add(obj);
            _context.SaveChanges();
            return obj;
        }

        public Acabamento UpdateObj(Acabamento obj)
        {
            _context.Acabamentos.Update(obj);
            _context.SaveChanges();
            return obj;
        }

        public List<Acabamento> GetAll()
        {
            return _context.Acabamentos.ToList();
        }
        public List<Acabamento> GetListActives()
        {
            return _context.Acabamentos.Where(x => x.Ativo).ToList();
        }
        public List<Acabamento> GetList()
        {
            return _context.Acabamentos.Where(x => !x.Excluido).Include(b=> b.Client).ToList();
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
                _context.Acabamentos.Update(obj);
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
