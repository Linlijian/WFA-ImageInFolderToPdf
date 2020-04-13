using Image2PdfLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA_ImageInFolderToPdf
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void generateButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(imagesFolderTextBox.Text))
            {
                MessageBox.Show("Choose folder of images!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(pdfFileTextBox.Text))
            {
                MessageBox.Show("Select PDF file!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var imagesToPdf = new Images2Pdf();
                imagesToPdf.Margin = 0;
                imagesToPdf.AddModelDirectory(imagesFolderTextBox.Text);
                imagesToPdf.FilePath = pdfFileTextBox.Text;

                imagesToPdf.GenerateFolderPdf(imagesToPdf._Model);

                MessageBox.Show("PDF file successfully generated!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
