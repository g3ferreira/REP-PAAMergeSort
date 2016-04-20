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


        public void lerEscreverArquivo(string caminhoArquivoLeitura, string caminhoArquivoEscrita, string tamArquivoPrincipal, string tamArquivoSecundario, string KVetores)
        {
            int KVEtores = Convert.ToInt32(KVetores);
            int quantidadeBytes = Convert.ToInt32(tamArquivoSecundario) * (1024 ^ 2);
            byte[] bytesToRead = new byte[quantidadeBytes];

            Utils.logList.Add("Tamanho Arquivo Principal Desordenado: " + tamArquivoPrincipal + "MB");
            Utils.logList.Add("Tamanho Arquivo Secundario Desordenado: " + tamArquivoSecundario + "KB");

            double quantidadeArquivosDouble = (double)(Convert.ToInt32(tamArquivoPrincipal) * 1024) / (double)Convert.ToInt32(tamArquivoSecundario);
            int quantidadeArquivosSecudarios = (int) quantidadeArquivosDouble;

            if (!(quantidadeArquivosSecudarios == quantidadeArquivosDouble))
            {
                quantidadeArquivosSecudarios++;
            }

            Utils.logList.Add("Serão Criados " + quantidadeArquivosSecudarios + " Arquivos Secundários de: " + tamArquivoSecundario + "KB");

            try
            {
               // separarArquivoSecundarios(caminhoArquivoLeitura, caminhoArquivoEscrita, bytesToRead, quantidadeArquivosSecudarios);
                combinarArquivosSecundariosByKVetores(quantidadeArquivosSecudarios, KVEtores, caminhoArquivoEscrita + @"\arquivos-secundarios-ordenados", caminhoArquivoEscrita + @"\arquivos-combinados-ordenados", bytesToRead);
            }
            catch (FileNotFoundException ioEx)
            {
                Console.WriteLine(ioEx.Message);
            }

        }

        public void separarArquivoSecundarios(string caminhoArquivoLeitura, string caminhoArquivoEscrita, byte[] bytesToRead, int quantidadeArquivosSecudarios)
        {
            using (FileStream fsSource = new FileStream(caminhoArquivoLeitura, FileMode.Open, FileAccess.Read))
            {
                int numBytesToRead = (int)bytesToRead.Length;

                int posicaoAtual = numBytesToRead;

                for (int i = 1; i <= quantidadeArquivosSecudarios; i++)
                {
                    posicaoAtual = getPosicao(i, posicaoAtual, numBytesToRead);
                    fsSource.Seek(posicaoAtual, SeekOrigin.Begin);
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
                    result = retirarSeparador(result);
                    int[] numeros = Array.ConvertAll(result.Split('-'), b => Convert.ToInt32(b));
                    mergeSortController.mergeSortRecursivo(numeros, 0, numeros.Length - 1);
                    escreverNumerosArquivo(numeros, caminhoArquivoEscrita + @"\arquivos-secundarios-ordenados", @"\arquivo-secundario-ordernado", i);

                }
            }

        
        }
        public void combinarArquivosSecundariosByKVetores(int quantidadeArquivosCriados, int KVetores, string caminhoArquivoLeitura, string caminhoArquivoEscrita, byte[] bytesToRead)
        {

            int numBytesToRead = (int)bytesToRead.Length;
            int posicaoAtual = numBytesToRead;
            int[] vetorArquivoPrimario;
            int[] vetorArquivoSecudario;
            int contadorArquivoCombinado = 1;
            int combinacoes =1;

            double quantidadeDeCombinacoesDouble = (double)quantidadeArquivosCriados / (double) KVetores;
            int quantidadeDeCombinacoes = (int)quantidadeDeCombinacoesDouble;

            if (!(quantidadeDeCombinacoes == quantidadeDeCombinacoesDouble))
            {
                quantidadeDeCombinacoes ++;
            }

            Console.WriteLine("Quantidade de Combinacoes: " + quantidadeDeCombinacoes);
            int contadorArquivoLeitura =1 ;
            for (; combinacoes <= quantidadeDeCombinacoes; combinacoes++)
            {
                Console.WriteLine("Arquivo Primario a Combinar: " + contadorArquivoLeitura);
                Console.WriteLine("Arquivo Secundario a Combinar: " + (contadorArquivoLeitura+1));

                vetorArquivoPrimario = lerArquivoPorPosicao(caminhoArquivoLeitura, contadorArquivoLeitura, posicaoAtual, bytesToRead);

                if (contadorArquivoLeitura == ((quantidadeDeCombinacoes+combinacoes)-1))
                {
                    string caminhoArquivoNaoCombinado = caminhoArquivoLeitura + @"\arquivo-secundario-ordernado-" + contadorArquivoLeitura + ".txt";
                    Utils.logList.Add("Arquivo Não Combinado: " + caminhoArquivoNaoCombinado);
                    File.Copy(caminhoArquivoNaoCombinado, caminhoArquivoEscrita + @"\arquivos-combinados-ordenados\arquivo-secundario-ordernado-" + contadorArquivoLeitura + ".txt");
                    break;  
                }

                vetorArquivoSecudario = lerArquivoPorPosicao(caminhoArquivoLeitura, (contadorArquivoLeitura + 1), posicaoAtual, bytesToRead);

               /* int[] vetorResultadoCombinacao = new int[vetorArquivoPrimario.Length + vetorArquivoSecudario.Length];
                vetorArquivoPrimario.CopyTo(vetorResultadoCombinacao, 0);
                vetorArquivoSecudario.CopyTo(vetorResultadoCombinacao, vetorArquivoPrimario.Length);
                */

                int[] vetorResultadoCombinacao = concatenarVetores(vetorArquivoPrimario, vetorArquivoSecudario);

                mergeSortController.mergeSortRecursivo(vetorResultadoCombinacao, 0, vetorResultadoCombinacao.Length - 1);
                escreverNumerosArquivo(vetorResultadoCombinacao, caminhoArquivoEscrita + @"\arquivos-combinados-ordenados", @"\arquivos-combinados-ordenados", contadorArquivoCombinado);
                
                contadorArquivoCombinado++;               
                contadorArquivoLeitura = contadorArquivoLeitura + 2;
            }
                      
        
        }

        public int[] concatenarVetores(int[] vetorArquivoPrimario, int[] vetorArquivoSecudario)
        {

            int[] vetorResultadoCombinacao = new int[vetorArquivoPrimario.Length + vetorArquivoSecudario.Length];
            vetorArquivoPrimario.CopyTo(vetorResultadoCombinacao, 0);
            vetorArquivoSecudario.CopyTo(vetorResultadoCombinacao, vetorArquivoPrimario.Length);
            return vetorResultadoCombinacao;

        }

        public int[] lerArquivoPorPosicao(string caminhoArquivoLeitura, int contadorArquivo, int posicaoAtual, byte[] bytesToRead)
        {
            int[] vetorArquivoPrimario;
            int numBytesToRead = (int)bytesToRead.Length;

            using (FileStream fsSource = new FileStream(caminhoArquivoLeitura + @"\arquivo-secundario-ordernado-" + contadorArquivo + ".txt", FileMode.Open, FileAccess.Read))
            {
                posicaoAtual = getPosicao(contadorArquivo, posicaoAtual, numBytesToRead);
                fsSource.Seek(posicaoAtual, SeekOrigin.Begin);
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
                result = retirarSeparador(result);
                vetorArquivoPrimario = Array.ConvertAll(result.Split('-'), b => Convert.ToInt32(b));
                fsSource.Close();
            }

            return vetorArquivoPrimario;
        
        }

        public void escreverNumerosArquivo(int[] numeros, string caminhoArquivoEscritaPasta, string nomeArquivo, int contadorArquivo)
        {
            if(!Directory.Exists(caminhoArquivoEscritaPasta))   Directory.CreateDirectory(caminhoArquivoEscritaPasta);        

            string caminhoArquivoOrdenado = caminhoArquivoEscritaPasta +nomeArquivo + "-" +contadorArquivo + ".txt";
            File.WriteAllText(caminhoArquivoOrdenado, String.Join("-", numeros.Select(x => x.ToString())));
            Utils.logList.Add("Arquivo: " + caminhoArquivoOrdenado + "    Criado!!");

        }

        public int getPosicao(int contadorPosicao, int posicaoAtual, int numeroBytesLeitura)
        {
            int posicaoIniciarLeitura = 0;
            switch (contadorPosicao)
            {
                case 1:
                   // Console.WriteLine("Posicao Atual: " + posicaoAtual);
                   // Console.WriteLine("posicaoAtual: " + 0);
                    posicaoIniciarLeitura = 0;
                    break;

                case 2:
                    posicaoIniciarLeitura = posicaoAtual + numeroBytesLeitura + 1;
                   // Console.WriteLine("Numero de Bytes Para Leitura: " + numeroBytesLeitura);
                   // Console.WriteLine("Posicao Atual: " + posicaoAtual);
                   // Console.WriteLine("Posicao Iniciar Leitura: " + posicaoIniciarLeitura);
                    break;

                default:
                    posicaoIniciarLeitura = (posicaoAtual + numeroBytesLeitura);
                  //  Console.WriteLine("Posicao Atual: " + posicaoAtual);
                  //  Console.WriteLine("Posicao Iniciar Leitura: " + posicaoIniciarLeitura);
                    break;
            }

            return posicaoIniciarLeitura;

        }

        public string retirarSeparador(string numerosComSeparador)
        {
            if ((numerosComSeparador.StartsWith("-") && (numerosComSeparador.StartsWith("-"))))
            {
                numerosComSeparador = numerosComSeparador.Substring(1, numerosComSeparador.Length - 2);
            }
            else if (numerosComSeparador.StartsWith("-"))
            {
                numerosComSeparador = numerosComSeparador.Substring(1);
            }
            else if (numerosComSeparador.EndsWith("-"))
            {
                numerosComSeparador = numerosComSeparador.Substring(0, numerosComSeparador.Length - 1);
            }

            return numerosComSeparador;

        }



    }
}
