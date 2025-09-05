namespace Framework.Domain.Dtos.Payments
{
    public class SubscriptionRequest
    {
        public string CardNumber { get; set; }           // Número do cartão
        public string CardHolderName { get; set; }       // Nome do portador do cartão
        public string HolderDocument { get; set; }       // CPF ou CNPJ do portador do cartão (opcional)
        public int ExpMonth { get; set; }                // Mês de validade do cartão
        public int ExpYear { get; set; }                 // Ano de validade do cartão
        public string Cvv { get; set; }                  // Código de segurança do cartão

     
    }



}
