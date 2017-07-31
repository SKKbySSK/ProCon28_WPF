using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
using System.IO;

namespace ProCon28_WPF
{
    class ReportCollection : ObservableCollection<IReport>
    {
        public void Export(string DirectoryPath)
        {
            Directory.CreateDirectory(DirectoryPath);
            foreach(IReport rep in this)
            {
                rep.Sample.SaveImage(Path.Combine(DirectoryPath, rep.Title + ".png"));
            }
        }
    }

    class RecognizerCollection : ObservableCollection<ISampleRecognizer>
    {
        bool abort = false;
        Task<ReportCollection> task = null;

        public RecognizerCollection() { }
        public RecognizerCollection(ISampleReader SampleReader)
        {
            this.SampleReader = SampleReader;
        }

        public ISampleReader SampleReader { get; set; }

        public void Recognize()
        {
            if(task == null || task.IsCompleted || task.IsFaulted)
            {
                task = Task.Run(() =>
                {
                    if (SampleReader == null)
                        throw new Exception("Readerが設定されていません");

                    ReportCollection reports = new ReportCollection();
                    int count = Count;
                    if(count > 0)
                    {
                        using (Mat image = SampleReader.Sample())
                        {
                            foreach (ISampleRecognizer rec in this)
                            {
                                IReport[] reps = rec.Recognize(image);
                                foreach (IReport rep in reps) reports.Add(rep);

                                if (abort)
                                {
                                    abort = false;
                                    return reports;
                                }
                            }
                        }
                    }
                    return reports;
                });
            }
            else
            {
                throw new Exception("認識中の処理があるため、Abortメソッドを先に呼ぶ必要があります");
            }
        }

        public ReportCollection GetReports()
        {
            if (task != null)
            {
                return task.Result;
            }
            else
                throw new Exception("認識が開始されていません");
        }

        public void AbortRecognition()
        {
            abort = true;
        }
    }
}
