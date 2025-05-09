using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelSystem
{
    public class Customer
    {
        public int CustomerId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public Customer(string name, string email, string phone)
        {
            Name = name;
            Email = email;
            Phone = phone;
        }

      //  public Customer() { }

        public void UpdateInfo(string name, string email, string phone)
        {
            Name = name;
            Email = email;
            Phone = phone;
        }
    }
}

