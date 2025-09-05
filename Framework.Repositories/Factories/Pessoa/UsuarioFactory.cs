using Data;
using Data.Models.Pessoa;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Factories.Pessoa
{
    public class UsuarioFactory
    {
        private readonly ApplicationDbContext _context;

        public UsuarioFactory(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Crud

        public Usuario GetObj(int id)
        {
            return _context.Usuario.Where(b => b.Id == id).FirstOrDefault();
        }

        public Usuario SaveObj(Usuario obj)
        {
            _context.Add(obj);
            _context.SaveChanges();

            return obj;
        }

        public Usuario UpdateObj(Usuario obj)
        {
            _context.Update(obj);
            _context.SaveChanges();
            return obj;
        }

        public List<Usuario> GetAll()
        {
            return _context.Usuario.ToList();
        }

        public List<Usuario> GetAllAtivo()
        {
            return _context.Usuario.Where(b=> b.Ativo == true).ToList();
        }

        public List<Usuario> GetAllExcluido()
        {
            return _context.Usuario.Where(b => b.Excluido == true).ToList();
        }

        public Boolean RemoveObj(Usuario obj)
        {
            try
            {
                _context.Usuario.Remove(obj);
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
                var obj =_context.Usuario.Where(b => b.Id == Id).FirstOrDefault();

                obj.Ativo = false;
                obj.Excluido = true;

                _context.Usuario.Update(obj);
                _context.SaveChanges();
                
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Usuario GetObjByLogin(String login)
        {
            return _context.Usuario.Where(b => b.Login == login).FirstOrDefault();
        }

        public Usuario GetByEmail(string email)
        {
            return _context.Usuario.FirstOrDefault(u => u.Email == email);
        }

        public Usuario GetUser(string email)
        {
            return _context.Usuario.Where(b => b.Email == email).FirstOrDefault();
        }

        public Usuario GetUser(string email, string cpf)
        {
            return _context.Usuario.Where(b => b.Email == email).FirstOrDefault();
        }

        public Usuario GetUserStudent(string email, string cpf)
        {
            return _context.Usuario.Where(b => b.Email == email && b.RoleGate == "User").FirstOrDefault();
        }

        public async Task<Usuario> AddUsuarioAsync(Usuario usuario)
        {
             _context.Add(usuario);

            await _context.SaveChangesAsync();

            return usuario;
        }

        public async Task<Usuario> GetUserAsync(string email)
        {
            return await _context.Usuario
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
        }


        public async Task<Usuario> UpdateObjAsync(Usuario usuario)
        {
            _context.Update(usuario);
            await _context.SaveChangesAsync();

            return usuario;
        }

        public async Task<Usuario> GetByPasswordResetGuidAsync(object guid)
        {
            return await _context.Usuario
                 .Where(u => u.PasswordToken == guid && u.PasswordTokenExpiry >= DateTime.Now)
                 .FirstOrDefaultAsync();
        }
        #endregion


    }
}
