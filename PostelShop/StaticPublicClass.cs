using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostelShop
{
    static class StaticPublicClass
    {
        public static bool FilterBOOL;
        public static object[,] ParametrFilter;

        public static int ActivePN;
        public static int CountPN;
        public static bool PagePNBOOL = true;

        public static string LastUrl;

        // Урл Второй страници с двумя фильтрами оригинал
        //string dd = http://opttextil.ru/postelnoe-bele/?filter=[["2"%2C"2021"]%2C["1"%2C"214"]]&p=2;

        //Добавление параметров в фильр или их изменение
        private static int[,] ActiveFilterArr;

        #region хранение жизнидеятельности фильтров
        public static int[] dataPid;
        public static int[] dataValue;
        public static bool FilterBoolIzmenen;
        public static string LasFilter;
        #endregion

        #region жадный процент
        public static double Procent = 0.75;
        #endregion

        #region
        public static bool FullSizeBool = false;
        public static string FullSizeName;
        #endregion
    }
}
