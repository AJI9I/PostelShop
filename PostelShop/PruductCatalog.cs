using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Threading;

namespace PostelShop
{
    public partial class PruductCatalog : UserControl
    {
        bool tovarAddBOOL = false;

        Thread TovarControlThread;

        UCTovar[] uctovar;

        delegate void ControllAdedDelegate(UCTovar uctovar);
        ControllAdedDelegate controlladeddelegate;

        public PruductCatalog()
        {
            InitializeComponent();
        }

        public void tovarAddControl(HtmlNode[] doc)
        {
            if (tovarAddBOOL)
            {
                ClearTovarControl();
            }

            uctovar = new UCTovar[doc.Length];
            TovarContorllAdd(doc);
            tovarAddBOOL = true;
        }

        private void ClearTovarControl()
        {
            for (int i=0; i<uctovar.Length; i++)
            {
                uctovar[i].Dispose();
            }
            tovarAddBOOL = false;
        }

        private void TovarContorllAdd(HtmlNode[] doc)
        {
            for (int i=0;i<doc.Length; i++)
            {
                threadingStartTovarControl(doc[i], i);
            }
        }

        private void threadingStartTovarControl(HtmlNode doc, int i)
        {
            object[] threadNode = new object[] { doc,i };
            TovarControlThread = new Thread(threadStart);
            TovarControlThread.Start(threadNode);

        }
        private void threadStart(object threadNode)
        {
            object[] node = (object[])threadNode;
            HtmlNode docc = (HtmlNode)node[0];
            int number = (int)node[1];
            AddControl(docc, number);
        }

        private int[] PositionControl(int i)
        {
            int[] xr = new int[2];
            int xR = 380;
            int xL = 10;

            int y = 320;

            if (i == 0)
            {
                //врнуть начальные координаты
                xr[0] = xL;
                xr[1] = 0;
                return xr;
            }
            else
            {
                double f = i / 2.0;
                int g = (int)f;
                double b = f - g;

                if (b > 0)
                {
                    xr[0] = xR;
                    xr[1] = y * g;
                    return xr;
                }
                else
                {
                    xr[0] = xL;
                    xr[1] = y * g;
                    return xr;

                }
            }
        }

        private void AddControl(HtmlNode docc, int number)
        {
            uctovar[number] = new UCTovar();
            //uctovar[number].Size = new Size();
            int[] xy = PositionControl(number);
            uctovar[number].Location = new Point(xy[0], xy[1]);
            uctovar[number].AddNode(docc);
            controlladeddelegate = new ControllAdedDelegate(ControllsAdded);
            Invoke(controlladeddelegate, uctovar[number]);
        }

        private void ControllsAdded(UCTovar uctovar)
        {
            Controls.Add(uctovar);
        }
    }
}
