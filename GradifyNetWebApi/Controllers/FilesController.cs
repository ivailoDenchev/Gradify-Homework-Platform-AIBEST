using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;

namespace GradifyNetWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly string _bucketName = "homeworkplatformdrive-429008.appspot.com";
        private readonly StorageClient _storageClient;
        private readonly ILogger<FilesController> _logger;

        public FilesController(StorageClient storageClient, ILogger<FilesController> logger)
        {
            _storageClient = storageClient;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ListFiles()
        {
            try
            {
                var files = await ListFilesAsync();
                return Ok(files);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing files.");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromQuery] string folder)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                var objectName = !string.IsNullOrEmpty(folder) ? $"{folder}/{file.FileName}" : file.FileName;
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

        [HttpGet("download/{*objectName}")]
        public async Task<IActionResult> DownloadFile(string objectName)
        {
            try
            {
                var memoryStream = new MemoryStream();
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

        [HttpDelete("delete/{*objectName}")]
        public async Task<IActionResult> DeleteFile(string objectName)
        {
            try
            {
                await _storageClient.DeleteObjectAsync(_bucketName, objectName);
                return Ok($"File deleted successfully: {objectName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file.");
                return StatusCode(500, "Internal server error.");
            }
        }

        private async Task<IEnumerable<string>> ListFilesAsync()
        {
            var files = new List<string>();
            await foreach (var storageObject in _storageClient.ListObjectsAsync(_bucketName))
            {
                files.Add(storageObject.Name);
            }
            return files;
        }
    }
}
