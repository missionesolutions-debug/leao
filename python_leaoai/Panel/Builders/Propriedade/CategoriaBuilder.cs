using Data;
using Data.Models.Propriedade;
using Framework.Factories.Propriedade;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;

namespace Painel.Builders.Propriedade
{
    public class CategoriaBuilder
    {
        private static ApplicationDbContext _context;

        private CategoriaFactory _factory;

        public CategoriaBuilder(ApplicationDbContext context)
        {
            _context = context;
            _factory = new CategoriaFactory(context);
        }

        public Categoria SaveOrUpdate(Categoria obj)
        {
            try
            {
                obj.Excluido = false;

                if (obj.TipoCategoriaId == 0)
                    obj.TipoCategoriaId = null;

                if (obj.Id == 0)
                {
                    obj.DataCriacao = DateTime.Now;
                    _factory.SaveObj(obj);
                }
                else
                {
                    obj.DataEdicao = DateTime.Now;
                    _factory.UpdateObj(obj);
                }
            }
            catch (EntityException ex)
            {
            }

            return obj;
        }

        public List<Categoria> List()
        {
            return _factory.GetAllAtivo().ToList();
        }

        public bool DeleteObj(int id)
        {
            return _factory.DeleteObj(id);
        }
    }
}
