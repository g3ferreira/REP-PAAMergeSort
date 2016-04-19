using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PAAMergeSort.controllers
{
    public class ArquivoController
    {
        MergeSortController mergeSortController = new MergeSortController();
        public static string getNumerosByArquivo(string caminhoArquivo)
        {
            string numeros = System.IO.File.ReadAllText(caminhoArquivo);
            return numeros;
        }

        public static int[] ordenarNumeros(int[] numeros)
        {
            int[] numerosArray = new int[numeros.Length];
            // numerosArray = fazerMergeRecursivo(numeros);
            return numerosArray;

        }

        public void escreverNumerosNoArquivo(string caminhoArquivo, int[] arrayNumeros)
        {
            System.IO.File.WriteAllText(caminhoArquivo, String.Join("-", arrayNumeros.Select(x => x.ToString())));
        }

        public int[] fazerMergeRecursivo(int[] numerosArray)
        {
            mergeSortController.mergeSortRecursivo(numerosArray, 0, numerosArray.Length - 1);
            Console.WriteLine(string.Empty);
            return numerosArray;
        }

        public void getNumerosByLineArquivo(string caminhoArquivo)
        {
            string[] lines = System.IO.File.ReadAllLines(caminhoArquivo);
            System.Console.WriteLine("Conteudo do arquivo: ");
            foreach (string line in lines)
            {
                Console.WriteLine("\t" + line);
            }

        }


        public void lerEscreverArquivo(string caminhoArquivoLeitura, string caminhoArquivoEscrita, string tamArquivoPrincipal, string tamArquivoSecundario)
        {


            int quantidadeBytes = Convert.ToInt32(tamArquivoSecundario) * (1024 ^ 2);
            byte[] bytesToRead = new byte[quantidadeBytes];

            Utils.logList.Add("Tamanho Arquivo Principal Desordenado: " + tamArquivoPrincipal + "MB");
            Utils.logList.Add("Tamanho Arquivo Secundario Desordenado: " + tamArquivoSecundario + "KB");


            double quantidadeArquivosDouble = (double)(Convert.ToInt32(tamArquivoPrincipal) * 1024) / (double)Convert.ToInt32(tamArquivoSecundario);
            int quantidadeArquivos = 0;

            if (quantidadeArquivosDouble < (quantidadeArquivosDouble + 0.1))
            {
                quantidadeArquivos = Convert.ToInt32(Math.Round(quantidadeArquivosDouble + 1));
            }

            Utils.logList.Add("Serão Criados " + quantidadeArquivos + " Arquivos Secundários de: " + tamArquivoSecundario + "KB");


            try
            {
                using (FileStream fsSource = new FileStream(caminhoArquivoLeitura, FileMode.Open, FileAccess.Read))
                {
                    int numBytesToRead = (int)bytesToRead.Length;

                    int posicao = numBytesToRead;

                    for (int i = 1; i <= quantidadeArquivos; i++)
                    {
                        switch (i)
                        {
                            case 1:
                                Console.WriteLine("posicao: " + 0);
                                fsSource.Seek(0, SeekOrigin.Begin);
                                break;

                            case 2:
                                posicao = posicao + 1;
                                Console.WriteLine("Bytesto read: " + numBytesToRead);
                                Console.WriteLine("posicao: " + posicao);
                                fsSource.Seek(posicao, SeekOrigin.Begin);
                                break;

                            default:
                                posicao = (posicao + numBytesToRead);
                                Console.WriteLine("posicao: " + posicao);
                                fsSource.Seek(posicao, SeekOrigin.Begin);
                                break;
                        }

                        int numBytesRead = 0;
                        numBytesToRead = (int)bytesToRead.Length;
                        while (numBytesToRead > 0)
                        {
                            int n = fsSource.Read(bytesToRead, numBytesRead, numBytesToRead);

                            if (n == 0) break;

                            numBytesRead += n;
                            numBytesToRead -= n;
                        }

                        numBytesToRead = bytesToRead.Length;
                        string result = System.Text.Encoding.UTF8.GetString(bytesToRead);

                        //  string result = "-10-1-2-3-4-5-6-7-8-9-10-";

                        if ((result.StartsWith("-") && (result.StartsWith("-"))))
                        {
                            result = result.Substring(1, result.Length - 2);
                        }
                        else if (result.StartsWith("-"))
                        {
                            result = result.Substring(1);
                        }
                        else if (result.EndsWith("-"))
                        {
                            result = result.Substring(0, result.Length - 1);
                        }


                        int[] numeros = Array.ConvertAll(result.Split('-'), b => Convert.ToInt32(b));
                        mergeSortController.mergeSortRecursivo(numeros, 0, numeros.Length - 1);
                        string caminhoArquivoOrdenado = caminhoArquivoEscrita + @"\arquivo-ordernado-" + i + ".txt";
                        File.WriteAllText(caminhoArquivoOrdenado, String.Join("-", numeros.Select(x => x.ToString())));
                        Utils.logList.Add("Arquivo: " + caminhoArquivoOrdenado + "    Criado!!");

                    }
                }
            }
            catch (FileNotFoundException ioEx)
            {
                Console.WriteLine(ioEx.Message);
            }


        }



    }
}
