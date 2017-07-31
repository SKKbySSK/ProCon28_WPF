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
        private Window window;
        private Task updater;

        protected WindowSamplerBase()
        {
            updater = Task.Run(() =>
            {
                while (true)
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
            });
        }

        public void ShowWindow(string Name)
        {
            if (window != null) CloseWindow();
            window = new Window(Name);
            wname = Name;
        }

        public void CloseWindow()
        {
            window.Close();
            window.Dispose();
            window = null;
            wname = null;
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
        protected virtual Mat OnUpdate() { throw new NotImplementedException(); }

        public abstract Mat Sample();
    }
}
