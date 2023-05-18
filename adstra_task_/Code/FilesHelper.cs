using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace adstra_task_.Code
{
    public class FilesHelper
    {
        private readonly IWebHostEnvironment webHost;

        public FilesHelper(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
        }
        public FilesHelper()
        {

        }
        //Upload File
        public string UploadFile(IFormFile file,string folder)
        {
            if(file != null)
            {
                var fileDir = Path.Combine(webHost.ContentRootPath, folder);
                var fileName = Guid.NewGuid() + "_" + file.FileName;
                var filePath = Path.Combine(fileDir, fileName);
                using(FileStream fileStream = new FileStream(filePath,FileMode.Create))
                {
                    file.CopyTo(fileStream);
                    return fileName;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        
    }
}
