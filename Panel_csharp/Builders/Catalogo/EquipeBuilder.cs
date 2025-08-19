using Data;
using Data.Models.Catalogo;
using System.Linq;
using System.Data.Entity.Core;
using System.Collections.Generic;
using Framework.Factories.Catalogo;

namespace Painel.Builders.Conteudo
{
    public class EquipeBuilder
    {
        private static ApplicationDbContext _context;
        private EquipeFactory _factory;

        /// <summary>
        /// Instância da Classe EquipeBuilder
        /// </summary>
        /// <param name="context"></param>
        public EquipeBuilder(ApplicationDbContext context)
        {
            _context = context;
            _factory = new EquipeFactory(context);
        }

        /// <summary>
        /// Salva ou Atualiza o Objeto
        /// Criar regras lógicas nesta etapa
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Equipe SaveOrUpdate(Equipe obj)
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

        /// <summary>
        /// Lista todos os Ativos para o Index
        /// </summary>
        /// <returns></returns>
        public List<Equipe> List()
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
