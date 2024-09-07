using Dashdine.CrossCutting.Enums.Usuario;

namespace Dashdine.Domain.Domain.Usuario.Cache
{
    public class CacheUsuario
    {
        public string IdUsuario { get; set; }
        public EnumTipoDeUsuario TipoDeUsuario { get; set; }
        public string CodigoGerado { get; set; }
    }
}
