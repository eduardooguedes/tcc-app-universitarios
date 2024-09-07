namespace Dashdine.CrossCutting.Extensions
{
    public static class DecimalExtensions
    {
        public static decimal ArredondarPreco(this decimal preco) => Math.Round(preco, 2, MidpointRounding.ToPositiveInfinity);
    }
}
