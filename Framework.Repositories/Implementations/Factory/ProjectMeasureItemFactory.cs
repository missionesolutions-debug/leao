using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Framework.Data.Models.Factory;

namespace Data.Repositories
{
	public class ProjectMeasureItemFactory
	{
		private readonly ApplicationDbContext _context;

		public ProjectMeasureItemFactory(ApplicationDbContext context)
		{
			_context = context;
		}

		#region Crud
		public ProjectMeasureItem GetObj(int id)
		{
			return _context.ProjectMeasureItems.FirstOrDefault(pmi => pmi.Id == id);
		}

		public ProjectMeasureItem SaveObj(ProjectMeasureItem obj)
		{
			_context.ProjectMeasureItems.Add(obj);
			_context.SaveChanges();
			return obj;
		}

		public ProjectMeasureItem UpdateObj(ProjectMeasureItem obj)
		{
			_context.ProjectMeasureItems.Update(obj);
			_context.SaveChanges();
			return obj;
		}

		public List<ProjectMeasureItem> GetAll()
		{
			return _context.ProjectMeasureItems.ToList();
		}

		public List<ProjectMeasureItem> GetList()
		{
			return _context.ProjectMeasureItems.ToList();
		}

		public bool DeleteObj(int id)
		{
			var obj = GetObj(id);
			if (obj == null)
				return false;
			try
			{
				_context.ProjectMeasureItems.Remove(obj);
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
		public List<ProjectMeasureItem> GetMeasureItemsByProjectBlock(int projectBlockId)
		{
			return _context.ProjectMeasureItems
						   .Where(pmi => pmi.ProjectBlockId == projectBlockId)
						   .ToList();
		}
		#endregion
	}
}
