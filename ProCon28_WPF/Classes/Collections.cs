using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using System.IO;

namespace ProCon28_WPF
{
    class ReportCollection : ObservableCollection<IReport>, IDisposable
    {
        public ReportCollection(Mat Sample)
        {
            this.Sample = Sample;
        }

        public Mat Sample { get; private set; }

        public void Dispose()
        {
            Sample = null;
            foreach(IReport rep in this)
            {
                rep.Dispose();
            }
        }

        public void Export(string DirectoryPath)
        {
            Directory.CreateDirectory(DirectoryPath);
            foreach(IReport rep in this)
            {
                Sample.SaveImage(Path.Combine(DirectoryPath, rep.Title + ".png"));
            }
        }

        public T Select<T>(int Index = 0) where T : class, IReport
        {
            int ind = 0;
            foreach(IReport rep in this)
            {
                if (rep is T)
                {
                    if (ind == Index)
                        return (T)rep;
                    else
                        ind++;
                }
            }

            return null;
        }
    }

    class RecognizerCollection : ObservableCollection<ISampleRecognizer>
    {
        public ReportCollection Recognize(Mat Sample)
        {
            ReportCollection reports = new ReportCollection(Sample);
            int count = Count;
            if (count > 0)
            {
                foreach (ISampleRecognizer rec in this)
                {
                    IReport[] reps = rec.Recognize(Sample);
                    foreach (IReport rep in reps) reports.Add(rep);
                }
            }
            return reports;
        }
    }
}
