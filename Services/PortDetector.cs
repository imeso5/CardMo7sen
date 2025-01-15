using System;
using System.IO.Ports;

public class PortDetector
{
    public string DetectCardDispenserPort()
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException("Port detection is only supported on Windows.");
        }

        foreach (var portName in SerialPort.GetPortNames())
        {
            Console.WriteLine($"Checking port: {portName}");
            if (IsCardDispenserConnected(portName))
            {
                return portName;
            }
        }

        return null; // No card dispenser detected
    }

    private bool IsCardDispenserConnected(string portName)
    {
        try
        {
            using (var serialPort = new SerialPort(portName, 9600)) // Use the correct baud rate
            {
                serialPort.Open();

                // Send a command to the card dispenser to check its presence
                serialPort.WriteLine("COMMAND_TO_CHECK"); // Replace with the actual command
                System.Threading.Thread.Sleep(200); // Wait for the response

                string response = serialPort.ReadExisting();
                return ValidateResponse(response);
            }
        }
        catch
        {
            return false; // Port not responding or invalid
        }
    }

    private bool ValidateResponse(string response)
    {
        // Implement logic to validate the card dispenser's response
        return response.Contains("EXPECTED_RESPONSE"); // Replace with actual expected response
    }
}
