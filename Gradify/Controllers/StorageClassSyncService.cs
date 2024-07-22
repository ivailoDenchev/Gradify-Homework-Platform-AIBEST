using Google.Cloud.Storage.V1;
using Gradify.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gradify.Services
{
    public class StorageClassSyncService
    {
        private readonly StorageClient _storageClient;
        private readonly AppDbContext _context;
        private readonly string _bucketName = "homeworkplatformdrive-429008.appspot.com";
        private readonly ILogger<StorageClassSyncService> _logger;

        public StorageClassSyncService(StorageClient storageClient, AppDbContext context, ILogger<StorageClassSyncService> logger)
        {
            _storageClient = storageClient;
            _context = context;
            _logger = logger;
        }

        public async Task SyncClassesAsync()
        {
            try
            {
                var classFolders = new HashSet<string>();

                await foreach (var storageObject in _storageClient.ListObjectsAsync(_bucketName))
                {
                    var folderName = storageObject.Name.Split('/')[0];
                    if (!classFolders.Contains(folderName) && storageObject.Name.EndsWith("/"))
                    {
                        classFolders.Add(folderName);
                    }
                }

                var existingClasses = _context.Classes.ToDictionary(c => c.Id);
                var newClasses = new List<Class>();

                foreach (var folderName in classFolders)
                {
                    if (!existingClasses.ContainsKey(folderName))
                    {
                        newClasses.Add(new Class { Id = folderName, Name = folderName, Description = $"N/A {folderName}" });
                    }
                }

                if (newClasses.Count > 0)
                {
                    _context.Classes.AddRange(newClasses);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error synchronizing classes with storage.");
            }
        }
    }
}
