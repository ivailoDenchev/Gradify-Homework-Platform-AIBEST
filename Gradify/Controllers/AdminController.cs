using Microsoft.AspNetCore.Mvc;
using Gradify.Services;

namespace Gradify.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly StorageClassSyncService _syncService;

        public AdminController(StorageClassSyncService syncService)
        {
            _syncService = syncService;
        }

        [HttpPost("sync-classes")]
        public async Task<IActionResult> SyncClasses()
        {
            await _syncService.SyncClassesAsync();
            return Ok("Classes synchronized successfully.");
        }
    }
}
