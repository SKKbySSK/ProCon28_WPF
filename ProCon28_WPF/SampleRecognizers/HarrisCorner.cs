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
            Mat cvt = new Mat(), bin = new Mat(), binex = new Mat();
            Cv2.CvtColor(Sample, cvt, ColorConversionCodes.BGR2GRAY);
            Cv2.Threshold(cvt, bin, Threshold, 255, ThresholdTypes.Tozero);
            Cv2.Threshold(bin, binex, 0, 255, ThresholdTypes.Binary | ThresholdTypes.Otsu);
            Point2f[] corners = Cv2.GoodFeaturesToTrack(binex, 400, QualityLevel, MinDistance, null, BlockSize, true, 10);

            cvt.Dispose();
            bin.Dispose();
            binex.Dispose();

            return new IReport[]
            {
                new Reports.CornerReport("Harris Corner", corners)
            };
        }
    }
}
