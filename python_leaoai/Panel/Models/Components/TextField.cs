namespace Painel.Models.Components
{
    public class TextField
    {
        /// <summary>
        /// Set Componente TextField
        /// </summary>
        /// <param name="Label">Nome da Label</param>
        /// <param name="Name">Nome do Campo</param>
        /// <param name="Value">Valor do Campo</param>
        /// <param name="Placeholder">Placeholder do Campo</param>
        /// <param name="Required">Required quando campo é obrigatório = required</param>
        /// <param name="Obs">Observação abaixo do input</param>
        /// <param name="Tipo">Tipo do Input</param>
        /// <param name="Class">Classe CSS</param>
        /// <param name="Col">Tamanho da coluna</param>
        public TextField(String Label, String Name, String Value, String Placeholder, String Required = "", String Obs = "", String Tipo = "", String Class = "", int Col = 12, string Id = "", string accept = null)
        {
            this.Value = Value;
            this.Col = Col;
            this.Required = Required;
            this.Obs = Obs;
            this.Name = Name;
            this.Placeholder = Placeholder;
            this.Label = Label;
            this.Tipo = Tipo;
            this.Class = Class;
            this.Id = Id;
            Accept = Accept;
        }

        public String Label { get; set; }
        public String Name { get; set; }
        public String Placeholder { get; set; }
        public String Value { get; set; }
        public String Required { get; set; }
        public String Obs { get; set; }
        public String Tipo { get; set; }
        public String Class { get; set; }
        public int Col { get; set; }
        public string Id { get; set; }
        public string Accept { get; set; }

    }
}