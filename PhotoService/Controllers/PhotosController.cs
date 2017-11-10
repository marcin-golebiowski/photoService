using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using PhotoService.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PhotoService.Models;

namespace PhotoService.Controllers
{
    [Route("api/[controller]")]
    public class PhotosController : Controller
    {
        private readonly PhotoServiceDbContext _context;

        public PhotosController(PhotoServiceDbContext context)
        {
            _context = context;
        }
        // GET api/photos
        [HttpGet]
        public async Task<IEnumerable<Guid>> Get()
        {
            return await _context.Photos.Select(photo => photo.ID).ToListAsync();
        }

        // GET api/photos/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var photoToDownload = await _context.Photos.FirstOrDefaultAsync(photo => photo.ID == id);
            if (photoToDownload != null)
            {
                return File(photoToDownload.Content, photoToDownload.ContentType, photoToDownload.FileName);
            }
            return NotFound();
        }

        // POST api/photos
        [HttpPost()]
        public async Task<IActionResult> Post(IFormFile image)
        {
            if (ModelState.IsValid && image != null)
            {
                byte[] uploadedContent = null;
                using (var fileStream = image.OpenReadStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        fileStream.CopyTo(memoryStream);
                        uploadedContent = memoryStream.ToArray();
                    }
                }

                var photo = new Photo
                {
                    Content = uploadedContent,
                    ContentType = image.ContentType,
                    FileName = image.FileName
                };

                _context.Photos.Add(photo);
                await _context.SaveChangesAsync();

                return Ok(photo.ID);
            }

            return BadRequest();
        }
    }
}
