public interface ICardDispenserService
{
    /// <summary>
    /// Opens the port for communication with the card dispenser.
    /// </summary>
    void OpenPort();

    /// <summary>
    /// Closes the communication port.
    /// </summary>
    void ClosePort();

    /// <summary>
    /// Dispenses a card by ejecting it.
    /// </summary>
    void DispenseCard();

    /// <summary>
    /// Moves the card to a specified position.
    /// </summary>
    /// <param name="position">The position to move the card to (e.g., RF or IC position).</param>
    void MoveCard(byte position);

    /// <summary>
    /// Reads magnetic card data from the specified track.
    /// </summary>
    /// <param name="trackNum">The track number to read (e.g., 1, 2, or 3).</param>
    /// <returns>The data read from the magnetic card.</returns>
    string ReadMagneticCard(int trackNum);

    /// <summary>
    /// Gets the current status of the card dispenser device.
    /// </summary>
    /// <returns>The status of the device as a string.</returns>
    string GetDeviceStatus();
}