using Data;
using Data.Models.Conteudo;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Factories.Conteudo
{
    public class PaginaFactory
    {
        private readonly ApplicationDbContext _context;

        public PaginaFactory(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Crud

        public Pagina GetObj(int id)
        {
            return _context.Pagina.Where(b => b.Id == id).FirstOrDefault();
        }

        public Pagina GetObjByUrlhome()
        {
            return _context.Pagina.Where(b => b.Ativo == true && b.Url == "home").FirstOrDefault();
        }

        public Pagina GetObjByUrlSobre()
        {
            return _context.Pagina.Where(b => b.Ativo == true && b.Url == "quem-somos").FirstOrDefault();
        }

        public Pagina GetObjByUrlEspiritualidade()
        {
            return _context.Pagina.Where(b => b.Ativo == true && b.Url == "inteligencia-comercial").FirstOrDefault();
        }

        public Pagina GetObjByUrlmkt()
        {
            return _context.Pagina.Where(b => b.Ativo == true && b.Url == "marketing").FirstOrDefault();
        }

        public Pagina GetObjByUrlAtividade()
        {
            return _context.Pagina.Where(b => b.Ativo == true && b.Url == "para-empresas").FirstOrDefault();
        }
        public Pagina SaveObj(Pagina obj)
        {
            _context.Add(obj);
            _context.SaveChanges();

            return obj;
        }

        public Pagina UpdateObjAsync(Pagina obj)
        {
            _context.Update(obj);
            _context.SaveChanges();
            return obj;
        }

        public async Task<Pagina> UpdateObjAsyncs(Pagina obj)
        {
            _context.Update(obj);
            _context.SaveChanges();
            return obj;
        }

        public List<Pagina> GetAll()
        {
            return _context.Pagina.ToList();
        }

        public List<Pagina> GetAllAtivo()
        {
            return _context.Pagina.Where(b=> b.Ativo == true).Include(b=> b.Categoria).ToList();
        }

        public List<Pagina> GetAllExcluido()
        {
            return _context.Pagina.Where(b => b.Excluido == true).ToList();
        }

        public Boolean RemoveObj(Pagina obj)
        {
            try
            {
                _context.Pagina.Remove(obj);
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
                var obj =_context.Pagina.Where(b => b.Id == Id).FirstOrDefault();

                obj.Ativo = false;
                obj.Excluido = true;

                _context.Pagina.Update(obj);
                _context.SaveChanges();
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        public Pagina GetObjByUrl(string url)
        {
            return _context.Pagina.Where(b =>b.Ativo == true && b.Url == url).FirstOrDefault();
        }

        public List<Pagina> GetPaginasByCategoriaId(int id)
        {
            return _context.Pagina.Where(b => b.Ativo == true && b.CategoriaId == id).ToList();
        }

        public List<Pagina> GetAllToBlob()
        {
            return _context.Pagina
                .Where(x => (!string.IsNullOrWhiteSpace(x.Imagem) && !x.Imagem.Contains("http"))
                || (!string.IsNullOrWhiteSpace(x.Thumbnail) && !x.Thumbnail.Contains("http"))
                )
                .ToList();
        }

        public Pagina UpdateObj(Pagina obj)
        {
            _context.Update(obj);
            _context.SaveChanges();
            return obj;
        }

		public List<Pagina> GetAllToCDN()
		{
			return _context.Pagina
				.Where(x =>
			(!string.IsNullOrWhiteSpace(x.Imagem) && x.Imagem.Contains("http") && !x.Imagem.StartsWith("https://cdn.codiehost.com.br/")) ||
			(!string.IsNullOrWhiteSpace(x.Thumbnail) && x.Thumbnail.Contains("http")! && x.Thumbnail.StartsWith("https://cdn.codiehost.com.br/"))
			)
				.ToList();
		}
	}
}
