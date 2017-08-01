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
            CreateTrackbar("Points", VerticalPoints, 50, (val) =>
            {
                VerticalPoints = val;
                HorizontalPoints = val;
            });
            CreateTrackbar("Thickness", PointThickness, 10, (val) => PointThickness = val);
            CreateTrackbar("Update", UpdateInterval, 500, (val) => UpdateInterval = val);
        }

        public override Mat Sample()
        {
            return cap.RetrieveMat();
        }

        protected override Mat OnUpdate()
        {
            Mat mat = cap.RetrieveMat();

            if (DrawGrid)
                DrawGridFrame(mat);

            ReportCollection reps = Recognizers.Recognize(mat);
            DrawPoints(mat, reps);
            reps.Dispose();

            return mat;
        }

        void DrawGridFrame(Mat Mat)
        {
            int l = GridMargin.Left, t = GridMargin.Top, r = Mat.Width - GridMargin.Right, b = Mat.Height - GridMargin.Bottom;
            Mat.Line(new Point(l, t), new Point(l, b), GridFrame.Color, GridFrame.Thickness);
            Mat.Line(new Point(l, t), new Point(r, t), GridFrame.Color, GridFrame.Thickness);
            Mat.Line(new Point(r, t), new Point(r, b), GridFrame.Color, GridFrame.Thickness);
            Mat.Line(new Point(r, b), new Point(l, b), GridFrame.Color, GridFrame.Thickness);
        }

        void DrawPoints(Mat Mat, ReportCollection Reports)
        {
            int w = Mat.Width - GridMargin.Left - GridMargin.Right;
            int h = Mat.Height - GridMargin.Top - GridMargin.Bottom;

            float winterval = (float)w / (HorizontalPoints + 1);
            float hinterval = (float)h / (VerticalPoints + 1);

            const int Range = 7;

            Reports.CornerReport corner = Reports.Select<Reports.CornerReport>();

            for (int i = 1;HorizontalPoints >= i; i++)
            {
                for(int j = 1;VerticalPoints >= j; j++)
                {
                    float xpos = GridMargin.Left + (winterval * i);
                    float ypos = GridMargin.Top + (hinterval * j);

                    Scalar Color = Scalar.White;

                    if (InRange(xpos - Range, xpos + Range, MouseLocation.X, true) &&
                        InRange(ypos - Range, ypos + Range, MouseLocation.Y, true))
                        Color = Scalar.Blue;
                    else
                    {
                        foreach (Point2f p in corner.Corners)
                        {
                            if (InRange(xpos - Range, xpos + Range, p.X, true) &&
                                InRange(ypos - Range, ypos + Range, p.Y, true))
                                Color = Scalar.Red;
                        }
                    }

                    Mat.Circle(new Point(xpos, ypos), 3, Color, PointThickness, LineTypes.Filled);
                }
            }
        }

        bool InRange(double X1, double X2, double Value, bool IncludeEqual)
        {
            if (IncludeEqual)
            {
                if (X1 == Value || X2 == Value)
                    return true;
            }

            if (X2 > Value && Value > X1)
                return true;
            if (X1 > Value && Value > X2)
                return true;

            return false;
        }

        protected override void Release()
        {
            cap.Dispose();
        }

        public bool DrawGrid { get; set; } = true;
        public Frame GridFrame { get; set; } = new Frame(Scalar.Blue, 3);
        public Thickness GridMargin { get; set; } = new Thickness(10);

        public int HorizontalPoints { get; set; } = 20;
        public int VerticalPoints { get; set; } = 20;

        public int PointThickness { get; set; } = 3;

        public override RecognizerCollection Recognizers { get; } = new RecognizerCollection();

        internal struct Thickness
        {
            public Thickness(int UniformLength) : this(UniformLength, UniformLength, UniformLength, UniformLength) { }

            public Thickness(int Left, int Top, int Right, int Bottom)
            {
                this.Left = Left;
                this.Top = Top;
                this.Right = Right;
                this.Bottom = Bottom;
            }

            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        internal struct Frame
        {
            public Frame(Scalar Color, int Thickness)
            {
                this.Color = Color;
                this.Thickness = Thickness;
            }

            public Scalar Color { get; set; }
            public int Thickness { get; set; }
        }
    }
}
