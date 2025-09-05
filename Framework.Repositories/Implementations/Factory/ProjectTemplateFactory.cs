using System;
using System.Collections.Generic;
using System.Linq;
using Data.Models;

namespace Data.Repositories
{
    public class ProjectTemplateFactory
    {
        private readonly ApplicationDbContext _context;

        public ProjectTemplateFactory(ApplicationDbContext context)
        {
            _context = context;
        }

        #region CRUD
        public ProjectTemplate GetObj(int id)
        {
            return _context.ProjectTemplates.FirstOrDefault(pt => pt.Id == id && !pt.Excluido);
        }

        public ProjectTemplate SaveObj(ProjectTemplate obj)
        {
            _context.ProjectTemplates.Add(obj);
            _context.SaveChanges();
            return obj;
        }

        public ProjectTemplate UpdateObj(ProjectTemplate obj)
        {
            _context.ProjectTemplates.Update(obj);
            _context.SaveChanges();
            return obj;
        }

        public List<ProjectTemplate> GetAll()
        {
            return _context.ProjectTemplates.ToList();
        }

        public List<ProjectTemplate> GetList()
        {
            return _context.ProjectTemplates.Where(pt => !pt.Excluido).ToList();
        }

        public bool DeleteObj(int id)
        {
            try
            {
                var obj = GetObj(id);
                if (obj == null)
                    return false;
                obj.Ativo = false;
                obj.Excluido = true;
                _context.ProjectTemplates.Update(obj);
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
