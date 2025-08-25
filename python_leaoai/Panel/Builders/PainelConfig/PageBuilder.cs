using Data;
using Data.Models.PainelConfig;
using Framework.Factories.System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;

namespace Painel.Builders.PainelConfig
{
    public class PageBuilder
    {
        private static ApplicationDbContext _context;

        private PageFactory _factory;

        public PageBuilder(ApplicationDbContext context)
        {
            _context = context;
            _factory = new PageFactory(context);
        }

        public Page SaveOrUpdate(Page obj)
        {
            try
            {
                obj.Excluido = false;

                if (obj.Id == 0)
                {
                    obj.Guid = Helpers.GenerateGUID();
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

        public List<Page> List()
        {
            return _factory.GetList().ToList();
        }

        public bool DeleteObj(int id)
        {
            return _factory.DeleteObj(id);
        }
    }
}
