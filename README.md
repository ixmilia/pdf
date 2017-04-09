IxMilia.Pdf
===========

A portable .NET library for creating simple PDF documents based off the Adobe
specification from [here](http://wwwimages.adobe.com/content/dam/Adobe/en/devnet/pdf/pdfs/PDF32000_2008.pdf).

## Usage

Create a PDF file:

``` C#
using System.IO;
using IxMilia.Pdf;
// ...

// create the page
PdfPage page = PdfPage.NewLetter(); // 8.5" x 11"
PdfPoint topLeft = new PdfPoint(0.0, page.Height);
PdfPoint bottomLeft = new PdfPoint(0.0, 0.0);
PdfPoint bottomRight = new PdfPoint(page.Width, 0.0);

// add text
page.Items.Add(new PdfText("some text", new PdfFont("Helvetica"), 12.0, bottomLeft));

// add a line and circle
PdfPathBuilder builder = new PdfPathBuilder()
{
    new PdfLine(topLeft, bottomRight),
    new PdfCircle(new PdfPoint(page.Width / 2.0, page.Height / 2.0), page.Width / 2.0)
};
page.Items.Add(builder.ToPath());

// create the file and add the page
PdfFile file = new PdfFile();
file.Pages.Add(page);

using (FileStream fs = new FileStream(@"C:\Path\To\File.pdf", FileMode.Create))
{
    file.Save(fs);
}
```
