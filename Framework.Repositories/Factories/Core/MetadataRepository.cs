using Data;
using Framework.Data.Models.Core;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Repositories.Factories.Core
{
    public class MetadataRepository
    {
        private readonly ApplicationDbContext _context;

        public MetadataRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Crud
        public Metadata GetObj(string id)
        {
            // Converte a string 'id' para um Guid
            Guid guidId;

            // Tenta fazer a conversão da string para Guid
            if (Guid.TryParse(id, out guidId))
            {
                // Se a conversão for bem-sucedida, faz a busca no banco de dados
                return _context.Metadata
                    .Where(b => b.Id == guidId)
                    .FirstOrDefault();
            }

            // Retorna nulo se a conversão falhar ou o objeto não for encontrado
            return null;
        }

        public async Task<Metadata> GetObjByGuidAsync(string id)
        {
            return await _context.Metadata.Where(b => b.Guid == id).FirstOrDefaultAsync();
        }
        public Metadata GetObjByGuid(string id)
        {
            return  _context.Metadata.Where(b => b.Guid == id).FirstOrDefault();
        }

        public List<Metadata> GetObjsByGuid(string id)
        {
            return _context.Metadata.Where(b => b.Guid == id).ToList();
        }

        public List<Metadata> GetObjsByRefGuid(string referenc)
        {
            return _context.Metadata.Where(b => b.MetadataRef.ToUpper() == referenc.ToUpper()).ToList();
        }


     

        public Metadata SaveObj(Metadata obj)
        {
            _context.Add(obj);
            _context.SaveChanges();

            return obj;
        }
        public Metadata UpdateObj(Metadata obj)
        {
            _context.Update(obj);
            _context.SaveChanges();
            return obj;
        }
        public List<Metadata> GetAll()
        {
            return _context.Metadata.ToList();
        }

        public Boolean RemoveObj(Metadata obj)
        {
            try
            {
                _context.Metadata.Remove(obj);
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Boolean DeleteObj(string Id)
        {
            try
            {
                var obj = GetObj(Id);

                _context.Metadata.Update(obj);
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
