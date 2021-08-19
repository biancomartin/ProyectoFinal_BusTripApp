using System;

namespace Helper.Functions
{
    public static class ObtenerDiaPorTimestamp
    {
        public static int ObtenerDia(long timestamp)
        {
            try
            {
                return ObtenerFechaAPartirLong.ObtenerDateTime(timestamp).Day;
            }
            catch (Exception ex)
            {
                Exception error = new Exception("Dia no valido", ex.InnerException);
                throw error;
            }
        }
    }
}
