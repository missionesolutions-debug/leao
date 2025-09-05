namespace Framework.Domain.Dtos.Profile
{
    public class UserMeDto
    {
        public string Avatar { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public int Id { get; set; }
        public string Imagem { get; set; }
        public bool IsExternal { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string State { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public string Role { get; set; }
        public int JourneyId { get; set; }
        public int SubscriptionId { get; set; }
        public bool HasJourney { get; set; }
        public bool IsSubscriptionActive {  get; set; } 
    }

}
