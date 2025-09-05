using Data;
using Framework.Data.Models.Core;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.Repositories.Factories.Core
{
    public class SectionRepository
    {
        private readonly ApplicationDbContext _context;

        public SectionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Crud
        public Section GetObj(int id)
        {
            return _context.Section.Where(b => b.Id == id).FirstOrDefault();
        }

        public async Task<Section> GetObjAsync(int id)
        {
            return await _context.Section.Where(b => b.Id == id).FirstOrDefaultAsync();
        }

        public Section GetObjWithTranslation(int id)
        {
            var sectionTranslations = _context.SectionTranslation.Where(b => b.SectionId == id).ToList();

            return _context.Section.Include(s => s.SectionTranslations).FirstOrDefault(s => s.Id == id);

        }


        public Section SaveObj(Section obj)
        {
            _context.Add(obj);
            _context.SaveChanges();

            return obj;
        }
        public Section UpdateObj(Section obj)
        {
            _context.Update(obj);
            _context.SaveChanges();
            return obj;
        }
        public List<Section> GetAll()
        {
            return _context.Section.ToList();
        }

        public Boolean RemoveObj(Section obj)
        {
            try
            {
                _context.Section.Remove(obj);
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
                var obj = _context.Section.Where(b => b.Id == Id).FirstOrDefault();

                _context.Section.Update(obj);
                _context.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Section GetObj(string reference)
        {
            return _context.Section.Where(b => b.Ref == reference).FirstOrDefault();
        }

        public List<Section> GetObjsByPageRef(string page)
        {
            // Primeiro, carrega todas as seções filtrando pelo campo 'Ref'
            List<Section> sections = _context.Section
                .Where(b => b.Ref.Contains(page) || b.Ref.Contains("global")) // Filtra as seções pelo campo Ref contendo a string 'page'
                .ToList();

            List<Metadata> mobileChilds = new List<Metadata>();

            // Para cada seção, carregamos manualmente as traduções associadas
            foreach (var section in sections)
            {
                section.SectionTranslations = _context.SectionTranslation
                    .Where(t => t.SectionId == section.Id) // Carrega as traduções relacionadas a esta seção
                    .ToList();

                // Carrega as imagens (Metadata) associadas à seção
                section.Images = _context.Metadata
                    .Where(m => m.SectionId == section.Id)
                    .ToList();
               
            }

            // Retorna as seções com as traduções carregadas
            return sections;
        }

        #endregion
    }
}
