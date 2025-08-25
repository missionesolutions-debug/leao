using Data;
using Data.Models.Pessoa;
using Framework.Factories.Pessoa;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;

namespace Painel.Builders.Pessoa
{
    public class ClienteBuilder
    {
        private static ApplicationDbContext _context;

        private ClienteFactory _factory;

        public ClienteBuilder(ApplicationDbContext context)
        {
            _context = context;
            _factory = new ClienteFactory(context);
        }

        public Cliente SaveOrUpdate(Cliente obj)
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

        public List<Cliente> List()
        {
            return _factory.GetList().ToList();
        }

        public bool DeleteObj(int id)
        {
            return _factory.DeleteObj(id);
        }
    }
}
