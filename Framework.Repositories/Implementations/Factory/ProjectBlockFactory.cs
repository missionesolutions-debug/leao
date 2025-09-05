using System.Collections.Generic;
using System.Linq;
using Framework.Data.Models.Factory;

namespace Data.Repositories
{
	public class ProjectBlockFactory
	{
		private readonly ApplicationDbContext _context;

		public ProjectBlockFactory(ApplicationDbContext context)
		{
			_context = context;
		}

		#region Crud
		public ProjectBlock GetObj(int id)
		{
			return _context.ProjectBlocks.FirstOrDefault(pb => pb.Id == id);
		}

		public ProjectBlock SaveObj(ProjectBlock obj)
		{
			_context.ProjectBlocks.Add(obj);
			_context.SaveChanges();
			return obj;
		}

		public ProjectBlock UpdateObj(ProjectBlock obj)
		{
			_context.ProjectBlocks.Update(obj);
			_context.SaveChanges();
			return obj;
		}

		public List<ProjectBlock> GetAll()
		{
			return _context.ProjectBlocks.ToList();
		}

		public List<ProjectBlock> GetList()
		{
			return _context.ProjectBlocks.Where(pb => !pb.Excluido).ToList();
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
				_context.ProjectBlocks.Update(obj);
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
		public List<ProjectBlock> GetBlocksByProject(int projectId)
		{
			return _context.ProjectBlocks
						   .Where(pb => pb.ProjectId == projectId && !pb.Excluido)
						   .ToList();
		}

		public List<ProjectBlock> GetBlocksByProjectPhase(int projectPhaseId)
		{
			return _context.ProjectBlocks
						   .Where(pb => pb.ProjectPhaseId == projectPhaseId && !pb.Excluido)
						   .ToList();
		}

		public List<ProjectBlock> GetBlocksForConclude()
		{
			return _context.ProjectBlocks
						   .Where(pb => pb.BlockForConclude && !pb.Excluido)
						   .ToList();
		}

		public ProjectBlock GetBlockByCode(string code)
		{
			return _context.ProjectBlocks
						   .FirstOrDefault(pb => pb.Code == code && !pb.Excluido);
		}

		public List<ProjectBlock> GetBlocksByUser(int usuarioId)
		{
			return _context.ProjectBlocks
						   .Where(pb => pb.UsuarioId == usuarioId && !pb.Excluido)
						   .ToList();
		}
		#endregion
	}
}
