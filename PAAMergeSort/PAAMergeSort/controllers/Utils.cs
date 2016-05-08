using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace PAAMergeSort.controllers
{
    class Utils
    {

        public static List<String> logList = new List<string>();
        static int p = 0;
        public static int randonRangeNumber(int initialNumber, int finalNumber)
        {
            Random randonNumber = new Random();
            return randonNumber.Next(initialNumber, finalNumber);
        }

        public static void armazenarListaResultado(List<int> listaResultado, string caminhoArquivoEscrita, string nomeArquivo, int posicaoEscrita)
        {
            ArquivoController.escreverArquivoPosicao(caminhoArquivoEscrita, nomeArquivo, posicaoEscrita, listaResultado.ToArray());

        }

        
        public static List<int> armazenar(List<int> l1, List<int> l2, string caminhoArquivoEscrita, string nomeArquivo, int posicaoEscrita)
        {
            // Console.WriteLine("Escrcita em : " + caminhoArquivoEscrita + nomeArquivo);
            //ArquivoController.escreverArquivoPosicao(caminhoArquivoEscrita, nomeArquivo, posicaoEscrita, new byte[2]);
            List<int> listaArmazenar = new List<int>();
            List<int> listaMaior = new List<int>();

            int i = 0;
            int j = 0;
            try
            {
                while ((l1.Count != 0) && (l2.Count != 0))
                {
                    // p++;

                    if (l1[i] == l2[j])
                    {
                        //Console.WriteLine("l1 == l2");
                        listaArmazenar.Add(l1[i]);
                        listaArmazenar.Add(l2[j]);
                        l1.RemoveAt(i);
                        l2.RemoveAt(j);
                        //i++;
                        //j++;
                    }
                    else if (l1[i] > l2[j])
                    {
                        //Console.WriteLine("l1 > l2");

                        listaArmazenar.Add(l2[j]);

                        l2.RemoveAt(j);
                        //j++;
                        //i++;
                    }
                    else if (l1[i] < l2[j])
                    {
                        //Console.WriteLine("l1 < l2");
                        listaArmazenar.Add(l1[i]);
                        l1.RemoveAt(i);

                        //i++;
                        // j++;
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("pos i: " + i);
                Console.WriteLine("pos j: " + j);
                Console.WriteLine("ERROR: " + e.Message);
            }

            //  Console.WriteLine("tamanho de bytes: " + getByteArray(listaArmazenar));
            ArquivoController.escreverArquivoPosicao(caminhoArquivoEscrita, nomeArquivo, posicaoEscrita, listaArmazenar.ToArray());

            if (l1.Count == 0 && l2.Count == 0)
            {
                //  Console.WriteLine("lista 1 vazia");
                // Console.WriteLine("lista 2 vazia");
                listaMaior = l1;
                // Console.WriteLine("Pilha Iguais");
                //l1 = getListN(3);
                //l2 = getListN(3);
                //armazenar(l1, l2);
                // break;

            }

            if (l1.Count == 0 && l2.Count > 0)
            {
                // Console.WriteLine("lista 1 vazia");
                //  Console.WriteLine("lista 2 cheia");
                listaMaior = l2;
                //Console.WriteLine("Pilha B maior");
                //l1 = getListN(3);

                // armazenar(l1, l2);
            }
            else if (l1.Count > 0 && l2.Count == 0)
            {
                // Console.WriteLine("lista 1 cheia");
                // Console.WriteLine("lista 2 vazia");
                listaMaior = l1;

                //Console.WriteLine("Pilha A maior");
                // l2 = getListN(3);
                // armazenar(l1, l2);
            }

            /*  foreach (var item in listR)
              {
                  Console.WriteLine("numero: " + item);
              }
              */

            return listaMaior;
        }

        public static List<int> getListN(int numero)
        {
            List<int> a = new List<int>();
            a.Add(numero);
            a.Add(numero);
            return a;

        }

        public static byte[] getByteArray(List<int> lista)
        {
            int[] intArray = lista.ToArray();
            byte[] result = new byte[intArray.Length * sizeof(int)];
            Buffer.BlockCopy(intArray, 0, result, 0, result.Length);
            return result;
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
