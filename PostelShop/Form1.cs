using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using HtmlAgilityPack;


namespace PostelShop
{
    public partial class Form1 : Form
    {
        Thread threadWhile;
        ImageFullSize imagefullsize;

        delegate void ImageFullSizeDelegate(ImageFullSize str);
        ImageFullSizeDelegate IMAGEFULLSIZEDELEGATE;

        Button[] button;
        string[] linq;

        Button[] ButttonPN;

        ComboBox[] comboboxArr;
        Label[] LabelArr;

        PruductCatalog productcatalog;

        #region положение кнопок иммитации меню
        int XLoc = 15;
        int YLoc = 50;

        #endregion

        public Form1()
        {
            InitializeComponent();
            BrowseUrl("http://opttextil.ru/");
            AddContainerControl();
            thread();
        }


        #region Поток ожидания
        private void thread()
        {
            threadWhile = new Thread(WhileStarted);
            threadWhile.Start();
        }
        private void WhileStarted()
        {
            while (true)
            {
                Thread.Sleep(100);

                if (StaticPublicClass.FullSizeBool)
                {
                    StaticPublicClass.FullSizeBool = false;

                    imagefullsize = new ImageFullSize();
                    IMAGEFULLSIZEDELEGATE = new ImageFullSizeDelegate(AddControllInForm);

                    imagefullsize.WH("http://193.107.239.115/original/" + StaticPublicClass.FullSizeName);
                    imagefullsize.Location = new Point(230,50);
                    imagefullsize.Size = new Size(500, 500);

                    //Controls.Add(imagefullsize);
                    //imagefullsize.BringToFront();
                    Invoke(IMAGEFULLSIZEDELEGATE, imagefullsize);

                }

            }
        }

        private void AddControllInForm(ImageFullSize str)
        {
            Controls.Add(imagefullsize);
            imagefullsize.BringToFront();
        }
        #endregion


        #region Загрузка урла
        private void BrowseUrl(string url)
        {
            
            webBrowser1.Navigate(url);
        }

        #endregion

        #region Ответ от браузера
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (sender != null || e != null)
            {
                string url = webBrowser1.Url.ToString();
                HtmlAgilityPack.HtmlDocument docc = new HtmlAgilityPack.HtmlDocument();
                if (url == "http://opttextil.ru/")
                { 
                
                docc.LoadHtml(webBrowser1.DocumentText);
                document(docc);
                }
                else
                { 

                //if (url == "http://opttextil.ru/postelnoe-bele/")
                //{
                    docc.LoadHtml(webBrowser1.DocumentText);
                    NextPage(docc);
                //}
                //if (url == "http://opttextil.ru/makhrovye-izdeliya/")
                //{
                //    docc.LoadHtml(webBrowser1.DocumentText);
                //    NextPage(docc);
                //}
                }
            }
        }

        #endregion

        #region Обработка второй страници после меню
        //private void NextPage(HtmlAgilityPack.HtmlDocument html)
        //{
        //    var doc = html.DocumentNode.ChildNodes.Where(x => x.Name == "html").ToArray();
        //    var docc = doc[0].ChildNodes.Where(x => x.Name == "body").ToArray();
        //    var doccc = docc[0].ChildNodes.Where(x => x.Name == "div").ToArray();
        //    var docccc = doccc[7].ChildNodes.Where(x => x.Name == "div").ToArray();
        //    var doccccc = docccc[0].ChildNodes.Where(x => x.Name == "div").ToArray();
        //    //Пятый узел содержит тело
        //    var docccccc = doccc[7].ChildNodes.Where(x => x.Name == "div").ToArray();

        //    //Шестой узел содержит навигацию
        //    if (StaticPublicClass.PagePNBOOL)
        //        //второй узел слдержит фильтры
        //        filterDiv(doccccc[2].ChildNodes.Where(x => x.Name == "div").ToArray());
        //    StaticPublicClass.PagePNBOOL = true;
        //    ////Пятый узел содержит тело
        //    //var docccccc = doccccc[5].ChildNodes.Where(x => x.Name == "div").ToArray();
        //    NextPageNoteTovar(docccccc);

            
        //    Pagenation(doccccc[6]);
        //}


