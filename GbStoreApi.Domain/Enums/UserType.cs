using System.ComponentModel;

namespace GbStoreApi.Domain.enums
{
    public enum UserType
    {
        [Description("Comum")]
        Common = 1,
        [Description("Administrador")]
        Administrator = 2,
    }
}
