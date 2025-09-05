using System.Collections.Generic;

namespace Data.Models.PainelConfig
{
    public class Menu : Base
    {
        public Menu()
        {
            this.Page = new HashSet<Page>();
        }

        public virtual ICollection<Page> Page { get; set; }

        public string Nome { get; set; }
        public string RoleGate { get; set; }
        public string Icon { get; set; }

    }
}
