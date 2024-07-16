using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace GradifyNetWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestPingController : ControllerBase
    {
        [HttpGet("PingGoogle")]
        public async Task<IActionResult> PingGoogle()
        {
            var ping = new Ping();
            var pingReply = await ping.SendPingAsync("google.com");

            if (pingReply.Status == IPStatus.Success)
            {
                return Ok(new
                {
                    Status = "Success",
                    RoundtripTime = pingReply.RoundtripTime,
                    Address = pingReply.Address.ToString()
                });
            }

            return StatusCode(500, new
            {
                Status = "Failed",
                PingStatus = pingReply.Status
            });
        }
    }
}
