IxMilia.Pdf
===========

A portable .NET library for creating simple PDF documents based off the Adobe
specification from [here](http://wwwimages.adobe.com/content/dam/Adobe/en/devnet/pdf/pdfs/PDF32000_2008.pdf).

[![Build Status](https://dev.azure.com/ixmilia/public/_apis/build/status/Pdf?branchName=master)](https://dev.azure.com/ixmilia/public/_build/latest?definitionId=12)

## Usage

Create a PDF file:

``` C#
using System.IO;
using IxMilia.Pdf;
// ...

// create the page
PdfPage page = PdfPage.NewLetter(); // 8.5" x 11"
PdfPoint topLeft = new PdfPoint(PdfMeasurement.Zero, page.Height);
PdfPoint bottomLeft = new PdfPoint(PdfMeasurement.Zero, PdfMeasurement.Zero);
PdfPoint bottomRight = new PdfPoint(page.Width, PdfMeasurement.Zero);

// add text
page.Items.Add(new PdfText("some text", new PdfFontType1(PdfFontType1Type.Helvetica), PdfMeasurement.Points(12.0), bottomLeft));

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
