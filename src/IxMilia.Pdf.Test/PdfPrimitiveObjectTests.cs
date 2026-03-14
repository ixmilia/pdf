using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IxMilia.Pdf.Objects;
using Xunit;

namespace IxMilia.Pdf.Test;

public class PdfPrimitiveObjectTests
{
    [Fact]
    public void BooleanToString()
    {
        Assert.Equal("true", new PdfBoolean(true).ToString());
        Assert.Equal("false", new PdfBoolean(false).ToString());
    }

    [Fact]
    public void IntegerToString()
    {
        Assert.Equal("123", new PdfInteger(123).ToString());
        Assert.Equal("-456", new PdfInteger(-456).ToString());
    }

    [Fact]
    public void DoubleToString()
    {
        Assert.Equal("3.14", new PdfReal(3.14).ToString());
        Assert.Equal("-0.001", new PdfReal(-0.001).ToString());
    }

    [Fact]
    public void StringToString()
    {
        Assert.Equal("(Hello, World!)", new PdfString("Hello, World!").ToString());
        Assert.Equal("(Line1\\n\\r\\t\\b\\f\\(\\)\\\\Line2)", new PdfString("Line1\n\r\t\b\f()\\Line2").ToString());
    }

    [Theory]
    [InlineData("Name1", "/Name1")]
    [InlineData("A;Name_With-Various***Characters?", "/A;Name_With-Various***Characters?")]
    [InlineData("1.2", "/1.2")]
    [InlineData("$$", "/$$")]
    [InlineData("@pattern", "/@pattern")]
    [InlineData(".notdef", "/.notdef")]
    [InlineData("Lime Green", "/Lime#20Green")]
    [InlineData("pared()parentheses", "/pared#28#29parentheses")]
    [InlineData("The_Key_of_F#_Minor", "/The_Key_of_F#23_Minor")]
    public void NameToString(string stringValue, string expectedValue)
    {
        var actual = new PdfName(stringValue).ToString();
        Assert.Equal(expectedValue, actual);
    }

    [Fact]
    public void ArrayToString()
    {
        var array = new PdfArray();
        array.Items.Add(new PdfInteger(549));
        array.Items.Add(new PdfReal(3.14));
        array.Items.Add(new PdfBoolean(false));
        array.Items.Add(new PdfString("Ralph"));
        array.Items.Add(new PdfName("SomeName"));
        var actual = array.ToString();
        var expected = "[549 3.14 false (Ralph) /SomeName]";
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void DictionaryToString()
    {
        var dict = new PdfDictionary();
        dict.Items["Type"] = new PdfName("Example");
        dict.Items["Subtype"] = new PdfName("DictionaryExample");
        dict.Items["Version"] = new PdfReal(0.01);
        dict.Items["IntegerItem"] = new PdfInteger(12);
        dict.Items["StringItem"] = new PdfString("a string");
        var subdict = new PdfDictionary();
        subdict.Items["Item1"] = new PdfReal(0.4);
        subdict.Items["Item2"] = new PdfBoolean(true);
        subdict.Items["LastItem"] = new PdfString("not!");
        subdict.Items["VeryLastItem"] = new PdfString("OK");
        dict.Items["Subdictionary"] = subdict;

        var actual = dict.ToString();
        var expected = """
            << /Type /Example
               /Subtype /DictionaryExample
               /Version 0.01
               /IntegerItem 12
               /StringItem (a string)
               /Subdictionary << /Item1 0.4
                                 /Item2 true
                                 /LastItem (not!)
                                 /VeryLastItem (OK)
                              >>
            >>

            """.Replace("\r", "").Replace("\n", "\r\n");
        Assert.Equal(expected, actual);
    }
}
