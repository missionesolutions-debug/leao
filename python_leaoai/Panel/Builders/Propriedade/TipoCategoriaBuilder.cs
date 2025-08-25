using Data;
using Data.Models.Propriedade;
using Framework.Factories.Propriedade;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;

namespace Painel.Builders.Propriedade
{
    public class TipoCategoriaBuilder
    {
        private static ApplicationDbContext _context;

        private TipoCategoriaFactory _factory;

        public TipoCategoriaBuilder(ApplicationDbContext context)
        {
            _context = context;
            _factory = new TipoCategoriaFactory(context);
        }

        public TipoCategoria SaveOrUpdate(TipoCategoria obj)
        {
            try
            {
                obj.Excluido = false;

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

        public List<TipoCategoria> List()
        {
            return _factory.GetAll().Where(b=> b.Excluido != true).ToList();
        }

        public bool DeleteObj(int id)
        {
            return _factory.DeleteObj(id);
        }
    }
}
