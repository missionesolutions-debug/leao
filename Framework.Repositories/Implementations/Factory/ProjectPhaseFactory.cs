using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Framework.Data.Models.Factory;

namespace Data.Repositories
{
	public class ProjectPhaseFactory
	{
		private readonly ApplicationDbContext _context;

		public ProjectPhaseFactory(ApplicationDbContext context)
		{
			_context = context;
		}

		#region Crud
		public ProjectPhase GetObj(int id)
		{
			return _context.ProjectPhases.FirstOrDefault(pp => pp.Id == id);
		}

		public ProjectPhase SaveObj(ProjectPhase obj)
		{
			_context.ProjectPhases.Add(obj);
			_context.SaveChanges();
			return obj;
		}

		public ProjectPhase UpdateObj(ProjectPhase obj)
		{
			_context.ProjectPhases.Update(obj);
			_context.SaveChanges();
			return obj;
		}

		public List<ProjectPhase> GetAll()
		{
			return _context.ProjectPhases.ToList();
		}

		public List<ProjectPhase> GetList()
		{
			return _context.ProjectPhases.Where(pp => !pp.Excluido).ToList();
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
				_context.ProjectPhases.Update(obj);
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
		public List<ProjectPhase> GetPhasesByProject(int projectId)
		{
			return _context.ProjectPhases
						   .Where(pp => pp.ProjectId == projectId && !pp.Excluido)
						   .ToList();
		}

		public List<ProjectPhase> GetPhasesByProjectItem(int projectItemId)
		{
			return _context.ProjectPhases
						   .Where(pp => pp.ProjectItemId == projectItemId && !pp.Excluido)
						   .ToList();
		}
		#endregion
	}
}