        #region  Ребрендинг второй страници после меню Замена предыдущего варианта
        private void NextPage(HtmlAgilityPack.HtmlDocument html)
        {
            // Получаем полную структуру полученного хтмл файла
            var HmlFileFull = html.DocumentNode.ChildNodes.Where(x => x.Name == "html").ToArray();
            var BodyHtmlFileFull = HmlFileFull[0].ChildNodes.Where(x => x.Name == "body").ToArray();

            //Колекция элементов Div находящихся в боди
            var DivBodyCollection = BodyHtmlFileFull[0].ChildNodes.Where(x => x.Name == "div").ToArray();
            BodyCollectionSeparator(DivBodyCollection);


        }

        private void BodyCollectionSeparator(HtmlNode[] DivBodyCollection)
        {
            foreach (var node in DivBodyCollection)
            {
                if(node.Attributes.Count>0)
                {
                    var atributNode = node.Attributes[0].Value;
                    if (atributNode == "content")
                        contentBody(node);
                }
            }

        }
        #region Обработка контента
        private void contentBody(HtmlNode ContentNode)
        {
            var ContentNodeDiv = ContentNode.ChildNodes.Where(x => x.Name == "div").ToArray();
            ContentNodeDiv = ContentNodeDiv[0].ChildNodes.Where(x => x.Name == "div").ToArray();
            ContentFilterPrserNode(ContentNodeDiv);

        }
        private void ContentFilterPrserNode(HtmlNode[] ContentNodeDiv)
        {
            foreach(var node in ContentNodeDiv)
            if (node.Attributes.Count > 0)
            {
                    var valueAtr = node.Attributes[0].Value;
                    
                    if (valueAtr == "product-category")
                        NextPageNoteTovar(node.ChildNodes.Where(x => x.Name == "div").ToArray()); 
                    if (valueAtr == "filter-line")
                    {
                        if (StaticPublicClass.PagePNBOOL)
                            //второй узел слдержит фильтры
                            filterDiv(node.ChildNodes.Where(x => x.Name == "div").ToArray());
                        StaticPublicClass.PagePNBOOL = true;
                    }
                    if (valueAtr == "pagination")
                        Pagenation(node);
                    
                }

        }
        #endregion
        #endregion



        #region Обработка и добавлние постраничной навигации

        private void ButtonPnDispose()
        {
            foreach (Button btn in ButttonPN)
            {
                btn.Dispose();
            }
        }

        private void Pagenation(HtmlNode doc)
        {
            var docc = doc.ChildNodes.Where(x => x.Name == "a").ToArray();
            string ActivePn;
            foreach (HtmlNode node in docc)
            {
                if (node.Attributes[0].Value == "active-pagination")
                {
                    ActivePn = node.InnerText;
                    StaticPublicClass.ActivePN = Convert.ToInt32(ActivePn);
                }
            }
            string CountPN = docc.Last().InnerText;

            
            StaticPublicClass.CountPN = Convert.ToInt32(CountPN);
            AddControlPn();
        }

        public void AddControlPn()
        {
            //if (ButttonPN != null)
            //    ButtonPnDispose();
            if (StaticPublicClass.CountPN >= 10)
                AddButtonPN10();
            if (StaticPublicClass.CountPN < 10)
                AddButonPNMin10();
        }

        #region Если кнопок меньше 10
        private void AddButonPNMin10()
        {
            if (ButttonPN.Length != StaticPublicClass.CountPN)
            {
                ButtonPnDispose();
                ButttonPN = null;
            }
            AddBtnControll();

        }

        private void AddBtnControll()
        {
            if(ButttonPN == null)
            ButttonPN = new Button[StaticPublicClass.CountPN];
            AddButtonMin();
        }
        #endregion


