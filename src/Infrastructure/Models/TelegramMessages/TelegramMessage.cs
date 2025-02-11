using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure.Models.BotUsers;
using Infrastructure.Models.Orders;

namespace Infrastructure.Models.TelegramMessages;

public class TelegramMessage : BaseEntity
{
    [Key] public int Id { get; set; }
    
    [ForeignKey("BotUser")] 
    public int BotUserId { get; set; }
    public BotUser BotUser { get; set; }
    
    [MaxLength(512)]
    public string Text { get; set; }
    [ForeignKey("Order")] 
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int MessageId { get; set; }
}