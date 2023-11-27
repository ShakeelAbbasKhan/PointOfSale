using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PointOfSale.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;   // to access wwwroot folder
        public ProductController(IWebHostEnvironment environment)
        {

            _environment = environment;

        }

        [Authorize(Policy = "CookiePolicy")]

        [HttpGet("GetByCookie")]
        public IActionResult GetByCookie()
        {
            return Ok("you hit by cookie");
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "JwtPolicy")]
        [HttpGet("GetByJWT")]

        public IActionResult GetByJWT()
        {
            return Ok("you hit by jwt");
        }

        [Authorize]
        [HttpGet("GetByBoth")]

        public IActionResult GetByBoth()
        {
            return Ok("you hit by both jwt and cookie");
        }

        [HttpPut("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file, string productcode)
        {
           // APIResponse response = new APIResponse();
            try
            {
                string FilePath = GetFilePath(productcode);

                if(!System.IO.Directory.Exists(FilePath))
                {
                    System.IO.Directory.CreateDirectory(FilePath);
                }
                string imagePath = FilePath + "\\" + productcode + ".png";

                if(System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                using(FileStream stream = System.IO.File.Create(imagePath))
                {
                    await file.CopyToAsync(stream);
                    return Ok();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return Ok();
        }

        [HttpPut("UploadMultiImage")]
        public async Task<IActionResult> UploadMultiImage(List<IFormFile> files, string productcode)
        {
            // APIResponse response = new APIResponse();
            try
            {
                string FilePath = GetFilePath(productcode);

                if (!System.IO.Directory.Exists(FilePath))
                {
                    System.IO.Directory.CreateDirectory(FilePath);
                }

                foreach (var file in files)
                {
                    string imagePath = FilePath + "\\" + file.FileName;

                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }

                    using (FileStream stream = System.IO.File.Create(imagePath))
                    {
                        await file.CopyToAsync(stream);
                        return Ok();
                    }

                }

                
                
            }
            catch (Exception)
            {

                throw;
            }
            return Ok();
        }

        [NonAction]

        private string GetFilePath(string productcode)
        {
            return _environment.WebRootPath + "\\Upload\\product\\" + productcode;
        }
    }
}
