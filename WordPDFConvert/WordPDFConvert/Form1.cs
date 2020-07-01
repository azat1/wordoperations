using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordPDFConvert
{
    public partial class Form1 : Form
    {
        Microsoft.Office.Interop.Word.Application wapp;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this)==DialogResult.OK)
            {
                listBox1.Items.AddRange(openFileDialog1.FileNames);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            wapp = new Microsoft.Office.Interop.Word.Application();
            
            ConvertToPdf();
            wapp.Quit();
            
        }

        private void ConvertToPdf()
        {
            foreach (string s in listBox1.Items)
            {
                ConvertOneFile(s);
            }
        }

        private void ConvertOneFile(string s)
        {
            string pdfname=Path.ChangeExtension(s,"PDF");
            Object path = s;
            Document doc =wapp.Documents.Open(ref path);
            doc.ExportAsFixedFormat(pdfname, WdExportFormat.wdExportFormatPDF);
            doc.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderSelect();
        }

        private void FolderSelect()
        {
            if (folderBrowserDialog1.ShowDialog()==DialogResult.OK)
            {
                AddFiles(folderBrowserDialog1.SelectedPath);
            }
        }

        private void AddFiles(string selectedPath)
        {
            var files=Directory.GetFiles(selectedPath,"*.docx");
            listBox1.Items.AddRange(files);
            var dirs = Directory.GetDirectories(selectedPath);
            foreach (string dir in dirs)
            {
                AddFiles(dir);
            }
        }

        private void bPrintPage_Click(object sender, EventArgs e)
        {
            int pagenum = int.Parse(tbPageNum.Text);
            PrintPage(pagenum);
        }

        private void PrintPage(int pagenum)
        {
            wapp = new Microsoft.Office.Interop.Word.Application();
            foreach (string fname in listBox1.Items)
            {
                PrintPages(wapp, pagenum,fname);
            }
            wapp.Quit();
        }

        private void PrintPages(Microsoft.Office.Interop.Word.Application wapp, int pagenum,string filename)
        {

            object path = filename;
            object pages = 3;
            object from = pagenum.ToString();
            object to = pagenum.ToString();
            object miss = System.Reflection.Missing.Value;
            Document doc = wapp.Documents.Open(ref path);
            doc.PrintOut(Range: ref pages, From: ref from, To: ref to);
        
            doc.Close();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            var sel=listBox1.SelectedIndices;
            for (int i=sel.Count-1;i>=0;i--)         
            {
                listBox1.Items.RemoveAt(sel[i]);
            }
        }
    }
}
