using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace ProCon28_WPF.SampleReaders
{
    class CameraSampler : WindowSamplerBase
    {
        private VideoCapture cap;
        public CameraSampler(int Device)
        {
            cap = new VideoCapture(Device);

            UpdateInterval = 50;
            ShowWindow("Camera Sampler");
        }

        public override Mat Sample()
        {
            return cap.RetrieveMat();
        }

        protected override Mat OnUpdate()
        {
            return cap.RetrieveMat();
        }
    }
}
