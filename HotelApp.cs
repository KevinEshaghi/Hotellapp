using HotelSystem.HotelSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace HotelSystem
    {
        public class HotelApp
        {
            public readonly ApplicationDbContext _context;

            public List<Room> Rooms => _context.Rooms.ToList();
            public List<Customer> Customers => _context.Customers.ToList();
            public List<Booking> Bookings => _context.Bookings.ToList();

            public HotelApp(ApplicationDbContext context)
            {
                _context = context;

                
                if (!_context.Rooms.Any())
                {
                    var seededRooms = new List<Room>
                {
                   
                    new Room(101, "Enkelrum", 1),
                    
                    new Room(102, "Dubbelrum", 2, 1),
                    
                    new Room(103, "Dubbelrum", 2, 2),
                   
                    new Room(104, "Enkelrum", 1)
                };
                    _context.Rooms.AddRange(seededRooms);
                }

                
                if (!_context.Customers.Any())
                {
                    var seededCustomers = new List<Customer>
                {
                    new Customer("Kally Karlsson", "kally@gmail.com", "072345678"),
                    new Customer("Sofia Petersson", "sofia@gmail.com", "0767773535"),
                    new Customer("Emil Anderson", "emil@gmail.com", "07123456789"),
                    new Customer("Lenny Adolfsson", "lenny@gmail.com", "071245657890+")
                };
                    _context.Customers.AddRange(seededCustomers);
                }
                _context.SaveChanges();
            }

            public void AddRoom(Room newRoom)
            {
                if (newRoom == null)
                {
                    Console.WriteLine("Ogiltig rumsdata.");
                    return;
                }
                _context.Rooms.Add(newRoom);
                _context.SaveChanges();
                Console.WriteLine($"Rum {newRoom.RoomNumber} har registrerats nu :).");
            }

            public void UpdateRoom(Room updatedRoom)
            {
                _context.Rooms.Update(updatedRoom);
                _context.SaveChanges();
                Console.WriteLine($"Rum {updatedRoom.RoomNumber} har uppdaterats. :)");
            }

            public void AddCustomer(Customer newCustomer)
            {
                if (newCustomer == null)
                {
                    Console.WriteLine("Ogiltig kunddata.");
                    return;
                }
                _context.Customers.Add(newCustomer);
                _context.SaveChanges();
                Console.WriteLine($"Kund {newCustomer.Name} har registrerats.");
            }

            public void UpdateCustomer(Customer updatedCustomer)
            {
                _context.Customers.Update(updatedCustomer);
                _context.SaveChanges();
                Console.WriteLine($"Kund {updatedCustomer.Name} har uppdaterats.");
            }

            public void ShowAvailableRooms(DateTime startDate, DateTime endDate, int numOfPeople)
            {
                
                var availableRooms = Rooms.Where(room =>
                {
                    
                    var roomBookings = _context.Bookings.Where(b => b.RoomId == room.roomId).ToList();

                   
                    bool isBooked = roomBookings.Any(b =>
                        startDate < b.EndDate && endDate > b.StartDate);

             
                    int capacity = room.MaxOccupants;
                    if (room.RoomType.Equals("Dubbelrum", StringComparison.OrdinalIgnoreCase))
                    {
                        capacity += room.ExtraBeds;
                    }

                    return !isBooked && capacity >= numOfPeople;
                }).ToList();

                if (availableRooms.Any())
                {
                    Console.WriteLine("Lediga rum:");
                    foreach (var room in availableRooms)
                    {
                        int capacity = room.MaxOccupants;
                        if (room.RoomType.Equals("Dubbelrum", StringComparison.OrdinalIgnoreCase))
                        {
                            capacity += room.ExtraBeds;
                        }
                        Console.WriteLine($"Rum {room.RoomNumber}: {room.RoomType} (Kapacitet: {capacity} personer)");
                    }
                }
                else
                {
                    Console.WriteLine("Inga lediga rum hittades för angivet datumintervall och antal personer.");
                }
            }

            public void MakeBooking(Customer customer, Room room, DateTime startDate, DateTime endDate)
            {
         
                if (startDate < DateTime.Today)
                {
                    Console.WriteLine("Bokningens startdatum får inte ligga i det förflutna.");
                    return;
                }

              
                var conflictingBooking = _context.Bookings.FirstOrDefault(b =>
                    b.RoomId == room.roomId &&
                    startDate < b.EndDate &&
                    endDate > b.StartDate);

                if (conflictingBooking != null)
                {
                    Console.WriteLine($"Rummet är redan bokat från {conflictingBooking.StartDate.ToShortDateString()} till {conflictingBooking.EndDate.ToShortDateString()}.");
                    return;
                }

                var booking = new Booking
                {
                    Room = room,
                    Customer = customer,
                    StartDate = startDate,
                    EndDate = endDate
                };

                _context.Bookings.Add(booking);
                _context.SaveChanges();

                Console.WriteLine($"Bokning genomförd: Rum {room.RoomNumber} bokat av {customer.Name} från {startDate.ToShortDateString()} till {endDate.ToShortDateString()}.");
            }

            public void CancelBooking(int bookingId)
            {
                var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);
                if (booking == null)
                {
                    Console.WriteLine("Ingen bokning hittades med angivet ID.");
                    return;
                }

                _context.Bookings.Remove(booking);
                _context.SaveChanges();
                Console.WriteLine($"Bokning med ID {bookingId} har avbokats.");
            }
        }
    }
