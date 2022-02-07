namespace Infrastructure.Bot;

public static class MessageConstants
{
    public const string AccessDeniedMessage = "У вас нет доступа для работы с ботом. Обратитесь к администратору." +
                                              "\r\nДля проверки доступа отправьте любое текстовое сообщение.";

    public const string SelectMenuItem = "Выберите пункт меню";

    public const string NoOrders = "В выбранном периоде не было заказов.";

    public const string OnlyAdministratorsMessage = "Для доступа к выбранному разделу необходимы права администратора.";
    public const string EnterNewMinimalPrice = "Отправьте новое значение минимальной цены";
    public const string EnterNewMinimalStock = "Отправьте новое значение минимального остатка";
}