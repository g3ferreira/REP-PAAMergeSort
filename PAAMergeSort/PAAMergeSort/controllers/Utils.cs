using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Drawing;

namespace PAAMergeSort.controllers
{
    class Utils
    {

        public static List<String> logList = new List<string>();

        public static int randonRangeNumber(int initialNumber, int finalNumber)
        {
            Random randonNumber = new Random();
            return randonNumber.Next(initialNumber, finalNumber);
        }



        public static string GetMD5Hash(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();

        }

        public static byte[] combineByteArray(string message)
        {

            Byte[] CR = new Byte[] { 0x0D };
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            byte[] c = new byte[data.Length + CR.Length];
            byte[] initc = new byte[1];
            initc[0] = data[0];
            System.Buffer.BlockCopy(data, 0, c, 0, data.Length);
            System.Buffer.BlockCopy(CR, 0, c, 0, CR.Length);
            string a = System.Text.Encoding.UTF8.GetString(c);
            c[c.Length - 1] = c[0];
            c[0] = initc[0];

            return c;

        }
      
       
      

        
    }
}
