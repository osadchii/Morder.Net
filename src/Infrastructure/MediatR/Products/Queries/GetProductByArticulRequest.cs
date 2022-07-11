using System.ComponentModel.DataAnnotations;
using Infrastructure.Common;
using Infrastructure.Models.Products;
using MediatR;

namespace Infrastructure.MediatR.Products.Queries;

public class GetProductByArticulRequest : IRequest<ProductDto>
{
    [Required]
    [MaxLength(Limits.ProductArticul)]
    public string Articul { get; set; }
}