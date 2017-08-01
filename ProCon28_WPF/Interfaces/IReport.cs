using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCon28_WPF
{
    interface IReport : IDisposable
    {
        string Title { get; }
    }
}
