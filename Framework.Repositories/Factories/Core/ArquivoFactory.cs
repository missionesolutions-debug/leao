using Data;
using Data.Models.Core;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Factories.Core
{
    public class ArquivoFactory
    {
        private readonly ApplicationDbContext _context;

        public ArquivoFactory(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Crud
        public Arquivo GetObj(int id)
        {
            return _context.Arquivo.Where(b => b.Id == id).FirstOrDefault();
        }
        public Arquivo SaveObj(Arquivo obj)
        {
            _context.Add(obj);
            _context.SaveChanges();

            return obj;
        }
        public Arquivo UpdateObj(Arquivo obj)
        {
            _context.Update(obj);
            _context.SaveChanges();
            return obj;
        }
        public List<Arquivo> GetAll()
        {
            return _context.Arquivo.ToList();
        }
        public List<Arquivo> GetList()
        {
            return _context.Arquivo.Where(b => b.Excluido != true).ToList();
        }
        public List<Arquivo> GetAllAtivo()
        {
            return _context.Arquivo.Where(b => b.Ativo == true).ToList();
        }
        public List<Arquivo> GetAllExcluido()
        {
            return _context.Arquivo.Where(b => b.Excluido == true).ToList();
        }
        public Boolean RemoveObj(Arquivo obj)
        {
            try
            {
                _context.Arquivo.Remove(obj);
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
                var obj = _context.Arquivo.Where(b => b.Id == Id).FirstOrDefault();

                obj.Ativo = false;
                obj.Excluido = true;

                _context.Arquivo.Update(obj);
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        public List<Arquivo> GetArquivosWithoutTableActionAndTableId()
        {
            return _context.Arquivo.Where(b => b.TableAction == null && b.TableId == 0 && b.Excluido != true).ToList();
        }
        public List<Arquivo> GetArquivosByTableActionAndTableId(string TableAction, int TableId)
        {
            List<Arquivo> arquivos = _context.Arquivo.Where(b => b.TableAction == TableAction && b.TableId == TableId && b.Excluido != true).ToList();

            if (arquivos != null)
                return arquivos;

            return new List<Arquivo>();
        }
    }
}
