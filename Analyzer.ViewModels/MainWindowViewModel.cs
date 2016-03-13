using System;
using System.Threading;
using System.Windows.Threading;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Analyzer.ViewModels
{
    public class MainWindowViewModel : WorkspaceViewModel
    {
        private bool _isBackButtonEnabled;
        private string _versionString;
        private ViewModelBase _selectedItem;
        private ItemLoadingViewModel itemLoadingViewModel;


        public ICommand TreeViewExplodeCommand { get; set; }
        public ICommand GoBackCommand { get; set; }
        public ICommand AnalyzeCommand { get; set; }
        public ICommand ReportBugCommand { get; set; }
        public ICommand JoinCommand { get; set; }
        public ICommand ShowDocumentationCommand { get; set; }
        public ICommand ShowAboutCommand { get; set; }

        public ObservableCollection<WorkspaceViewModel> Workspaces;

        private ObservableCollection<MyComputerTreeViewModel> _myComputer;

        public MainWindowViewModel()
        {
            SubscribeToAggregator();
            PublishStatusMessage("StatusInformation", "Loading..", "", "InWork");

            WorkSpaces = new ObservableCollection<WorkspaceViewModel>();
            WorkSpaces.CollectionChanged += ItemCollectionChanged;
            MyComputer = new ObservableCollection<MyComputerTreeViewModel>();
            MyComputer.CollectionChanged += ItemCollectionChanged;
            MyComputer.Add(new MyComputerTreeViewModel());

            InitializeCommands();

            if (MyComputer[0].SelectedPath == null)
                IsBackButtonEnabled = false;

            itemLoadingViewModel = new ItemLoadingViewModel();
            _selectedItem = new WorkspaceViewModel();


            // get version of assembly

            PublishStatusMessage("StatusSuccess", "Success", "", "");

        }

        private void InitializeCommands()
        {
            TreeViewExplodeCommand = new MVVMRoutedCommand(
                (o) => TreeviewExplode(), (o) => TreeViewCanExplode());

            GoBackCommand = new MVVMRoutedCommand(
                (o) => GoBack(), (o) => CanGoBack());

            AnalyzeCommand = new MVVMRoutedCommand(
                (o) => BeginAnalysisOperation(), (o) => CanAnalyze());

            ReportBugCommand = new MVVMRoutedCommand(
                (o) => OpenIE("http://dart.codeplex.com/workitem/list/basic"), (o) => CanAnalyze());

            JoinCommand = new MVVMRoutedCommand(
                (o) => OpenIE("http://dart.codeplex.com/team/view"), (o) => CanAnalyze());

            ShowDocumentationCommand = new MVVMRoutedCommand(
                (o) => OpenIE("http://dart.codeplex.com/documentation"), (o) => CanAnalyze());

            ShowAboutCommand = new MVVMRoutedCommand(
                (o) => OpenIE("http://dart.codeplex.com/"), (o) => CanAnalyze());

        }

        protected override void SubscribeToAggregator()
        {
            base.SubscribeToAggregator();
            //this.aggregator.GetEvent<MessageFromAnalyzerViewModel>().Subscribe(ListenToSubscribedEvent);
            this.aggregator.Subscribe<MessageFromAnalyzerViewModel, StatusBar>(ListenToSubscribedEvent);
        }

        private void ListenToSubscribedEvent(PresentationEvent<StatusBar> e)
        {

            StatusBar = e.Payload;
        }

        public bool IsBackButtonEnabled
        {
            get { return _isBackButtonEnabled; }
            set
            {
                _isBackButtonEnabled = value;
                OnPropertyChanged("IsBackButtonEnabled");
            }
        }

        public string Version
        {
            get { return _versionString; }

            set
            {
                _versionString = value;
                OnPropertyChanged("Version");
            }
        }

        public ObservableCollection<WorkspaceViewModel> WorkSpaces
        {
            get { return Workspaces; }
            set
            {
                Workspaces = value;
                OnPropertyChanged("WorkSpaces");
            }
        }

        public ObservableCollection<MyComputerTreeViewModel> MyComputer
        {
            get { return _myComputer; }
            set
            {
                _myComputer = value;
                OnPropertyChanged("MyComputer");
            }
        }

        public ViewModelBase SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }


        private void WorkspaceRequestClose(object sender, EventArgs e)
        {
            var workspace = (WorkspaceViewModel)sender;
            WorkSpaces.Remove(workspace);
            workspace.Dispose();
        }


        private void TreeviewExplode()
        {
            try
            {
                PublishStatusMessage("StatusInformation", "Loading Directory List", "", "InWork");
                string path = MyComputer[0].SelectedPath; // When the SelectedPath is null ?
                if (path == null)
                    path = System.IO.Path.GetPathRoot(path); // I wonder in which situation this code gets executed?

                if (path != null && !(System.IO.Path.HasExtension(path)))
                {
                    ReloadMyComputer(path);
                }

                IsBackButtonEnabled = true;
                PublishStatusMessage("StatusSuccess", "Success", "", "Complete");
            }
            catch (Exception ex)
            {
                PublishStatusMessage("StatusError", "An error has occurred", ex.Message, "Complete");
            }

        }

        private bool TreeViewCanExplode()
        {
            return true;
        }

        private void GoBack()
        {
            try
            {
                // same thing as above
                string path = MyComputer[0].SelectedPath;
                path = System.IO.Path.GetDirectoryName(path);
                if (path == null)
                    path = System.IO.Path.GetPathRoot(path);

                if (path == null) return;
                IsBackButtonEnabled = true;
                ReloadMyComputer(path);
            }
            catch (Exception ex)
            {
                PublishStatusMessage("StatusError", "An error has occurred", ex.Message, "Complete");
            }


        }
        private bool CanGoBack()
        {
            return true;
        }

        private void BeginAnalysisOperation()
        {
            try
            {
                PublishStatusMessage("StatusInformation", "Running Analysis..", "", "InWork");
                itemLoadingViewModel.DisplayName = "Loading item " + MyComputer[0].SelectedPath;

                WorkSpaces.Add(itemLoadingViewModel);

                new Thread(Analyze).Start();
            }
            catch (Exception ex)
            {
                PublishStatusMessage("StatusError", "An error has occurred", ex.Message, "Complete");
            }

        }

        protected override void ItemCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                base.ItemCollectionChanged(sender, e);

                if (e.Action.ToString() == "Add")
                {
                    if (e.NewItems != null)
                    {
                        foreach (ViewModelBase item in e.NewItems)
                        {
                            SelectedItem = item;
                        }
                    }
                }
                else
                {
                    if (e.OldItems != null)
                    {
                        //
                    }
                }
            }
            catch (Exception ex)
            {

                PublishStatusMessage("StatusError", "An error has occurred", ex.Message, "Complete");
            }

        }

        private void Analyze()
        {
            try
            {
                string path = MyComputer[0].SelectedPath;
                //if (Path == null)
                //    Path =  this.MyComputer[0].SelectedDrive+this.MyComputer[0].FirstGeneration[0].Name;             

                AnalyzerViewModel anzViewModel;

                anzViewModel = path != null ? new AnalyzerViewModel(path) { IsSelected = true } 
                    : new AnalyzerViewModel(MyComputer[0].SelectedDrive) { IsSelected = true };

                Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    (SendOrPostCallback)delegate
                    {
                        WorkSpaces.Add(anzViewModel);
                        WorkSpaces.Remove(itemLoadingViewModel);
                        PublishStatusMessage("StatusSuccess", "Success", "", "Complete");
                    }, null);
                //PublishStatusMessage("StatusSuccess", "Success", "", "Complete");
            }
            catch (Exception ex)
            {
                PublishStatusMessage("StatusError", "An error has occurred", ex.Message, "Complete");
            }

        }

        private bool CanAnalyze()
        {
            return true;
        }

        private bool ReloadMyComputer(string path)
        {
            try
            {
                string selectedPath = MyComputer[0].SelectedPath;
                MyComputer = new ObservableCollection<MyComputerTreeViewModel>();
                MyComputer.CollectionChanged += ItemCollectionChanged;
                MyComputer.Add(new MyComputerTreeViewModel(path) { SelectedPath = selectedPath });
                return true;
            }
            catch (Exception ex)
            {
                PublishStatusMessage("StatusError", "An error has occurred", ex.Message, "Complete");
            }
            return false;
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName != "IsReadyToRemove") return;
            var itemToBeRemoved = new WorkspaceViewModel();
            foreach (WorkspaceViewModel item in WorkSpaces)
            {
                if (item.IsReadyToRemove)
                    itemToBeRemoved = item;
            }

            WorkSpaces.Remove(itemToBeRemoved);
        }

        private bool OpenIE(string url)
        {
            try
            {
                System.Diagnostics.Process.Start("IEXPLORE.EXE", url);
                return true;
            }
            catch (Exception ex)
            {
                
            }
            return false;
        }

    }
}
