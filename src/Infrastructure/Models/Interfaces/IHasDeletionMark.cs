namespace Infrastructure.Models.Interfaces;

public interface IHasDeletionMark
{
    bool DeletionMark { get; set; }
}