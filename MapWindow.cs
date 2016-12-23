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
    public partial class MapWindow : Form
    {
        int zoom = 15;
        Point tile = new Point();
        public MapWindow()
        {
            Convert ToConvert = new Convert(DownloadWindow.SelectedCity.longitude, DownloadWindow.SelectedCity.latitude, zoom);
            tile = ToConvert.WorldToTilePosSpheroid();
            InitializeComponent();
        }


      

        private void panel_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void panel_MouseMove(object sender, MouseEventArgs e)
        {
           
        }

        private void panel_MouseDown(object sender, MouseEventArgs e)
        {
            
        }

        private void panel_MouseWheel(object sender, MouseEventArgs e)
        {
           

        }

        private void panel_MouseEnter(object sender, EventArgs e)
        {
           
        }
    }
}
