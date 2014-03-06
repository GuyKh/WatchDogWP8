using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using System.IO;
using NativeFaceDetector;

namespace SafeyTake1.Logic
{
    class Face
    {
        public Rectangle rectangle;
        public int timestamp;
        public WriteableBitmap image;
        private const double STAY_PUT_THRESHOLD = 5.0;

        public Face(Rectangle r, int t, WriteableBitmap i)
        {
            update(r, t, i);
        }

        public void update(Rectangle r, int t, WriteableBitmap i)
        {
            if (rectangle == null || rectangle.distanceTo(r) > STAY_PUT_THRESHOLD)
                rectangle = r;
            timestamp = t;

        }
    }

    class FaceRecognizer
    {
        public List<Face> faces { get; private set; }
        private int timestamp;
        private const double DISTANCE_THRESHOLD = 0.5;

        public FaceRecognizer()
        {
            faces = new List<Face>();
            timestamp = 0;
        }

        public void newFrame(List<Rectangle> rectangles, WriteableBitmap image)
        {
            timestamp++;

            List<Rectangle> newRectangles = new List<Rectangle>();
            foreach (Rectangle rectangle in rectangles)
            {
                double bestDistance = 0.0;
                Face bestFace = null;
                foreach (Face face in faces)
                {
                    double distance = rectangle.distanceTo(face.rectangle);
                    if (bestFace == null || bestDistance > distance)
                    {
                        bestFace = face;
                        bestDistance = distance;
                    }
                }

                if (bestFace != null && bestDistance <= DISTANCE_THRESHOLD * rectangle.diagonal())
                    bestFace.update(rectangle, timestamp, image);
                else
                    newRectangles.Add(rectangle);
            }

            List<Face> removeList = new List<Face>();
            foreach (Face face in faces)
            {
                if (face.timestamp < timestamp - 1)
                    removeList.Add(face);
            }

            foreach (Face face in removeList)
                faces.Remove(face);

            foreach (Rectangle rectangle in newRectangles)
                faces.Add(new Face(rectangle, timestamp, image));
        }




        public Face selectFace(int x, int y)
        {
            foreach (Face face in faces)
            {
                if (face.rectangle.contains(x, y))
                    return face;
            }

            return null;
        }
    }
}
