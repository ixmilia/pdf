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

PdfFile file = new PdfFile();
PdfPage page = new PdfPage(8.5 * 72, 11 * 72); // 8.5" x 11"
file.Pages.Add(page);

// line from the top left corner to the bottom right
// (the bottom left corner is the origin)
PdfPoint topLeft = new PdfPoint(0.0, page.Height);
PdfPoint bottomRight = new PdfPoint(page.Width, 0.0);
page.Items.Add(new PdfLine(topLeft, bottomRight));

// text in the bottom left corner
PdfPoint bottomLeft = new PdfPoint(0.0, 0.0);
page.Items.Add(new PdfText("some text", new PdfFont("Helvetica"), 12.0, bottomLeft));

using (FileStream fs = new FileStream(@"C:\Path\To\File.pdf", FileMode.Create))
{
    file.Save(fs);
}
```
