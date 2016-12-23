using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace samsonkina_map
{
    public class Convert
    {
        //долгота
        double longitude; 
        //широта
        double latitude;
        //зум
        int zoom;


        public Convert(double lon, double lat, int z)
        {
            longitude = lon;
            latitude = lat;
            zoom = z;
        }

        // перевод координат тайла в мировые координаты
        public PointF TileToWorldPos()
        {
            PointF p = new Point();
            float n = (float)(Math.PI - ((2.0 * Math.PI * latitude) / Math.Pow(2.0, zoom)));

            p.X = (float)((longitude / Math.Pow(2.0, zoom) * 360.0) - 180.0);
            p.Y = (float)(180.0 / Math.PI * Math.Atan(Math.Sinh(n)));

            return p;
        }

        // перевод мировых координат в координаты тайла
        public Point WorldToTilePosSpheroid()
        {
            Point p = new Point();
            p.X = (int)(((longitude + 180.0) / 360.0 * (1 << zoom)));
            p.Y = (int)((1.0 - Math.Log(Math.Tan(latitude * Math.PI / 180.0) +
                1.0 / Math.Cos(latitude * Math.PI / 180.0)) / Math.PI) / 2.0 * (1 << zoom));

            return p;
        }



    }
}
