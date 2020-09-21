using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PostelShop
{
    public partial class ImageFullSize : UserControl
    {
        Image image;
        PictureBox picBox;


        ImageHightWhightCalibration imagehightwhieghtcalibration;
        DownloadImage downloadimage;


        public ImageFullSize()
        {
            InitializeComponent();
        }

        public void WH(string url)
        {
            AddImage(url);
        }

        private void AddImage(string url)
        {
            picBox = new PictureBox();
            picBox.Image = ImageCalibration(url);
            picBox.Size = new Size(500,500);
            picBox.Location = new Point(0,0);
            picBox.Click += PicBox_Click;
            Controls.Add(picBox);
        }

        private void PicBox_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private Image ImageCalibration(string url)
        {
            imagehightwhieghtcalibration = new ImageHightWhightCalibration();
            return imagehightwhieghtcalibration.ScaleImage(ImageDownloadAndFind(url),500,500);
        }

        public Image ImageDownloadAndFind(string url)
        {
            downloadimage = new DownloadImage();
            return downloadimage.fileFind(url);
        }
    }
}
