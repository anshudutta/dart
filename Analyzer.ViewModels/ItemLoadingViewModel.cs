using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analyzer.ViewModels
{
    public class ItemLoadingViewModel:WorkspaceViewModel
    {
        public string DisplayName { get; set; }

        public string PercentageComplete { get; set; }
    }
}
