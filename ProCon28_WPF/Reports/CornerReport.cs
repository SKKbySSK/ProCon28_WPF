using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace ProCon28_WPF.Reports
{
    class CornerReport : IReport
    {
        public CornerReport(string Title, Mat Sample, Point2f[] Corners)
        {
            this.Title = Title;
            this.Sample = Sample;
            this.Corners = Corners;
        }

        public string Title { get; }

        public Mat Sample { get; }

        public Point2f[] Corners { get; }
    }
}
