using Newtonsoft.Json;

namespace Test_Scanner.DTOs
{
    public class Param
    {
        [JsonProperty("CARD_MAINID")]
        public string CardMainId { get; set; }

        [JsonProperty("CARD_SUBID")]
        public string CardSubId { get; set; }

        [JsonProperty("CARD_NAME")]
        public string CardName { get; set; }

        [JsonProperty("Reserve")]
        public string Reserve { get; set; }

        [JsonProperty("First Name")]
        public string FirstName { get; set; }

        [JsonProperty("Family Name")]
        public string FamilyName { get; set; }

        [JsonProperty("Home Address")]
        public string HomeAddress { get; set; }

        [JsonProperty("City")]
        public string City { get; set; }

        [JsonProperty("National ID")]
        public string NationalId { get; set; }

        [JsonProperty("Date of Birth")]
        public string DateOfBirth { get; set; }

        [JsonProperty("Number ID")]
        public string NumberId { get; set; }
    }
}
