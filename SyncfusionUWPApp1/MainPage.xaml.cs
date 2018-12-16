#region Copyright Syncfusion Inc. 2001-2018.
// Copyright Syncfusion Inc. 2001-2018. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using System;
using System.Drawing;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SyncfusionUWPApp1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreatePDFAsync();
        }

        private static async Task CreatePDFAsync()
        {
            PdfDocument document = new PdfDocument();
            //Add a page to the document.
            PdfPage page = document.Pages.Add();
            //Create PDF graphics for the page.
            PdfGraphics graphics = page.Graphics;
            //Set the standard font.
            PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

            //Draw the text.
            graphics.DrawString("This is a grid", font, PdfBrushes.Black, new PointF(10, 10));

            var pdfBitmap1 = await GetPdfBitmap("watch");
            graphics.DrawImage(pdfBitmap1, 10, 60, 120, 120);

            //Creates the PdfGrid
            PdfGrid pdfGrid = new PdfGrid();
            //Adds the columns
            pdfGrid.Columns.Add();
            pdfGrid.Columns.Add();

            pdfGrid.Columns[0].Width = 50;

            //Adding grid cell style
            PdfGridCellStyle rowStyle = new PdfGridCellStyle();
            //Creating Border
            PdfBorders border = new PdfBorders();
            border.All = PdfPens.Blue;
            //setting border to the style
            rowStyle.Borders = border;

            //Adds the row and value to the cell
            for (int i = 0; i < 10; i++)
            {
                PdfGridRow pdfGridRow = pdfGrid.Rows.Add();

                pdfGrid.Rows[i].Height = 80;

                pdfGridRow.Cells[1].Value = "asdf asdf";

                //pdfGridRow.Cells[0].Style = rowStyle;
                pdfGridRow.Cells[1].Style = rowStyle;

                //Applies the image 
                pdfGrid.Rows[i].Cells[0].Style.BackgroundImage = pdfBitmap1;
                pdfGrid.Rows[i].Cells[0].Value = "";
            }

            //Draws the Grid
            pdfGrid.Draw(page, new PointF(10, 200));

            await SaveFile(document);
        }

        private static async Task<PdfBitmap> GetPdfBitmap(string filename)
        {
            var storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///images/watch.jpg"));
            var imageStream = await storageFile.OpenAsync(FileAccessMode.Read);

            PdfBitmap image = new PdfBitmap(imageStream.AsStream());
            return image;
        }

        private static async System.Threading.Tasks.Task SaveFile(PdfDocument document)
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync("invoice.pdf", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            var stream = await sampleFile.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
            
            //Save the document.
            document.Save(stream.AsStream());
            //Close the document.
            document.Close(true);
        }
    }
}
