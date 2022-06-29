using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models.BotUsers;

[Table("BotUserUsageCounter", Schema = "dbo")]
public class BotUserUsageCounter : BaseEntity
{
    [ForeignKey("BotUser")] public int BotUserId { get; set; }

    public BotUser BotUser { get; set; } = null!;

    public long Count { get; set; }

    public DateTime LastUse { get; set; }
}