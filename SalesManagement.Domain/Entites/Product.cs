using System;
using SalesManagement.Domain.Exceptions;

namespace SalesManagement.Domain.Entities
{
    public class Product : EntityBase
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }
        public decimal Price { get; private set; }
        public string ImageUrl { get; private set; }
        public bool IsActive { get; private set; }

        protected Product() { }

        public Product(string name, string description, string category, decimal price, string? imageUrl = null)
        {
            if (price <= 0)
                throw new DomainException(@"O preço deve ser maior que zero");

            Name = name;
            Description = description;
            Category = category;
            Price = price;
            ImageUrl = imageUrl;
            IsActive = true;
        }

        public void Update(string name, string description, string category, decimal price, string imageUrl)
        {
            if (price <= 0)
                throw new DomainException(@"O preço deve ser maior que zero");

            Name = name;
            Description = description;
            Category = category;
            Price = price;
            ImageUrl = imageUrl;
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