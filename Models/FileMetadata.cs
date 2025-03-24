using System;
using System.ComponentModel.DataAnnotations;

namespace StorageMicroservice.Models
{
    public class FileMetadata
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string FileName { get; set; }

        [Required]
        public string FileUrl { get; set; }

        [Required]
        public string ContentType { get; set; }

        public long FileSize { get; set; }

        public DateTime UploadedOn { get; set; } = DateTime.UtcNow;
    }
}
