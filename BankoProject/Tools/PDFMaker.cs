using System;
using System.Collections.Generic;
using System.IO;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankoProject.Tools
{
  public class PDFMaker
  {
    public PDFMaker()
    {
    }


    private string _outputDirectory = null;

    public PDFMaker(string outputDirectory)
    {
      _outputDirectory = outputDirectory;
    }

    public List<int[,]> MakePDF(List<int[,]> cards, string _saveLoc)
    {
      string imgName = @"bingo1.png";
      string imgSource = Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\", @"Resources\", imgName);
      if (_outputDirectory != null)
      {
        imgSource = _outputDirectory;
      }
      string saveLoc = imgSource.Replace("png", "pdf");
      PdfDocument doc = new PdfDocument();
      saveLoc = _saveLoc + ".pdf";

      XImage img = XImage.FromFile(imgSource);
      XFont numberFont = new XFont("Verdana", 125, XFontStyle.Italic);
      XFont plateNumFont = new XFont("Verdana", 30, XFontStyle.Regular);
      XImage pdfSize = XImage.FromFile(imgSource);

      #region PixelStuff

      int startPlateY = 834;
      int startX = 356;
      int jumpX = 222;
      int jumpY = 240;
      int jumpYPlate = 796;
      int plateNumX = 45;
      int plateNumY = 3300;

      #endregion


      for (int i = 0; i < cards.Count; i++)
      {
        if (cards[i][0, 0] != -1)
        {
          PdfPage page = new PdfPage(); //Template for page

          //XSize size = PageSizeConverter.ToSize(PdfSharp.PageSize.A4);
          page.Width = pdfSize.PixelWidth;
          page.Height = pdfSize.PixelHeight;
          page.TrimMargins.Top = 0;
          page.TrimMargins.Right = 0;
          page.TrimMargins.Bottom = 0;
          page.TrimMargins.Left = -30;


          doc.Pages.Add(page);
          XGraphics xgr = XGraphics.FromPdfPage(doc.Pages[doc.PageCount - 1]);


          xgr.DrawImage(img, 0, 0, page.Width, page.Height);
          int plateChooser = startPlateY;
          int rowChooser = plateChooser;
          int columnChooser = startX;

          for (int j = 0; j < 3; j++) //Pladenummer. Styrer y-akses startpunkt
          {
            rowChooser = plateChooser;
            for (int row = 0; row < 3; row++) //rækkenummer. Placerer y-akseværdi
            {
              for (int column = 0; column < 9; column++)
              {
                if (cards[i][column, row] != 0)
                {
                  if (cards[i][column, row] < 10)
                  {
                    xgr.DrawString(" " + Convert.ToString(cards[i][column, row]), numberFont, XBrushes.Black,
                      new XRect(columnChooser, rowChooser, 10, 0), XStringFormats.TopLeft);
                  }
                  else
                    xgr.DrawString(Convert.ToString(cards[i][column, row]), numberFont, XBrushes.Black,
                      new XRect(columnChooser, rowChooser, 10, 0), XStringFormats.TopLeft);
                }

                columnChooser += jumpX; //Hopper med en kolonne per iteration          
              }
              columnChooser = startX;
              rowChooser += jumpY; //Hopper en række per iteration
            }

            plateChooser += jumpYPlate; // hopper en plade per iteration
          }

          xgr.DrawString("Pladenummer: " + Convert.ToString(i), plateNumFont, XBrushes.Black,
            new XRect(plateNumX, plateNumY, 10, 0), XStringFormats.TopLeft);
        }
      }

      doc.Save(saveLoc);
      doc.Close();
      return cards;
    }
  }
}