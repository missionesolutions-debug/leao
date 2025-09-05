using Data;
using Data.Models.Core;
using Framework.Factories.Core;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;

namespace Painel.Builders.Core
{
    public class ImagemBuilder
    {
        private static ApplicationDbContext _context;
        private ImagemFactory _factory;

        public ImagemBuilder(ApplicationDbContext context)
        {
            _context = context;
            _factory = new ImagemFactory(context);
        }

        public Imagem SaveOrUpdate(Imagem obj)
        {
            try
            {
                obj.DataCriacao = DateTime.Now;
                obj.DataEdicao = DateTime.Now;

                if (obj.Id == 0)
                    _factory.SaveObj(obj);
                else
                    _factory.UpdateObj(obj);
            }
            catch (EntityException)
            {

            }

            return obj;
        }

        public List<Imagem> GetListWithoutTableReference()
        {
            return _factory.GetImagensWithoutTableActionAndTableId();
        }

        public List<Imagem> GetListByTabAndId(string tab, int id)
        {
            return _factory.GetImagemsByTableActionAndTableId(tab, id);
        }

        public List<Imagem> List()
        {
            return _factory.GetAllAtivo().ToList();
        }

        public bool DeleteObj(int id)
        {
            return _factory.DeleteObj(id);
        }
    }
}
