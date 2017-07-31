using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace ProCon28_WPF.SampleRecognizers
{
    class HarrisCorner : ISampleRecognizer
    {
        public bool UseHarris { get; set; } = true;

        double Threshold { get; set; } = 0;

        double QualityLevel { get; set; } = 0.01;

        int BlockSize { get; set; } = 10;

        int MinDistance { get; set; } = 10;

        public int SamplingFrames { get; set; } = 60;

        public IReport[] Recognize(Mat Sample)
        {
            Mat cvt = new Mat(), bin = new Mat();
            Cv2.CvtColor(Sample, cvt, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(cvt, bin, Threshold, 255, ThresholdTypes.Tozero);
            Cv2.Threshold(bin, bin, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);
            Point2f[] corners = Cv2.GoodFeaturesToTrack(bin, 400, QualityLevel, MinDistance, null, BlockSize, true, 10);
            cvt.Dispose();


            Mat csample = new Mat();
            Sample.CopyTo(csample);

            foreach (Point2f p in corners)
            {
                Cv2.Circle(csample, p, 1, Scalar.Red);
                Cv2.Circle(bin, p, 1, Scalar.Red);
            }

            return new IReport[]
            {
                new Reports.CornerReport("Harris Corner[Binary]", bin, corners),
                new Reports.CornerReport("Harris Corner[Color]", csample, corners)
            };
        }
    }
}
