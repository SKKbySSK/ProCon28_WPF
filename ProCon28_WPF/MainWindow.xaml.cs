using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProCon28_WPF
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        RecognizerCollection Recognizers = new RecognizerCollection();

        public MainWindow()
        {
            InitializeComponent();
        }

        void Begin_Click(object sender, EventArgs e)
        {
            Recognizers.SampleReader = new SampleReaders.CameraSampler(0);
            Recognizers.Add(new SampleRecognizers.HarrisCorner());
        }

        void Recognize_Click(object sender, EventArgs e)
        {
            Recognizers.Recognize();
        }

        void Export_Click(object sender, EventArgs e)
        {
            ReportCollection reps = Recognizers.GetReports();
            reps.Export(ExportDirT.Text);
        }
    }
}
