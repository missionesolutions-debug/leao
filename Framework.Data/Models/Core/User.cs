using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Data.Models.Core
{
	public class User
    {

        public User()
        {
        }

        [Key]
        public int Id { get; set; }
        public Nullable<bool> Ativo { get; set; } = true;
        public Nullable<bool> Excluido { get; set; } = false;
        public Nullable<System.DateTime> DataCriacao { get; set; } = DateTime.Now;
        public Nullable<System.DateTime> DataEdicao { get; set; } = DateTime.Now;
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Avatar { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Guid { get; set; }
        public string Birthday { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public string Cpf { get; set; }
    }
}
