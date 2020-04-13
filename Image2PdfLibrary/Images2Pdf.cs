using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image2PdfLibrary
{
    public class Images2Pdf
    {
        public Rectangle PageSize { get; set; }
        public float Margin { get; set; }
        public readonly IList<Image2PdfModel> _Model;
        public string FilePath;
        public class Image2PdfModel
        {
            public string _imagesPaths;
            public string _FolderPaths;
        }

        public Images2Pdf()
        {
            _Model = new List<Image2PdfModel>();
            PageSize = iTextSharp.text.PageSize.A4;
            Margin = 15f;
        }
        public void AddModelDirectory(string directory)
        {
            string sourceDirectory = directory;
            string extensions = "*.jpg|*.png|*.bmp";
            var split = extensions.Split('|');

            try
            {
                var allFiles
                  = Directory.EnumerateFiles(sourceDirectory, "*", SearchOption.AllDirectories);

                foreach (string currentFile in allFiles)
                {
                    string fileName = currentFile.Substring(sourceDirectory.Length + 1);
                    var item = fileName.Split('\\');

                    _Model.Add(new Image2PdfModel { _FolderPaths = item[0], _imagesPaths = currentFile });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void GenerateFolderPdf(IList<Image2PdfModel> model)
        {

            string _path;
            var folder = model.GroupBy(item => item._FolderPaths)
                                 .Select(group => new { _pdfPath = group.Key, _image = group.ToList() })
                                 .ToList();


            foreach (var pdfPath in folder)
            {
                var doc = new Document();
                doc.SetMargins(Margin, Margin, Margin, Margin);
                _path = FilePath + @"\" + pdfPath._pdfPath+".pdf";

                using (var stream = new FileStream(_path, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    PdfWriter.GetInstance(doc, stream);

                    doc.Open();

                    foreach (var imagePath in pdfPath._image)
                    {
                        using (var imageStream = new FileStream(imagePath._imagesPaths, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            var image = Image.GetInstance(imageStream);

                            #region Checks orientation

                            doc.SetPageSize(image.Width > image.Height
                                      ? PageSize.Rotate()
                                      : PageSize);

                            #endregion Checks orientation

                            doc.NewPage();

                            #region Configures image

                            image.ScaleToFit(new Rectangle(0, 0, doc.PageSize.Width - (doc.RightMargin + doc.LeftMargin + 1), doc.PageSize.Height - (doc.BottomMargin + doc.TopMargin + 1)));
                            image.Alignment = Image.ALIGN_CENTER;

                            #endregion Configures image

                            #region Creates elements

                            var table = new PdfPTable(1)
                            {
                                WidthPercentage = 100
                            };

                            var cell = new PdfPCell
                            {
                                VerticalAlignment = Element.ALIGN_MIDDLE,
                                MinimumHeight = doc.PageSize.Height - (doc.BottomMargin + doc.TopMargin),
                                Border = 0,
                                BorderWidth = 0,
                                Padding = 0,
                                Indent = 0
                            };

                            cell.AddElement(image);

                            table.AddCell(cell);

                            #endregion Creates elements

                            doc.Add(table);
                        }
                    }

                    doc.Close();
                }
            }
            
        }
    }
}
