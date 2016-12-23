using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace samsonkina_map
{
    /// <summary>
    /// Класс для работы с файлами
    /// </summary>
    class Reading
    {
        /// <summary>
        /// заполняет матрицу содержимым файла, 
        /// разделенным символом табуляции
        /// </summary>
        /// <returns>
        /// разделенный файл
        /// </returns>
        public static string[,] SeparateFile(string path)
        {
            try
            {
                string[,] list;
                StreamReader sr = new StreamReader(path);

                // колво строк
                int countLines = 0;
                // строки под содержимое файла, изначально 0
                string[] lines = new string[countLines];

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    // добавили строчку
                    Array.Resize(ref lines, countLines + 1);
                    // считали строку
                    lines[countLines] = line;
                    countLines++;
                }
                // считали файл построчно и закрыли
                sr.Close();

                // колво элементов в строке
                int countElements = lines[0].Split('\t').Length;
                // указали размер будущей матрицы 
                list = new string[countLines, countElements];

                string[] tmp;
                // заполняем матрицу
                for (int i = 0; i < countLines; i++)
                {
                    tmp = null;
                    // содержимое каждой строки разбиваем на части
                    tmp = lines[i].Split('\t');
                    for (int j = 0; j < countElements; j++)
                    {
                        // заносим в строку матрицы поэлементно
                        list[i, j] = tmp[j];
                    }
                }
                // вернули матрицу
                return list;
            }
            catch
            {
                // что-то пошло не так и мы не можем прочитать файл
                MessageBox.Show("Невозможно считать файл");
                return null;
            }
        }

        /// <summary>
        /// возвращает [0] столбец матрицы в виде списка
        /// </summary>
        public static string[] Reader(string[,] s)
        {
            // массив под будущий список
            string[] list;
            try
            {
                // узнаем колво элеемнтов в столбце
                int count = s.GetLength(0);
                // создаем массив соответствующего размера
                list = new string[count];
                // заполняем массив
                for (int i = 0; i < count; i++)
                    list[i] = s[i, 0];
                // и возвращаем
                return list;
            }
            catch
            {
                // что-то пошло не так...
                MessageBox.Show("Не могу получить список элементов!");
                return null;
            }

        }
        /// <summary>
        /// проверка для загрузки карт при первом запуске
        /// </summary>
        public static string CheckPath()
        {
            // открываем файл с путем сохранения
            StreamReader sr = new StreamReader("systeminfo.txt");
            // читаем строки
            string path = sr.ReadLine();
            sr.Close();
            // если ничего не считали, т.е. файл пуст, выбираем путь дя загрузки
            if (path == null)
            {
                MessageBox.Show("Выберите путь для сохранения карт");
                var win = new FolderBrowserDialog();
                if (win.ShowDialog() == DialogResult.OK)
                    path = win.SelectedPath;
            }
            // записывам выбранный путь в файл
            StreamWriter sw = new StreamWriter("systeminfo.txt");
            sw.Write(path);
            sw.Close();
            return path;
        }

        /// <summary>
        /// получает список городов
        /// </summary>
        /// <returns>
        /// разделенный файл с городами
        /// </returns>
        public static string[] WriteListOfCities()
        {
            return Reader(SeparateFile("cities.txt"));
        }

        /// <summary>
        /// получает список серверов
        /// </summary>
        /// <returns>
        /// разделенный файл с серверами
        /// </returns>
        public static string[] WriteListOfServers()
        {
            return Reader(SeparateFile("servers.txt"));
        }


    }
}
