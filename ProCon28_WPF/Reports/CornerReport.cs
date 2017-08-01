using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace ProCon28_WPF.Reports
{
    class CornerReport : ReportBase
    {
        public CornerReport(string Title, Point2f[] Corners)
        {
            this.Title = Title;
            this.Corners = Corners;
        }

        public override string Title { get; }

        public Point2f[] Corners { get; }
    }
}
