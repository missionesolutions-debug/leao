using Data;
using Data.Models.Core;
using Framework.Factories.Core;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;

namespace Painel.Builders.Core
{
    public class ArquivoBuilder
    {
        private static ApplicationDbContext _context;
        private ArquivoFactory _factory;

        /// <summary>
        /// Instância da Classe ArquivoBuilder
        /// </summary>
        /// <param name="context"></param>
        public ArquivoBuilder(ApplicationDbContext context)
        {
            _context = context;
            _factory = new ArquivoFactory(context);
        }

        /// <summary>
        /// Salva ou Atualiza o Objeto
        /// Criar regras lógicas nesta etapa
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Arquivo SaveOrUpdate(Arquivo obj)
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

        public List<Arquivo> GetListWithoutTableReference()
        {
            return _factory.GetArquivosWithoutTableActionAndTableId();
        }

        public List<Arquivo> GetListByTabAndId(string tab, int id)
        {
            return _factory.GetArquivosByTableActionAndTableId(tab, id);
        }

        /// <summary>
        /// Lista todos os Ativos para o Index
        /// </summary>
        /// <returns></returns>
        public List<Arquivo> List()
        {
            return _factory.GetAllAtivo().ToList();
        }

        /// <summary>
        /// Deleção lógica no banco
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteObj(int id)
        {
            return _factory.DeleteObj(id);
        }
    }
}
