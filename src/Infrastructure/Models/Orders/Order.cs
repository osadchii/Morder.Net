using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;
using Infrastructure.Models.Interfaces;
using Infrastructure.Models.Marketplaces;
using Infrastructure.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Models.Orders;

[Table("Order", Schema = "dbo")]
public class Order : BaseEntity, IHasId, IHasExternalId
{
    [Key] public int Id { get; set; }

    [Required] public Guid ExternalId { get; set; }

    [Required] [ForeignKey("Marketplace")] public int MarketplaceId { get; set; }

    public Marketplace Marketplace { get; set; }

    [Required]
    [MaxLength(Limits.OrderNumber)]
    public string Number { get; set; }

    public bool Archived { get; set; }

    [Required] public OrderStatus Status { get; set; }

    [Required] public DateTime Date { get; set; }

    [Required] public DateTime ShippingDate { get; set; }

    [MaxLength(Limits.OrderCustomer)] public string Customer { get; set; }

    [MaxLength(Limits.OrderCustomerAddress)]
    public string CustomerAddress { get; set; }

    public DateTime? ConfirmedTimeLimit { get; set; }

    public DateTime? PackingTimeLimit { get; set; }

    public DateTime? ShippingTimeLimit { get; set; }

    public bool ExpressOrder { get; set; }
    public string TrackNumber { get; set; }

    public decimal Sum => Items.Where(i => !i.Canceled).Sum(i => i.Sum);

    public Collection<OrderItem> Items { get; set; }

    public Collection<OrderBox> Boxes { get; set; }

    [Owned]
    public class OrderItem
    {
        [Key] public int Id { get; set; }

        [ForeignKey("Product")] public int ProductId { get; set; }

        [Required] public Product Product { get; set; }

        [Required]
        [Range(Limits.OrderMinimalPrice, Limits.OrderMaximalPrice)]
        public decimal Price { get; set; }

        [Required]
        [Range(Limits.OrderMinimalCount, Limits.OrderMaximalCount)]
        public decimal Count { get; set; }

        public decimal Sum { get; set; }

        public string ExternalId { get; set; }

        public bool Canceled { get; set; }
    }

    [Owned]
    public class OrderBox
    {
        [Key] public int Id { get; set; }

        [ForeignKey("Product")] public int ProductId { get; set; }

        [Required] public Product Product { get; set; }

        [Required]
        [Range(Limits.OrderMinimalCount, Limits.OrderMaximalCount)]
        public decimal Count { get; set; }

        [Required]
        [Range(Limits.OrderBoxMinimalNumber, Limits.OrderBoxMaximalNumber)]
        public int Number { get; set; }
    }
}