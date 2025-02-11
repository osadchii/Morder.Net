using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.Bot.MediatR.Commands.Orders.Commands;

public static class Constants
{
    public const string ConfirmOrderText = "Подтвердите получение заказа.";
    public const string ApproveConfirmOrderText = "Уверены, что хотите ✅ подтвердить заказ?";
    public const string ApproveCancelOrderText = "Уверены, что хотите \u274c отменить заказ?";

    public static InlineKeyboardMarkup OrderActionChoice(int orderId)
    {
        var buttons = new[]
        {
            InlineKeyboardButton.WithCallbackData("✅ Подтвердить", $"confirm|{orderId}"),
            InlineKeyboardButton.WithCallbackData("\u274c Отменить", $"cancel|{orderId}")
        };

        var inlineKeyboard = new InlineKeyboardMarkup(buttons);
        return inlineKeyboard;
    }
}