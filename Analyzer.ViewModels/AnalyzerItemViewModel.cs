namespace Analyzer.ViewModels
{
    public class AnalyzerItemViewModel : ViewModelBase
    {
        private string _memory;

        public AnalyzerItemViewModel()
        {
            Name = string.Empty;
            ElementType = string.Empty;
            _memory = string.Empty;
        }

        public string Name { get; set; }

        public string ElementType { get; set; }

        public string Memory
        {
            get { return _memory; }
            set
            {
                _memory = value;
                OnPropertyChanged("Memory");
            }
        }
    }
}
