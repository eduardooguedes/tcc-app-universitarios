namespace Dashdine.CrossCutting.CrossCutting.CodigoDeAutenticidade
{
    public static class CodigoAutenticidade
    {
        public static string GerarCodigoAleatorioSeisDigitos()
        {
            Random random = new();
            string retorno = "";

            for (int i = 0; i < 6; i++)
            {
                retorno += random.Next(0, 9).ToString();
            }

            return retorno;
        }

    }
}
