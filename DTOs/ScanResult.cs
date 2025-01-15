namespace Test_Scanner.DTOs
{
    public class ScanResult
    {
        public string Type { get; set; }
        public string Command { get; set; }
        public string Operand { get; set; }
        public int CardType { get; set; }
        public Param Param { get; set; }
        public string TextData { get; set; } // Combined or extracted text fields
        public string Base64Images { get; set; } // Reserve this for image data
        public Dictionary<string, string> AdditionalFields { get; set; } // Flexible structure for extra data
    }
}
