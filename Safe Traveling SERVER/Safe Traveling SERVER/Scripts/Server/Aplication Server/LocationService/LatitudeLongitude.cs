using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Safe_Traveling_SERVER.Scripts.Server.Aplication_Server.LS
{
    class LatitudeLongitude
    {
        public string Latitude;
        public string Longitude;

        public LatitudeLongitude(string Lat, string Lon)
        {
            Latitude = Lat;
            Longitude = Lon;
        }

        public string GetDistance(LatitudeLongitude UserPos)
        {
            return GeoCodeCalc.CalcDistanceMeters(double.Parse(UserPos.Latitude), double.Parse(UserPos.Longitude), double.Parse(Latitude), double.Parse(Longitude)).ToString();
        }

        public string GetDistance(string Lat,string Long)
        {
            return GeoCodeCalc.CalcDistanceMeters(double.Parse(Lat), double.Parse(Long), double.Parse(Latitude), double.Parse(Longitude)).ToString();
        }

        public string GetCoord()
        {
            return (GetNormalFormat(Latitude) + "," + GetNormalFormat(Longitude));
        }

        private string GetNormalFormat(string Coord)
        {
            return Coord.Replace(",", ".");
        }
    }

    public static class GeoCodeCalc
    {
        public const double EarthRadiusInKilometers = 6367.0;

        public static double CalcDistanceKm(double lat1, double lng1, double lat2, double lng2)
        {
            return EarthRadiusInKilometers * 2 * Math.Asin(Math.Min(1, Math.Sqrt((Math.Pow(Math.Sin((DiffRadian(lat1, lat2)) / 2.0), 2.0) + Math.Cos(ToRadian(lat1)) * Math.Cos(ToRadian(lat2)) * Math.Pow(Math.Sin((DiffRadian(lng1, lng2)) / 2.0), 2.0)))));
        }

        public static double CalcDistanceMeters(double lat1, double lng1, double lat2, double lng2)
        {
            return Math.Round(CalcDistanceKm(lat1, lng1, lat2, lng2) * 1000);
        }

        private static double ToRadian(double val)
        {
            return val * (Math.PI / 180);
        }
        private static double DiffRadian(double val1, double val2)
        {
            return ToRadian(val2) - ToRadian(val1);
        }
    }

    public static class CalculareViteza
    {
        public static double KmPeSecToKmPeOra(double km, string t1, string t2)
        {
            int[] t1Array = StringToHMS(t1);
            int[] t2Array = StringToHMS(t2);

            double deltaT;

            double deltaH = (t1Array[0] - t2Array[0]) * 3600;
            double deltaM = (t1Array[1] - t2Array[1]) * 60;
            double deltaS = t1Array[2] - t2Array[2];

            deltaT = deltaH + deltaM + deltaS;

            return KmPeSecToKmPeOra(km, deltaT);
        }

        public static double KmPeSecToKmPeOra(double km, double sec)
        {
            return Math.Round((km / sec) * 3600 / 1000);
        }

        private static int[] StringToHMS(string t)
        {
            List<int> HMS = new List<int>();
            foreach (string val in Regex.Split(t, ":"))
                HMS.Add(int.Parse(val));

            return HMS.ToArray();
        }
    }
}
