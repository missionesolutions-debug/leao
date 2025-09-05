using Data;
using Data.Models.Core;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Factories.Core
{
    public class UserFactory 
    {
        private readonly ApplicationDbContext _context;

        public UserFactory(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Crud

        public User GetObj(int id)
        {
            return _context.User.Where(b => b.Id == id).FirstOrDefault();
        }

        public User SaveObj(User obj)
        {
            _context.Add(obj);
            _context.SaveChanges();

            return obj;
        }

        public User UpdateObj(User obj)
        {
            _context.Update(obj);
            _context.SaveChanges();
            return obj;
        }

        public List<User> GetAll()
        {
            return _context.User.ToList();
        }

        public List<User> GetAllAtivo()
        {
            return _context.User.Where(b => b.Ativo == true).ToList();
        }

        public User GetUserByEmail(string userEmail)
        {
            return _context.User.Where(b => b.Ativo == true && b.Email == userEmail).FirstOrDefault();
        }

        public User GetByEmail(string email)
        {
            return _context.User.Where(b => b.Email == email).FirstOrDefault();
        }

        public List<User> GetAllExcluido()
        {
            return _context.User.Where(b => b.Excluido == true).ToList();
        }

        public Boolean RemoveObj(User obj)
        {
            try
            {
                _context.User.Remove(obj);
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
                var obj = _context.User.Where(b => b.Id == Id).FirstOrDefault();

                obj.Ativo = false;
                obj.Excluido = true;

                _context.User.Update(obj);
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public User Get(string email, string password)
        {
            return _context.User.Where(x => x.Email.ToLower() == email.ToLower() && x.Password == password).FirstOrDefault();
        }
        #endregion
    }
}
