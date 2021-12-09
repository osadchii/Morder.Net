using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Common;
using Infrastructure.Models.Interfaces;

namespace Infrastructure.Models.BotUsers;

[Table("BotUser", Schema = "dbo")]
public class BotUser : BaseEntity, IHasId
{
    [Key] public int Id { get; set; }

    public long ChatId { get; set; }

    [MaxLength(Limits.BotUserUserName)] public string? UserName { get; set; }

    [MaxLength(Limits.BotFirstName)] public string? FirstName { get; set; }

    [MaxLength(Limits.BotLastName)] public string? LastName { get; set; }

    public bool Verified { get; set; }

    public bool Administrator { get; set; }
}