        #region В слуачае если страниц меньше 10
        private void AddButtonMin()
        {
            for (int i=0;i< StaticPublicClass.CountPN;i++)
            {
                if (ButttonPN[i] == null)
                    ButttonPN[i] = new Button();
                if (ButttonPN[i].Name == "")
                    ButttonPN[i].Name = i.ToString();
                ButttonPN[i].Text = Convert.ToString(i + 1);
                
                ButttonPN[i].Size = new Size(38, 23);
                ButttonPN[i].Location = new Point(500 / 10 * (i + 1) + 200, 600);
                if (Convert.ToInt32(ButttonPN[i].Text) == StaticPublicClass.ActivePN)
                    ButttonPN[i].Location = new Point(500 / 10 * (i + 1) + 200, 590);
                ButttonPN[i].Click += ButtonPnEvent;
                this.Controls.Add(ButttonPN[i]);
            }
        }
        #endregion


        #region В случае если страниц больше 10ти
        //Выполняется в случае если страниц больше 10ти
        private void AddButtonPN10()
        {
            if (ButttonPN != null && ButttonPN.Length < 10)
            {
                ButtonPnDispose();
                ButttonPN = null;
            }
            if (ButttonPN == null)
                ButttonPN = new Button[10];
            if (StaticPublicClass.ActivePN < 6)
                AddButtonPN5();
            if (StaticPublicClass.ActivePN > 5 && StaticPublicClass.ActivePN < StaticPublicClass.CountPN - StaticPublicClass.ActivePN)
                AddButonPN6();
            if (StaticPublicClass.CountPN - StaticPublicClass.ActivePN < 6)
                AddButtonPNMax();
        }

        //Если осталось меньше 5 страниц до последней страници
        private void AddButtonPNMax()
        {
            for (int i = 0; i < 10; i++)
            {
                if (ButttonPN[i] == null)
                    ButttonPN[i] = new Button();
                if (ButttonPN[i].Name == "")
                    ButttonPN[i].Name = i.ToString();
                ButttonPN[i].Text = Convert.ToString(StaticPublicClass.CountPN - (9 - i));
                //if (i + 1 == StaticPublicClass.ActivePN)
                ButttonPN[i].Size = new Size(38, 23);
                ButttonPN[i].Location = new Point(500 / 10 * (i + 1) + 200, 600);
                if (Convert.ToInt32(ButttonPN[i].Text) == StaticPublicClass.ActivePN)
                    ButttonPN[i].Location = new Point(500 / 10 * (i + 1) + 200, 590);
                ButttonPN[i].Click += ButtonPnEvent;
                this.Controls.Add(ButttonPN[i]);
            }
        }

        //Если текущая страница больше 5
        private void AddButonPN6()
        {
            for (int i = 0; i < 10; i++)
            {
                if (ButttonPN[i] == null)
                    ButttonPN[i] = new Button();
                if (ButttonPN[i].Name == "")
                    ButttonPN[i].Name = i.ToString();
                if (i<=5)
                {
                ButttonPN[i].Text = Convert.ToString(StaticPublicClass.ActivePN - (5 - i));
                }
                //if (i == 5)
                //{
                //    ButttonPN[i].Name = Convert.ToString(StaticPublicClass.ActivePN);
                //    ButttonPN[i].Text = Convert.ToString(StaticPublicClass.ActivePN);
                //}
                if (i >= 6)
                {
                    ButttonPN[i].Text = Convert.ToString(StaticPublicClass.ActivePN + (i - 5));
                }
                //if (i + 1 == StaticPublicClass.ActivePN)
                ButttonPN[i].Size = new Size(38, 23);
                ButttonPN[i].Location = new Point(500 / 10 * (i + 1) + 200, 600);
                if (Convert.ToInt32(ButttonPN[i].Text) == StaticPublicClass.ActivePN)
                    ButttonPN[i].Location = new Point(500 / 10 * (i + 1) + 200, 590);
                ButttonPN[i].Click += ButtonPnEvent;
                this.Controls.Add(ButttonPN[i]);
            }

        }

