using Data.Models.PainelConfig;
using Data.Models.Pessoa;
using Painel.Models.Components;
using System.Collections.Generic;

namespace Painel.Models.Pages
{
    public class UsuariosPage
    {
        public ContentConfig ContentConfig { get; set; }
        public Page Page { get; set; }
        public Info Info { get; set; }
        public List<Usuario> Usuarios { get; set; }
        public Usuario Usuario { get; set; }
    }
}
