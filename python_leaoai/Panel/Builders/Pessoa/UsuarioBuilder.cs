using Data;
using Data.Models.Pessoa;
using Framework.Factories.Pessoa;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;

namespace Painel.Builders.Pessoa
{
    public class UsuarioBuilder
    {
        private static ApplicationDbContext _context;

        private UsuarioFactory _factory;

        public UsuarioBuilder(ApplicationDbContext context)
        {
            _context = context;
            _factory = new UsuarioFactory(context);
        }

        public Usuario SaveOrUpdate(Usuario obj)
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

        public List<Usuario> List()
        {
            return _factory.GetAllAtivo().ToList();
        }

        public bool DeleteObj(int id)
        {
            return _factory.DeleteObj(id);
        }
    }
}
