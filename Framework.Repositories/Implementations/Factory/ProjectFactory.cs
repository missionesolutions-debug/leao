using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Framework.Data.Models.Factory;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class ProjectFactory
    {
        private readonly ApplicationDbContext _context;

        public ProjectFactory(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Crud Básico
        public Project GetObj(int id)
        {
            return _context.Projects.FirstOrDefault(p => p.Id == id && !p.Excluido);
        }

        public Project SaveObj(Project obj)
        {
            _context.Projects.Add(obj);
            _context.SaveChanges();
            return obj;
        }

        public Project UpdateObj(Project obj)
        {
            _context.Projects.Update(obj);
            _context.SaveChanges();
            return obj;
        }

        public List<Project> GetAll()
        {
            return _context.Projects.ToList();
        }

        public List<Project> GetList()
        {
            return _context.Projects.Where(p => !p.Excluido).ToList();
        }

        public bool DeleteObj(int id)
        {
            var obj = GetObj(id);
            if (obj == null)
                return false;
            try
            {
                obj.Ativo = false;
                obj.Excluido = true;
                _context.Projects.Update(obj);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region Métodos Full (com Includes)
        /// <summary>
        /// Retorna um Project com todas as propriedades de navegação carregadas.
        /// </summary>
        public Project GetObjFull(int id)
        {
            return _context.Projects
                .Include(p => p.ProjectStatus)
                .Include(p => p.Client)
                .Include(p => p.ProjectUsuarios)
                .Include(p => p.ProjectItems)
                    .ThenInclude(pi => pi.ProjectPhases)
                        .ThenInclude(pp => pp.ProjectGroups)
                            .ThenInclude(pg => pg.ProjectBlocks)
                .FirstOrDefault(p => p.Id == id && !p.Excluido);
        }

        /// <summary>
        /// Retorna todos os Projects ativos, com as propriedades de navegação carregadas.
        /// </summary>
        public List<Project> GetAllFull()
        {
            return _context.Projects
                .Include(p => p.ProjectStatus)
                .Include(p => p.Client)
                .Include(p => p.ProjectUsuarios)
                .Include(p => p.ProjectItems)
                .Where(p => !p.Excluido)
                .ToList();
        }

        public List<Project> GetAllFullMech()
        {
            return _context.Projects
                .Include(p => p.ProjectStatus)
                .Include(p => p.Client)
                .Include(p => p.ProjectUsuarios)
                .Include(p => p.ProjectItems)
                    .ThenInclude(pi => pi.ProjectPhases)
                        .ThenInclude(pp => pp.ProjectGroups)
                            .ThenInclude(pg => pg.ProjectBlocks)
                                .ThenInclude(pb => pb.ProjectMeasureItems)
                .Include(p => p.ProjectItems)
                    .ThenInclude(pi => pi.ProjectPhases)
                        .ThenInclude(pp => pp.ProjectGroups)
                            .ThenInclude(pg => pg.ProjectBlocks)
                                .ThenInclude(pb => pb.ProjectImages)
                .Where(p => !p.Excluido)
                .ToList();
        }

        #endregion

        #region Filtros Customizados
        /// <summary>
        /// Retorna os projects filtrados por Client.
        /// </summary>
        public List<Project> GetProjectsByClient(int clientId)
        {
            return _context.Projects
                .Include(p => p.ProjectStatus)
                .Include(p => p.Client)
                .Include(p => p.Supplier)
                .Include(p => p.ProjectUsuarios)
                .Include(p => p.ProjectItems)
                .Where(p => p.ClientId == clientId && !p.Excluido)
                .ToList();
        }

        /// <summary>
        /// Retorna os projects filtrados por Supplier.
        /// </summary>
        public List<Project> GetProjectsBySupplier(int supplierId)
        {
            return _context.Projects
                .Include(p => p.ProjectStatus)
                .Include(p => p.Client)
                .Include(p => p.Supplier)
                .Include(p => p.ProjectUsuarios)
                .Include(p => p.ProjectItems)
                .Where(p => p.SupplierId == supplierId && !p.Excluido)
                .ToList();
        }

        /// <summary>
        /// Retorna os projects nos quais o usuário informado tem vínculo na tabela ProjectUsuario.
        /// </summary>
        public List<Project> GetProjectsByUsuario(int usuarioId)
        {
            return _context.Projects
                .Include(p => p.ProjectStatus)
                .Include(p => p.Client)
                .Include(p => p.Supplier)
                .Include(p => p.ProjectUsuarios)
                .Include(p => p.ProjectItems)
                .Where(p => p.ProjectUsuarios.Any(pu => pu.UsuarioId == usuarioId) && !p.Excluido)
                .ToList();
        }
        #endregion
    }
}
