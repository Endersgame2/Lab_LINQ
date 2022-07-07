
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

        public Reservation GetReservation(string clientName)
        {
            // TO-DO: Maybe use clientId instead?
            return Reservations.FirstOrDefault(reservation => reservation.StartDate.Date == DateTime.Now.Date && reservation.Client.Name == clientName); 
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

        public Reservation AutomaticReservation(int clientID, int occupants, DateTime startDate)
        {
            var room = Rooms.Where(room => room.Capacity >= occupants && !room.Reservations.Any(reservation => reservation.StartDate == startDate)).FirstOrDefault();
            return ReserveRoom(room, clientID, occupants, startDate);
        }

        public Reservation ReserveRoom(Room room, int clientID, int occupants, DateTime startDate) { 
        
            if (room == null || room.Capacity < occupants || room.Reservations.Any(reservation => reservation.StartDate == startDate))
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
        public void Checkin(string clientName)
        {
            var reservation = GetReservation(clientName);
            if (reservation == null)
            {
                // Error must be thrown because this is a void function
                throw new ApplicationException("No valid reservation"); 
            }
            reservation.IsCurrent = true;
            reservation.Room.Occupied = true;
        }

            // This should probably be string
        public void CheckoutRoom(int RoomNumber) 
        {
            var room = GetRoom(RoomNumber.ToString());
            var reservation = room.Reservations.FirstOrDefault(reservation => reservation.IsCurrent);
            if (room == null || !room.Occupied || reservation == null || !reservation.IsCurrent)
            {
                // Error must be thrown because this is a void function
                throw new ApplicationException("Room is not occupied");
            }
            reservation.IsCurrent = false;
            room.Occupied = false;
        }

        public void CheckoutRoom(string clientName)
        {
            // Cannot use get reservation for this because date would have changed during checkout
            var reservation = Reservations.FirstOrDefault(x => x.IsCurrent && x.Client.Name == clientName);
            if (reservation == null)
            {
                throw new Exception("Client not checked in");
            }
            CheckoutReservation(reservation);
        }

        private void CheckoutReservation(Reservation reservation)
        {
            reservation.IsCurrent = false;
            reservation.Room.Occupied = false;
        }

        public int TotalCapacityRemaining()
        {
            return Rooms.Sum((room) =>
            {
                var reservation = room.Reservations.FirstOrDefault(reservation => reservation.IsCurrent);
                var occupiedCapacity = reservation != null ? reservation.Occupants : 0;
                return room.Capacity - occupiedCapacity;
            });
        }

        public int AverageOccupancyPercentage()
        {
            //"Average of averages" will not work with rooms of different capacitie. So we shall calculate the total capacity and total occupancy and divide the two
            var totalCapacity = (double)Rooms.Sum(room => room.Capacity);
            var ratio = TotalCapacityRemaining() / totalCapacity;
            return (int)Math.Round(ratio * 100);
        }

        public List<Reservation> FutureBookings()
        {
            // Not simply Reservations.Where(reservation => reservation.StartDate.Date > DateTime.Now) to accounts for today's pending checkins
            var reservations = Reservations.Where(reservation => reservation.StartDate.Date >= DateTime.Now && !reservation.IsCurrent);
            return reservations.ToList();
        }
    }
}