using System.ComponentModel;

namespace GbStoreApi.Domain.Enums
{
    public enum DeliveryType
    {
        [Description("Retirada na loja")]
        StorePickup,
        [Description("Entrega no endereço")]
        Shipping
    }
}
