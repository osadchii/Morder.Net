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

    [MaxLength(Limits.BotUserFirstName)] public string? FirstName { get; set; }

    [MaxLength(Limits.BotUserLastName)] public string? LastName { get; set; }

    [MaxLength(Limits.BotUserCurrentState)]
    public string? CurrentState { get; set; }

    [MaxLength(Limits.BotUserCurrentStateKey)]
    public string? CurrentStateKey { get; set; }

    public bool Verified { get; set; }

    public bool Administrator { get; set; }
}