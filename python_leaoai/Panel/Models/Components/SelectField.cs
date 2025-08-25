namespace Painel.Models.Components
{
    public class SelectField
    {
        /// <summary>
        /// Set Componente Dropdown
        /// </summary>
        /// <param name="Label">Nome da Label</param>
        /// <param name="Name">Nome do Campo</param>
        /// <param name="Value">Valor do Campo</param>
        /// <param name="FieldID">Field ID</param>
        /// <param name="Required">Required quando campo é obrigatório = required</param>
        /// <param name="Class">Classe CSS</param>
        /// <param name="Col">Tamanho da coluna</param>
        public SelectField(String Label, String Name, String Value, String FieldID, String Required = "", String Class = "", int Col = 12)
        {
            this.Label = Label;
            this.Name = Name;
            this.Value = Value;
            this.FieldID = FieldID;
            this.Required = Required;
            this.Class = Class;
            this.Col = Col;
        }

        public String Label { get; set; }
        public String Name { get; set; }
        public String Value { get; set; }
        public String FieldID { get; set; }
        public String Required { get; set; }
        public String Class { get; set; }
        public int Col { get; set; }

    }
}
