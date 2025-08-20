using Data.Models.PainelConfig;
using Painel.Models.Components;
using Painel.Models.Configs;

namespace Panel.Builders
{
	public static class ContentBuilder
	{

		#region Generate
		public static ContentConfig GenerateContentConfig(Page page)
		{
			ContentConfig model = new ContentConfig()
			{
				//Nome - Page
				Nome = page.Nome,
				//Route - List
				Route = page.Backplace,
				//Route - Delete
				DeleteRoute = page.DeleteRoute,
				//Route - Create/Edit
				Post = page.Post,
				//Route - Detail
				DetailRoute = page.DetailRoute,
				//
				Tela = page.Tela,
				//Observações da Tela
				Obs = page.Obs,
				//
				Table = page.TableAction,
				//
				IsContentTime = true,
				//Show Tabs
				NavTabs = page.NavTabs,
				//Wich TabItem To Show
				NavTabsConfig = new NavTabsConfig()
				{
					ShowTabs = page.NavTabs,
					ShowUsersJobTask = page.ShowUsersJobTask,
					ShowContentDetailItem = page.ShowContentDetailItem,
					ShowFileItem = page.ShowFileItem,
					ShowImageItem = page.ShowImageItem,
					ShowJobTasksItem = page.ShowJobTaskItem
				}
			};

			return model;
		}
		#endregion
	}
}
