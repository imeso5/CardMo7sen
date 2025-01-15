using System;
using System.Runtime.InteropServices;
using Test_Scanner.IServices;
using Test_Scanner.Services;

public class K100CardDispenserService : ICardDispenserService
{
    private IntPtr _comHandle = IntPtr.Zero;
    private readonly PortDetector _portDetector;

    // DLL imports for K100 API functions
    [DllImport("K100API.dll", EntryPoint = "K100_CommOpen")]
    private static extern IntPtr K100_CommOpen(string port);

    [DllImport("K100API.dll", EntryPoint = "K100_CommClose")]
    private static extern int K100_CommClose(IntPtr comHandle);

    [DllImport("K100API.dll", EntryPoint = "K100_MoveCard")]
    private static extern int K100_MoveCard(IntPtr comHandle, bool bIsMac, byte mac, byte action, string recordInfo);

    [DllImport("K100API.dll", EntryPoint = "K100_EnterCard")]
    private static extern int K100_EnterCard(IntPtr comHandle, bool bIsMac, byte mac, byte enterType, string recordInfo);

    [DllImport("K100API.dll", EntryPoint = "K100_ReadMagcardDecode")]
    private static extern int K100_ReadMagcardDecode(IntPtr comHandle, bool bIsMac, byte mac, int trackNum, IntPtr dataBuffer);

    [DllImport("K100API.dll", EntryPoint = "K100_GetDeviceStatus")]
    private static extern int K100_GetDeviceStatus(IntPtr comHandle, bool bIsMac, byte mac, IntPtr statusBuffer);

    public K100CardDispenserService(PortDetector portDetector)
    {
        _portDetector = portDetector;
    }

    /// <summary>
    /// Detects the port and opens a connection to the card dispenser.
    /// </summary>
    public void OpenPort()
    {
        string portName = _portDetector.DetectCardDispenserPort();

        if (string.IsNullOrEmpty(portName))
        {
            throw new InvalidOperationException("No card dispenser detected on available ports.");
        }

        _comHandle = K100_CommOpen(portName);

        if (_comHandle == IntPtr.Zero)
        {
            throw new InvalidOperationException($"Failed to open port: {portName}");
        }
    }

    /// <summary>
    /// Closes the connection to the card dispenser.
    /// </summary>
    public void ClosePort()
    {
        if (_comHandle == IntPtr.Zero) return;

        int result = K100_CommClose(_comHandle);
        if (result != 0)
        {
            throw new InvalidOperationException("Failed to close the port.");
        }

        _comHandle = IntPtr.Zero;
    }

    /// <summary>
    /// Dispenses a card by ejecting it.
    /// </summary>
    public void DispenseCard()
    {
        if (_comHandle == IntPtr.Zero)
        {
            throw new InvalidOperationException("Port is not open.");
        }

        // Action `0x33` corresponds to "Eject Card" in the K100 API
        int result = K100_MoveCard(_comHandle, false, 0, 0x33, null);
        if (result != 0)
        {
            throw new InvalidOperationException("Failed to dispense the card.");
        }
    }

    /// <summary>
    /// Moves the card to a specific position (e.g., RF or IC position).
    /// </summary>
    public void MoveCard(byte position)
    {
        if (_comHandle == IntPtr.Zero)
        {
            throw new InvalidOperationException("Port is not open.");
        }

        int result = K100_MoveCard(_comHandle, false, 0, position, null);
        if (result != 0)
        {
            throw new InvalidOperationException($"Failed to move the card to position: {position}");
        }
    }

    /// <summary>
    /// Reads magnetic card data from the specified track.
    /// </summary>
    public string ReadMagneticCard(int trackNum)
    {
        if (_comHandle == IntPtr.Zero)
        {
            throw new InvalidOperationException("Port is not open.");
        }

        IntPtr dataBuffer = Marshal.AllocHGlobal(256); // Allocate buffer for card data

        try
        {
            int result = K100_ReadMagcardDecode(_comHandle, false, 0, trackNum, dataBuffer);
            if (result != 0)
            {
                throw new InvalidOperationException($"Failed to read magnetic card data from track {trackNum}.");
            }

            string data = Marshal.PtrToStringAnsi(dataBuffer);
            return data;
        }
        finally
        {
            Marshal.FreeHGlobal(dataBuffer); // Free the allocated memory
        }
    }

    /// <summary>
    /// Gets the current status of the device.
    /// </summary>
    public string GetDeviceStatus()
    {
        if (_comHandle == IntPtr.Zero)
        {
            throw new InvalidOperationException("Port is not open.");
        }

        IntPtr statusBuffer = Marshal.AllocHGlobal(256); // Allocate buffer for status data

        try
        {
            int result = K100_GetDeviceStatus(_comHandle, false, 0, statusBuffer);
            if (result != 0)
            {
                throw new InvalidOperationException("Failed to get device status.");
            }

            string status = Marshal.PtrToStringAnsi(statusBuffer);
            return status;
        }
        finally
        {
            Marshal.FreeHGlobal(statusBuffer); // Free the allocated memory
        }
    }
}