namespace IxMilia.Pdf.Encoders
{
    public class CustomPassThroughEncoder : IPdfEncoder
    {
        public string DisplayName { get; private set; }

        public CustomPassThroughEncoder(string displayName)
        {
            DisplayName = displayName;
        }

        public byte[] Encode(byte[] data)
        {
            return data;
        }
    }
}
