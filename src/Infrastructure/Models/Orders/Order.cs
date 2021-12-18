using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;
using Infrastructure.Models.Interfaces;
using Infrastructure.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Models.Orders;

[Table("Order", Schema = "dbo")]
public class Order : BaseEntity, IHasId, IHasExternalId
{
    [Key] public int Id { get; set; }
    public Guid ExternalId { get; set; }

    [MaxLength(Limits.OrderNumber)] public string Number { get; set; }

    public DateTime Date { get; set; }

    public decimal Sum => Items.Sum(i => i.Sum);

    public Collection<OrderItem> Items { get; set; }

    [Owned]
    public class OrderItem
    {
        [Key] public int Id { get; set; }

        [ForeignKey("Product")] public int ProductId { get; set; }

        [Required] public Product Product { get; set; }

        public decimal Price { get; set; }
        public decimal Count { get; set; }
        public decimal Sum { get; set; }
    }
}