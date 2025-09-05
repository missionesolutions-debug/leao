namespace Painel.Models.Components
{
    public class DropItem
    {
        public DropItem(String Name, String Value)
        {
            this.Name = Name;
            this.Value = Value;
           
        }

        public DropItem(String Name, String Value, bool Selected)
        {
            this.Name = Name;
            this.Value = Value;
            this.Selected = Selected;
        }

        public bool Selected { get; set; }
        public String Name { get; set; }
        public String Value { get; set; }
    }
}
