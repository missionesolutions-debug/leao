using System.Collections.Generic;
using System.Linq;
using Framework.Data.Models.Factory;

namespace Data.Repositories
{
	public class ProjectGroupFactory
	{
		private readonly ApplicationDbContext _context;

		public ProjectGroupFactory(ApplicationDbContext context)
		{
			_context = context;
		}

		#region Crud
		public ProjectGroup GetObj(int id)
		{
			return _context.ProjectGroups.FirstOrDefault(pg => pg.Id == id);
		}

		public ProjectGroup SaveObj(ProjectGroup obj)
		{
			_context.ProjectGroups.Add(obj);
			_context.SaveChanges();
			return obj;
		}

		public ProjectGroup UpdateObj(ProjectGroup obj)
		{
			_context.ProjectGroups.Update(obj);
			_context.SaveChanges();
			return obj;
		}

		public List<ProjectGroup> GetAll()
		{
			return _context.ProjectGroups.ToList();
		}

		public List<ProjectGroup> GetList()
		{
			return _context.ProjectGroups.Where(pg => !pg.Excluido).ToList();
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
				_context.ProjectGroups.Update(obj);
				_context.SaveChanges();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		#endregion

		#region Custom Queries
		public List<ProjectGroup> GetGroupsByProjectPhase(int projectPhaseId)
		{
			return _context.ProjectGroups
						   .Where(pg => pg.ProjectPhaseId == projectPhaseId && !pg.Excluido)
						   .ToList();
		}

		public List<ProjectGroup> GetGroupsByProject(int projectId)
		{
			return _context.ProjectGroups
						   .Where(pg => pg.ProjectId == projectId && !pg.Excluido)
						   .ToList();
		}
		#endregion
	}
}
