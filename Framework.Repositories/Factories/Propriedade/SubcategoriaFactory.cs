using Data;
using Data.Models.Propriedade;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Factories.Propriedade
{
    public class SubCategoriaFactory
    {
        private readonly ApplicationDbContext _context;

        public SubCategoriaFactory(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Crud

        public SubCategoria GetObj(int id)
        {
            return _context.SubCategoria.Where(b => b.Id == id).FirstOrDefault();
        }

        public SubCategoria SaveObj(SubCategoria obj)
        {
            _context.Add(obj);
            _context.SaveChanges();

            return obj;
        }

        public SubCategoria UpdateObj(SubCategoria obj)
        {
            _context.Update(obj);
            _context.SaveChanges();
            return obj;
        }

        public List<SubCategoria> GetAll()
        {
            return _context.SubCategoria.ToList();
        }

        public List<SubCategoria> GetAllAtivo()
        {
            return _context.SubCategoria.Where(b=> b.Ativo == true).Include(b=>b.Categoria).Include(b=>b.TipoCategoria).ToList();
        }

        public List<SubCategoria> GetAllExcluido()
        {
            return _context.SubCategoria.Where(b => b.Excluido == true).ToList();
        }

        public Boolean RemoveObj(SubCategoria obj)
        {
            try
            {
                _context.SubCategoria.Remove(obj);
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
                var obj =_context.SubCategoria.Where(b => b.Id == Id).FirstOrDefault();

                obj.Ativo = false;
                obj.Excluido = true;

                _context.SubCategoria.Update(obj);
                _context.SaveChanges();
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        public SubCategoria GetObjByUrl(String url)
        {
            return _context.SubCategoria.Where(b =>b.Ativo == true && b.Url == url).FirstOrDefault();
        }
      
    }
}
