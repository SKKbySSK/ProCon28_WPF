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
        ISampleReader Sampler { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        void Begin_Click(object sender, EventArgs e)
        {
            Sampler?.Dispose();

            switch (SamplerBox.SelectedIndex)
            {
                case 0:
                    Sampler = new SampleReaders.CameraSampler(0);
                    Sampler.Recognizers.Add(new SampleRecognizers.HarrisCorner());
                    break;
            }

            if(Sampler is SampleReaders.WindowSamplerBase window)
            {
                window.BeginUpdate();
            }
        }
    }
}
