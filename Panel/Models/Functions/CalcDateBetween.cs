namespace Painel.Models.Functions
{
    public class CalcDateBetween
    {
        public DateTime Entrada { get; set; }
        public DateTime Saida { get; set; }

        public CalcDateBetween(DateTime _Entrada, DateTime _Saida)
        {
            Entrada = _Entrada;
            Saida = _Saida;

        }



    }
}
