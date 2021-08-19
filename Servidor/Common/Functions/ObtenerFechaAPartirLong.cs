using System;

namespace Helper.Functions
{
    public static class ObtenerFechaAPartirLong
    {
        public static DateTime ObtenerDateTime(long time)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(time).ToLocalTime();
            return dtDateTime;
        }
    }
}
