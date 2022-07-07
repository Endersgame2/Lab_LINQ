
namespace Lab_LINQ
{
     class Client
    {
        public int ClientId { get; set; }
        public string Name { get; set; }
        public int CreditCard { get; set; }

        public List<Reservation> Reservations { get; set; }


        public Client()
        {
            Reservations = new List<Reservation>();
        }

        public Client(string name, int creditCard)
        {
            Name = name;
            CreditCard = creditCard;
            Reservations = new List<Reservation>();
        }
    }
}
