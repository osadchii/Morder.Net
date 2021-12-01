namespace Infrastructure.Models.Interfaces;

public interface IHasExternalId
{
    Guid ExternalId { get; set; }
}