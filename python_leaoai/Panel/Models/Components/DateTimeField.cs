namespace Painel.Models.Components
{
    public class DateTimeField
	{
		/// <summary>
		/// Set Componente DateTimeField
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
		public DateTimeField(string Label, string Name, DateTime? Value, string Required = "", string Obs = "", string Class = "", int Col = 12)
		{
			this.Value = Value;
			this.Col = Col;
			this.Required = Required;
			this.Obs = Obs;
			this.Name = Name;
			this.Label = Label;
			this.Class = Class;
		}

		public string Label { get; set; }
		public string Name { get; set; }
		public DateTime? Value { get; set; }
		public string Required { get; set; }
		public string Obs { get; set; }
		public string Class { get; set; }
		public int Col { get; set; }

	}
}