        //Если текущая страница меньше 6
        private void AddButtonPN5()
        {
            for (int i = 0; i < 10; i++)
            {
                if (ButttonPN[i] == null)
                    ButttonPN[i] = new Button();
                if(ButttonPN[i].Name == "")
                ButttonPN[i].Name = i.ToString();
                ButttonPN[i].Text = Convert.ToString(i + 1);
                //if (i + 1 == StaticPublicClass.ActivePN)
                ButttonPN[i].Size = new Size(38, 23);
                ButttonPN[i].Location = new Point(500/10*(i+1)+200,600);
                if (Convert.ToInt32(ButttonPN[i].Text) == StaticPublicClass.ActivePN)
                    ButttonPN[i].Location = new Point(500 / 10 * (i + 1) + 200, 590);
                ButttonPN[i].Click += ButtonPnEvent;
                this.Controls.Add(ButttonPN[i]);
            }
        }



        #region Обработка события вызова кнопки
        private void ButtonPnEvent(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int PagePn = Convert.ToInt32(btn.Text);
            string UrlPn = "p=" + btn.Text;
            StaticPublicClass.PagePNBOOL = false;
            if (StaticPublicClass.FilterBoolIzmenen)
            {
                string replaceSimbol = "%" + 2 + "C";
                string urlFilter = StaticPublicClass.LasFilter.Replace(",", replaceSimbol);
                BrowseUrl(StaticPublicClass.LastUrl + urlFilter + "&" +UrlPn);
                //StaticPublicClass.FilterBoolIzmenen = false;
            }
            else
            {
                BrowseUrl(StaticPublicClass.LastUrl + "?"+UrlPn);
            }
        }
        #endregion

        #endregion
        #endregion

        #region обработка фильтров
        private void filterDiv(HtmlNode[] doc)
        {
            if (StaticPublicClass.FilterBOOL)
                FilterDisposed();
            FilterParametrParser(doc);
        }

        private void FilterDisposed()
        {
            for (int i = 0; i < comboboxArr.Length-1;i++)
            {
                LabelArr[i].Dispose();
                comboboxArr[i].Dispose();
            }
            StaticPublicClass.FilterBOOL = false;
        }

        private void FilterParametrParser(HtmlNode[] doc)
        {
            object[,] prametrFilterArr = new object[(doc.Length -1)/2, 3];
            int[] prametrIndexFilter = new int[(doc.Length - 1) / 2];
            int b = 0;
            for (int i=0; i < doc.Length;i++)
            {
                var docc = doc[i].ChildNodes.Where(x => x.Name == "div").ToArray();
                
                if(docc.Length != 0)
                 { 
                     string valueBlock = docc[0].Attributes[0].Value;

                     if (valueBlock == "filterblock")
                     {
                        prametrIndexFilter[b] = Convert.ToInt32(docc[0].Attributes[2].Value);
                        if(prametrIndexFilter[b]!=0)
                        { 
                        prametrFilterArr[b,0] = Convert.ToInt32(docc[0].Attributes[2].Value);
                        prametrFilterArr[b,1] = docc[0].InnerText.Replace("\r\n", "").Replace("  ", "");
                        b++;
                        }

                    }
                    else 
                    {
                        if (prametrIndexFilter[b-1] != 0)
                        {
                            valueBlock = docc[0].Attributes[0].Value;
                            if (valueBlock == "filter-panel-inner")
                            {
                                var d = docc;
                                var doccc = docc[0].ChildNodes.Where(x => x.Name == "li").ToArray();
                                string[] parametrNameArr = new string[doccc.Length];
                                int index = 0;
                                int dataPid= 777;
                                for (int g = 0; g < doccc.Length; g++)
                                {
                                    var docccc = doccc[g].ChildNodes.Where(x => x.Name == "label").ToArray();
                                    string ValueText = docccc[0].InnerText.Replace("\r\n", "").Replace("  ", "");
                                    var doccccc = docccc[0].ChildNodes.Where(x => x.Name == "input").ToArray();
                                    int dataValue = Convert.ToInt32(doccccc[0].Attributes[4].Value);
                                    dataPid = Convert.ToInt32(doccccc[0].Attributes[3].Value);
                                    //parametrNameArr[g, 0] = ValueText;
                                    // parametrNameArr[g, 1] = Convert.ToString(dataValue);
                                    parametrNameArr[g] = ValueText + "--" + Convert.ToString(dataValue);
                                    
                                }
                                index = Array.FindIndex(prametrIndexFilter, p => p == dataPid);
                                if (index==13)
                                {
                                    int f = 6;
                                }
                                if(index !=-1)
                                prametrFilterArr[index, 2] = parametrNameArr;
                            }

                        }
                    }
                }
            }
            StaticPublicClass.ParametrFilter = prametrFilterArr;

            FilterPanel();
            StaticPublicClass.FilterBOOL = true;
        }

