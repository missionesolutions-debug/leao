using Data;
using Data.Models.PainelConfig;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Factories.System
{
    public class PageFactory
    {
        private readonly ApplicationDbContext _context;

        public PageFactory(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Crud

        public Page GetObj(int id)
        {
            return _context.Page.Where(b => b.Id == id).FirstOrDefault();
        }

        public Page SaveObj(Page obj)
        {
            _context.Add(obj);
            _context.SaveChanges();

            return obj;
        }

        public Page UpdateObj(Page obj)
        {
            _context.Update(obj);
            _context.SaveChanges();
            return obj;
        }

        public List<Page> GetAll()
        {
            return _context.Page.ToList();
        }

        public List<Page> GetList()
        {
            return _context.Page.Where(b => b.Excluido != true).Include(a => a.Menu).ToList();
        }

        public List<Page> GetAllAtivo()
        {
            return _context.Page.Where(b => b.Ativo == true).ToList();
        }

        public List<Page> GetAllExcluido()
        {
            return _context.Page.Where(b => b.Excluido == true).ToList();
        }

        public Boolean RemoveObj(Page obj)
        {
            try
            {
                _context.Page.Remove(obj);
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
                var obj = _context.Page.Where(b => b.Id == Id).FirstOrDefault();

                obj.Ativo = false;
                obj.Excluido = true;

                _context.Page.Update(obj);
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Page GetByTableAction(string tab)
        {
            return _context.Page.Where(b => b.TableAction == tab).FirstOrDefault();
        }

        public async Task<IEnumerable<Page>> GetPagesForSitemapAsync()
        {
            return await _context.Page.Where(b => b.Ativo == true && b.IsSiteUrlActive == true).ToListAsync();

        }
        #endregion
    }
}
