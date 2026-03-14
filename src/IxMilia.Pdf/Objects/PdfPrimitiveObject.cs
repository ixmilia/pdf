using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IxMilia.Pdf.Objects;

public abstract class PdfPrimitiveObject
{
}

public class PdfBoolean : PdfPrimitiveObject
{
    public bool Value { get; }
    public PdfBoolean(bool value)
    {
        Value = value;
    }

    public override string ToString() => Value.ToString().ToLowerInvariant();
}

public class PdfInteger : PdfPrimitiveObject
{
    public int Value { get; }
    public PdfInteger(int value)
    {
        Value = value;
    }

    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}

public class PdfReal : PdfPrimitiveObject
{
    public double Value { get; }
    public PdfReal(double value)
    {
        Value = value;
    }

    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}

public class PdfString : PdfPrimitiveObject
{
    public string Value { get; }
    public PdfString(string value)
    {
        Value = value;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append('(');
        foreach (var c in Value)
        {
            switch (c)
            {
                case '\\':
                    sb.Append("\\\\");
                    break;
                case '(':
                    sb.Append("\\(");
                    break;
                case ')':
                    sb.Append("\\)");
                    break;
                case '\n':
                    sb.Append("\\n");
                    break;
                case '\r':
                    sb.Append("\\r");
                    break;
                case '\t':
                    sb.Append("\\t");
                    break;
                case '\b':
                    sb.Append("\\b");
                    break;
                case '\f':
                    sb.Append("\\f");
                    break;
                default:
                    sb.Append(c);
                    break;
            }
        }

        sb.Append(')');
        return sb.ToString();
    }
}

public class PdfName : PdfPrimitiveObject
{
    public string Value { get; }
    public PdfName(string value)
    {
        Value = value;
    }

    public override string ToString() => AsName(Value);

    internal static string AsName(string value)
    {
        var sb = new StringBuilder();
        sb.Append('/');
        foreach (var c in value)
        {
            if (c == '/')
            {
                sb.Append('/');
            }
            else if (c >= '!' && c <= '~' && c != '#' && c!= '(' && c != ')')
            {
                sb.Append(c);
            }
            else
            {
                sb.Append('#');
                sb.Append(((int)c).ToString("X2", CultureInfo.InvariantCulture));
            }
        }

        return sb.ToString();
    }
}

public class PdfArray : PdfPrimitiveObject
{
    public IList<PdfPrimitiveObject> Items { get; }
    public PdfArray(params PdfPrimitiveObject[] items)
    {
        Items = items.ToList();
    }

    public override string ToString() => $"[{string.Join(" ", Items)}]";
}

public class PdfDictionary : PdfPrimitiveObject
{
    public IDictionary<string, PdfPrimitiveObject> Items { get; }
    public PdfDictionary()
    {
        Items = new Dictionary<string, PdfPrimitiveObject>();
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("<<");
        var isFirst = true;
        foreach (var item in Items)
        {
            var key = PdfName.AsName(item.Key);
            var prefix = isFirst ? " " : "   ";
            var value = item.Value.ToString()!.TrimEnd('\r', '\n');
            var lines = value.Split('\n').Select(l => l.TrimEnd('\r')).ToArray();
            sb.Append($"{prefix}{key} {lines[0]}\r\n");
            var extraIndent = new string(' ', prefix.Length + key.Length + 1);
            foreach (var extraLine in lines.Skip(1))
            {
                sb.Append($"{extraIndent}{extraLine}\r\n");
            }

            isFirst = false;
        }

        sb.Append(">>\r\n");
        return sb.ToString();
    }
}
