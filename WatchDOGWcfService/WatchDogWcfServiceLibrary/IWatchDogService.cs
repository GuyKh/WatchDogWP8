using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace WatchDogWcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IWatchDogService
    {
        [OperationContract]
        bool IsThereFaces(Image<Bgr, Byte> image);

        [OperationContract]
        FacesWithEyes GetFaces(Image<Bgr, Byte> image);

        [OperationContract]
        FacesWithEyes GetFacesWithCustomData(Image<Bgr, Byte> image, string facesXmlModel, string eyesXmlModel, double faceRatio, double eyeRatio);
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    // You can add XSD files into the project. After building the project, you can directly use the data types defined there, with the namespace "WatchDogWcfServiceLibrary.ContractType".
    [DataContract]
    public class FacesWithEyes
    {
        [DataMember]
        public List<Rectangle> Faces { get; set; }

        [DataMember]
        public List<Rectangle> Eyes { get; set; }
    }
}
