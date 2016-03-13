using System.Collections.ObjectModel;

namespace Analyzer.ViewModels
{   

    public class StatusBar
    {       
        public StatusBar()
        {
            StatusAnimation = new ObservableCollection<ViewModelBase>();
        }

        public string StatusMessage { get; set; }
        public string StatusMessageType { get; set; }
        public string DetailedStatusMessage { get; set; }
        public ObservableCollection<ViewModelBase> StatusAnimation { get; set; }        
    }
}
