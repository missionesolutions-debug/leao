using Data;
using Data.Models.PainelConfig;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Factories.System
{
    public class MenuFactory
    {
        private readonly ApplicationDbContext _context;

        public MenuFactory(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Crud

        public Menu GetObj(int id)
        {
            return _context.Menu.Where(b => b.Id == id).FirstOrDefault();
        }

        public Menu SaveObj(Menu obj)
        {
            _context.Add(obj);
            _context.SaveChanges();

            return obj;
        }

        public Menu UpdateObj(Menu obj)
        {
            _context.Update(obj);
            _context.SaveChanges();
            return obj;
        }

        public List<Menu> GetAll()
        {
            return _context.Menu.ToList();
        }

        public List<Menu> GetList()
        {
            return _context.Menu.Where(b => b.Excluido != true).ToList();
        }

        public List<Menu> GetAllAtivoWithPages()
        {
            return _context.Menu.Where(b => b.Excluido != true).Include(b=>b.Page).ToList();
        }

        public List<Menu> GetAllAtivo()
        {
            return _context.Menu.Where(b => b.Ativo == true).ToList();
        }

        public List<Menu> GetAllExcluido()
        {
            return _context.Menu.Where(b => b.Excluido == true).ToList();
        }

        public Boolean RemoveObj(Menu obj)
        {
            try
            {
                _context.Menu.Remove(obj);
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Boolean DeleteObj(int Id)
        {
            try
            {
                var obj = _context.Menu.Where(b => b.Id == Id).FirstOrDefault();

                obj.Ativo = false;
                obj.Excluido = true;

                _context.Menu.Update(obj);
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
