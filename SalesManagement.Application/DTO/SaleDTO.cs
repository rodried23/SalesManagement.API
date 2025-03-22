using System;
using System.Collections.Generic;
using SalesManagement.Domain.Enums;

namespace SalesManagement.Application.DTOs
{
    public class SaleDTO
    {
        public Guid Id { get; set; }
        public int SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public decimal TotalAmount { get; set; }
        public SaleStatus Status { get; set; }
        public bool IsCanceled { get; set; }
        public ICollection<SaleItemDTO> Items { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}