using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace ProCon28_WPF.Reports
{
    abstract class ReportBase : IReport
    {
        public abstract string Title { get; }
        public virtual void Dispose() { }
    }
}
