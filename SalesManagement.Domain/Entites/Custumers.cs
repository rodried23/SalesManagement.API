using System;

namespace SalesManagement.Domain.Entities
{
    public class Customer : EntityBase
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }

        protected Customer() { }

        public Customer(string name, string email, string phone)
        {
            Name = name;
            Email = email;
            Phone = phone;
        }

        public void Update(string name, string email, string phone)
        {
            Name = name;
            Email = email;
            Phone = phone;
            UpdatedAtNow();
        }
    }
}