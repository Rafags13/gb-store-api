using System.ComponentModel;

namespace GbStoreApi.Domain.Enums
{
    public enum PaymentMethod
    {
        [Description("Cartão de crédito")]
        CreditCard,
        [Description("Dinheiro")]
        Money
    }
}
