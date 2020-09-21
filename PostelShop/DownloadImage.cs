using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Net;

namespace PostelShop
{
    class DownloadImage
    {

        //передаем урл картинки
        //В ответ приходит картинка
        public Image fileFind(string fileUrl)
        {
            string[] PachUrl = FilePatch(fileUrl);
            string filePatch = AppDomain.CurrentDomain.BaseDirectory + "img\\" + PachUrl[PachUrl.Length - 3] + '\\' + PachUrl[PachUrl.Length - 2] + '\\' + PachUrl[PachUrl.Length - 1];
            string filePatchName = AppDomain.CurrentDomain.BaseDirectory + "img\\" + PachUrl[PachUrl.Length - 3] + '\\' + PachUrl[PachUrl.Length - 2] + '\\';
            //если картики не существует то путь приходит ошибка
            if (File.Exists(filePatch))
            {
                return LoadImgFile(filePatch);
            }
            //переходим к загрузке картинки
            else
            {
                return LoadImgFile(DownloadImageFile(filePatchName, fileUrl, PachUrl[PachUrl.Length - 1]));
            }
        }

        private string[] FilePatch(string fileUrl)
        {
            return fileUrl.Split(new Char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private Image LoadImgFile(string filePatch)
        {
            try
            {
                Image image = new Bitmap(filePatch);
                return image;
            }
            catch
            {
                return ErorExeption();
            }
        }

        public Image ErorExeption()
        {
            Image image = new Bitmap(Resource1.Eror);
            return image;
        }

        private string DownloadImageFile(string filePatchName, string fileUrl, string fileName)
        {
            if (!Directory.Exists(filePatchName))
            {
                Directory.CreateDirectory(filePatchName);
            }
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFile(fileUrl, filePatchName + fileName);
                }
                catch
                {

                }
            }
            return filePatchName + fileName;
        }

    }
}
