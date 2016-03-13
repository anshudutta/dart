using System;
using System.Collections.Specialized;
using Analyzer.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Data;
using System.Windows.Input;

namespace Analyzer.ViewModels
{
    public class EntityViewModel : ViewModelBase
    {
        private AnalyzerModel model;
        private ObservableCollection<AnalyzerItemViewModel> _items;
        private ObservableCollection<ItemDetails> _notes;
        private string _rootElementPath;
        private string _elementName;
        private string _elementType;
        private string _totalMemory;
        private string _usedMemory;
        private string _freeMemory;
        private string path;
        private List<ModelFileMemoryList> _memoryList;
        private Dictionary<string, double> _memoryPercentage;
        private List<string> _elementDetailsAttribute;
        private List<string> _elementDetailsValue;
        private List<string> _inaccssibleList;
        private ObservableCollection<AccessControlList> _accessListDetails;
        private ObservableCollection<ChartDataItem> _chartItems;

        // private ICommand closeCommand;

        public EntityViewModel()
        {
        }
        public EntityViewModel(string Path)
        {
            if (Path == null) throw new ArgumentNullException("Path");
            Initialize();
            model = new AnalyzerModel(Path);
            ElementName = System.IO.Path.GetFileName(Path);
            RootElementPath = Path;
            FreeMemory = model.FreeMemory;
            UsedMemory = model.UsedMemory;
            TotalMemory = model.TotalMemory;
            path = Path;

            foreach (ModelFileMemoryList item in model.MemoryList)
            {
                Items.Add(new AnalyzerItemViewModel
                {
                    Name = System.IO.Path.GetFileName(item.FileName),
                    Memory = item.ItemMemory,
                    ElementType = item.MyType
                });
            }

            // add inaccssible list
            foreach (string item in model.InaccssibleList)
            {
                Items.Add(new AnalyzerItemViewModel
                {
                    Name = System.IO.Path.GetFileName(item),
                    Memory = "Hidden",
                    ElementType = "Hidden"
                });
            }

            ElementType = model.ElementType;
            if (model.History != null)
            {
                foreach (KeyValuePair<string, string> item in model.History)
                {
                    ElementDetailsAttribute.Add(item.Key);
                    ElementDetailsValue.Add(item.Value);
                }
            }

            InaccessibleList = model.InaccssibleList;

            // initialize graph
            InitializeGraph();
            InitializeNotes();

            AccessList = model.AccessList;
            //LoadAccessList();
        }

        private void Initialize()
        {
            _items = new ObservableCollection<AnalyzerItemViewModel>();
            _chartItems = new ObservableCollection<ChartDataItem>();
            ElementName = string.Empty;
            RootElementPath = string.Empty;
            _elementDetailsAttribute = new List<string>();
            _elementDetailsValue = new List<string>();
            _inaccssibleList = new List<string>();
            _items.CollectionChanged += ItemCollectionChanged;
            _chartItems.CollectionChanged += ItemCollectionChanged;

            SaveSnapshotCommand = new MVVMRoutedCommand(
                (o) => SaveSnapshot(), (o) => CanSaveSnapshot());

            ExportToExcelCommand = new MVVMRoutedCommand(
                (o) => ExportToExcel(), (o) => CanExportToExcel());

            CreateEmailCommand = new MVVMRoutedCommand(
                (o) => SendEmail(), (o) => CanSendEmail());
        }

        /// <summary>
        /// Unused method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void chartItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void InitializeGraph()
        {
            //model = new AnalyzerModel(Path);
            _memoryList = new List<ModelFileMemoryList>();
            _memoryPercentage = new Dictionary<string, double>();
            _memoryList = model.MemoryList;

            foreach (ModelFileMemoryList item in model.MemoryList)
            {
                string unit = item.ItemMemory.Split(' ')[1];
                double memory = Convert.ToDouble(item.ItemMemory.Split(' ')[0]);

                switch (unit)
                {
                    case "KB":
                        memory = memory / (1024 * 1024);
                        break;
                    case "MB":
                        memory = memory / 1024;
                        break;
                }

                unit = model.TotalMemory.Split(' ')[1];
                double totalMemory = Convert.ToDouble(model.TotalMemory.Split(' ')[0]);
                switch (unit)
                {
                    case "KB":
                        totalMemory = memory / (1024 * 1024);
                        break;
                    case "MB":
                        totalMemory = memory / 1024;
                        break;
                }


                double percentage = Math.Round((memory / totalMemory) * 100, 2);
                string fileName = System.IO.Path.GetFileName(item.FileName);
                if (fileName != null)
                {
                    MemoryPercentage.Add(fileName, percentage);
                }
                ChartItems.Add(new ChartDataItem
                                   {
                                       Title = System.IO.Path.GetFileName(item.FileName),
                                       Value = percentage
                                   });
            }
        }

