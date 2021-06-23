using System.Collections.Generic;

namespace IxMilia.Pdf
{
    public class PdfImageItem : PdfStreamItem
    {
        public PdfImageObject Image { get; set; }
        public PdfStreamState State { get; set; }
        public IList<PdfMatrix> Transforms { get; } = new List<PdfMatrix>();

        public PdfImageItem(PdfImageObject image, PdfStreamState state = default(PdfStreamState))
        {
            Image = image;
            State = state;
        }

        public PdfImageItem(PdfImageObject image, params PdfMatrix[] transforms)
            : this(image)
        {
            foreach (var transform in transforms)
            {
                Transforms.Add(transform);
            }
        }

        internal override void Write(PdfStreamWriter writer)
        {
            writer.SetState(State);
            writer.WriteLine("q"); // save graphics state
            foreach (var transform in Transforms)
            {
                writer.WriteLine($"  {transform}");
            }

            writer.WriteLine($"  /{Image.ReferenceId} Do"); // paint
            writer.WriteLine("Q"); // restore graphics state
        }
    }
}
