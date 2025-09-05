using Data;
using Data.Models.Core;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Factories.Core
{
    public class ImagemFactory
    {
        private readonly ApplicationDbContext _context;

        public ImagemFactory(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Crud
        public Imagem GetObj(int id)
        {
            return _context.Imagem.Where(b => b.Id == id).FirstOrDefault();
        }
        public Imagem SaveObj(Imagem obj)
        {
            _context.Add(obj);
            _context.SaveChanges();

            return obj;
        }
        public Imagem UpdateObj(Imagem obj)
        {
            _context.Update(obj);
            _context.SaveChanges();
            return obj;
        }
        public List<Imagem> GetAll()
        {
            return _context.Imagem.ToList();
        }
        public List<Imagem> GetList()
        {
            return _context.Imagem.Where(b => b.Excluido != true).ToList();
        }
        public List<Imagem> GetAllAtivo()
        {
            return _context.Imagem.Where(b => b.Ativo == true).ToList();
        }
        public List<Imagem> GetAllExcluido()
        {
            return _context.Imagem.Where(b => b.Excluido == true).ToList();
        }
        public Boolean RemoveObj(Imagem obj)
        {
            try
            {
                _context.Imagem.Remove(obj);
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
                var obj = _context.Imagem.Where(b => b.Id == Id).FirstOrDefault();

                obj.Ativo = false;
                obj.Excluido = true;

                _context.Imagem.Update(obj);
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        public List<Imagem> GetImagensWithoutTableActionAndTableId()
        {
            return _context.Imagem.Where(b => b.TableAction == null && b.TableId == 0 && b.Excluido != true).ToList();
        }

        public List<Imagem> GetAllToBlob()
        {
            return _context.Imagem.Where(x => x.Url == null).ToList();
        }
        public List<Imagem> GetAllToCDN()
        {
            return _context.Imagem.Where(x => x.Url != null && x.Url.StartsWith("https://cdn.codiehost.com.br/")).ToList();
        }


        public List<Imagem> GetImagemsByTableActionAndTableId(string TableAction, int TableId)
        {
            List<Imagem> Imagens = _context.Imagem.Where(b => b.TableAction == TableAction && b.TableId == TableId && b.Excluido != true).ToList();

            if (Imagens != null)
                return Imagens;

            return new List<Imagem>();

        } 
        
        public List<Imagem> GetImagemsByTableAction(string TableAction)
        {
            List<Imagem> Imagens = _context.Imagem.Where(b => b.TableAction == TableAction && b.Excluido != true).ToList();

            if (Imagens != null)
                return Imagens;

            return new List<Imagem>();

        }
        
        public List<Imagem> GetImagemsByTableId(int TableId)
        {
            List<Imagem> Imagens = _context.Imagem.Where(b => b.TableId == TableId && b.Excluido != true).ToList();
          
            if (Imagens != null)
                return Imagens;

            return new List<Imagem>();

        }
    }
}
