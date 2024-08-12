using System.ComponentModel;

namespace GbStoreApi.Domain.Enums
{
    public enum PurchaseState
    {
        [Description("Processando a Compra")]
        Processing,
        [Description("Postado para entrega")]
        PostedToDelivery,
        [Description("Entregue")]
        Delivered,
    }
}
