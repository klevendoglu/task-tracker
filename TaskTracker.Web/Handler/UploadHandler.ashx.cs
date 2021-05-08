using System.IO;
using System.Web;
using System.Web.Mvc;

namespace TaskTracker.Web.Handler
{
    /// <summary>
    /// Summary description for UploadHandler
    /// </summary>
    public class UploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Files.Count > 0)
            {
                HttpFileCollection files = context.Request.Files;
                var randomNumber = context.Request.Form["randomNumber"];
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFile file = files[i];
                    string fname = context.Server.MapPath("..\\Uploads\\" + randomNumber + "_" + file.FileName);
                    var serverPath = context.Server.MapPath("..\\Uploads\\");
                    if (!Directory.Exists(serverPath))
                    {
                        Directory.CreateDirectory(serverPath);
                    }
                    var toSave = randomNumber + "_" + file.FileName;
                    var path = Path.Combine(serverPath, toSave);
                    file.SaveAs(path);                    
                }
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write("File/s uploaded successfully!");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}