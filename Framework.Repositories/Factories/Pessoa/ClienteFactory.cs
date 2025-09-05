using Data;
using Data.Models.Pessoa;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Factories.Pessoa
{
    public class ClienteFactory
    {
        private readonly ApplicationDbContext _context;

        public ClienteFactory(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Crud

        public Cliente GetObj(int id)
        {
            return _context.Cliente.Where(b => b.Id == id).FirstOrDefault();
        }

        public Cliente SaveObj(Cliente obj)
        {
            _context.Add(obj);
            _context.SaveChanges();

            return obj;
        }

        public Cliente UpdateObj(Cliente obj)
        {
            _context.Update(obj);
            _context.SaveChanges();
            return obj;
        }

        public List<Cliente> GetAll()
        {
            return _context.Cliente.ToList();
        }

        public List<Cliente> GetList()
        {
            return _context.Cliente.Where(b => b.Excluido != true).ToList();
        }

        public List<Cliente> GetAllAtivo()
        {
            return _context.Cliente.Where(b=> b.Ativo == true).ToList();
        }

        public List<Cliente> GetAllExcluido()
        {
            return _context.Cliente.Where(b => b.Excluido == true).ToList();
        }

        public Boolean RemoveObj(Cliente obj)
        {
            try
            {
                _context.Cliente.Remove(obj);
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        } 

        public Boolean DeleteObj(int Id)
        {
            try
            {
                var obj =_context.Cliente.Where(b => b.Id == Id).FirstOrDefault();

                obj.Ativo = false;
                obj.Excluido = true;

                _context.Cliente.Update(obj);
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
