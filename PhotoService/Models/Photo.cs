using System;

namespace PhotoService.Models
{
    public class Photo
    {
        public Guid ID { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public byte[] Content { get; set; }
    }
}
