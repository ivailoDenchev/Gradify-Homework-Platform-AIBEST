using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gradify.Models;

namespace Gradify.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly string _bucketName = "homeworkplatformdrive-429008.appspot.com";
        private readonly StorageClient _storageClient;
        private readonly ILogger<FilesController> _logger;
        private readonly AppDbContext _context;

        public FilesController(StorageClient storageClient, ILogger<FilesController> logger, AppDbContext context)
        {
            _storageClient = storageClient;
            _logger = logger;
            _context = context;
        }

        [HttpGet("classes")]
        public async Task<IActionResult> GetAllClasses()
        {
            try
            {
                var classes = await _context.Classes.ToListAsync();
                return Ok(classes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving classes.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{classId}")]
        public async Task<IActionResult> ListClassFiles(string classId)
        {
            var classEntity = await _context.Classes.FindAsync(classId);
            if (classEntity == null)
            {
                return NotFound("Class not found.");
            }

            try
            {
                var files = new List<string>();
                await foreach (var storageObject in _storageClient.ListObjectsAsync(_bucketName, classEntity.Id))
                {
                    files.Add(storageObject.Name);
                }
                return Ok(files);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing files.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromQuery] string classId)
        {
            var classEntity = await _context.Classes.FindAsync(classId);
            if (classEntity == null)
            {
                return NotFound("Class not found.");
            }

            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                var objectName = $"{classEntity.Id}/{file.FileName}";
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;
                    await _storageClient.UploadObjectAsync(_bucketName, objectName, null, stream);
                }

                return Ok($"File uploaded successfully: {objectName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("create-folder")]
        public async Task<IActionResult> CreateFolder([FromQuery] string folderName)
        {
            try
            {
                if (string.IsNullOrEmpty(folderName))
                {
                    return BadRequest("Folder name cannot be empty.");
                }

                var objectName = $"{folderName}/";
                using (var stream = new MemoryStream(new byte[0]))
                {
                    await _storageClient.UploadObjectAsync(_bucketName, objectName, "application/x-directory", stream);
                }

                return Ok($"Folder created successfully: {folderName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating folder.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("download/{classId}/{*fileName}")]
        public async Task<IActionResult> DownloadFile(string classId, string fileName)
        {
            var classEntity = await _context.Classes.FindAsync(classId);
            if (classEntity == null)
            {
                return NotFound("Class not found.");
            }

            try
            {
                var memoryStream = new MemoryStream();
                var objectName = $"{classEntity.Id}/{fileName}";
                await _storageClient.DownloadObjectAsync(_bucketName, objectName, memoryStream);
                memoryStream.Position = 0;
                return File(memoryStream, "application/octet-stream", Path.GetFileName(objectName));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("delete/{classId}/{*fileName}")]
        public async Task<IActionResult> DeleteFile(string classId, string fileName)
        {
            var classEntity = await _context.Classes.FindAsync(classId);
            if (classEntity == null)
            {
                return NotFound("Class not found.");
            }

            try
            {
                var objectName = $"{classEntity.Id}/{fileName}";
                await _storageClient.DeleteObjectAsync(_bucketName, objectName);
                return Ok($"File deleted successfully: {objectName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
