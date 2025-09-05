using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models.Core
{
    public class Contact:Base
    {
        public Nullable<int> TipoContactId { get; set; }
        public Nullable<int> ProdutoId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        [NotMapped]
        public IFormFile CurriculumFile { get; set; }
        public string CurriculumUrl { get; set; }
        public string CurriculumFileName { get; set; } // Propriedade para armazenar temporariamente o nome do arquivo

    }
}
