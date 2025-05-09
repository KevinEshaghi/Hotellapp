using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSystem
{
    public class Menu
    {
        private readonly HotelApp hotelApp;

        public Menu(HotelApp hotelApp)
        {
            this.hotelApp = hotelApp ?? throw new ArgumentNullException(nameof(hotelApp));
        }

        public void DisplayMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n=============================");
            Console.WriteLine("   Välkommen till Hotel System");
            Console.WriteLine("=============================");
            Console.ResetColor();
            Console.WriteLine("1. Visa lediga rum");
            Console.WriteLine("2. Gör bokning");
            Console.WriteLine("3. Avboka bokning");
            Console.WriteLine("4. Lägg till kund");
            Console.WriteLine("5. Uppdatera kundinformation");
            Console.WriteLine("6. Lägg till rum");
            Console.WriteLine("7. Uppdatera rumsinformation");
            Console.WriteLine("8. Avsluta");
        }

        public void Run()
        {
            bool running = true;
            while (running)
            {
                DisplayMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAvailableRooms();
                        break;
                    case "2":
                        MakeBooking();
                        break;
                    case "3":
                        CancelBooking();
                        break;
                    case "4":
                        AddCustomer();
                        break;
                    case "5":
                        UpdateCustomerInfo();
                        break;
                    case "6":
                        AddRoom();
                        break;
                    case "7":
                        UpdateRoomInfo();
                        break;
                    case "8":
                        running = false;
                        Console.WriteLine("Programmet avslutas...");
                        break;
                    default:
                        Console.WriteLine("Ogiltigt val, försök igen.");
                        break;
                }
                Console.WriteLine("Tryck på valfri tangent för att fortsätta...");
                Console.ReadKey();
            }
        }

        public void ShowAvailableRooms()
        {
            DateTime startDate = GetFutureDateInput("Ange startdatum (yyyy-mm-dd): ");
            DateTime endDate = GetFutureDateInput("Ange slutdatum (yyyy-mm-dd): ");
            int numOfPeople = GetIntInput("Ange antal personer: ");

            hotelApp.ShowAvailableRooms(startDate, endDate, numOfPeople);
        }

        public void MakeBooking()
        {
            Console.WriteLine("Bokning görs:");
            // Lista kunder
            var customers = hotelApp.Customers;
            for (int i = 0; i < customers.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {customers[i].Name}");
            }
            int customerIndex = GetIntInput("Välj kund (ange siffra): ") - 1;
            if (customerIndex < 0 || customerIndex >= customers.Count)
            {
                Console.WriteLine("Ogiltigt kundval.");
                return;
            }
            var customer = customers[customerIndex];

            // Lista rum
            var rooms = hotelApp.Rooms;
            for (int i = 0; i < rooms.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Rum {rooms[i].RoomNumber} - {rooms[i].RoomType}");
            }
            int roomIndex = GetIntInput("Välj rum (ange siffra): ") - 1;
            if (roomIndex < 0 || roomIndex >= rooms.Count)
            {
                Console.WriteLine("Ogiltigt rumsval.");
                return;
            }
            var room = rooms[roomIndex];

            DateTime startDate = GetFutureDateInput("Ange bokningens startdatum (yyyy-mm-dd): ");
            DateTime endDate = GetFutureDateInput("Ange bokningens slutdatum (yyyy-mm-dd): ");
            if (endDate <= startDate)
            {
                Console.WriteLine("Slutdatum måste vara efter startdatum.");
                return;
            }

            hotelApp.MakeBooking(customer, room, startDate, endDate);
        }

        public void CancelBooking()
        {
            int bookingId = GetIntInput("Ange boknings-ID att avboka: ");
            hotelApp.CancelBooking(bookingId);
        }

        public void AddCustomer()
        {
            Console.WriteLine("Registrera en ny kund:");
            string name = GetStringInput("Ange namn: ");
            string email = GetStringInput("Ange e-post: ");
            string phone = GetStringInput("Ange telefonnummer: ");
            var newCustomer = new Customer(name, email, phone);
            hotelApp.AddCustomer(newCustomer);
        }

        public void UpdateCustomerInfo()
        {
            var customers = hotelApp.Customers;
            for (int i = 0; i < customers.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {customers[i].Name}");
            }
            int customerIndex = GetIntInput("Välj kund att uppdatera (ange siffra): ") - 1;
            if (customerIndex < 0 || customerIndex >= customers.Count)
            {
                Console.WriteLine("Ogiltigt kundval.");
                return;
            }
            var customer = customers[customerIndex];

            string name = GetStringInput($"Nuvarande namn: {customer.Name}. Ange nytt namn (eller tryck enter för att behålla): ");
            if (!string.IsNullOrWhiteSpace(name))
                customer.Name = name;

            string email = GetStringInput($"Nuvarande e-post: {customer.Email}. Ange ny e-post (eller tryck enter för att behålla): ");
            if (!string.IsNullOrWhiteSpace(email))
                customer.Email = email;

            string phone = GetStringInput($"Nuvarande telefonnummer: {customer.Phone}. Ange nytt telefonnummer (eller tryck enter för att behålla): ");
            if (!string.IsNullOrWhiteSpace(phone))
                customer.Phone = phone;

            hotelApp.UpdateCustomer(customer);
        }

        public void AddRoom()
        {
            Console.WriteLine("Registrera ett nytt rum:");
            int roomNumber = GetIntInput("Ange rumsnummer: ");
            string roomType = GetStringInput("Ange rumstyp (Enkelrum/Dubbelrum): ");
            int maxOccupants = GetIntInput("Ange max antal gäster (utan extrasäng): ");

            int extraBeds = 0;
            if (roomType.Equals("Dubbelrum", StringComparison.OrdinalIgnoreCase))
            {
                extraBeds = GetIntInput("Ange antal extrasängar (1 eller 2): ");
                if (extraBeds < 1 || extraBeds > 2)
                {
                    Console.WriteLine("Ogiltigt antal extrasängar. Sätts till 0.");
                    extraBeds = 0;
                }
            }

            var newRoom = new Room(roomNumber, roomType, maxOccupants, extraBeds);
            hotelApp.AddRoom(newRoom);
        }

        public void UpdateRoomInfo()
        {
            var rooms = hotelApp.Rooms;
            for (int i = 0; i < rooms.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Rum {rooms[i].RoomNumber} - {rooms[i].RoomType}");
            }
            int roomIndex = GetIntInput("Välj rum att uppdatera (ange siffra): ") - 1;
            if (roomIndex < 0 || roomIndex >= rooms.Count)
            {
                Console.WriteLine("Ogiltigt val.");
                return;
            }
            var room = rooms[roomIndex];

            int roomNumber = GetIntInput($"Nuvarande rumsnummer: {room.RoomNumber}. Ange nytt rumsnummer (eller skriv samma nummer): ");
            string roomType = GetStringInput($"Nuvarande rumstyp: {room.RoomType}. Ange ny rumstyp (Enkelrum/Dubbelrum) eller tryck enter för att behålla: ");
            if (!string.IsNullOrWhiteSpace(roomType))
                room.RoomType = roomType;

            int maxOccupants = GetIntInput($"Nuvarande max antal gäster (utan extrasäng): {room.MaxOccupants}. Ange nytt värde: ");
            room.MaxOccupants = maxOccupants;

            if (room.RoomType.Equals("Dubbelrum", StringComparison.OrdinalIgnoreCase))
            {
                int extraBeds = GetIntInput($"Nuvarande antal extrasängar: {room.ExtraBeds}. Ange nytt antal extrasängar (1 eller 2): ");
                if (extraBeds >= 1 && extraBeds <= 2)
                    room.ExtraBeds = extraBeds;
                else
                    Console.WriteLine("Ogiltigt antal extrasängar. Ingen ändring gjordes.");
            }
            else
            {
                // För enkelrum sätt extraBeds till 0
                room.ExtraBeds = 0;
            }

            hotelApp.UpdateRoom(room);
        }

        public int GetIntInput(string prompt)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out value))
                    return value;
                Console.WriteLine("Felaktig inmatning, försök igen.");
            }
        }

        public string GetStringInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        public DateTime GetDateInput(string prompt)
        {
            DateTime date;
            while (true)
            {
                Console.Write(prompt);
                if (DateTime.TryParse(Console.ReadLine(), out date))
                    return date;
                Console.WriteLine("Felaktigt datumformat, försök igen.");
            }
        }

        // Säkerställer att datumet är idag eller i framtiden
        public DateTime GetFutureDateInput(string prompt)
        {
            DateTime date;
            while (true)
            {
                Console.Write(prompt);
                if (DateTime.TryParse(Console.ReadLine(), out date))
                {
                    if (date < DateTime.Today)
                        Console.WriteLine("Datumet kan inte vara i det förflutna. Försök igen.");
                    else
                        return date;
                }
                else
                {
                    Console.WriteLine("Felaktigt datumformat, försök igen.");
                }
            }
        }
    }
}
