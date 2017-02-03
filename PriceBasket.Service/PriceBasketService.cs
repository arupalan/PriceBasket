using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace PriceBasket.Service
{
    partial class PriceBasketService : ServiceBase
    {
        public PriceBasketService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            StartUp();
        }

        private void StartUp()
        {
            throw new NotImplementedException();
        }

        protected override void OnStop()
        {
            Shutdown();
        }

        private void Shutdown()
        {
            throw new NotImplementedException();
        }
    }
}
