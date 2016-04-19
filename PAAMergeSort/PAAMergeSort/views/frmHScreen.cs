/**
 * **********************************************************************************************************
 * @copyright   (c) 2016 - g3ferreira      
 * @brief      PAA Merge Sort Trabalho
 *                                                                                                    
 * @details                                                                                      
 *                                                                                                    
 * @author      Genilson Ferreira <gr.ferreira@live.com>                                                                
 * @since       Abr 12, 2016                                                                     
 *                                                                                                    
 * @sa           
 **********************************************************************************************************
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Timers;
using System.Xml;
using System.Runtime.InteropServices;
using System.IO;
using System.Net.Sockets;
using PAAMergeSort.controllers;
using System.Threading;

namespace PAAMergeSort
{
    public partial class frmHomeScreen : Form
    {
        public static System.Timers.Timer _timer;
        ArquivoController arquivoController = new ArquivoController();
        Thread threadMergeSort;

        public frmHomeScreen()
        {
            InitializeComponent();
            txtArqDes.Text = @"D:\genilson-ferreira\documents\faculdade\paa\trabalho\arquivos\arquivo-desordenado.txt";
            txtArqOrd.Text = @"D:\genilson-ferreira\documents\faculdade\paa\trabalho\arquivos";
            txtTamMEM.Text = "100";
            txtArquiGB.Text = "2";
        }

        public void AppendLogMessage(List<String> logList)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<List<String>>(AppendLogMessage), new object[] { logList });
                return;
            }
            lstbLog.DataSource = logList;
            lstbLog.SelectedIndex = this.lstbLog.Items.Count - 1;
        }

        public void updateLog()
        {
            _timer = new System.Timers.Timer(2000); 
            _timer.Elapsed += new ElapsedEventHandler(_timer_Elapsed);
            _timer.Enabled = true; 
        }

        public void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            List<String> logbt = new List<string>();
            logbt = null;
            AppendLogMessage(logbt);
            AppendLogMessage(Utils.logList);
        }



        private void button1_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            //txtPathVMI.Text = fbd.SelectedPath;
            // string[] files = Directory.GetFiles(fbd.SelectedPath);
            //System.Windows.Forms.MessageBox.Show("Files found: " + files.Length.ToString(), "Message");
        }

        private void button1_Click_2(object sender, EventArgs e)
        {

            //  tagXml.generateReport(docXML, "REPORT","001"); //gera o relatorio
            // tagXml.AtualizaValorNodo(docXML,"COMPONENT", "777%");
            // tagXml.setValueQR1Report(docXML, "COMPONENT", 0, "350%"); change value for QR1 tag

            // fillDataGrid();

            //  RepositoryWatcher.Run(@"C:\config-autovmi\pasta");

        }

        private void btnPathAOI_Click(object sender, EventArgs e)
        {
            txtArqDes.Text = selectPath();
        }

        public String selectPath()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;

            DialogResult result = openFileDialog.ShowDialog();
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            return openFileDialog.FileName;
        }

        public string selectPathArquivoDesordenado()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            return fbd.SelectedPath;
        }

        private void btnPathVMI_Click(object sender, EventArgs e)
        {
            txtArqOrd.Text = selectPathArquivoDesordenado();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            startThreadMergeSort();
        }

        public void iniciarMergeSort()
        {
            try
            {

                if (validateParameters())
                {
                    criarArquivosByMB(txtArquiGB.Text.Trim(), txtTamMEM.Text.Trim(), txtArqOrd.Text);
                    //lerArquivoTodo();
                    lerEscreverArquivoByBytes(txtArqDes.Text.Trim(), txtArqOrd.Text.Trim(), txtArquiGB.Text.Trim(), txtTamMEM.Text.Trim());
                }
                else
                {
                    MessageBox.Show("Por Favor, Preencha os Campos!");
                }
            }
            catch (Exception ex)
            {
                Utils.logList.Add("ERROR: " + ex.Message);

            }

        }

        public void startThreadMergeSort()
        {
            threadMergeSort = new Thread(new ThreadStart(this.iniciarMergeSort));
            threadMergeSort.IsBackground = true;
            threadMergeSort.Start();
            Utils.logList.Add("| Thread MergeSort Started !");

        }

        public void criarArquivosByMB(string tamArquivo, string tamanhoMB, string caminhoPastaArquivo)
        {
            //  arquivoController.criarArquivos(Convert.ToInt32(tamArquivo),Convert.ToInt32(tamanhoMB), caminhoPastaArquivo);
        }
        public void lerEscreverArquivoByBytes(string caminhoArquivoLeitura, string caminhoArquivoEscrita, string tamArquivoLeitura, string tamArquivoSecundario)
        {
            arquivoController.lerEscreverArquivo(caminhoArquivoLeitura, caminhoArquivoEscrita, tamArquivoLeitura, tamArquivoSecundario);

        }


        public void lerArquivoTodo()
        {

            /* long memoria1 = GC.GetTotalMemory(true);
             Utils.logList.Add("Tamanho Memoria1: " + memoria1);
             string numerosByArquivo = ArquivoController.getNumerosByArquivo(txtArqDes.Text);
             Utils.logList.Add("Tamanho Memoria2: " + memoria1);
             Utils.logList.Add("Numeros Desordenados: " + numerosByArquivo);
             int[] numerosOrdenados = ArquivoController.ordenarNumeros(Array.ConvertAll(numerosByArquivo.Split('-'), int.Parse));
             Utils.logList.Add("Numeros Ordenados: " + String.Join("-", numerosOrdenados.Select(n => n.ToString())));
             arquivoController.escreverNumerosNoArquivo(txtArqOrd.Text + @"\arquivo-ordenado.txt", numerosOrdenados);
             Utils.logList.Add("Numeros Ordenados, Salvo em: " + txtArqOrd.Text + @"\arquivo-ordenado.txt");*/

        }

        public void disableEnableFieldsHScreen(bool enableDisable)
        {
            txtArqDes.Enabled = enableDisable;
            txtArqOrd.Enabled = enableDisable;
            btnPathAOI.Enabled = enableDisable;
            btnPathVMI.Enabled = enableDisable;

        }

        public bool validateParameters()
        {

            if (!(txtArqDes.Text.Equals(string.Empty) || txtArqDes.Text.Equals(string.Empty)))
            {
                return true;

            }
            else
            {
                return false;
            }

        }


        private void frmHomeScreen_Load(object sender, EventArgs e)
        {

            updateLog();

        }

    


    }
}
