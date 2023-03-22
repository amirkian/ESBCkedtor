using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using Newtonsoft.Json.Linq;
using ESBCkedtor.Models;
using System.IO;
using ESBCkedtor.Models.ViewModels;
using System.Collections.Generic;
using WebMarkupMin.Core;
using ZetaProducerHtmlCompressor.Internal;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.Extensions.Options;
using Corsis.Xhtml;
using System.Xml;
using Aspose.Html;
using Aspose.Html.Converters;
using Aspose.Html.Saving;

namespace ESBCkedtor.Services
{
    public class confluenceService
    {



            public async Task<string> Get(string version)
            {

            HttpClient client = new HttpClient();

            var byteArray = Encoding.ASCII.GetBytes("o.kashfi:@mId96385201988");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        //    HttpResponseMessage response = await client.GetAsync(@$"https://confluence.dotin.ir/rest/api/content/108820815?expand=body.view.value");
            HttpResponseMessage response = await client.GetAsync(@$"https://confluence.dotin.ir/rest/api/content/{version}?expand=body.view.value");
            HttpContent content = response.Content;

           var statuscode= (int)response.StatusCode;

            string result = await content.ReadAsStringAsync();

            MyDetail myDetail= JsonConvert.DeserializeObject<MyDetail>(result);


            return myDetail.body.view.value;
        }

        public async Task<ConfluenceResponse> Updateconfluence(FileContentViewModel obj)
        {
            ConfluenceResponse confluenceResponse = new ConfluenceResponse();
            string minifiedContent = MinifyCode(obj.FileContent);
            string formatedContent =  ChangeDoublleQouteWithSingleQute(minifiedContent);
             formatedContent = ChangeDoublleQouteWithSingleQute(minifiedContent);
            MakeHtml(formatedContent);
             MakeXhtml();

            ConfluenceBody confluenceBody = new ConfluenceBody { storage = new Storage { value = formatedContent, representation = "storage" } };

            var data = new Confluence
            {
                version = new ESBCkedtor.Models.Version { number = obj.Version },
                title = "testCall",
                type = "page",
                body = confluenceBody
            };
            var confluence = JsonConvert.SerializeObject(data);

            var requestContent = new StringContent(confluence, Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();

            var byteArray = Encoding.ASCII.GetBytes("o.kashfi:@mId96385201988");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            HttpResponseMessage response = await client.PutAsync("https://confluence.dotin.ir/rest/api/content/108820815?expand=body.view.value", requestContent);
            HttpContent content = response.Content;

            var statuscode = (int)response.StatusCode;

            string result = await content.ReadAsStringAsync();


            if (statuscode == 200)
            {
              

                confluenceResponse.Message = "اطلاعات با موفقیت آپدیت شد";
            }
            else 
            {
                confluenceResponse.Message = "آپدیت اطلاعات به خطا خورد";
                ErrorResult errorResult = JsonConvert.DeserializeObject<ErrorResult>(result);
                confluenceResponse.ErrorResult = errorResult;
            }
            return confluenceResponse;

        }

        public string MinifyCode(string htmlInput)
        {

            string pattern = @"<!--(.*?)-->|\s\B";
            string substitution = @"";

            RegexOptions options = RegexOptions.Multiline;

            Regex regex = new Regex(pattern, options);
            string result = regex.Replace(htmlInput, substitution);
            return result;
        }

        public string ChangeDoublleQouteWithSingleQute(string htmlInput)
        {


            string result = htmlInput.Replace("\"", "'").Replace("<br>", "<br/>").Replace("<hr>", "<hr/>").Replace("<colgroup>", "").Replace("</colgroup>", "");


             result = Regex.Replace(result, @"<img\s[^>]*>(?:\s*?</img>)?", "", RegexOptions.IgnoreCase);

            return result;

        }

        public void MakeHtml(string htmlInput)
        {
            if (File.Exists(@"wwwroot\htmlInput.html"))
            {
                File.Delete(@"wwwroot\htmlInput.html");
            }
      
            using (FileStream fs = new FileStream(@"wwwroot\htmlInput.html", FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(htmlInput);
                }
            }
        }
        public void MakeXhtml()
        {
            //// load the HTML file to be converted 
            //var document = new HTMLDocument("template.html");
            //// Create Instance of XHTML Options 
            //var options = new HTMLSaveOptions();
            //// save HTML as a XHTML 
            //Aspose.Html.Converters.Converter.ConvertHTML(document, options, "output.xhtml");

            using var document = new HTMLDocument(@"wwwroot\htmlInput.html");
            document.Save(@"wwwroot\output.xhtml", new HTMLSaveOptions() { DocumentType = HTMLSaveOptions.XHTML });
        }






    }


    }
    


