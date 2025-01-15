using System.Runtime.InteropServices;
using Test_Scanner.DTOs;
using Test_Scanner.IServices;

namespace Test_Scanner.Services
{
    public class ScannerService : IScannerService
    {
        [DllImport("ScanDll.dll", EntryPoint = "InitializeScanner")]
        private static extern int InitializeScanner();

        [DllImport("ScanDll.dll", EntryPoint = "SetScannerSettings")]
        private static extern int SetScannerSettings(int resolution, string colorMode, string outputFormat);

        [DllImport("ScanDll.dll", EntryPoint = "StartScanning")]
        private static extern int StartScanning(string outputPath);

        [DllImport("ScanDll.dll", EntryPoint = "ShutdownScanner")]
        private static extern int ShutdownScanner();

        public void Initialize()
        {
            if (InitializeScanner() != 0)
            {
                throw new InvalidOperationException("Failed to initialize the scanner.");
            }
        }

        public void Configure(A4ScannerSettings settings)
        {
            if (SetScannerSettings(settings.Resolution, settings.ColorMode, settings.OutputFormat) != 0)
            {
                throw new InvalidOperationException("Failed to configure the scanner.");
            }
        }

        public A4ScanResult Scan()
        {
            string outputPath = "scanned_document.pdf";

            if (StartScanning(outputPath) != 0)
            {
                throw new InvalidOperationException("Failed to start scanning.");
            }

            return new A4ScanResult
            {
                FilePath = outputPath,
                ScannedData = System.IO.File.ReadAllBytes(outputPath)
            };
        }

        public void Shutdown()
        {
            if (ShutdownScanner() != 0)
            {
                throw new InvalidOperationException("Failed to shut down the scanner.");
            }
        }
    }
}
