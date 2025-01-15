using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/card-dispenser")]
public class CardDispenserController : ControllerBase
{
    private readonly ICardDispenserService _cardDispenserService;

    public CardDispenserController(ICardDispenserService cardDispenserService)
    {
        _cardDispenserService = cardDispenserService;
    }

    /// <summary>
    /// Detects and opens the port connected to the card dispenser.
    /// </summary>
    [HttpPost("detect-and-open-port")]
    public IActionResult DetectAndOpenPort()
    {
        try
        {
            _cardDispenserService.OpenPort(); // Automatically detects and opens the port
            return Ok("Card dispenser port detected and opened successfully.");
        }
        catch (PlatformNotSupportedException ex)
        {
            return StatusCode(500, $"Platform not supported: {ex.Message}");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Closes the currently open port.
    /// </summary>
    [HttpPost("close-port")]
    public IActionResult ClosePort()
    {
        try
        {
            _cardDispenserService.ClosePort();
            return Ok("Port closed successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Dispenses a card by ejecting it from the dispenser.
    /// </summary>
    [HttpPost("dispense-card")]
    public IActionResult DispenseCard()
    {
        try
        {
            _cardDispenserService.DispenseCard();
            return Ok("Card dispensed successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Moves the card to a specified position (e.g., RF or IC position).
    /// </summary>
    /// <param name="position">The position code (e.g., 0x30 for RF, 0x31 for IC).</param>
    [HttpPost("move-card")]
    public IActionResult MoveCard([FromQuery] byte position)
    {
        try
        {
            _cardDispenserService.MoveCard(position);
            return Ok($"Card moved to position {position} successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Reads magnetic card data from the specified track.
    /// </summary>
    /// <param name="trackNum">The track number to read (e.g., 1, 2, or 3).</param>
    [HttpGet("read-magnetic-card")]
    public IActionResult ReadMagneticCard([FromQuery] int trackNum)
    {
        try
        {
            string cardData = _cardDispenserService.ReadMagneticCard(trackNum);
            return Ok(new { Track = trackNum, Data = cardData });
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves the status of the card dispenser device.
    /// </summary>
    [HttpGet("status")]
    public IActionResult GetDeviceStatus()
    {
        try
        {
            string status = _cardDispenserService.GetDeviceStatus();
            return Ok(new { Status = status });
        }
        catch (Exception ex)
        {
            return BadRequest($"Error: {ex.Message}");
        }
    }
}