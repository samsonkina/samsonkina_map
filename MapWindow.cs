using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace samsonkina_map
{
    public partial class MapWindow : Form
    {
        int zoom = 15;

        public MapWindow()
        {
            Convert ToConvert = new Convert(DownloadWindow.SelectedCity.longitude, DownloadWindow.SelectedCity.latitude, zoom);
            tile = ToConvert.WorldToTilePosSpheroid();
            InitializeComponent();
        }

        Bitmap background = new Bitmap(5 * 256, 5 * 256);

        bool firstload = true;
        /// <summary>
        /// для отрисовки карты в панель пытаемся загрузить скаченные карты из папки функцией OpenTile()
        /// </summary>
        private void panel_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                // если первый запуск создаем путь до папки
                if (firstload)
                {
                    CreateFilePath();
                    firstload = false;
                }

                Rectangle rect = new Rectangle(); //прямоугольник для отрисовки 
                rect.X = 0;
                rect.Y = 0;
                rect.Height = 256;
                rect.Width = 256;
                //отрисовываем тайл
                PaintTile();

                //то что видим
                var visibleRect = new Rectangle((background.Size.Width - panel.Size.Width) / 2 + delta.X - (dx * 256), (background.Size.Height - panel.Size.Height) / 2 + delta.Y - (dy * 256) /*cx*/, panel.Width, panel.Height); // ПРЯМОУГОЛЬНИК КОТОРЫЙ НАДО ВОТКНУТЬ В  ТО ЧТО НАДО ВИДЕТЬ
                                                                                                                                                                                                                                   // создаем битмап размером с панельку
                var bmp = new Bitmap(panel.Width, panel.Height); // ТО ЧТО НАДО ВИДЕТЬ

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.DrawImage(background, 0, 0, visibleRect, GraphicsUnit.Pixel); // ФОН ВПИХИВАЕМ В ТО ЧТО НАДО ВИДЕТЬ
                }
                e.Graphics.DrawImage(bmp, 0, 0);

            }
            catch
            {
                MessageBox.Show("Невозможно загрузить карту!");
            }

        }
        //путь к дефолтной картинке
        string notfound = "notfound.png";

        Point tile = new Point();
        Point from = new Point();
        Point to = new Point();

        //переменные для работы с мышкой
        int dx = 0;
        int dy = 0;
        Point delta;

        /// <summary>
        /// открываем поочередно необходимые тайлы
        /// </summary>
        private void PaintTile()
        {

            Image newImage;
            int _i = 0;
            int _j = 0;
            string currentimg = "";
            // смещение мышки
            dx = delta.X / 256;
            dy = delta.Y / 256;
            Point temp = new Point();
            temp.X = from.X + dx;
            temp.Y = from.Y + dy;
            using (Graphics g = Graphics.FromImage(background))  // ЗАПОЛНЯЕМ ФОН
            {
                for (int i = (from.X + dx); i <= (to.X + dx); i++)
                {
                    for (int j = (from.Y + dy); j <= (to.Y + dy); j++)
                    {
                        // путь до тайла
                        currentimg = catalogPath + i.ToString() + "_" + j.ToString() + ".png";

                        if (!File.Exists(currentimg))
                        {
                            // загружаем если нет
                            currentimg = DownloadTile(i, j);
                        }

                        try
                        {
                            // врисовывам в фон
                            using (Bitmap b = new Bitmap(currentimg))
                            {
                                newImage = Image.FromFile(currentimg);
                                g.DrawImage(newImage, _i, _j);
                            }
                        }
                        catch
                        {
                            MessageBox.Show("err");
                        }

                        if ((j - temp.Y == 2) && ((i - temp.X == 2)))
                        {
                            tile.X = i;
                            tile.Y = j;
                        }
                        // переход к следующей точке отрисовки
                        _j += 256; ;
                    }
                    // переход к следующей точке отрисовки
                    _i += 256;
                    _j = 0;

                }

            }


        }

        string catalogPath = DownloadWindow.catalogPath;

        private void CreateFilePath()
        {
            //путь до папки
            catalogPath += zoom + "\\";
            // создали если нет
            if (!Directory.Exists(catalogPath))
                Directory.CreateDirectory(catalogPath);
            // перевели географические координаты в координаты тайла

            // от какого тайла рисуем
            from.X = tile.X - 2;
            from.Y = tile.Y - 2;
            //до какого
            to.X = tile.X + 2;
            to.Y = tile.Y + 2;

        }
        /// <summary>
        /// пытаемся загрузить тайл
        /// </summary>
        /// <returns></returns>
        private string DownloadTile(int x, int y)
        {
            bool saved = false;
            // количество серверов сервиса
            int count = DownloadWindow.SelectedServer.servers.Length;
            int i = 0;
            // путь к дефолтной картинке
            string savepath = notfound;
            while (!saved)
            {
                try
                {
                    //сохраняем изображние и выходим
                    savepath = SaveImage(i, x, y, zoom);
                    saved = true;
                }
                catch
                {
                    // передираем сервера
                    i++;
                    // если дошли до последнего сервера из списка, то выходим из цикла
                    if (i == count)
                        saved = true;
                }
            }
            return savepath;
        }

        /// <summary>
        /// сохраняем тайл
        /// </summary>
        /// <param name="index">индекс сервера</param>
        /// <param name="x">широта тайла</param>
        /// <param name="y">долгота тайла</param>
        /// <param name="zoom">масштаб</param>
        /// <returns></returns>
        private string SaveImage(int index, int x, int y, int zoom)
        {
            //формируем ссылку для скачивания
            string sURL = "";
            sURL = String.Format(DownloadWindow.SelectedServer.url, DownloadWindow.SelectedServer.servers[index], x, y, zoom);

            var hwr = (HttpWebRequest)WebRequest.Create(sURL);
            hwr.UserAgent = "Chrome/45.0.2454.93";
            //путь для сохранения
            string savepath = catalogPath;
            using (var response = hwr.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    using (var image = Bitmap.FromStream(stream))
                    {
                        // формируем путь и сохраняем
                        savepath += x + "_" + y + ".png";
                        if (!File.Exists(savepath))
                            image.Save(savepath);
                    }
                }
            }
            return savepath;
        }



        Point begin;
        Point end;
        bool isDragging = false;
        bool first = true;


        private void panel_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        private void panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                end.X = begin.X - e.X;
                end.Y = begin.Y - e.Y;

                delta.X += end.X;
                delta.Y += end.Y;

                begin.X = e.X;
                begin.Y = e.Y;

                panel.Invalidate();
            }
        }

        private void panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (first)
                first = false;

            isDragging = true;

            begin.X = e.X;
            begin.Y = e.Y;
        }

        private void panel_MouseWheel(object sender, MouseEventArgs e)
        {
            PointF centralzoomtile = new PointF();
            PointF centralzoomcoords = new PointF();
            centralzoomtile.X = tile.X;
            centralzoomtile.Y = tile.Y;
            Convert ToConvert = new Convert(centralzoomtile.X, centralzoomtile.Y, zoom);
            centralzoomcoords = ToConvert.TileToWorldPos();
            if ((e.Delta > 0) && (zoom < 16) && (zoom >= 1))
            {
                zoom++;
            }
            else
            {
                if ((e.Delta < 0) && (zoom > 1) && (zoom <= 16))
                {
                    zoom--;
                }
            }
            ToConvert = new Convert(centralzoomcoords.X, centralzoomcoords.Y, zoom);
            centralzoomtile = ToConvert.WorldToTilePosSpheroid();
            tile = ToConvert.WorldToTilePosSpheroid();
            firstload = true;
            panel.Invalidate();
            MouseWheel -= new MouseEventHandler(panel_MouseWheel);

        }

        private void panel_MouseEnter(object sender, EventArgs e)
        {
            panel.Focus();
        }

    }
}
