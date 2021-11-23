namespace Infrastructure.Interfaces;

public interface IHasExternalId
{
    Guid ExternalId { get; set; }
}