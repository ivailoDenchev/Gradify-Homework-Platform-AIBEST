using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;

namespace Gradify.Controllers
{
    public class FilesController : Controller
    {
        private readonly string _bucketName = "homeworkplatformdrive-429008.appspot.com";
        private readonly StorageClient _storageClient;

        public FilesController(StorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var files = await ListFilesAsync();
            return View(files);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "No file uploaded.";
                return View("Index");
            }

            var objectName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;
                await _storageClient.UploadObjectAsync(_bucketName, objectName, null, stream);
            }

            ViewBag.Message = $"File uploaded successfully: {objectName}";
            var files = await ListFilesAsync();
            return View("Index", files);
        }

        [HttpGet("Files/Download/{objectName}")]
        public async Task<IActionResult> DownloadFile(string objectName)
        {
            var memoryStream = new MemoryStream();
            await _storageClient.DownloadObjectAsync(_bucketName, objectName, memoryStream);
            memoryStream.Position = 0;
            return File(memoryStream, "application/octet-stream", objectName);
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
