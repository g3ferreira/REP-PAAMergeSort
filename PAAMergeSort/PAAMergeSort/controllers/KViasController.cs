using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PAAMergeSort.controllers
{
    class KViasController
    {
        public ArquivoController arquivoc = new ArquivoController();
        public void lerEscreverArquivo(string caminhoArquivoLeitura, string caminhoArquivoEscrita, string tamArquivoPrincipal, string tamArquivoSecundario, string KVetores)
        {

            int KVEtores = Convert.ToInt32(KVetores);
            int quantidadeBytes = Convert.ToInt32(tamArquivoSecundario) * (1024 ^ 2);
            byte[] bytesToRead = new byte[quantidadeBytes];

            Utils.logList.Add("Tamanho Arquivo Principal Desordenado: " + tamArquivoPrincipal + "MB");
            Utils.logList.Add("Tamanho Arquivo Secundario Desordenado: " + tamArquivoSecundario + "KB");

            double quantidadeArquivosDouble = (double)(Convert.ToInt32(tamArquivoPrincipal) * 1024) / (double)Convert.ToInt32(tamArquivoSecundario);
            int quantidadeArquivosSecudarios = (int)quantidadeArquivosDouble;

            if (!(quantidadeArquivosSecudarios == quantidadeArquivosDouble))
            {
                quantidadeArquivosSecudarios++;
            }

            Utils.logList.Add("Serão Criados " + quantidadeArquivosSecudarios + " Arquivos Secundários de: " + tamArquivoSecundario + "KB");

            try
            {
                arquivoc.separarArquivoSecundarios(caminhoArquivoLeitura, caminhoArquivoEscrita, bytesToRead, quantidadeArquivosSecudarios);
                arquivoc.combinarArquivosSecundariosByKVetores(quantidadeArquivosSecudarios, KVEtores, caminhoArquivoEscrita + @"\arquivos-secundarios-ordenados", caminhoArquivoEscrita + @"\arquivos-combinados-ordenados", bytesToRead);
                //string arquivoNaoCombinado = @"D:\genilson-ferreira\documents\faculdade\paa\trabalho\arquivos\arquivos-combinados-ordenados\arquivo-secundario-ordernado-21.txt";
                // string caminhoArquivoLeituraB = @"D:\genilson-ferreira\documents\faculdade\paa\trabalho\arquivos\arquivos-combinados-ordenados";
                //int quantidadeCombinacoes = 11;
                //int KVetoresB = 2;
                //int bytesToReadB = 102600;

                //  lerArquivosByAlturaArvore(quantidadeCombinacoes, KVetoresB, arquivoNaoCombinado, caminhoArquivoLeituraB, @"\arquivos-combinados-ordenados", @"\arquivos-combinados-ordenados-H", bytesToRead);

            }
            catch (FileNotFoundException ioEx)
            {
                Console.WriteLine(ioEx.Message);
            }

        }




    }
}
