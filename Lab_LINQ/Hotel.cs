
namespace Lab_LINQ
{
    class Hotel
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public List<Client> Clients { get; set; }
        public List<Room> Rooms { get; set; }
        public List<Reservation> Reservations { get; set; }



        public Hotel(string name, string address)
        {
            Name = name;
            Address = address;
            Clients = new List<Client>();
            Rooms = new List<Room>();
            Reservations = new List<Reservation>();
        }
        public Client GetClient(int clientID)
        {
            return Clients?.FirstOrDefault(client => client.ClientId == clientID);
        }

        public Reservation GetReservation(int ID)
        {
            return Reservations?.FirstOrDefault(reservation => reservation.ID == ID);
        }

        public Room GetRoom(string roomNumber)
        {
            return Rooms.FirstOrDefault(room => room.Number == roomNumber);
        }

        public List<Room> GetVacantRooms()
        {
            return Rooms.Where(room => !room.Occupied).ToList();
        }

        public List<Client> TopThreeClients()
        {
            return Clients.OrderByDescending(client => client.Reservations.Count).Take(3).ToList();
        }

        public Reservation AutomaticReservation(int clientID, int occupants)
        {
            var room = Rooms.Where(room => room.Capacity >= occupants).FirstOrDefault();
            if (room == null)
                return null;
            var client = GetClient(clientID);
            var reservation = new Reservation(occupants, client, room)
            {
                //TO-DO: Check for integer overflow. Use long instead of int?
                ID = Reservations.Max(x => x.ID) + 1
            };
            client.Reservations.Add(reservation);
            room.Reservations.Add(reservation);
            Reservations.Add(reservation);
            return reservation;
        }
    }
}