using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageMicroservice.Data;
using StorageMicroservice.Models;
using StorageMicroservice.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace StorageMicroservice.Controllers
{
    [Route("api/storage")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private readonly FileStorageService _fileStorageService;
        private readonly StorageDbContext _context;

        public StorageController(FileStorageService fileStorageService, StorageDbContext context)
        {
            _fileStorageService = fileStorageService;
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Invalid file.");

            var fileId = Guid.NewGuid();
            var fileUrl = await _fileStorageService.UploadFileAsync(file.OpenReadStream(), fileId.ToString(), file.ContentType);

            var metadata = new FileMetadata
            {
                Id = fileId,
                FileName = file.FileName,
                FileUrl = fileUrl,
                ContentType = file.ContentType,
                FileSize = file.Length
            };

            _context.FileMetadata.Add(metadata);
            await _context.SaveChangesAsync();

            return Ok(new { fileId, fileUrl });
        }

        [HttpGet("{fileId}")]
        public async Task<IActionResult> GetFile(Guid fileId)
        {
            var metadata = await _context.FileMetadata.FindAsync(fileId);
            if (metadata == null)
                return NotFound("File not found.");

            return Ok(new { metadata.FileUrl });
        }

        [HttpDelete("{fileId}")]
        public async Task<IActionResult> DeleteFile(Guid fileId)
        {
            var metadata = await _context.FileMetadata.FindAsync(fileId);
            if (metadata == null)
                return NotFound("File not found.");

            await _fileStorageService.DeleteFileAsync(fileId.ToString());
            _context.FileMetadata.Remove(metadata);
            await _context.SaveChangesAsync();

            return Ok("File deleted successfully.");
        }
    }
}
