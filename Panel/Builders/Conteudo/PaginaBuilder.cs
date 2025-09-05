using Data;
using Data.Models.Conteudo;
using Framework.Factories.Conteudo;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;

namespace Painel.Builders.Conteudo
{
    public class PaginaBuilder
    {
        private static ApplicationDbContext _context;
        private PaginaFactory _factory;

        public PaginaBuilder(ApplicationDbContext context)
        {
            _context = context;
            _factory = new PaginaFactory(context);
        }

        public Pagina SaveOrUpdate(Pagina obj)
        {
            try
            {
                obj.DataCriacao = DateTime.Now;
                obj.DataEdicao = DateTime.Now;
                
                if (obj.CategoriaId == 0)
                    obj.CategoriaId = null;

                if (obj.Id == 0)
                    _factory.SaveObj(obj);
                else
                    _factory.UpdateObjAsync(obj);
            }
            catch (EntityException ex)
            {
            }

            return obj;
        }

        public List<Pagina> List()
        {
            return _factory.GetAllAtivo().ToList();
        }

        public bool DeleteObj(int id)
        {
            return _factory.DeleteObj(id);
        }
    }
}
