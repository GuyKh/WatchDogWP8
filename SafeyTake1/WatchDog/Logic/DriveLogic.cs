using WatchDOG.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;

namespace WatchDOG.Logic
{
    class DriveLogic
    {
        //dani comment
        CameraCaptureTask cameraCaptureTask;
        private int[] _continousSafeTime;
        private List<AlertEvent> _eventsList;
        private Speed _speed;
        private float _score;

        public float Score
        {
            get { return _score; }
            set { _score = value; }
        }

        public int[] ContinousSafeTime
        {
            get { return _continousSafeTime; }
            set { _continousSafeTime = value; }
        }


        internal List<AlertEvent> EventsList
        {
            get { return _eventsList; }
            set { _eventsList = value; }
        }


        public Speed Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }


        private float checkDistance()
        {
            return 0;
        }

        private float checkLane()
        {
            return 0;
        }

        private float checkEyes()
        {
            return 0;
        }

        private void createAlert() { }

        public static void StartLoop()
        {
            CameraCaptureTask cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += new EventHandler<PhotoResult>(cameraCaptureTask_Completed);

            cameraCaptureTask.Show();


        }

        static void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                ShellToast toast= new ShellToast();
                toast.Title = "I hate Guy";
                toast.Content = "I really hate him";
                toast.Show();
                //Code to display the photo on the page in an image control named myImage.
                //System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                //bmp.SetSource(e.ChosenPhoto);
                //myImage.Source = bmp;
            }
        }
        public void StopLoop() { }
    }
}
