using System.Collections.Generic;
using System.Linq;
using Framework.Data.Models.Factory;

namespace Data.Repositories
{
	public class SupplierFactory
	{
		private readonly ApplicationDbContext _context;

		public SupplierFactory(ApplicationDbContext context)
		{
			_context = context;
		}

		#region Crud
		public Supplier GetObj(int id)
		{
			return _context.Suppliers.FirstOrDefault(s => s.Id == id);
		}

		public Supplier SaveObj(Supplier obj)
		{
			_context.Suppliers.Add(obj);
			_context.SaveChanges();
			return obj;
		}

		public Supplier UpdateObj(Supplier obj)
		{
			_context.Suppliers.Update(obj);
			_context.SaveChanges();
			return obj;
		}

		public List<Supplier> GetAll()
		{
			return _context.Suppliers.ToList();
		}

		public List<Supplier> GetList()
		{
			return _context.Suppliers.Where(s => !s.Excluido).ToList();
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
				_context.Suppliers.Update(obj);
				_context.SaveChanges();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
		#endregion
	}
}
