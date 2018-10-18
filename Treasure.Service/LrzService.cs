using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Treasure.Service
{
    public partial class LrzService : ServiceBase
    {
        public LrzService()
        {
            InitializeComponent();
            base.ServiceName = "Lrz的服务";
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
