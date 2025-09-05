using Data;
using Data.Models.Propriedade;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Factories.Propriedade
{
    public class CategoriaFactory
    {
        private readonly ApplicationDbContext _context;

        public CategoriaFactory(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Crud

        public Categoria GetObj(int id)
        {
            return _context.Categoria.Where(b => b.Id == id).FirstOrDefault();
        }

        public Categoria GetObjById(int id)
        {
            return EncapsulateCategoria(_context.Categoria.Where(b => b.Id == id).FirstOrDefault());
        }
        
        public List<Categoria> GetAllCategory()
        {
            var categorias = _context.Categoria.Where(b => b.Ativo == true).ToList();
            List<Categoria> model = new List<Categoria>();
            foreach (var item in categorias)
            {
                model.Add(EncapsulateCategoria(item));
            }
            return model;
        }

       
        public Categoria EncapsulateCategoria(Categoria Categoria)
        {
            Categoria model = new Categoria();
            model.Id = Categoria.Id;
            model.Titulo = Categoria.Titulo;
            model.Url = Categoria.Url;

            return model;
        }

        public Categoria SaveObj(Categoria obj)
        {
            _context.Add(obj);
            _context.SaveChanges();

            return obj;
        }

        public Categoria UpdateObj(Categoria obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            _context.Update(obj);
            _context.SaveChanges();

            return obj;
        }



        public List<Categoria> GetAll()
        {
            return _context.Categoria.ToList();
        }

        public List<Categoria> GetAllAtivo()
        {
            return _context.Categoria.Where(b=> b.Ativo == true).Include(b=>b.TipoCategoria).ToList();
        }

        public List<Categoria> GetAtivos()
        {
            return _context.Categoria.Where(b => b.Ativo == true).ToList();
        }

        public List<Categoria> GetAllExcluido()
        {
            return _context.Categoria.Where(b => b.Excluido == true).ToList();
        }

        public Boolean RemoveObj(Categoria obj)
        {
            try
            {
                _context.Categoria.Remove(obj);
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
                var obj =_context.Categoria.Where(b => b.Id == Id).FirstOrDefault();

                obj.Ativo = false;
                obj.Excluido = true;

                _context.Categoria.Update(obj);
                _context.SaveChanges();
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        public Categoria GetObjByUrl(String url)
        {
            return _context.Categoria.Where(b =>b.Ativo == true && b.Url == url).FirstOrDefault();
        }
        
        public Categoria GetObjByService(String url)
        {
            return _context.Categoria.Where(b =>b.Ativo == true && b.Url == url).FirstOrDefault();
        }

        public List<Categoria> GetAllToBlob()
        {
            return _context.Categoria
                .Where(x => (!string.IsNullOrWhiteSpace(x.Imagem) && !x.Imagem.Contains("http"))
                || (!string.IsNullOrWhiteSpace(x.Thumbnail) && !x.Thumbnail.Contains("http"))
                )
                .ToList();
        }

		public List<Categoria> GetAllToCDN()
		{
			return _context.Categoria
				.Where(x =>
			(!string.IsNullOrWhiteSpace(x.Imagem) && x.Imagem.Contains("http") && !x.Imagem.StartsWith("https://cdn.codiehost.com.br/")) ||
			(!string.IsNullOrWhiteSpace(x.Thumbnail) && x.Thumbnail.Contains("http")! && x.Thumbnail.StartsWith("https://cdn.codiehost.com.br/"))
			)
				.ToList();
		}
	}
}
