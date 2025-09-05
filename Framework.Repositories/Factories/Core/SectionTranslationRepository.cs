using Data;
using Framework.Data.Models.Core;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Repositories.Factories.Core
{
    public class SectionTranslationRepository
    {
        private readonly ApplicationDbContext _context;

        public SectionTranslationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Crud
        public SectionTranslation GetObj(int id)
        {
            return _context.SectionTranslation.Where(b => b.Id == id).FirstOrDefault();
        }
        public SectionTranslation SaveObj(SectionTranslation obj)
        {
            _context.Add(obj);
            _context.SaveChanges();

            return obj;
        }
        public SectionTranslation UpdateObj(SectionTranslation obj)
        {
            _context.Update(obj);
            _context.SaveChanges();
            return obj;
        }
        public List<SectionTranslation> GetAll()
        {
            return _context.SectionTranslation.ToList();
        }
       
        public Boolean RemoveObj(SectionTranslation obj)
        {
            try
            {
                _context.SectionTranslation.Remove(obj);
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
                var obj = _context.SectionTranslation.Where(b => b.Id == Id).FirstOrDefault();

                _context.SectionTranslation.Update(obj);
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
