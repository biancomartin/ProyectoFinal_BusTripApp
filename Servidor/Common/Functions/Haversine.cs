using Domain.Entities;
using System;

namespace Helper.Functions
{
    /// <summary>
    /// Enum con los tipos de distancias
    /// </summary>
    public enum DistanceType { Miles, Kilometers };

    public class Haversine
    {
        /// <summary>
        /// Retorna la distancia entre dos coordendadas
        /// latitude / longitude points.
        /// </summary>
        /// <param name=”pos1″></param>
        /// <param name=”pos2″></param>
        /// <param name=”type”></param>
        /// <returns></returns>
        public double Distance(Coordenada pos1, Coordenada pos2, DistanceType type)
        {
            double R = (type == DistanceType.Miles) ? 3960 : 6371;

            double dLat = this.ToRadian(pos2.Latitude - pos1.Latitude);
            double dLon = this.ToRadian(pos2.Longitude - pos1.Longitude);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(this.ToRadian(pos1.Latitude)) * Math.Cos(this.ToRadian(pos2.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Asin(Math.Min(1, Math.Sqrt(a)));
            double d = R * c;

            return d;
        }

        /// <summary>
        /// Convertor a radianes
        /// </summary>
        /// <param name=”val”></param>
        /// <returns></returns>
        private double ToRadian(double val)
        {
            return (Math.PI / 180) * val;
        }
    }
}