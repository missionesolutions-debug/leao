using System.Collections.Generic;
using System.Linq;
using Framework.Data.Models.Factory;

namespace Data.Repositories
{
	public class ClientFactory
	{
		private readonly ApplicationDbContext _context;

		public ClientFactory(ApplicationDbContext context)
		{
			_context = context;
		}

		#region Crud
		public Client GetObj(int id)
		{
			return _context.Clients.FirstOrDefault(c => c.Id == id);
		}

		public Client SaveObj(Client obj)
		{
			_context.Clients.Add(obj);
			_context.SaveChanges();
			return obj;
		}

		public Client UpdateObj(Client obj)
		{
			_context.Clients.Update(obj);
			_context.SaveChanges();
			return obj;
		}

		public List<Client> GetAll()
		{
			return _context.Clients.ToList();
		}

		public List<Client> GetList()
		{
			return _context.Clients.Where(c => !c.Excluido).ToList();
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
				_context.Clients.Update(obj);
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
