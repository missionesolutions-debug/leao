using Data;
using Data.Models.Propriedade;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Factories.Propriedade
{
    public class TipoCategoriaFactory
    {
        private readonly ApplicationDbContext _context;

        public TipoCategoriaFactory(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Crud

        public TipoCategoria GetObj(int id)
        {
            return _context.TipoCategoria.Where(b => b.Id == id).FirstOrDefault();
        }
        public TipoCategoria SaveObj(TipoCategoria obj)
        {
            _context.Add(obj);
            _context.SaveChanges();

            return obj;
        }

        public TipoCategoria UpdateObj(TipoCategoria obj)
        {
            _context.Update(obj);
            _context.SaveChanges();
            return obj;
        }

        public List<TipoCategoria> GetAll()
        {
            return _context.TipoCategoria.ToList();
        }

        public List<TipoCategoria> GetAllAtivo()
        {
            return _context.TipoCategoria.Where(b=> b.Ativo == true).ToList();
        }

        public List<TipoCategoria> GetAllExcluido()
        {
            return _context.TipoCategoria.Where(b => b.Excluido == true).ToList();
        }

        public Boolean RemoveObj(TipoCategoria obj)
        {
            try
            {
                _context.TipoCategoria.Remove(obj);
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
                var obj =_context.TipoCategoria.Where(b => b.Id == Id).FirstOrDefault();

                obj.Ativo = false;
                obj.Excluido = true;

                _context.TipoCategoria.Update(obj);
                _context.SaveChanges();
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        public TipoCategoria GetObjByUrl(String url)
        {
            return _context.TipoCategoria.Where(b =>b.Ativo == true && b.Url == url).FirstOrDefault();
        }
      
    }
}
