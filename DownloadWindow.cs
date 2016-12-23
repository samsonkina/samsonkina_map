using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace samsonkina_map
{
    public partial class DownloadWindow : Form
    {
        // путь к папке с картами
        private string path;
        private int selectedServer;
        private int selectedCity;
        private string[,] cities;
        private string[,] servers;

        public DownloadWindow()
        {
            // получаем путь из файла
            path = Reading.CheckPath();
            cities = Reading.SeparateFile("cities.txt");
            servers = Reading.SeparateFile("servers.txt");
            // инициализация компонентов
            InitializeComponent();

            // заполняем список городов
            ListOfCities.Items.AddRange(Reading.WriteListOfCities());
            ListOfCities.Text = ListOfCities.Items[0].ToString();
            // заполняем список серверов
            ListOfServers.Items.AddRange(Reading.WriteListOfServers());
            ListOfServers.Text = ListOfServers.Items[0].ToString();
        }

        static public DownloadInfo.City SelectedCity;
        static public DownloadInfo.Server SelectedServer;
        public static string catalogPath;
        /// <summary>
        /// нажатие на кнопку
        /// </summary>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            // получаем инфу о сервере и городе
            GetDownloadInfo();
            // создаем путь до папки города
            CreateCatalogPath();

            MapWindow mw = new MapWindow();
            if (!mw.IsDisposed)
                mw.Show();

        }

        private void GetDownloadInfo()
        {
            selectedCity = ListOfCities.SelectedIndex;
            selectedServer = ListOfServers.SelectedIndex;

            // создаем класс для выбранного города  имя -- широта -- долгота
            SelectedCity = new DownloadInfo.City(cities[selectedCity, 0], Double.Parse(cities[selectedCity, 1].Replace('.', ',')),
                                    Double.Parse(cities[selectedCity, 2].Replace('.', ',')));

            // создаем класс для выбранного сервера  имя -- ссылка -- список серверов
            SelectedServer = new DownloadInfo.Server(servers[selectedServer, 0], servers[selectedServer, 1], servers[selectedServer, 2].Split(' '));

        }



        private void CreateCatalogPath()
        {
            catalogPath += path + "\\" + SelectedServer.name + "\\" + SelectedCity.name + "\\";
            // создаем каталог если нет
            if (!Directory.Exists(catalogPath))
                Directory.CreateDirectory(catalogPath);
        }

        private void btnLoad_Click_1(object sender, EventArgs e)
        {

        }
    }
}
