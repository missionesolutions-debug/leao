using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;
using Framework.Data.Models.Factory;

namespace Data.Repositories
{
	public class ProjectImageFactory
	{
		private readonly ApplicationDbContext _context;

		public ProjectImageFactory(ApplicationDbContext context)
		{
			_context = context;
		}

		#region Crud
		public ProjectImage GetObj(int id)
		{
			return _context.ProjectImages.FirstOrDefault(pi => pi.Id == id);
		}

		public ProjectImage SaveObj(ProjectImage obj)
		{
			_context.ProjectImages.Add(obj);
			_context.SaveChanges();
			return obj;
		}

		public ProjectImage UpdateObj(ProjectImage obj)
		{
			_context.ProjectImages.Update(obj);
			_context.SaveChanges();
			return obj;
		}

		public List<ProjectImage> GetAll()
		{
			return _context.ProjectImages.ToList();
		}

		public List<ProjectImage> GetList()
		{
			return _context.ProjectImages.ToList();
		}

		public bool DeleteObj(int id)
		{
			var obj = GetObj(id);
			if (obj == null)
				return false;
			try
			{
				_context.ProjectImages.Remove(obj);
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
		public List<ProjectImage> GetImagesByProject(int projectId)
		{
			return _context.ProjectImages
						   .Where(pi => pi.ProjectId == projectId)
						   .ToList();
		}

		public List<ProjectImage> GetImagesByProjectBlock(int projectBlockId)
		{
			return _context.ProjectImages
						   .Where(pi => pi.ProjectBlockId == projectBlockId)
						   .ToList();
		}
		#endregion
	}
}
