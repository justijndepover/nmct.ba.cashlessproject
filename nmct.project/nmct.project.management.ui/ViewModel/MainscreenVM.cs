using Newtonsoft.Json;
using nmct.project.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.management.ui.ViewModel
{
    class MainscreenVM: ObservableObject, IPage
    {
        public string Name
        {
            get { return "Mainscreen"; }
        }
    }
}
