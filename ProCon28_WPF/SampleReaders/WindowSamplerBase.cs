using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace ProCon28_WPF.SampleReaders
{
    abstract class WindowSamplerBase : ISampleReader
    {
        private bool abort = false;
        private Window window;
        private Task updater;

        public void BeginUpdate()
        {
            updater = Task.Run(() =>
            {
                while (!abort)
                {
                    if (CurrentMat != null)
                    {
                        CurrentMat.Dispose();
                        CurrentMat = null;
                    }

                    if (UpdateInterval > 0)
                    {
                        CurrentMat = OnUpdate();
                        System.Threading.Thread.Sleep(UpdateInterval);
                    }
                }

                abort = false;
                updater = null;
            });
        }

        public void StopUpdate()
        {
            abort = true;
        }

        public void ShowWindow(string Name)
        {
            if (window != null) CloseWindow();
            window = new Window(Name);
            window.OnMouseCallback += Window_OnMouseCallback;
            wname = Name;
        }

        public void CloseWindow()
        {
            window.Close();
            window.OnMouseCallback -= Window_OnMouseCallback;
            window.Dispose();
            window = null;
            wname = null;
        }

        private void Window_OnMouseCallback(MouseEvent MouseEvent, int X, int Y, MouseEvent Flags)
        {
            MouseLocation = new Point(X, Y);
        }

        protected Point MouseLocation { get; private set; }

        protected void CreateTrackbar(string Name, int Value, int Max, Action<int> Callback)
        {
            window.CreateTrackbar2(Name, Value, Max, new CvTrackbarCallback2((val, _) => Callback(val)), null);
        }

        Mat mat;
        protected Mat CurrentMat
        {
            get { return mat; }
            set
            {
                mat = value;
                window?.ShowImage(mat);
            }
        }

        string wname = null;
        public string WindowName
        {
            get { return wname; }
        }

        protected int UpdateInterval { get; set; } = 0;
        public abstract RecognizerCollection Recognizers { get; }

        protected virtual Mat OnUpdate() { throw new NotImplementedException(); }

        public abstract Mat Sample();

        public void Dispose()
        {
            StopUpdate();
            CloseWindow();
            Release();
            GC.SuppressFinalize(this);
        }

        protected virtual void Release() { }
    }
}
