using Data;
using Data.Models.Catalogo;
using Data.Models.Conteudo;
using Data.Models.Core;
using Data.Models.PainelConfig;
using Data.Models.Pessoa;
using Data.Models.Propriedade;
using Framework.Factories.System;
using Painel.Models;
using Painel.Models.Components;
using Painel.Models.Configs;
using Painel.Models.Pages;
using Painel.Models.Pages.Core;
using System.Collections.Generic;

namespace Painel.Builders
{
	public class PagesBuilder
    {
        private static ApplicationDbContext _context;
        
        private PageFactory _faPage;
        public PagesBuilder(ApplicationDbContext context)
        {
            _context = context;
            _faPage = new PageFactory(context);
        }
        private ContentConfig GenerateContentConfig(Page page)
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
        public ClientesPage BuildViewClientes(Info Info, List<Cliente> clientes)
        {
            Page page = _faPage.GetObj(1);

            return new ClientesPage()
            {
                ContentConfig = GenerateContentConfig(page),
                Page = page,
                Info = Info,
                Clientes = clientes
            };
        }
        public ClientesPage BuildViewCliente(Info Info, Cliente cliente)
        {
            Page page = _faPage.GetObj(1);

            return new ClientesPage()
            {
                ContentConfig = GenerateContentConfig(page),
                Page = page,
                Info = Info,
                Cliente = cliente
            };
        }
        public UsuariosPage BuildViewUsuarios(Info Info, List<Usuario> usuarios)
        {
            Page page = _faPage.GetObj(4);

            return new UsuariosPage()
            {
                ContentConfig = GenerateContentConfig(page),
                Page = page,
                Info = Info,
                Usuarios = usuarios
            };
        }
        public UsuariosPage BuildViewUsuario(Info Info, Usuario usuario)
        {
            Page page = _faPage.GetObj(4);

            return new UsuariosPage()
            {
                ContentConfig = GenerateContentConfig(page),
                Page = page,
                Info = Info,
                Usuario = usuario
            };
        }
        public MenusPage BuildViewMenus(Info Info, List<Menu> menus)
        {
            Page page = _faPage.GetObj(5);

            return new MenusPage()
            {
                ContentConfig = GenerateContentConfig(page),
                Page = page,
                Info = Info,
                Menus = menus
            };
        }
        public MenusPage BuildViewMenu(Info Info, Menu menu)
        {
            Page page = _faPage.GetObj(5);

            return new MenusPage()
            {
                ContentConfig = GenerateContentConfig(page),
                Page = page,
                Info = Info,
                Menu = menu
            };
        }
        public EquipesPage BuildViewEquipes(Info Info, List<Equipe> Equipes)
        {
            Page page = _faPage.GetObj(25);

            return new EquipesPage()
            {
                ContentConfig = GenerateContentConfig(page),
                Page = page,
                Info = Info,
                Equipes = Equipes
            };
        }
        public EquipesPage BuildViewEquipe(Info Info, Equipe Equipe)
        {
            Page page = _faPage.GetObj(25);

            return new EquipesPage()
            {
                ContentConfig = GenerateContentConfig(page),
                Page = page,
                Info = Info,
                Equipe = Equipe
            };
        }
        public PaginaPage BuildViewPaginas(Info Info, List<Pagina> Paginas)
        {
            Page page = _faPage.GetObj(10);

            return new PaginaPage()
            {
                ContentConfig = GenerateContentConfig(page),
                Page = page,
                Info = Info,
                Paginas = Paginas
            };
        }
        public PaginaPage BuildViewPagina(Info Info, Pagina Pagina)
        {
            Page page = _faPage.GetObj(10);

            return new PaginaPage()
            {
                ContentConfig = GenerateContentConfig(page),
                Page = page,
                Info = Info,
                Pagina = Pagina
            };
        }
        public TipoCategoriasPage BuildViewTipoCategorias(Info Info, List<TipoCategoria> clientes)
        {
            Page page = _faPage.GetObj(18);

            return new TipoCategoriasPage()
            {
                ContentConfig = GenerateContentConfig(page),
                Page = page,
                Info = Info,
                TipoCategorias = clientes
            };
        }
        public TipoCategoriasPage BuildViewTipoCategoria(Info Info, TipoCategoria cliente)
        {
            Page page = _faPage.GetObj(18);

            return new TipoCategoriasPage()
            {
                ContentConfig = GenerateContentConfig(page),
                Page = page,
                Info = Info,
                TipoCategoria = cliente
            };
        }
        public CategoriasPage BuildViewCategorias(Info Info, List<Categoria> clientes)
        {
            Page page = _faPage.GetObj(19);

            return new CategoriasPage()
            {
                ContentConfig = GenerateContentConfig(page),
                Page = page,
                Info = Info,
                Categorias = clientes
            };
        }
        public CategoriasPage BuildViewCategoria(Info Info, Categoria cliente)
        {
            Page page = _faPage.GetObj(19);

            return new CategoriasPage()
            {
                ContentConfig = GenerateContentConfig(page),
                Page = page,
                Info = Info,
                Categoria = cliente
            };
        }
        public SubCategoriasPage BuildViewSubCategorias(Info Info, List<SubCategoria> clientes)
        {
            Page page = _faPage.GetObj(20);

            return new SubCategoriasPage()
            {
                ContentConfig = GenerateContentConfig(page),
                Page = page,
                Info = Info,
                SubCategorias = clientes
            };
        }
        public SubCategoriasPage BuildViewSubCategoria(Info Info, SubCategoria cliente)
        {
            Page page = _faPage.GetObj(20);

            return new SubCategoriasPage()
            {
                ContentConfig = GenerateContentConfig(page),
                Page = page,
                Info = Info,
                SubCategoria = cliente
            };
        }
        public ArquivoPage BuildViewArquivos(Info info, List<Arquivo> Arquivos, string tab, int id)
        {
            Page page = _faPage.GetObj(39);

            return new ArquivoPage()
            {
                ContentConfig = GenerateContentConfig(page),
                Page = page,
                Info = info,
                Arquivos = Arquivos,
                Id = id,
                Tab = tab
            };
        }
        public ImagemPage BuildViewImagems(Info info, List<Imagem> Imagems, string tab, int id)
        {
            Page page = _faPage.GetObj(40);

            if (tab is null || tab.Length == 0)
            {
                return new ImagemPage()
                {
                    ContentConfig = GenerateContentConfig(page),
                    Page = page,
                    Info = info,
                    Imagems = Imagems,
                    Id = id,
                    Tab = tab
                };


            }
            else
            {
                Page pageVerify = _faPage.GetByTableAction(tab);

                ImagemPage model = new ImagemPage()
                {
                    ContentConfig = GenerateContentConfig(page),
                    Page = page,
                    Info = info,
                    Imagems = Imagems,
                    Id = id,
                    Tab = tab
                };

                if (pageVerify != null && pageVerify.Id > 0)
                {
                    model.ContentConfig.NavTabsConfig.ShowImageItem = pageVerify.ShowImageItem;
                    model.ContentConfig.NavTabsConfig.ShowFileItem = pageVerify.ShowFileItem;
                }

                return model;
            }

        }
        public ArquivoPage BuildViewArquivos(Info info, List<Arquivo> Arquivos)
        {
            Page page = _faPage.GetObj(39);

            return new ArquivoPage()
            {
                ContentConfig = GenerateContentConfig(page),
                Page = page,
                Info = info,
                Arquivos = Arquivos
            };
        }
        public ImagemPage BuildViewImagems(Info info, List<Imagem> Imagems)
        {
            Page page = _faPage.GetObj(40);

            return new ImagemPage()
            {
                ContentConfig = GenerateContentConfig(page),
                Page = page,
                Info = info,
                Imagems = Imagems,
            };
        }

    }
}
