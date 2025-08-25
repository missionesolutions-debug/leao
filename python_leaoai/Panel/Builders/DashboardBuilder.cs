using Data;
using Data.Models.Pessoa;
using Framework.Factories.System;
using Painel.Models.Pages;
using System.Threading.Tasks;

namespace Painel.Builders
{
    public class DashboardBuilder
    {
        private PageFactory _faPage;
        public DashboardBuilder(ApplicationDbContext context, Usuario usuario)
        {
            _faPage = new PageFactory(context);
        }

        public async Task<DashboardPage> Build()
        {
            DashboardPage page = new DashboardPage()
            {
                Page = _faPage.GetObj(31),
            };

            return page;
        }

    }
}
