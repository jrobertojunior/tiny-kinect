using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace SkeletonData
{
    class Program
    {
        static void Main(string[] args)
        {
            KinectController kinect = new KinectController();

            Console.ReadKey();
        }
    }

    class KinectController
    {
        private KinectSensor kinectSensor;
        private BodyFrameReader bodyFrameReader;
        private Body[] bodies = null;

        public IReadOnlyDictionary<JointType, Joint> Joints { get; set; }

        public KinectController()
        {
            this.kinectSensor = KinectSensor.GetDefault();
            this.bodyFrameReader = kinectSensor.BodyFrameSource.OpenReader();
            this.kinectSensor.Open();

            this.bodyFrameReader.FrameArrived += Update;
        }

        private void Update(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (this.bodies == null)
                    {
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }
                    
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    dataReceived = true;
                }

                if (dataReceived)
                {
                    foreach (Body body in bodies)
                    {
                        IReadOnlyDictionary<JointType, Joint> joints = body.Joints;

                        // do something here!
                        Console.WriteLine("{0}: {1}", JointType.Head, joints[JointType.Head].Position.X);
                    }
                }
            }

            throw new NotImplementedException();
        }
    }
}
