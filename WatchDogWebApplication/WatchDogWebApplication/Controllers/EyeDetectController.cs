using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WatchDogWebApplication.Models;
using Stream = System.IO.Stream;

namespace WatchDogWebApplication.Controllers
{
    public class EyeDetectController : ApiController
    {
        public string Get()
        {
            return "This is working";
        }

        public HttpResponseMessage AreThereFaces()
        {
            var result = new HttpResponseMessage(HttpStatusCode.NotFound);
            if (Request.Content.IsMimeMultipartContent())
            {
                Request.Content.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(new MultipartMemoryStreamProvider())
                    .ContinueWith(
                        (task) =>
                        {
                            MultipartMemoryStreamProvider provider = task.Result;
                            foreach (HttpContent content in provider.Contents)
                            {
                                Stream stream = content.ReadAsStreamAsync().Result;
                                Image image = Image.FromStream(stream);


                                bool res = EyeDetector.ContainsFaces(image);
                                result = Request.CreateResponse<bool>(HttpStatusCode.OK, res);

                            }
                        }
                    );

                return result;

            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
            }
        
        }

        public HttpResponseMessage DetectFaces()
        {
            var result = new HttpResponseMessage(HttpStatusCode.NotFound);
            if (Request.Content.IsMimeMultipartContent())
            {
                Request.Content.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(new MultipartMemoryStreamProvider())
                    .ContinueWith(
                        (task) =>
                        {
                            MultipartMemoryStreamProvider provider = task.Result;
                            foreach (HttpContent content in provider.Contents)
                            {
                                Stream stream = content.ReadAsStreamAsync().Result;
                                Image image = Image.FromStream(stream);


                                var facesAndEyes = EyeDetector.GetFaces(image);
                                result = Request.CreateResponse<FacesWithEyes>(HttpStatusCode.Created, facesAndEyes);

                            }
                        }
                    );

                return result;

            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted"));
            }


        }
    }
}
