using System.Collections.Generic;
using System.Linq;
using Framework.Data.Models.Factory;

namespace Data.Repositories
{
	public class ProjectItemFactory
	{
		private readonly ApplicationDbContext _context;

		public ProjectItemFactory(ApplicationDbContext context)
		{
			_context = context;
		}

		#region Crud
		public ProjectItem GetObj(int id)
		{
			return _context.ProjectItems.FirstOrDefault(pi => pi.Id == id);
		}

		public ProjectItem SaveObj(ProjectItem obj)
		{
			_context.ProjectItems.Add(obj);
			_context.SaveChanges();
			return obj;
		}

		public ProjectItem UpdateObj(ProjectItem obj)
		{
			_context.ProjectItems.Update(obj);
			_context.SaveChanges();
			return obj;
		}

		public List<ProjectItem> GetAll()
		{
			return _context.ProjectItems.ToList();
		}

		public List<ProjectItem> GetList()
		{
			return _context.ProjectItems.Where(pi => !pi.Excluido).ToList();
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
				_context.ProjectItems.Update(obj);
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
		public List<ProjectItem> GetProjectItemsByProject(int projectId)
		{
			return _context.ProjectItems
						   .Where(pi => pi.ProjectId == projectId && !pi.Excluido)
						   .ToList();
		}
		#endregion
	}
}
