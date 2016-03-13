using System.Linq;
using System.Collections.ObjectModel;
using Analyzer.Framework;

namespace Analyzer.ViewModels
{
    public class MyComputerTreeViewModel:ViewModelBase
    {
        private ObservableCollection<ParentViewModel> _firstGeneration;
        private readonly ReadOnlyCollection<string> _drives;        
        private DeviceIO _root;
        private ParentViewModel _rootDevice;
        private string _selectedDrive;       

        public MyComputerTreeViewModel()
        {
            var mydrives = System.IO.DriveInfo.GetDrives().Select(item => item.Name).ToList();
            _drives = new ReadOnlyCollection<string>(mydrives);
            SelectedDrive = Drives[0];
        }
        
        public MyComputerTreeViewModel(string path)
        {
            var mydrives = System.IO.DriveInfo.GetDrives().Select(item => item.Name).ToList();
            _drives = new ReadOnlyCollection<string>(mydrives);
            _root = new DeviceIO(path);
            _selectedDrive = System.IO.Path.GetPathRoot(path);            
            LoadDeviceInfo();
            FirstGeneration[0].IsSelected = true;
        }        

        private void LoadDeviceInfo()
        {
            _root.Children = _root.GetChildren(_root.Path);
            _rootDevice = new ParentViewModel(_root);
            FirstGeneration = new ObservableCollection<ParentViewModel>();
            
            FirstGeneration.CollectionChanged += ItemCollectionChanged;
            FirstGeneration.Add(new ParentViewModel(_root));
        }        

        public ObservableCollection<ParentViewModel> FirstGeneration
        {
            get { return _firstGeneration; }
            set
            {
                _firstGeneration = value;
                OnPropertyChanged("FirstGeneration");
            }
        }

        public ReadOnlyCollection<string> Drives
        {
            get { return _drives; }
        }

        public string SelectedDrive
        {
            get { return _selectedDrive; }
            set
            {
                _selectedDrive = value;
                OnPropertyChanged("SelectedDrive");
                _root = new DeviceIO(SelectedDrive);               
                LoadDeviceInfo();
            }
        }

        public string SelectedPath { get; set; }       

        protected override void ItemPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.ItemPropertyChanged(sender, e);
            if (e.PropertyName == "IsSelected")
            {
                var parent = (ParentViewModel)sender;

                if (parent.IsSelected)
                    SelectedPath = _root.Path;
                else
                {
                    foreach (ParentViewModel item in parent.Children)
                    {
                        if (item.IsSelected)
                        {
                            SelectedPath = _root.Path + "\\" + item.Name;
                        }
                    }
                }
            }
        }     

        
    }
}
