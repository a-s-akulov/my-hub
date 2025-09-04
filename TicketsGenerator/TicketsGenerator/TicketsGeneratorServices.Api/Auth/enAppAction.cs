using System.ComponentModel;


namespace TicketsGeneratorServices.Api.Auth
{
    /// <summary>
    /// Действия приложения для авторизации
    /// </summary>
    public enum enAppAction
    {
        // "Пустая" роль - роль не указана
        None = 0,

        // Partners
        [Description("PartnersRead")] PartnersRead = 1001,

        // MyAwesomeProducts
        [Description("MyAwesomeProductsRead")] MyAwesomeProductsRead = 2001,
        [Description("MyAwesomeProductsAdd")] MyAwesomeProductsAdd = 2002,
        [Description("MyAwesomeProductsUpdate")] MyAwesomeProductsUpdate = 2003,
        [Description("MyAwesomeProductsDelete")] MyAwesomeProductsDelete = 2004
    }
}