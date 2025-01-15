using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test_Scanner.IServices;

namespace Test_Scanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScanController : ControllerBase
    {
        private readonly IPassportReaderService _passportReaderService;

        public ScanController(IPassportReaderService passportReaderService)
        {
            _passportReaderService = passportReaderService;
        }

        [HttpPost("trigger-scan")]
        public async Task<IActionResult> TriggerScan()
        {
            var result = await _passportReaderService.TriggerScanAsync();
            if (result == null)
                return StatusCode(500, "Failed to retrieve scan results.");

            return Ok(result);
        }
    }
}
