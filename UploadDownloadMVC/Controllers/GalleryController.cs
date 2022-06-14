using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace UploadDownloadMVC.Controllers
{
    public class GalleryController : Controller
    {
        private readonly string attachmentDirectory = Path.Combine(Environment.CurrentDirectory, "Attachment");//Path.Combine(Directory.GetCurrentDirectory(), "Attachment");//https://www.youtube.com/watch?v=6mz84tEayFQ
        public IActionResult Index()
        {
            List<string> files = Directory.GetFiles(attachmentDirectory).Select(Path.GetFileName).ToList();
            return View(files);
        }
        [HttpPost]
        public async Task<IActionResult> Index(IFormFile myFile)
        {
            if (myFile != null)
            {
                var directory = Path.Combine(attachmentDirectory,myFile.FileName);
                using (var stream = new FileStream(directory, FileMode.Create))
                {
                    await myFile.CopyToAsync(stream);
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Download(string filePath)
        {
            var path = Path.Combine(Environment.CurrentDirectory,"Attachment", filePath);
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            var contentType = "APPLICATION/octet-stream";
            var fileName = Path.GetFileName(path);
            return File(memory,contentType,fileName);
        }
    }
}
