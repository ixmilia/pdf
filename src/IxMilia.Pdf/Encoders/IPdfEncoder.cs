namespace IxMilia.Pdf.Encoders
{
    public interface IPdfEncoder
    {
        string DisplayName { get; }
        byte[] Encode(byte[] data);
    }
}
