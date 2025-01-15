using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Test_Scanner.DTOs;
using Test_Scanner.IServices;

namespace Test_Scanner.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class A4ScannerController : ControllerBase
    {
        private readonly IScannerService _scannerService;

        public A4ScannerController(IScannerService scannerService)
        {
            _scannerService = scannerService;
        }

        [HttpPost("initialize")]
        public IActionResult Initialize()
        {
            try
            {
                _scannerService.Initialize();
                return Ok("Scanner initialized successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("configure")]
        public IActionResult Configure([FromBody] A4ScannerSettings settings)
        {
            try
            {
                _scannerService.Configure(settings);
                return Ok("Scanner configured successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("scan")]
        public IActionResult Scan()
        {
            try
            {
                var result = _scannerService.Scan();
                return File(result.ScannedData, "application/pdf", "scanned_document.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("shutdown")]
        public IActionResult Shutdown()
        {
            try
            {
                _scannerService.Shutdown();
                return Ok("Scanner shut down successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
