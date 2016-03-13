namespace Analyzer.ViewModels
{
    public class ChartDataItem : ViewModelBase
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        public string Title { get; set; }
        private double _value;
        public double Value
        {
            get { return this._value; }
            set
            {
                this._value = value;
                // notify subscribers that Value property has changed
                //if (this.PropertyChanged != null)
                //    PropertyChanged(this, new PropertyChangedEventArgs("Value"));
                OnPropertyChanged("Value");
            }
        }
    }
}