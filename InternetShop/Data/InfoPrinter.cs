using System;
using System.Collections.Generic;
using System.Threading;
using InternetShop.Shop;

namespace InternetShop.Data
{
    public static class InfoPrinter
    {

        public const int TableWidth = 203;

        public static void PrintLine()
        {
            Console.WriteLine(new string('-', TableWidth));
        }

        public static void PrintRow(params string[] columns)
        {
            int width = (TableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        public static void PrintOneRow(params string[] columns)
        {
            PrintRow(columns);
            PrintLine();
        }
        
        public static void PrintPriceRow(params double[] columns)
        {
            int width = (TableWidth - columns.Length) / columns.Length;
            string row = "|";
            foreach (int column in columns)
            {
                row += AlignCentre((column > 0 ? " Price: " + column + " UAH" : ""), width) + "|";
            }
            Console.WriteLine(row);
            PrintLine();
        }

        private static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', width);
            }
            else
            {
                return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
            }
        }
    }
}