using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PAAMergeSort.controllers
{
    public class ArquivoController
    {
        static int posicaoArquivoA = 0;
        static int posicaoArquivoB = 0;
        static int posicaoArquivoCombinado = 0;
        static int tamanhoArquivoA = 1;
        static int tamanhoArquivoB = 1;



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
            int umMB = 1024 * 1024;
            // long cemMB = umMB * 100;
            //      long cinquentaMB = cemMB / 2;

            int quantidadeBytes = (Convert.ToInt32(tamArquivoSecundario) * (umMB));
            byte[] bytesToRead = new byte[quantidadeBytes];

            Utils.logList.Add("Tamanho Arquivo Principal Desordenado: " + tamArquivoPrincipal + "MB");
            Utils.logList.Add("Tamanho Arquivo Secundario Desordenado: " + tamArquivoSecundario + "MB");

            double quantidadeArquivosDouble = (double)(Convert.ToInt32(tamArquivoPrincipal) * (1024)) / ((double)Convert.ToInt32(tamArquivoSecundario) * (1024));
            int quantidadeArquivosSecudarios = (int)quantidadeArquivosDouble;

            if (!(quantidadeArquivosSecudarios == quantidadeArquivosDouble))
            {
                quantidadeArquivosSecudarios++;
            }

            Utils.logList.Add("Serão Criados " + quantidadeArquivosSecudarios + " Arquivos Secundários de: " + tamArquivoSecundario + "MB");

            try
            {
           

                separarArquivoSecundarios(caminhoArquivoLeitura, caminhoArquivoEscrita, bytesToRead, quantidadeArquivosSecudarios);
                // combinarArquivosSecundariosByKVetores(quantidadeArquivosSecudarios, KVEtores, caminhoArquivoEscrita + @"\arquivos-secundarios-ordenados", caminhoArquivoEscrita + @"\arquivos-combinados-ordenados", bytesToRead);
                combinarArquivosSecundariosByKVetores2(quantidadeArquivosSecudarios, KVEtores, caminhoArquivoEscrita + @"\arquivos-secundarios-ordenados", caminhoArquivoEscrita + @"\arquivos-combinados-ordenados", bytesToRead);

                //string arquivoNaoCombinado = @"D:\genilson-ferreira\documents\faculdade\paa\trabalho\arquivos\arquivos-combinados-ordenados\arquivo-secundario-ordernado-21.txt";

                //  lerArquivosByAlturaArvore(quantidadeCombinacoes, KVetoresB, arquivoNaoCombinado, caminhoArquivoLeituraB, @"\arquivos-combinados-ordenados", @"\arquivos-combinados-ordenados-H", bytesToRead);

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
                Console.WriteLine("Tamanho do Arquivo: " + fsSource.Length);
                int numBytesToRead = (int)bytesToRead.Length;
                int posicaoAtual = 0;

                for (int i = 1; i < quantidadeArquivosSecudarios; i++)
                {

                    posicaoAtual = getPosicao(i, numBytesToRead);
                    fsSource.Seek(posicaoAtual, SeekOrigin.Begin);

                    int numBytesRead = 0;
                    if (i == quantidadeArquivosSecudarios)
                    {
                        int bytesRead = (int)fsSource.Length - posicaoAtual;
                        bytesToRead = new byte[bytesRead];
                        numBytesToRead = (int)bytesToRead.Length;
                    }
                    while (numBytesToRead > 0)
                    {
                        int n = fsSource.Read(bytesToRead, numBytesRead, numBytesToRead);
                        if (n == 0) break;
                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                    string result = string.Empty;
                    try
                    {

                        numBytesToRead = bytesToRead.Length;
                        result = System.Text.Encoding.UTF8.GetString(bytesToRead);
                        string a = retirarSeparador(result.Trim());
                        var numeros = Array.ConvertAll(a.Split('-'), b => Convert.ToInt32(b));
                        mergeSortController.mergeSortRecursivo(numeros, 0, numeros.Length - 1);
                        //escreverArquivoPosicao(caminhoArquivoEscrita + @"\arquivos-secundarios-ordenados", @"\arquivo-secundario-ordernado-" + i, posicaoAtual, numeros);

                        escreverNumerosArquivo(numeros, caminhoArquivoEscrita + @"\arquivos-secundarios-ordenados", @"\arquivo-secundario-ordernado", i);
                    }
                    catch (Exception e)
                    {
                        //Console.WriteLine("error: " + result);
                    }

                }
            }



        }

        void Foo(byte[] a)
        {
            a = new byte[0];
        }
        public void combinarArquivosSecundariosByKVetores(int quantidadeArquivosCriados, int KVetores, string caminhoArquivoLeitura, string caminhoArquivoEscrita, byte[] bytesToRead)
        {

            int numBytesToRead = (int)bytesToRead.Length;
            int posicaoAtual = 1;
            int[] vetorArquivoPrimario;
            int[] vetorArquivoSecudario;
            int contadorArquivoCombinado = 1;
            int combinacoes = 1;
            string caminhoArquivoNaoCombinado = "";
            double quantidadeDeCombinacoesDouble = (double)quantidadeArquivosCriados / (double)KVetores;
            int quantidadeDeCombinacoes = (int)quantidadeDeCombinacoesDouble;

            if (!(quantidadeDeCombinacoes == quantidadeDeCombinacoesDouble))
            {
                quantidadeDeCombinacoes++;
            }

            Console.WriteLine("Quantidade de Combinacoes: " + quantidadeDeCombinacoes);
            int contadorArquivoLeitura = 1;
            for (; combinacoes <= quantidadeDeCombinacoes; combinacoes++)
            {
                Console.WriteLine("Arquivo Primario a Combinar: " + contadorArquivoLeitura);
                Console.WriteLine("Arquivo Secundario a Combinar: " + (contadorArquivoLeitura + 1));

                vetorArquivoPrimario = lerArquivoPorPosicao(caminhoArquivoLeitura, @"\arquivo-secundario-ordernado-" + contadorArquivoLeitura, posicaoAtual, bytesToRead);

                if (contadorArquivoLeitura == ((quantidadeDeCombinacoes + combinacoes) - 1))
                {
                    caminhoArquivoNaoCombinado = caminhoArquivoLeitura + @"\arquivo-secundario-ordernado-" + contadorArquivoLeitura + ".txt";
                    Utils.logList.Add("Arquivo Não Combinado: " + caminhoArquivoNaoCombinado);
                    File.Copy(caminhoArquivoNaoCombinado, caminhoArquivoEscrita + @"\arquivo-secundario-ordernado-" + contadorArquivoLeitura + ".txt");
                    break;
                }

                vetorArquivoSecudario = lerArquivoPorPosicao(caminhoArquivoLeitura, @"\arquivo-secundario-ordernado-" + (contadorArquivoLeitura + 1), posicaoAtual, bytesToRead);

                int[] vetorResultadoCombinacao = concatenarVetores(vetorArquivoPrimario, vetorArquivoSecudario);

                mergeSortController.mergeSortRecursivo(vetorResultadoCombinacao, 0, vetorResultadoCombinacao.Length - 1);
                escreverNumerosArquivo(vetorResultadoCombinacao, caminhoArquivoEscrita, @"\arquivos-combinados-ordenados", contadorArquivoCombinado);

                posicaoAtual++;
                contadorArquivoCombinado++;
                contadorArquivoLeitura = contadorArquivoLeitura + 2;
            }

            lerArquivosByAlturaArvore(quantidadeDeCombinacoes, KVetores, caminhoArquivoNaoCombinado, caminhoArquivoEscrita, @"\arquivos-combinados-ordenados", @"\arquivos-combinados-ordenados-H", bytesToRead);


        }




        public void lerArquivosByAlturaArvore(int quantidadeCombinacoes, int KVetores, string caminhoArquivoNaoCombinado, string caminhoArquivoLeitura, string nomeArquivo, string nomePastaEscrita, byte[] bytesToRead)
        {
            //byte[] bytesToRead2 = new byte[bytesToRead.Length * 2];
            Console.WriteLine("lerArquivosByAlturaArvore :");

            Console.WriteLine("quantidadeCombinacoes Antes :" + quantidadeCombinacoes);
            Console.WriteLine("KVetores :" + KVetores);
            Console.WriteLine("caminhoArquivoNaoCombinado :" + caminhoArquivoNaoCombinado);
            Console.WriteLine("caminhoArquivoLeitura :" + caminhoArquivoLeitura);
            Console.WriteLine("bytesToRead :" + bytesToRead);

            double quantidadeDeCombinacoesDouble = (double)quantidadeCombinacoes / (double)KVetores;
            int quantidadeDeCombinacoes = (int)quantidadeDeCombinacoesDouble;
            if (!(quantidadeDeCombinacoes == quantidadeDeCombinacoesDouble))
            {
                quantidadeDeCombinacoes++;
            }

            string caminhoLeitura = caminhoArquivoLeitura;
            string nomeArquivob = nomeArquivo + "-";
            string caminhoEscrita = nomePastaEscrita + quantidadeDeCombinacoes;

            string caminhoLeituraProximo = caminhoArquivoLeitura + nomePastaEscrita + quantidadeDeCombinacoes;
            string nomeArquivoProximo = nomePastaEscrita + quantidadeDeCombinacoes;
            string nomeProximaPastaEscrita = nomePastaEscrita + quantidadeDeCombinacoes;

            if (!(quantidadeDeCombinacoes == 1))
            {
                if (quantidadeDeCombinacoes % 2 == 0) // par
                {
                    Console.WriteLine("Combinacoes Par: " + quantidadeDeCombinacoes);
                    Console.WriteLine("Caminho Leitura " + caminhoLeitura);
                    Console.WriteLine("Nomea Arquivo " + nomeArquivob);
                    Console.WriteLine("Arquivo nao combinado" + caminhoArquivoNaoCombinado);
                    Console.WriteLine("Nomea Pasta Escrita " + caminhoEscrita);

                    caminhoArquivoNaoCombinado = leitura(quantidadeDeCombinacoes, caminhoLeitura, nomeArquivob, caminhoArquivoNaoCombinado, caminhoEscrita, bytesToRead);

                    Console.WriteLine("Caminho Leitura Proximo " + caminhoLeituraProximo);
                    Console.WriteLine("Nomea Arquivo Proximo " + nomeArquivoProximo);
                    Console.WriteLine("Arquivo nao combinado" + caminhoArquivoNaoCombinado);
                    Console.WriteLine("Nome Pasta Escrita Proximo" + nomeProximaPastaEscrita);

                    lerArquivosByAlturaArvore(quantidadeDeCombinacoes, KVetores, caminhoArquivoNaoCombinado, caminhoLeituraProximo, nomeArquivoProximo, nomeProximaPastaEscrita, bytesToRead);
                }
                else // impar
                {
                    Console.WriteLine("Combinacoes Impar: " + quantidadeDeCombinacoes);
                    Console.WriteLine("Caminho Leitura " + caminhoLeitura);
                    Console.WriteLine("Nomea Arquivo " + nomeArquivob);
                    Console.WriteLine("Arquivo nao combinado" + caminhoArquivoNaoCombinado);
                    Console.WriteLine("Nomea Pasta Escrita " + caminhoEscrita);

                    caminhoArquivoNaoCombinado = leitura(quantidadeDeCombinacoes, caminhoArquivoLeitura, nomeArquivo + "-", caminhoArquivoNaoCombinado, nomePastaEscrita + quantidadeDeCombinacoes, bytesToRead);

                    Console.WriteLine("Caminho Leitura Proximo " + caminhoLeituraProximo);
                    Console.WriteLine("Nomea Arquivo Proximo " + nomeArquivoProximo);
                    Console.WriteLine("Arquivo nao combinado" + caminhoArquivoNaoCombinado);
                    Console.WriteLine("Nome Pasta Escrita Proximo" + nomeProximaPastaEscrita);

                    lerArquivosByAlturaArvore(quantidadeDeCombinacoes, KVetores, caminhoArquivoNaoCombinado, caminhoLeituraProximo, nomeArquivoProximo, nomeProximaPastaEscrita, bytesToRead);

                }

            }
            else
            {
                int[] vetorArquivoPrimario = null;
                int[] vetorArquivoSecudario = null;
                int[] vetorResultadoCombinacao = null;

                DirectoryInfo di = new DirectoryInfo(caminhoArquivoLeitura);
                string ultimoArquivo = di.GetFiles().Select(fi => fi.Name).FirstOrDefault();
                ultimoArquivo = ultimoArquivo.Replace(".txt", string.Empty);

                vetorArquivoPrimario = lerArquivoPorPosicao(caminhoArquivoLeitura, nomeArquivob + "1", 1, bytesToRead);
                vetorArquivoSecudario = lerArquivoPorPosicao(caminhoArquivoLeitura, @"\\" + ultimoArquivo, 1, bytesToRead);
                vetorResultadoCombinacao = concatenarVetores(vetorArquivoPrimario, vetorArquivoSecudario);

                mergeSortController.mergeSortRecursivo(vetorResultadoCombinacao, 0, vetorResultadoCombinacao.Length - 1);
                escreverNumerosArquivo(vetorResultadoCombinacao, caminhoArquivoLeitura + nomeArquivoProximo, "arquivo-ordenado", 1);
                Utils.logList.Add(">>>>>>>>>>>>>>>>>>>>>>>>>>    Ordenação Concluída com Sucesso !!!");
                frmHomeScreen._timerCronometro.Stop();

            }

        }

        public string leitura(int quantidadeDeCombinacoes, string caminhoArquivoLeitura, string nomeArquivo, string caminhoArquivoNaoCombinado, string nomeArquivoEscrita, byte[] bytesToRead)
        {
            int[] vetorArquivoPrimario;
            int[] vetorArquivoSecudario = null;
            int[] vetorResultadoCombinacao;
            int posicaoAtual = 1;
            int contadorArquivoPrimario = 1;

            for (int contadorArquivoLeitura = 1; contadorArquivoLeitura <= quantidadeDeCombinacoes; contadorArquivoLeitura++)
            {
                Console.WriteLine("Arquivo Primario a Combinar: " + nomeArquivo + contadorArquivoPrimario);
                Console.WriteLine("Arquivo Secundario a Combinar: " + nomeArquivo + (contadorArquivoPrimario + 1));
                vetorArquivoPrimario = lerArquivoPorPosicao(caminhoArquivoLeitura, nomeArquivo + contadorArquivoPrimario, posicaoAtual, bytesToRead);

                if (quantidadeDeCombinacoes % 2 == 0) // par
                {
                    vetorArquivoSecudario = lerArquivoPorPosicao(caminhoArquivoLeitura, nomeArquivo + (contadorArquivoPrimario + 1), posicaoAtual, bytesToRead);

                    if (contadorArquivoLeitura == (quantidadeDeCombinacoes - 1))
                    {
                        vetorResultadoCombinacao = concatenarVetores(vetorArquivoPrimario, vetorArquivoSecudario);
                        mergeSortController.mergeSortRecursivo(vetorResultadoCombinacao, 0, vetorResultadoCombinacao.Length - 1);
                        escreverNumerosArquivo(vetorResultadoCombinacao, caminhoArquivoLeitura + nomeArquivoEscrita, nomeArquivoEscrita, contadorArquivoLeitura);
                        if (!(caminhoArquivoNaoCombinado.Equals(string.Empty)))
                        {
                            string nomeArquivoNaoCombinado = caminhoArquivoNaoCombinado.Split('\\')[8].Replace(".txt", string.Empty);
                            File.Copy(caminhoArquivoNaoCombinado, caminhoArquivoLeitura + nomeArquivoEscrita + @"\" + nomeArquivoNaoCombinado + ".txt");
                            caminhoArquivoNaoCombinado = caminhoArquivoLeitura + nomeArquivo + @"\" + nomeArquivoNaoCombinado + ".txt";
                            Utils.logList.Add("Arquivo Nao Combinado Copiado: " + nomeArquivoNaoCombinado);
                            break;
                        }
                        else
                        {
                            string arquivoACopiar = caminhoArquivoLeitura + nomeArquivo + (contadorArquivoPrimario + 2) + ".txt";
                            string[] nomeArquivoSplit = arquivoACopiar.Split('\\');
                            string destFile = caminhoArquivoLeitura + nomeArquivoEscrita + @"\" + nomeArquivoSplit[10];
                            File.Copy(arquivoACopiar, destFile);
                            break;

                        }
                    }

                }
                else
                {
                    if (contadorArquivoLeitura == quantidadeDeCombinacoes)
                    {
                        if (!(caminhoArquivoNaoCombinado.Equals(string.Empty)))
                        {
                            string nomeArquivoNaoCombinado = caminhoArquivoNaoCombinado.Split('\\')[9].Replace(".txt", string.Empty);
                            vetorArquivoSecudario = lerArquivoPorPosicao(caminhoArquivoLeitura, @"\" + nomeArquivoNaoCombinado, posicaoAtual, bytesToRead);
                            caminhoArquivoNaoCombinado = string.Empty;
                        }
                    }
                    else
                    {
                        vetorArquivoSecudario = lerArquivoPorPosicao(caminhoArquivoLeitura, nomeArquivo + (contadorArquivoPrimario + 1), posicaoAtual, bytesToRead);

                    }

                }

                vetorResultadoCombinacao = concatenarVetores(vetorArquivoPrimario, vetorArquivoSecudario);
                mergeSortController.mergeSortRecursivo(vetorResultadoCombinacao, 0, vetorResultadoCombinacao.Length - 1);
                escreverNumerosArquivo(vetorResultadoCombinacao, caminhoArquivoLeitura + nomeArquivoEscrita, nomeArquivoEscrita, contadorArquivoLeitura);

                posicaoAtual++;
                contadorArquivoPrimario = contadorArquivoPrimario + 2;
            }

            return caminhoArquivoNaoCombinado;
        }


        public int[] concatenarVetores(int[] vetorArquivoPrimario, int[] vetorArquivoSecudario)
        {
            int[] vetorResultadoCombinacao = new int[vetorArquivoPrimario.Length + vetorArquivoSecudario.Length];
            vetorArquivoPrimario.CopyTo(vetorResultadoCombinacao, 0);
            vetorArquivoSecudario.CopyTo(vetorResultadoCombinacao, vetorArquivoPrimario.Length);
            return vetorResultadoCombinacao;
        }

        public int[] lerArquivoPorPosicao(string caminhoArquivoLeitura, string nomeArquivo, int posicaoAtual, byte[] bytesToRead)
        {
            int[] vetorArquivoPrimario;
            int numBytesToRead; //(int)bytesToRead.Length;
            using (FileStream fsSource = new FileStream(caminhoArquivoLeitura + nomeArquivo + ".txt", FileMode.Open, FileAccess.Read))
            {
                numBytesToRead = Convert.ToInt32(fsSource.Length);
                //posicaoAtual = getPosicao(posicaoAtual, numBytesToRead);
                fsSource.Seek(0, SeekOrigin.Begin);
                int numBytesRead = 0;

                bytesToRead = new byte[fsSource.Length];

                Console.WriteLine("tamanho do arquivo: " + fsSource.Length);
                Console.WriteLine("tamanho do numBytesToRead: " + numBytesToRead);
                Console.WriteLine("tamanho do vetor de bytes: " + bytesToRead.Length);

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
            }

            return vetorArquivoPrimario;

        }

        public int[] lerArquivoPrincipal(int pilhaMaior, int[] pilhaA, int[] pilhaB, int[] pilhaResultado, byte[] bytesToRead, int contadorArquivoLeitura, string caminhoArquivoLeitura, string nomeArquivoLeitura, string nomePastaArquivoCombinado, string nomeArquivoCombinado)
        {
            switch (pilhaMaior)
            {
                case 1:
                    pilhaA = lerArquivoAPorPosicao2(caminhoArquivoLeitura + nomePastaArquivoCombinado, nomeArquivoCombinado + (contadorArquivoLeitura - 1), bytesToRead);
                    if (!(pilhaA[0] == 1990))
                    {
                        pilhaResultado = Utils.armazenar(pilhaResultado.ToList(), pilhaA.ToList(), caminhoArquivoLeitura + nomePastaArquivoCombinado, nomeArquivoCombinado + contadorArquivoLeitura, 0).ToArray();
                    }
                    else
                    {
                        break;
                    }
                    break;
                case 0:
                    pilhaA = lerArquivoAPorPosicao2(caminhoArquivoLeitura + nomePastaArquivoCombinado, nomeArquivoCombinado + (contadorArquivoLeitura - 1), bytesToRead);
                    pilhaB = lerArquivoBPorPosicao2(caminhoArquivoLeitura, nomeArquivoLeitura + (contadorArquivoLeitura + 1), bytesToRead);
                    pilhaResultado = Utils.armazenar(pilhaA.ToList(), pilhaB.ToList(), caminhoArquivoLeitura + nomePastaArquivoCombinado, nomeArquivoCombinado + contadorArquivoLeitura, 0).ToArray();
                    break;
                case 2:


                    pilhaB = lerArquivoAPorPosicao2(caminhoArquivoLeitura + nomePastaArquivoCombinado, nomeArquivoCombinado + (contadorArquivoLeitura - 1), bytesToRead);
                    if (!(pilhaB[0] == 1990))
                    {
                        pilhaResultado = Utils.armazenar(pilhaResultado.ToList(), pilhaB.ToList(), caminhoArquivoLeitura + nomePastaArquivoCombinado, nomeArquivoCombinado + contadorArquivoLeitura, 0).ToArray();
                    }
                    else
                    {
                        break;
                    }

                    break;

                default:
                    if (!(pilhaA[0] == 1990 && pilhaB[0] == 1990))
                    {
                        pilhaA = lerArquivoAPorPosicao2(caminhoArquivoLeitura + nomePastaArquivoCombinado, nomeArquivoCombinado + (contadorArquivoLeitura - 1), bytesToRead);
                        pilhaB = lerArquivoBPorPosicao2(caminhoArquivoLeitura, nomeArquivoLeitura + (contadorArquivoLeitura + 1), bytesToRead);
                        pilhaResultado = Utils.armazenar(pilhaA.ToList(), pilhaB.ToList(), caminhoArquivoLeitura + nomePastaArquivoCombinado, nomeArquivoCombinado + contadorArquivoLeitura, 0).ToArray();
                    }
                    else
                    {
                        break;
                    }
                    break;
            }

            return pilhaResultado;

        }
        public void combinarArquivosSecundariosByKVetores2(int quantidadeArquivosCriados, int KVetores, string caminhoArquivoLeitura, string caminhoArquivoEscrita, byte[] bytesToRead)
        {
            int pilhaMaior = 0;
            int numBytesToRead = (int)bytesToRead.Length;
            int[] pilhaA = null;
            int[] pilhaB = null;
            int[] pilhaResultado = null;
            string nomeArquivoLeitura = @"\arquivo-secundario-ordernado-";
            string nomePastaArquivoCombinado = @"\combinado-ordenado\";
            string nomeArquivoCombinado = "arquivo-combinado-ordenado-";

            int combinacoes = 1;
            double quantidadeDeCombinacoesDouble = (double)quantidadeArquivosCriados / (double)KVetores;
            int quantidadeDeCombinacoes = (int)quantidadeDeCombinacoesDouble;

            if (!(quantidadeDeCombinacoes == quantidadeDeCombinacoesDouble))
            {
                quantidadeDeCombinacoes++;
            }

            Console.WriteLine("Quantidade de Combinacoes: " + ((quantidadeDeCombinacoes * 2)-1));
            int contadorArquivoLeitura = 1;

            for (; combinacoes <= (quantidadeDeCombinacoes * 2) - 1; combinacoes++)
            {

                while ((posicaoArquivoA != tamanhoArquivoA) && (posicaoArquivoB != tamanhoArquivoB))
                {
                    if (combinacoes > 1)
                    {

                        pilhaResultado = lerArquivoPrincipal(pilhaMaior, pilhaA, pilhaB, pilhaResultado, bytesToRead, contadorArquivoLeitura, caminhoArquivoLeitura, nomeArquivoLeitura, nomePastaArquivoCombinado, nomeArquivoCombinado);
                    }
                    else
                    {
                        switch (pilhaMaior)
                        {
                            case 1:
                                pilhaA = lerArquivoAPorPosicao2(caminhoArquivoLeitura, nomeArquivoLeitura + (contadorArquivoLeitura), bytesToRead);
                                if (!(pilhaA[0] == 1990))
                                {
                                    pilhaResultado = Utils.armazenar(pilhaResultado.ToList(), pilhaA.ToList(), caminhoArquivoLeitura + nomePastaArquivoCombinado, nomeArquivoCombinado + contadorArquivoLeitura, 0).ToArray();
                                }
                                else
                                {
                                    break;
                                }
                                break;

                            case 2:

                                pilhaB = lerArquivoBPorPosicao2(caminhoArquivoLeitura, nomeArquivoLeitura + (contadorArquivoLeitura + 1), bytesToRead);
                                if (!(pilhaB[0] == 1990))
                                {
                                    pilhaResultado = Utils.armazenar(pilhaResultado.ToList(), pilhaB.ToList(), caminhoArquivoLeitura + nomePastaArquivoCombinado, nomeArquivoCombinado + contadorArquivoLeitura, 0).ToArray();
                                }
                                else
                                {
                                    break;
                                }

                                break;
                            case 0:
                                pilhaA = lerArquivoAPorPosicao2(caminhoArquivoLeitura, nomeArquivoLeitura + (contadorArquivoLeitura), bytesToRead);
                                pilhaB = lerArquivoBPorPosicao2(caminhoArquivoLeitura, nomeArquivoLeitura + (contadorArquivoLeitura + 1), bytesToRead);
                                pilhaResultado = Utils.armazenar(pilhaA.ToList(), pilhaB.ToList(), caminhoArquivoLeitura + nomePastaArquivoCombinado, nomeArquivoCombinado + contadorArquivoLeitura, 0).ToArray();
                                break;

                            default:
                                if (!(pilhaA[0] == 1990 && pilhaB[0] == 1990))
                                {
                                    pilhaA = lerArquivoAPorPosicao2(caminhoArquivoLeitura + nomePastaArquivoCombinado, nomeArquivoCombinado, bytesToRead);
                                    pilhaB = lerArquivoBPorPosicao2(caminhoArquivoLeitura, nomeArquivoLeitura + (contadorArquivoLeitura + 1), bytesToRead);
                                    pilhaResultado = Utils.armazenar(pilhaA.ToList(), pilhaB.ToList(), caminhoArquivoLeitura + nomePastaArquivoCombinado, nomeArquivoCombinado + contadorArquivoLeitura, 0).ToArray();
                                }
                                else
                                {
                                    break;
                                }
                                break;
                        }
                    }
                    if (pilhaResultado.Length > 0)
                    {
                        //Console.WriteLine(pilhaResultado[pilhaResultado.Length - 1]);
                        // Console.WriteLine(pilhaA[pilhaA.Length - 1]);
                        if (pilhaResultado[pilhaResultado.Length - 1] == pilhaA[pilhaA.Length - 1])
                        {
                            pilhaMaior = 2; // Pilha A maior 
                            // Console.WriteLine("Pilha A maior ");
                        }
                        else
                        {
                            pilhaMaior = 1; // Pilha B maior
                            // Console.WriteLine("Pilha B maior ");
                        }
                    }
                    else
                    {
                        pilhaMaior = 4; // pilhas vazias
                        //Console.WriteLine("Pilha A e B Vazia ");
                    }

                    //int[] vetorResultadoCombinacao = concatenarVetores(vetorArquivoPrimario, vetorArquivoSecudario);

                    // mergeSortController.mergeSortRecursivo(vetorResultadoCombinacao, 0, vetorResultadoCombinacao.Length - 1);
                    // escreverArquivoPosicao(caminhoArquivoLeitura + "\\"

                }



                if (combinacoes > 1)
                {

                    while (posicaoArquivoA != tamanhoArquivoA)
                    {

                        byte[] byteToWrite = new byte[tamanhoArquivoA - posicaoArquivoA];
                        pilhaA = lerArquivoAPorPosicao3(caminhoArquivoLeitura + nomePastaArquivoCombinado, nomeArquivoCombinado + (contadorArquivoLeitura - 1), byteToWrite, posicaoArquivoA);
                        pilhaResultado = Utils.armazenar(pilhaA.ToList(), pilhaResultado.ToList(), caminhoArquivoLeitura + nomePastaArquivoCombinado, nomeArquivoCombinado + contadorArquivoLeitura, 0).ToArray();
                        Utils.armazenarListaResultado(pilhaResultado.ToList(), caminhoArquivoLeitura + nomePastaArquivoCombinado, nomeArquivoCombinado + contadorArquivoLeitura, posicaoArquivoA);
                    }
                    /* if (posicaoArquivoB == tamanhoArquivoB)
                     {
                         Utils.armazenarListaResultado(pilhaA.ToList(), caminhoArquivoLeitura + nomePastaArquivoCombinado, nomeArquivoCombinado + contadorArquivoLeitura, posicaoArquivoA);

                         //  escreverArquivoPosicao(caminhoArquivoLeitura + nomePastaArquivoCombinado, nomeArquivoCombinado + (contadorArquivoLeitura), posicaoArquivoA + 1, pilhaA);
                     }*/


                }
                else
                {
                    Utils.armazenarListaResultado(pilhaResultado.ToList(), caminhoArquivoLeitura + nomePastaArquivoCombinado, nomeArquivoCombinado + contadorArquivoLeitura, posicaoArquivoA);

                }

                contadorArquivoLeitura++;
                posicaoArquivoA = 0;
                posicaoArquivoB = 0;
                tamanhoArquivoA = 1;
                tamanhoArquivoB = 1;
                pilhaMaior = 0;
            }


            //  lerArquivosByAlturaArvore(quantidadeDeCombinacoes, KVetores, caminhoArquivoNaoCombinado, caminhoArquivoEscrita, @"\arquivos-combinados-ordenados", @"\arquivos-combinados-ordenados-H", bytesToRead);
            Utils.logList.Add(caminhoArquivoLeitura + nomePastaArquivoCombinado + nomeArquivoCombinado + contadorArquivoLeitura + " Ordenado!!");

            Utils.logList.Add(">>>>>>>>>>>>>>>>>> Ordenação Concluída!!");

        }

        public int[] lerArquivoAPorPosicao2(string caminhoArquivoLeitura, string nomeArquivo, byte[] bytesToRead)
        {
            int[] pilha;
            int numBytesToRead;
            string caminhoArquivoLeituraNome = caminhoArquivoLeitura + nomeArquivo + ".txt";
            // Console.WriteLine("Arquivo A: " + caminhoArquivoLeituraNome);
            using (FileStream fsSource = new FileStream(caminhoArquivoLeituraNome, FileMode.Open, FileAccess.Read))
            {
                tamanhoArquivoA = (int)fsSource.Length;
                if (bytesToRead.Length < (int)fsSource.Length)
                {
                    numBytesToRead = bytesToRead.Length;

                }
                else
                {
                    numBytesToRead = bytesToRead.Length - (bytesToRead.Length - (int)fsSource.Length);
                }

                bytesToRead = new byte[numBytesToRead];

                posicaoArquivoA = getPosicao(posicaoArquivoA, numBytesToRead);
                if (posicaoArquivoA <= fsSource.Length)
                {
                    fsSource.Seek(posicaoArquivoA, SeekOrigin.Begin);
                    int numBytesRead = 0;
                    bytesToRead = new byte[numBytesToRead];
                    while (numBytesToRead > 0)
                    {
                        int n = fsSource.Read(bytesToRead, numBytesRead, numBytesToRead);
                        if (n == 0) break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }

                    if (bytesToRead.Length < (int)fsSource.Length)
                    {
                        numBytesToRead = bytesToRead.Length;
                        posicaoArquivoA = posicaoArquivoA + numBytesToRead;
                    }
                    else
                    {
                        numBytesToRead = bytesToRead.Length - (bytesToRead.Length - (int)fsSource.Length);
                        posicaoArquivoA = posicaoArquivoA + numBytesToRead;
                    }




                    //   Console.WriteLine("bytes leng: " + bytesToRead.Length);
                    string result = System.Text.Encoding.UTF8.GetString(bytesToRead);
                    result = retirarSeparador(result);
                    pilha = Array.ConvertAll(result.Split('-'), b => Convert.ToInt32(b));

                }
                else
                {
                    pilha = new int[1];
                    pilha[0] = 90;
                }


            }

            return pilha;

        }


        public int[] lerArquivoAPorPosicao3(string caminhoArquivoLeitura, string nomeArquivo, byte[] bytesToRead, int posicao)
        {
            int[] pilha;
            int numBytesToRead;
            string caminhoArquivoLeituraNome = caminhoArquivoLeitura + nomeArquivo + ".txt";
            // Console.WriteLine("Arquivo A: " + caminhoArquivoLeituraNome);
            using (FileStream fsSource = new FileStream(caminhoArquivoLeituraNome, FileMode.Open, FileAccess.Read))
            {
                tamanhoArquivoA = (int)fsSource.Length;
                if (bytesToRead.Length < (int)fsSource.Length)
                {
                    numBytesToRead = bytesToRead.Length;

                }
                else
                {
                    numBytesToRead = bytesToRead.Length - (bytesToRead.Length - (int)fsSource.Length);
                }

                bytesToRead = new byte[numBytesToRead];


                if (posicaoArquivoA <= fsSource.Length)
                {
                    fsSource.Seek(posicao, SeekOrigin.Begin);
                    int numBytesRead = 0;
                    bytesToRead = new byte[numBytesToRead];
                    while (numBytesToRead > 0)
                    {
                        int n = fsSource.Read(bytesToRead, numBytesRead, numBytesToRead);
                        if (n == 0) break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }

                    if (bytesToRead.Length < (int)fsSource.Length)
                    {
                        numBytesToRead = bytesToRead.Length;
                        posicaoArquivoA = posicaoArquivoA + numBytesToRead;
                    }
                    else
                    {
                        numBytesToRead = bytesToRead.Length - (bytesToRead.Length - (int)fsSource.Length);
                        posicaoArquivoA = posicaoArquivoA + numBytesToRead;
                    }
                    //    Console.WriteLine("bytes leng: " + bytesToRead.Length);
                    string result = System.Text.Encoding.UTF8.GetString(bytesToRead);
                    result = retirarSeparador(result);
                    pilha = Array.ConvertAll(result.Split('-'), b => Convert.ToInt32(b));

                }
                else
                {
                    pilha = new int[1];
                    pilha[0] = 90;
                }


            }

            return pilha;

        }


        public static void escreverArquivoPosicao(string caminhoArquivoEscrita, string nomeArquivo, int posicaoIniciarEscrita, int[] bytesToWrite)
        {
            int numBytesToWrite = bytesToWrite.Length;
            int numBytesRead = 0;
            string caminhoArquivoNomeEscrita = caminhoArquivoEscrita + nomeArquivo + ".txt";
            string numeros = String.Join("-", bytesToWrite.Select(x => x.ToString())) + "-";
            if (!Directory.Exists(caminhoArquivoEscrita)) Directory.CreateDirectory(caminhoArquivoEscrita);
            if (!(File.Exists(caminhoArquivoNomeEscrita))) File.WriteAllText(caminhoArquivoNomeEscrita, string.Empty);
            File.AppendAllText(caminhoArquivoNomeEscrita, numeros);
            /*  using (FileStream fsSource = new FileStream(caminhoArquivoEscrita + nomeArquivo + ".txt", FileMode.Open, FileAccess.Write))
              {
                  fsSource.Seek(fsSource.Length, SeekOrigin.End);
                  fsSource.Write(bytesToWrite, numBytesRead, numBytesToWrite);
              }
              */
        }


        public int[] lerArquivoBPorPosicao2(string caminhoArquivoLeitura, string nomeArquivo, byte[] bytesToRead)
        {
            int[] pilha;
            int numBytesToRead; //(int)bytesToRead.Length;

            string caminhoArquivoLeituraNome = caminhoArquivoLeitura + nomeArquivo + ".txt";
            //Console.WriteLine("Arquivo B: " + caminhoArquivoLeituraNome);
            using (FileStream fsSource = new FileStream(caminhoArquivoLeituraNome, FileMode.Open, FileAccess.Read))
            {
                tamanhoArquivoB = (int)fsSource.Length;
                if (bytesToRead.Length < (int)fsSource.Length)
                {
                    numBytesToRead = bytesToRead.Length;

                }
                else
                {
                    numBytesToRead = bytesToRead.Length - (bytesToRead.Length - (int)fsSource.Length);
                }
                bytesToRead = new byte[numBytesToRead];
                posicaoArquivoB = getPosicao(posicaoArquivoB, numBytesToRead);
                if (posicaoArquivoB <= fsSource.Length)
                {
                    fsSource.Seek(posicaoArquivoB, SeekOrigin.Begin);
                    int numBytesRead = 0;
                    bytesToRead = new byte[numBytesToRead];
                    while (numBytesToRead > 0)
                    {
                        int n = fsSource.Read(bytesToRead, numBytesRead, numBytesToRead);
                        if (n == 0) break;

                        numBytesRead += n;
                        numBytesToRead -= n;
                    }

                    if (bytesToRead.Length < (int)fsSource.Length)
                    {
                        numBytesToRead = bytesToRead.Length;
                        posicaoArquivoB = posicaoArquivoB + numBytesToRead;
                    }
                    else
                    {
                        numBytesToRead = bytesToRead.Length - (bytesToRead.Length - (int)fsSource.Length);
                        posicaoArquivoB = posicaoArquivoB + numBytesToRead;
                    }
                    string result = System.Text.Encoding.UTF8.GetString(bytesToRead);
                    result = retirarSeparador(result);
                    pilha = Array.ConvertAll(result.Split('-'), b => Convert.ToInt32(b));

                }
                else
                {
                    pilha = new int[1];
                    pilha[0] = 90;
                }


            }

            return pilha;

        }


        public void escreverNumerosArquivo(int[] numeros, string caminhoArquivoEscritaPasta, string nomeArquivo, int contadorArquivo)
        {
            if (!Directory.Exists(caminhoArquivoEscritaPasta)) Directory.CreateDirectory(caminhoArquivoEscritaPasta);

            string caminhoArquivoOrdenado = caminhoArquivoEscritaPasta + @"\" + nomeArquivo + "-" + contadorArquivo + ".txt";
            File.WriteAllText(caminhoArquivoOrdenado, String.Join("-", numeros.Select(x => x.ToString())));
            Utils.logList.Add("Arquivo: " + caminhoArquivoOrdenado + "    Criado!!");
            Utils.logList.Add(contadorArquivo + ". Arquivo Criado Em: " + frmHomeScreen.timeElapsed);
                   
        }

        public int getPosicao2(int posicaoAtual, int numeroBytesLeitura)
        {
            int posicaoIniciarLeitura = 0;

            if (posicaoAtual == 1)
            {
                posicaoIniciarLeitura = 0;
                //   Console.WriteLine("1 - Iniciar Leitura de: " + 0 + " ate " + numeroBytesLeitura);
                //Console.WriteLine("posicaoAtual: " + 0);

            }
            else if (posicaoAtual == 2)
            {
                posicaoIniciarLeitura = posicaoAtual + 1;
                //   Console.WriteLine("1 - Iniciar Leitura de: " + 0 + " ate " + numeroBytesLeitura);
                //Console.WriteLine("posicaoAtual: " + 0);

            }
            else if (posicaoAtual == numeroBytesLeitura)
            {
                posicaoIniciarLeitura = numeroBytesLeitura + 1;
                //   Console.WriteLine(" 2 - Posicao Atual: " + posicaoAtual);
                //    Console.WriteLine("Iniciar Leitura de: " + posicaoIniciarLeitura + " ate " + (posicaoIniciarLeitura + numeroBytesLeitura));

            }
            else
            {
                posicaoIniciarLeitura = (posicaoAtual + numeroBytesLeitura + 1);
                //  Console.WriteLine(" D - Posicao Atual: " + posicaoAtual);
                //  Console.WriteLine("Iniciar Leitura de: " + posicaoIniciarLeitura + " ate " + (posicaoIniciarLeitura + numeroBytesLeitura));


            }
            /* switch (posicaoAtual)
             {

                 case 1:
                     Console.WriteLine("1 - Iniciar Leitura de: " + 0 + " ate " + numeroBytesLeitura);
                     //Console.WriteLine("posicaoAtual: " + 0);
                     posicaoIniciarLeitura = 0;
                     break;

                 case 2:
                     posicaoIniciarLeitura = numeroBytesLeitura + 1;
                     Console.WriteLine(" 2 - Posicao Atual: " + numeroBytesLeitura);
                     Console.WriteLine("Iniciar Leitura de: " + posicaoIniciarLeitura + " ate " + (posicaoIniciarLeitura + numeroBytesLeitura));
                     break;

                 default:
                     posicaoIniciarLeitura = (posicaoAtual2 + numeroBytesLeitura + 1);
                     Console.WriteLine(" D - Posicao Atual: " + numeroBytesLeitura);
                     Console.WriteLine("Iniciar Leitura de: " + posicaoIniciarLeitura + " ate " + (posicaoIniciarLeitura + numeroBytesLeitura));
                     break;
             }*/



            return posicaoIniciarLeitura;

        }


        public int getPosicao(int posicaoAtual, int numeroBytesLeitura)
        {
            int posicaoIniciarLeitura = 0;

            if (posicaoAtual == 1)
            {
                posicaoIniciarLeitura = 0;
                //   Console.WriteLine("1 - Iniciar Leitura de: " + 0 + " ate " + numeroBytesLeitura);
                //Console.WriteLine("posicaoAtual: " + 0);

            }
            else if (posicaoAtual == numeroBytesLeitura)
            {
                posicaoIniciarLeitura = numeroBytesLeitura + 1;
                //   Console.WriteLine(" 2 - Posicao Atual: " + posicaoAtual);
                //    Console.WriteLine("Iniciar Leitura de: " + posicaoIniciarLeitura + " ate " + (posicaoIniciarLeitura + numeroBytesLeitura));

            }
            else
            {
                posicaoIniciarLeitura = (posicaoAtual + numeroBytesLeitura + 1);
                //  Console.WriteLine(" D - Posicao Atual: " + posicaoAtual);
                //  Console.WriteLine("Iniciar Leitura de: " + posicaoIniciarLeitura + " ate " + (posicaoIniciarLeitura + numeroBytesLeitura));


            }
            /* switch (posicaoAtual)
             {

                 case 1:
                     Console.WriteLine("1 - Iniciar Leitura de: " + 0 + " ate " + numeroBytesLeitura);
                     //Console.WriteLine("posicaoAtual: " + 0);
                     posicaoIniciarLeitura = 0;
                     break;

                 case 2:
                     posicaoIniciarLeitura = numeroBytesLeitura + 1;
                     Console.WriteLine(" 2 - Posicao Atual: " + numeroBytesLeitura);
                     Console.WriteLine("Iniciar Leitura de: " + posicaoIniciarLeitura + " ate " + (posicaoIniciarLeitura + numeroBytesLeitura));
                     break;

                 default:
                     posicaoIniciarLeitura = (posicaoAtual2 + numeroBytesLeitura + 1);
                     Console.WriteLine(" D - Posicao Atual: " + numeroBytesLeitura);
                     Console.WriteLine("Iniciar Leitura de: " + posicaoIniciarLeitura + " ate " + (posicaoIniciarLeitura + numeroBytesLeitura));
                     break;
             }*/



            return posicaoIniciarLeitura;

        }

        public string retirarSeparador(string numerosComSeparador)
        {
            if ((numerosComSeparador.StartsWith("-") && (numerosComSeparador.EndsWith("-"))))
            {
                //  Console.WriteLine("caso separador 1");
                numerosComSeparador = numerosComSeparador.Substring(1, numerosComSeparador.Length - 2);
            }
            else if (numerosComSeparador.StartsWith("-"))
            {

                //  Console.WriteLine("caso separador 2");
                numerosComSeparador = numerosComSeparador.Substring(1, numerosComSeparador.Length - 1);
            }
            else if (numerosComSeparador.EndsWith("-"))
            {

                //  Console.WriteLine("caso separador 3");
                numerosComSeparador = numerosComSeparador.Substring(0, numerosComSeparador.Length - 1);
            }
            else if (!((numerosComSeparador.StartsWith("-") && (numerosComSeparador.EndsWith("-")))))
            {
                //  Console.WriteLine("Cas nao inicia nem termina com -");
            }
            else
            {
                Console.WriteLine("Caso desconhecido");
            }

            return numerosComSeparador;

        }



    }
}
