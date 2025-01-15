using Test_Scanner.DTOs;

namespace Test_Scanner.IServices
{
    public interface IScannerService
    {
        void Initialize();
        void Configure(A4ScannerSettings settings);
        A4ScanResult Scan();
        void Shutdown();
    }
}
