using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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
        public async Task<IActionResult> UploadFile(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "No file uploaded.";
                var allFiles = await ListFilesAsync();
                return View("Index", allFiles);
            }

            var objectName = !string.IsNullOrEmpty(folder) ? $"{folder}/{file.FileName}" : file.FileName;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;
                await _storageClient.UploadObjectAsync(_bucketName, objectName, null, stream);
            }

            ViewBag.Message = $"File uploaded successfully: {objectName}";
            var filesList = await ListFilesAsync();
            return View("Index", filesList);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFolder(string folderName)
        {
            if (string.IsNullOrEmpty(folderName))
            {
                ViewBag.Message = "Folder name cannot be empty.";
                var allFiles = await ListFilesAsync();
                return View("Index", allFiles);
            }

            var objectName = $"{folderName}/";
            using (var stream = new MemoryStream(new byte[0]))
            {
                await _storageClient.UploadObjectAsync(_bucketName, objectName, "application/x-directory", stream);
            }

            ViewBag.Message = $"Folder created successfully: {folderName}";
            var filesList = await ListFilesAsync();
            return View("Index", filesList);
        }

        [HttpGet("Files/Download/{*objectName}")]
        public async Task<IActionResult> DownloadFile(string objectName)
        {
            var memoryStream = new MemoryStream();
            await _storageClient.DownloadObjectAsync(_bucketName, objectName, memoryStream);
            memoryStream.Position = 0;
            return File(memoryStream, "application/octet-stream", Path.GetFileName(objectName));
        }

        [HttpPost("Files/Delete/{*objectName}")]
        public async Task<IActionResult> DeleteFile(string objectName)
        {
            await _storageClient.DeleteObjectAsync(_bucketName, objectName);
            ViewBag.Message = $"File deleted successfully: {objectName}";
            var filesList = await ListFilesAsync();
            return View("Index", filesList);
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
