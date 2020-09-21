using System;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace PostelShop
{
    public partial class UCTovar : UserControl
    {
        //ImageFullSize imagefullsize;
        
        DownloadImage downloadimage;

        int[] Price = new int[0];
        string[] PriceInfo = new string[0];
        string[] OptionInfo = new string[0];
        

        Label[] LabelPrice;
        Button[] ButtonPrice;
        ToolTip[] tooltip;

        string ImgName;

        public UCTovar()
        {
            InitializeComponent();
        }

        public void AddNode(HtmlNode docc)
        {
            var doccc = docc.ChildNodes.Where(x => x.Name == "div").ToArray();
            AddControlsBox(doccc);
        }

        private void AddControlsBox(HtmlNode[] doc)
        {
            foreach (HtmlNode docc in doc)
            {
                var attr = docc.Attributes.Where(x => x.Name == "class").ToArray();
                if (attr[0].Value == "img-item")
                    ImgItem(docc);
                if (attr[0].Value == "desc-item")
                    DescItem(docc);
                if (attr[0].Value == "option-item")
                    OpionItem(docc);
            }
        }
        
        private void ImgItem(HtmlNode doc)
        {
            try{
                var docc = doc.ChildNodes.Where(x => x.Name == "a").ToArray();
                var doccc = docc[0].ChildNodes.Where(x => x.Name == "img").ToArray();
                var attr = doccc[0].Attributes.Where(x => x.Name == "data-layzr").ToArray();
                pictureImage(attr[0].Value);
            }
            catch {
                downloadimage = new DownloadImage();
                downloadimage.ErorExeption();
            }
        }

        private void DescItem(HtmlNode doc)
        {
            var docc = doc.ChildNodes.Where(x => x.Name == "div").ToArray();
            var doccc = docc[1].ChildNodes.Where(x => x.Name == "a").ToArray();
            DescItemControlAdd(doccc[0]);
            var docccc = docc[2].ChildNodes.Where(x => x.Name == "p").ToArray();
            parametr(docccc);

        }

        private void parametr(HtmlNode[] node)
        {
            Label[] LabelArr = new Label[node.Length];
            int xx = 175;
            int yy = 55;
            for (int i=0; i<node.Length;i++)
            {
                LabelArr[i] = new Label();
                LabelArr[i].Location = new Point(xx, yy);
                yy += 25;
                LabelArr[i].Text = node[i].InnerText;
                this.Controls.Add(LabelArr[i]);
            }
        }

        private void DescItemControlAdd(HtmlNode doc)
        {
            label1.Text = textO(doc.InnerText);
            label1.Font = new Font(label1.Font.Name, 11, label1.Font.Style);
        }

        private string textO(string str)
        {
            return str.Replace("&quot;", "");
        }

        //получаем узлы с описанием и цениками на разные размеры товара
        private void OpionItem(HtmlNode doc)
        {
            var docc = doc.ChildNodes.Where(x => x.Name == "span").ToArray();
            PriceOpen(docc);


        }

        //
        private void PriceOpen(HtmlNode[] doc)
        {
            // Price = (string[]) 
            int arrsize = Price.Length;
            arrsize++;
            Array.Resize(ref Price, arrsize);
            Array.Resize(ref PriceInfo, arrsize);
            Array.Resize(ref OptionInfo, arrsize);

            
            foreach (var docc in doc)
            {
                if(docc.Attributes.Count!=0)
                { 
                string arg = docc.Attributes[0].Value;
                if (arg == "size")
                    PriceInfo[PriceInfo.Length - 1] = docc.InnerText;
                if (arg == "price-prod")
                    Price[Price.Length - 1] = Convert.ToInt32(docc.InnerText);
                if (arg == "info-option")
                    OptionInfo[OptionInfo.Length - 1] = docc.InnerText;
                }
            }
            AddLabelBox();
        }

        private void AddLabelBox()
        {
            LabelPrice = new Label[PriceInfo.Length];
            ButtonPrice = new Button[PriceInfo.Length];
            tooltip = new ToolTip[OptionInfo.Length];

            for (int i =0; i< LabelPrice.Length;i++)
            {
                LabelPrice[i] = new Label();
                ButtonPrice[i] = new Button();

                


                LabelPrice[i].Name = i.ToString();
                ButtonPrice[i].Name = i.ToString();
                

                LabelPrice[i].Text = PriceInfo[i];
                int cena = Convert.ToInt32(Price[i] + (Price[i] * StaticPublicClass.Procent));
                ButtonPrice[i].Text = Convert.ToString(cena);

                LabelPrice[i].Location = new Point(3, 25* i+226 );
                ButtonPrice[i].Location = new Point(250, 25 * i + 226);

                #region Подсказка по комплектации с использованием хелп провидера

                tooltip[i] = new ToolTip();

                tooltip[i].AutoPopDelay = 20000;
                tooltip[i].InitialDelay = 1000;
                tooltip[i].ReshowDelay = 500;
                // Force the ToolTip text to be displayed whether or not the form is active.
                tooltip[i].ShowAlways = true;

                tooltip[i].SetToolTip(this.LabelPrice[i], OptionInfo[i].Replace("  ","").Replace("\r\n\r\n\r\n", ""));
               
                #endregion

                Controls.Add(LabelPrice[i]);
                Controls.Add(ButtonPrice[i]);
            }
        }

        private void pictureImage(string url)
        {
            downloadimage = new DownloadImage();
            ImgName = url.Split(new Char[] { '/' }).Last();
            pictureboxAdd(downloadimage.fileFind(url));
        }

        //Фотография для товара полученная с сайта мини размер 
        // при клике на фотографию фото передается в ожидающий поток главаной формы
        // и открывает ее поверх всего
        // при повторном клике на открытую фулл фото она закрывается
        private void pictureboxAdd(Image image)
        {
            PictureBox pickturebox = new PictureBox();
            pickturebox.Location = new Point(5, 20);
            pickturebox.Name = "box";
            pickturebox.Size = new Size(image.Width,image.Height);
            pickturebox.Image = image;
            pickturebox.Click += Pickturebox_Click;
            //pickturebox.BringToFront();
            this.Controls.Add(pickturebox);
        }

        private void Pickturebox_Click(object sender, EventArgs e)
        {
            StaticPublicClass.FullSizeName = ImgName;
            StaticPublicClass.FullSizeBool = true;
            
            //imagefullsize = new ImageFullSize();
            //imagefullsize.WH("http://193.107.239.115/original/"+ ImgName);
            //imagefullsize.Size = new Size(500,500);

            //Controls.Add(imagefullsize);
            //imagefullsize.BringToFront();
        }
    }
}
