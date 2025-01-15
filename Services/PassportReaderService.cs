using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using Test_Scanner.IServices;
using System.Threading;
using System.Threading.Tasks;
using Test_Scanner.DTOs;

namespace Test_Scanner.Services
{
    public class PassportReaderService : IPassportReaderService
    {
        private readonly string _scannerWebSocketUrl = "ws://127.0.0.1:90/echo";

        public async Task<ScanResult> TriggerScanAsync()
        {
            using var client = new ClientWebSocket();
            await client.ConnectAsync(new Uri(_scannerWebSocketUrl), CancellationToken.None);

            // Trigger scan command
            var command = JsonConvert.SerializeObject(new
            {
                Type = "Notify",
                Command = "Trigger",
                Operand = "ManualRecog",
                Param = new { Timeout = 30 }
            });
            await SendMessage(client, command);

            // Wait and receive results
            var result = await ReceiveMessage(client);
            return ProcessScanResult(result);
        }

        private async Task SendMessage(ClientWebSocket client, string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task<string> ReceiveMessage(ClientWebSocket client)
        {
            var buffer = new byte[4096];
            var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            return Encoding.UTF8.GetString(buffer, 0, result.Count);
        }

        private ScanResult ProcessScanResult(string jsonResponse)
        {
            try
            {
                // Deserialize JSON into a strongly-typed object
                var response = JsonConvert.DeserializeObject<ScanResult>(jsonResponse);

                // Validate deserialization
                if (response == null || response.Param == null)
                {
                    throw new Exception("Invalid or missing data in JSON response.");
                }

                // Return meaningful result
                return new ScanResult
                {
                    Type = response.Type,
                    Command = response.Command,
                    Operand = response.Operand,
                    CardType = response.CardType,
                    TextData = response.Param.FirstName + " " + response.Param.FamilyName,
                    Base64Images = response.Param.Reserve, // Update as needed for correct image field
                    AdditionalFields = new Dictionary<string, string>
            {
                { "HomeAddress", response.Param.HomeAddress },
                { "City", response.Param.City },
                { "NationalID", response.Param.NationalId },
                { "DateOfBirth", response.Param.DateOfBirth },
                { "CardName", response.Param.CardName }
            }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing scan result: {ex.Message}");
                return null;
            }
        }
    }
}
