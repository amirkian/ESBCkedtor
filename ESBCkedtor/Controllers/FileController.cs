using ESBCkedtor.Models;
using ESBCkedtor.Models.ViewModels;
using ESBCkedtor.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ESBCkedtor.Controllers
{
    public class FileController : Controller
    {
        public ActionResult FileReader(FileContentViewModel obj)
        {


            return View(new FileContentViewModel() { FileContent = obj.FileContent ,FilePath=obj.FilePath});
        }

        public ActionResult GetContentFromURI( string version)
        {
            confluenceService service = new confluenceService();
            string result = service.Get(version).Result;

            return View("FileReader", new FileContentViewModel() { FileContent = result.ToString(), FilePath = "" });
        }
        [HttpPost,HttpGet]
        [ActionName(nameof(Updateconfluence))]
        public ActionResult Updateconfluence(FileContentViewModel obj)
        {
            confluenceService service = new confluenceService();
            ConfluenceResponse result;
            if (Helper.fileContentViewMode != null) {
                 result = service.Updateconfluence(Helper.fileContentViewMode).Result;

            }
            else
            {
                 result = service.Updateconfluence(obj).Result;

            }
            string message;
            if (result.ErrorResult != null)
            {
                message = result.ErrorResult.message;
                if(message.Contains("Version must be incremented on update"))
                {
                    var resultString = Regex.Match(message, @"\d+").Value;
                    int newversion = int.Parse(resultString);
                    if (newversion > 0)
                    {
                        // TempData["datacontainer"] = new FileContentViewModel { FileContent = obj.FileContent, Version = newversion + 1 };
                        Helper.fileContentViewMode = new FileContentViewModel { FileContent = obj.FileContent, Version = newversion + 1 };

                          return RedirectToAction("Updateconfluence", "File");


                    }
                }

            }
            else
            {
                Helper.fileContentViewMode = null;
               message = result.Message;
            }


            return View("FileReader", new FileContentViewModel() {Message= message });

        }

        [HttpPost]
        [ActionName(nameof(ReadFile))]

        public ActionResult ReadFile(FileContentViewModel obj)
        {

            FileStream fileStream = new FileStream(obj.FilePath, FileMode.Open);
            string fileContent;
            using (StreamReader reader = new StreamReader(fileStream))
            {
                fileContent = reader.ReadLine();
            }

            return RedirectToAction("FileReader", "File", new FileContentViewModel() { FileContent = fileContent ,FilePath=obj.FilePath});


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FileContentViewModel obj)
        {
            try
            {
                FileStream f = new FileStream(obj.FilePath, FileMode.Truncate);

                StreamWriter s = new StreamWriter(f);

                s.WriteLine(obj.FileContent);

                //closing stream writer
                s.Close();
                f.Close();


                return RedirectToAction(nameof(FileReader));
            }
            catch
            {
                return RedirectToAction(nameof(FileReader));
            }
        }


    }
}