        #endregion

        #region Панель управления фильтрами
        private void FilterPanel()
        {
            comboboxArr = new ComboBox[StaticPublicClass.ParametrFilter.Length/3];
            LabelArr = new Label[StaticPublicClass.ParametrFilter.Length / 3];
            StaticPublicClass.dataPid = new int[StaticPublicClass.ParametrFilter.Length / 3];
            StaticPublicClass.dataValue = new int[StaticPublicClass.ParametrFilter.Length / 3];
            StaticPublicClass.FilterBoolIzmenen = false;

            for (int i = 0; i < StaticPublicClass.ParametrFilter.Length / 3;i++)
            {
                if(StaticPublicClass.ParametrFilter[i,0]!=null)
                { 
                int[] pointLocation = PointLabel(i);

                LabelArr[i] = new Label();
                comboboxArr[i] = new ComboBox();

                LabelArr[i].Location = new Point(pointLocation[0], pointLocation[1]);
                comboboxArr[i].Location = new Point(pointLocation[0], pointLocation[1]+25);

                LabelArr[i].Text = (string)StaticPublicClass.ParametrFilter[i, 1];

                string[] parametrArr = (string[])StaticPublicClass.ParametrFilter[i, 2];

                comboboxArr[i].Items.AddRange(parametrArr);

                LabelArr[i].Name = Convert.ToString(StaticPublicClass.ParametrFilter[i, 0]);
                comboboxArr[i].Name = Convert.ToString(StaticPublicClass.ParametrFilter[i, 0]);
                StaticPublicClass.dataPid[i] = (int) StaticPublicClass.ParametrFilter[i, 0];

                this.Controls.Add(LabelArr[i]);
                comboboxArr[i].BringToFront();

                comboboxArr[i].SelectedIndexChanged += ComboBoxEverSelectedIndexChanget;
                this.Controls.Add(comboboxArr[i]);
                }
            }

        }

        #region Событие выбора бокса
        private void ComboBoxEverSelectedIndexChanget(object sender, EventArgs e)
        {
            ComboBox cbBox = (ComboBox)sender;
            string[] separator = { "--" };
            string selected = cbBox.SelectedItem.ToString().Split(separator, StringSplitOptions.RemoveEmptyEntries).Last();
            string CbName = cbBox.Name;
            ParametrCBEvent(Convert.ToInt32(selected), Convert.ToInt32(CbName));
        }

        private void ParametrCBEvent(int dataValue, int dataPid)
        {
            int index = Array.FindIndex(StaticPublicClass.dataPid,p => p == dataPid);
            StaticPublicClass.dataValue[index] = dataValue;
            StaticPublicClass.FilterBoolIzmenen = true;
            urlUpdateFilter();

        }

