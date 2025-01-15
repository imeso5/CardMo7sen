using Test_Scanner.DTOs;

namespace Test_Scanner.IServices
{
    public interface IPassportReaderService
    {
        Task<ScanResult> TriggerScanAsync();
    }
}