        /// <summary>
        /// Unused method
        /// </summary>
        private void LoadAccessList()
        {
            AccessListDetails = new ObservableCollection<AccessControlList>();

            for (int i = 0; i < AccessList.Rows.Count; i++)
            {
                AccessListDetails.Add(new AccessControlList()
                {
                    Identity = AccessList.Rows[i]["Identity"].ToString(),
                    Control = AccessList.Rows[i]["Control"].ToString(),
                    Rights = AccessList.Rows[i]["Rights"].ToString()
                });
            }
        }

        private void InitializeNotes()
        {
            _notes = new ObservableCollection<ItemDetails>();

            var data = new StringBuilder();
            
            for (int i = 0; i < ElementDetailsAttribute.Count; i++)
            {
                data.AppendLine(ElementDetailsAttribute[i] + " : " + ElementDetailsValue[i]);
                //Notes.Add(new ItemDetails
                //              {
                //                  Details = ElementDetailsAttribute[i] + " : " + ElementDetailsValue[i]
                //              });
            }

            data.AppendLine("Element Type: " + ElementType);
            data.AppendLine("Total Memory :" + TotalMemory);
            //Notes.Add(new ItemDetails
            //              {
            //                  Details = "Element Type: " + ElementType
            //              });

            //Notes.Add(new ItemDetails
            //              {
            //                  Details = "Total Memory :" + TotalMemory
            //              });
            //data.AppendLine("Total Memory :" + this.TotalMemory);
            if (model.ElementType == "Drive")
            {

                data.AppendLine("Used Memory :" + UsedMemory);
                data.AppendLine("Free Memory :" + this.FreeMemory);

                //Notes.Add(new ItemDetails
                //              {
                //                  Details = "Used Memory :" + UsedMemory
                //              });
                //this.Notes.Add(new ItemDetails()
                //{
                //    Details = "Free Memory :" + this.FreeMemory
                //});

                //data.AppendLine("Used Memory :" + this.UsedMemory);
                //data.AppendLine("Free Memory :" + this.FreeMemory);
            }

            Notes.Add(new ItemDetails()
            {
                Details = data.ToString()
            });

        }

        public ICommand SaveSnapshotCommand { get; set; }
        public ICommand ExportToExcelCommand { get; set; }
        public ICommand CreateEmailCommand { get; set; }


