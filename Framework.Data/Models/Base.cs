using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public abstract class Base
    {
        [Key]
        public int Id { get; set; }
        public Nullable<bool> Ativo { get; set; }
        public Nullable<bool> Excluido { get; set; }
        public Nullable<bool> Destaque { get; set; }
        public Nullable<int> Ordem { get; set; }
        public Nullable<System.DateTime> DataCriacao { get; set; }
        public Nullable<System.DateTime> DataEdicao { get; set; }
      

    }
}
