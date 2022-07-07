
namespace Lab_LINQ
{
     class Reservation
    {
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartDate { get; set; }
        public int Occupants { get; set; }
        public bool IsCurrent { get; set; }
        public Client Client { get; set; }
        public Room Room { get; set; }


        // CONSTRUCTORS
        public Reservation() { }
        public Reservation(int occupants, Client client, Room room)
        {
            Created = DateTime.Now;
            Occupants = occupants;
            IsCurrent = false;
            Client = client;
            Room = room;
        }
    }
}
