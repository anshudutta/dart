using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Collections.Specialized;

namespace Analyzer.ViewModels
{
    public class AnalyzerViewModel : WorkspaceViewModel
    {
        private ObservableCollection<EntityViewModel> _entity;
        private string _displayName;
        private ICommand _closeCommand;
        //private ViewModelBase selectedItem;

        public AnalyzerViewModel(string path)
        {
            Entity = new ObservableCollection<EntityViewModel>();
            Entity.CollectionChanged += ItemCollectionChanged;
            Entity.Add(new EntityViewModel(path));
            _displayName = string.Empty;

            if (new Models.AnalyzerModel(path).ElementType == "Directory")
                DisplayName = System.IO.Path.GetFileName(path);
            else if (new Models.AnalyzerModel(path).ElementType == "File")
                DisplayName = System.IO.Path.GetFileName(path);
            else
                DisplayName = path;
            //selectedItem = new ViewModelBase();
            CloseCommand = new MVVMRoutedCommand((o) => OnRequestClose(), (o) => OnRequestCanClose());
        }

        public ObservableCollection<EntityViewModel> Entity
        {
            get { return _entity; }
            set
            {
                _entity = value;
                OnPropertyChanged("Entity");
            }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                OnPropertyChanged("DisplayName");
            }
        }

        public ICommand CloseCommand
        {
            get { return _closeCommand; }
            set
            {
                _closeCommand = value;
                OnPropertyChanged("CloseCommand");
            }
        }

        protected override void OnRequestClose()
        {
            base.OnRequestClose();
            //EntityViewModel itemToBeRemoved = new EntityViewModel();

            IsReadyToRemove = true;

            //foreach (EntityViewModel item in Entity)
            //{
            //    if (item.IsSelected)
            //        item.IsReadyToRemove = true;
            //}

            //Entity.Remove((EntityViewModel)itemToBeRemoved);
        }


        protected override void ItemCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            base.ItemCollectionChanged(sender, e);

            if (e.NewItems == null) return;
            foreach (ViewModelBase item in e.NewItems)
            {
                item.IsSelected = true;
            }
        }


    }
}
