using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        /// <summary>
        /// нажатие на кнопку
        /// </summary>
        private void btnLoad_Click(object sender, EventArgs e)
        {
            // получаем инфу о сервере и городе
            GetDownloadInfo();
            // создаем путь до папки города
            CreateCatalogPath();

        }

        private void GetDownloadInfo()
        {

        }



        private void CreateCatalogPath()
        {

        }

    }
}
