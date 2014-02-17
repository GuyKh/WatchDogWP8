using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WatchDogWebApplication.Models;

namespace WatchDogWebApplication.Controllers
{
    public class EyeDetectController : ApiController
    {
        public string Get()
        {
            return "This is working";
        }

        public HttpResponseMessage AreThereFaces(Image image)
        {
            bool val = EyeDetector.ContainsFaces(image);
            var response = Request.CreateResponse<bool>(HttpStatusCode.Created, val);

            string uri = Url.Link("DefaultApi", new {id = DateTime.Now.Ticks});
            response.Headers.Location = new Uri(uri);
            return response;
        }

        public HttpResponseMessage DetectFaces(Image image)
        {
            FacesWithEyes facesAndEyes = EyeDetector.GetFaces(image);
            var response = Request.CreateResponse<FacesWithEyes>(HttpStatusCode.Created, facesAndEyes);

            string uri = Url.Link("DefaultApi", new { id = DateTime.Now.Ticks });
            response.Headers.Location = new Uri(uri);
            return response;
        }
    }
}
