using Data;
using System.Linq;
using Data.Models.Catalogo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Factories.Catalogo
{
    public class EquipeFactory
    {
        private readonly ApplicationDbContext _context;

        public EquipeFactory(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Crud
        public Equipe GetObj(int id)
        {
            return _context.Equipe.Where(b => b.Id == id).FirstOrDefault();
        }
        public Equipe SaveObj(Equipe obj)
        {
            _context.Add(obj);
            _context.SaveChanges();

            return obj;
        }
        public Equipe UpdateObj(Equipe obj)
        {
            _context.Update(obj);
            _context.SaveChanges();
            return obj;
        }
        public List<Equipe> GetAll()
        {
            return _context.Equipe.ToList();
        }
        public List<Equipe> GetList()
        {
            return _context.Equipe.Where(b => b.Excluido != true).ToList();
        }
        public List<Equipe> GetAllAtivo()
        {
            return _context.Equipe.Where(b => b.Ativo == true).ToList();
        }
        public List<Equipe> GetAllExcluido()
        {
            return _context.Equipe.Where(b => b.Excluido == true).ToList();
        }
        public Boolean RemoveObj(Equipe obj)
        {
            try
            {
                _context.Equipe.Remove(obj);
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
                var obj = _context.Equipe.Where(b => b.Id == Id).FirstOrDefault();

                obj.Ativo = false;
                obj.Excluido = true;

                _context.Equipe.Update(obj);
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Equipe GetByID(int equipeID)
        {
            // Suponha que "Equipe" seja o nome da sua entidade no contexto do banco de dados
            Equipe equipe = _context.Equipe.FirstOrDefault(e => e.Id == equipeID);

            if (equipe == null)
            {
                throw new Exception($"Equipe com ID {equipeID} não encontrada.");
            }

            return equipe;
        }

        public List<Equipe> GetAllToBlob()
        {
            return _context.Equipe
                .Where(x => (!string.IsNullOrWhiteSpace(x.Imagem) && !x.Imagem.Contains("http"))
                || (!string.IsNullOrWhiteSpace(x.Thumbnail) && !x.Thumbnail.Contains("http"))
                )
                .ToList();
        }

        public async Task<Equipe> UpdateObjAsync(Equipe obj)
        {
            _context.Update(obj);
            _context.SaveChanges();

            return obj;
        }

		public List<Equipe> GetAllToCDN()
		{
			return _context.Equipe
				.Where(x =>
			(!string.IsNullOrWhiteSpace(x.Imagem) && x.Imagem.Contains("http") && !x.Imagem.StartsWith("https://cdn.codiehost.com.br/")) ||
			(!string.IsNullOrWhiteSpace(x.Thumbnail) && x.Thumbnail.Contains("http")! && x.Thumbnail.StartsWith("https://cdn.codiehost.com.br/"))
			)
				.ToList();
		}
		#endregion
	}
}
