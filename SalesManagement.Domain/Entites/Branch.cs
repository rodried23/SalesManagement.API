using System;

namespace SalesManagement.Domain.Entities
{
    public class Branch : EntityBase
    {
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public bool IsActive { get; private set; }

        protected Branch() { }

        public Branch(string name, string address, string city, string state)
        {
            Name = name;
            Address = address;
            City = city;
            State = state;
            IsActive = true;
        }

        public void Update(string name, string address, string city, string state)
        {
            Name = name;
            Address = address;
            City = city;
            State = state;
            UpdatedAtNow();
        }

        public void Activate()
        {
            IsActive = true;
            UpdatedAtNow();
        }

        public void Deactivate()
        {
            IsActive = false;
            UpdatedAtNow();
        }
    }
}