        private void urlUpdateFilter()
        {
            string UrlFilterPlus="";
            for (int i = 0; i < StaticPublicClass.dataValue.Length; i++)
            {
                if (StaticPublicClass.dataValue[i] != 0)
                {
                    string urlPlus = String.Format("[\"{0}\",\"{1}\"]", StaticPublicClass.dataPid[i], StaticPublicClass.dataValue[i]);
                    if (UrlFilterPlus == "")
                    {
                        UrlFilterPlus += urlPlus;
                    }
                    else
                    {
                        UrlFilterPlus += "," + urlPlus;
                    }
                }
            }
            StaticPublicClass.LasFilter = "?price_min=78&price_max=10000&filter=[" + UrlFilterPlus+"]";
            StaticPublicClass.PagePNBOOL = false;
            StaticPublicClass.FilterBoolIzmenen = true;
            BrowseUrl(StaticPublicClass.LastUrl + StaticPublicClass.LasFilter);
        }
        #endregion

        #region Расчет позиции каждого пункта фильтра
        private int[] PointLabel(int number)
        {
            int xLC = 911;
            int yLC = 50;

            int[] positionLC = new int[2];

            if (number == 0)
            {
                return new int[2] { xLC, yLC };
            }
            else
            {
                if (number > 8)
                {
                    xLC = 1050;
                    yLC = 50;
                    
                    if (number == 9)
                    {
                        return new int[2] { xLC,yLC };
                    }
                    else
                    {
                        number -= 9;
                        return new int[2] { xLC, number * 60 + yLC };
                    }
                    
                }
                else
                {
                    
                    return new int[2] { xLC, number * 60 + yLC };
                }
            }
        }
        #endregion
        #endregion

        private void NextPageNoteTovar(HtmlNode[] doc)
        {
            productcatalog.tovarAddControl(doc);
        }
        #endregion

        #region Обработка страници входа и построение меню
        private void document(HtmlAgilityPack.HtmlDocument html)
        {
            var doc = html.GetElementbyId("ddmenu").ChildNodes[11];
            var docum = doc.ChildNodes.Where(x => x.Name == "div").ToArray();
            var documm = docum[0].ChildNodes.Where(x => x.Name == "div").ToArray();
            var docummm = documm[1].ChildNodes.Where(x => x.Name == "div").ToArray();
            var docummmm = docummm[0].ChildNodes.Where(x => x.Name == "ul").ToArray();
            var docummmmm = docummmm[0].ChildNodes.Where(x => x.Name == "li").ToArray();

            documentAllTovar(docummmmm);

        }

        private void documentAllTovar(HtmlNode[] doc)
        {
            button = new Button[doc.Length];
            linq = new string[doc.Length];
            for (int i = 0; i < doc.Length; i++)
            {
                button[i] = new Button();
                button[i].Name = Convert.ToString(i);
                HtmlAgilityPack.HtmlNode[] name = doc[i].ChildNodes.Where(x => x.Name == "a").ToArray();
                button[i].Text = name[0].InnerText;
                linq[i] = name[0].Attributes[0].Value;
                sizeButton(i, button[i].Text);
                button[i].Click += ButtonMenuClik;
            }
        }

        private void ButtonMenuClik(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int number = Convert.ToInt32(btn.Name);
            string url = "http://opttextil.ru" + linq[number];
            StaticPublicClass.LastUrl = url ;
            BrowseUrl(url);
        }

        #region Добавление кнопок иммитации меню
        private void positionButton(int i, string Name)
        {
            button[i].Location = new Point(XLoc,YLoc);
            YLoc = YLoc + button[i].Size.Height + 10;
            Controls.Add(button[i]);
        }

        private void sizeButton(int i, string Name)
        {
            if (Name.Length > 14)
            {
                button[i].Size = new Size(100, 40);
                positionButton(i,Name);
            }
            else
            {
                button[i].Size = new Size(100, 25);
                positionButton(i, Name);
            }
        }

        #endregion
        #endregion

        #region Загрузка формы
        private void Form1_Load(object sender, EventArgs e)
        {
            webBrowser1.ScriptErrorsSuppressed = true;
        }
        #endregion

        #region Добавление контрола
        private void AddContainerControl()
        {
            productcatalog = new PruductCatalog();
            productcatalog.Location = new Point(130,50);
            productcatalog.Size = new Size(760, 530);
            Controls.Add(productcatalog);
        }
        #endregion
    }
}