        public ObservableCollection<AnalyzerItemViewModel> Items
        {
            get { return _items; }

            set
            {
                _items = value;
                OnPropertyChanged("Items");
            }
        }
        public string RootElementPath
        {
            get { return _rootElementPath.Replace("\\", @"\"); }
            set
            {
                _rootElementPath = value;
                OnPropertyChanged("RootElementPath");
            }
        }

        public string ElementName
        {
            get { return _elementName; }
            set
            {
                _elementName = value;
                OnPropertyChanged("ElementName");
            }
        }

        public string ElementType
        {
            get { return _elementType; }

            set
            {
                _elementType = value;
                OnPropertyChanged("ElementType");
            }
        }

        public List<string> ElementDetailsAttribute
        {
            get { return _elementDetailsAttribute; }
            set
            {
                _elementDetailsAttribute = value;
                OnPropertyChanged("ElementDetailsAttribute");
            }
        }

        public List<string> ElementDetailsValue
        {
            get { return _elementDetailsValue; }
            set
            {
                _elementDetailsValue = value;
                OnPropertyChanged("ElementDetailsAttribute");
            }
        }

        public List<string> InaccessibleList
        {
            get { return _inaccssibleList; }
            set
            {
                _inaccssibleList = value;
                OnPropertyChanged("InaccessibleList");
            }
        }

        public string TotalMemory
        {
            get { return _totalMemory; }
            set
            {
                _totalMemory = value;
                OnPropertyChanged("TotalMemory");
            }
        }

        public string UsedMemory
        {
            get { return _usedMemory; }
            set
            {
                _usedMemory = value;
                OnPropertyChanged("UsedMemory");
            }
        }

        public string FreeMemory
        {
            get { return _freeMemory; }
            set
            {
                _freeMemory = value;
                OnPropertyChanged("FreeMemory");
            }
        }

        public List<ModelFileMemoryList> MemoryList
        {
            get { return _memoryList; }

            set
            {
                _memoryList = value;
                OnPropertyChanged("MemoryList");
            }
        }

        public Dictionary<string, double> MemoryPercentage
        {
            get { return _memoryPercentage; }

            set
            {
                _memoryPercentage = value;
                OnPropertyChanged("EntityMemoryList");
            }
        }


        public ObservableCollection<ChartDataItem> ChartItems
        {
            get { return _chartItems; }
            set
            {
                _chartItems = value;
                OnPropertyChanged("ChartItems");
            }
        }

        public ObservableCollection<ItemDetails> Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                OnPropertyChanged("Notes");
            }
        }

        public DataTable AccessList { get; set; }

        public ObservableCollection<AccessControlList> AccessListDetails
        {
            get { return _accessListDetails; }
            set
            {
                _accessListDetails = value;
                OnPropertyChanged("AccessListDetails");
            }
        }

        private void SaveSnapshot()
        {
            StatusBar = CreateStatusMessage("StatusInformation", "Saving Snapshot...", "", "InWork");

            aggregator.SendMessage<MessageFromAnalyzerViewModel, StatusBar>
                (
                    new MessageFromAnalyzerViewModel { Payload = StatusBar }
                );

            var dialog = new Microsoft.Win32.SaveFileDialog();

            string fileName;

            if (model.ElementType == "Drive")
            {
                fileName = System.IO.Path.GetPathRoot(path);
                if (fileName != null) fileName = fileName.Replace("\\", "").Replace(":", "");
            }
            else if (model.ElementType == "Directory")
                fileName = System.IO.Path.GetFileName(path);
            else
                fileName = System.IO.Path.GetFileName(path);

            dialog.FileName = fileName + "_" + DateTime.Now.ToString("dd MMM YY");
            dialog.DefaultExt = ".xml";
            dialog.Filter = "XML Documents(.xml)|*.xml";
            // Show save file dialog box
            bool? result = dialog.ShowDialog();

            // Process save file dialog box results
            string filePath = string.Empty;
            if (result == true)
            {
                // Save document
                filePath = dialog.FileName;
            }

            if (filePath != null)
                model.Serialize(Items, typeof(ObservableCollection<AnalyzerItemViewModel>), filePath);
            StatusBar = CreateStatusMessage("StatusSuccess", "Success", "", "Complete");

            aggregator.SendMessage<MessageFromAnalyzerViewModel, StatusBar>
                (
                    new MessageFromAnalyzerViewModel { Payload = StatusBar }
                );
        }

        private bool CanSaveSnapshot()
        {
            return true;
        }

        private void ExportToExcel()
        {
        }

        private bool CanExportToExcel()
        {
            return true;
        }

        private void SendEmail()
        {
        }

        private bool CanSendEmail()
        {
            return true;
        }

        //public ObservableCollection<string> AccessListControl 
        //{
        //    get { return accessListControl; } 
        //    set
        //    {
        //        accessListControl=value;
        //        this.OnPropertyChanged("AccessListControl");
        //    }
        //}

        //public ObservableCollection<string> AccessListRights 
        //{ 
        //    get {return accessListRights;}
        //    set
        //    {
        //        accessListRights=value;
        //        this.OnPropertyChanged("AccessListRights");
        //    }
        //}
    }
}
