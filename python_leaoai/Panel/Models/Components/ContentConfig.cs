using Painel.Models.Configs;

namespace Painel.Models.Components
{
    public class ContentConfig
    {
        public String Nome { get; set; }
        public String Tela { get; set; }
        public String Route { get; set; }
        public String Backplace { get; set; }
        public String Obs { get; set; }
        public String Post { get; set; }
        public String DeleteRoute { get;set; }
        public String DetailRoute { get; set; }
        public int Id { get; set; }
        public String Table { get; set; }
        public bool NavTabs { get; set; }
        public bool IsContentTime { get; set; }
        public bool ModalForm { get; set; }
        public NavTabsConfig NavTabsConfig { get; set; }
        public bool TabSection { get; set; } = false;
        public string OwnerType { get; set; }
        public int OwnerId { get; set; }
    }
}
