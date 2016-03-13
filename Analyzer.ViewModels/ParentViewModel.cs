using System.Collections.Specialized;
using System.Text;
using System.Collections.ObjectModel;
using Analyzer.Framework;

namespace Analyzer.ViewModels
{
    public class ParentViewModel:ViewModelBase
    {
        private readonly ObservableCollection<ParentViewModel> _children;
        private readonly ParentViewModel _parent;
        private readonly DeviceIO _deviceIO;
        private StringBuilder sbTooTipInfo;
        //private List<ItemDetails> toolTipInformation;
        private string _elementType;
        private bool _isEnabled;
   
        private bool _isExpanded;
        private bool _isSelected;

        public ParentViewModel(DeviceIO parent)
            : this(parent, null)
        {
        }

        private ParentViewModel(DeviceIO deviceIO, ParentViewModel parent)
        {
            _deviceIO = deviceIO;
            _parent = parent;
            ElementType = deviceIO.ElementType;
            IsEnabled = ElementType != "File";
            //this.children = new ObservableCollection<ParentViewModel>(
            //        (from child in DeviceIO.Children
            //         select new ParentViewModel(child, this))
            //         .ToList<ParentViewModel>());
            _children = new ObservableCollection<ParentViewModel>();
            Children.CollectionChanged += ItemCollectionChanged;
            if (deviceIO.Children != null)
            {
                foreach (DeviceIO item in deviceIO.Children)
                {
                    Children.Add(new ParentViewModel(item));
                }   
            }

            //this.toolTipInformation = new List<ItemDetails>();
            //if (ElementType == "File")
            //{
            //    // load tool tip Information 
            //    System.IO.FileInfo fInfo = new System.IO.FileInfo(deviceIO.Path+"\\"+deviceIO.Name);
            //    ToolTipInformation = new List<ItemDetails>()
            //    {
            //        new ItemDetails() { Details = "Type : File" },
            //        new ItemDetails() { Details = "Creation Time :" + fInfo.CreationTime },
            //        new ItemDetails() { Details = "Length :" + fInfo.Length },
            //        new ItemDetails() { Details = "Last Access Time :" + fInfo.LastAccessTime },
            //        new ItemDetails() { Details = "Last Write Time :" + fInfo.LastWriteTime }
            //    };                    

            //}

            sbTooTipInfo = new StringBuilder();
            if (ElementType == "File")
            {
                // load tool tip Information 
                var fInfo = new System.IO.FileInfo(_deviceIO.Path+"\\"+_deviceIO.Name);
                sbTooTipInfo.AppendLine("Type : File");
                sbTooTipInfo.AppendLine("Creation Time :" + fInfo.CreationTime);
                sbTooTipInfo.AppendLine("Length :" + fInfo.Length);
                sbTooTipInfo.AppendLine("Last Access Time :" + fInfo.LastAccessTime);
                sbTooTipInfo.AppendLine("Last Write Time :" + fInfo.LastWriteTime);
            }
            else
                sbTooTipInfo.AppendLine("Type : Directory");
            //new ItemDetails() { Details = "Type : File" },
            //        new ItemDetails() { Details = "Creation Time :" + fInfo.CreationTime },
            //        new ItemDetails() { Details = "Length :" + fInfo.Length },
            //        new ItemDetails() { Details = "Last Access Time :" + fInfo.LastAccessTime },
            //        new ItemDetails() { Details = "Last Write Time :" + fInfo.LastWriteTime }

            
        }

        public ObservableCollection<ParentViewModel> Children
        {
            get { return _children; }
        }

        public string Name
        {
            get { return _deviceIO.Name; }
        }

        public ParentViewModel MyComputer
        {
            get { return _parent; }
        }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    OnPropertyChanged("IsExpanded");
                }

                // Expand all the way up to the root.
                if (_isExpanded && _parent != null)
                    _parent.IsExpanded = true;
            }
        }

        public new bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }

        public string ElementType
        {
            get { return _elementType; }
            set
            {
                _elementType = value;
                //this.OnPropertyChanged("ElementType");
            }
        }

        //public List<ItemDetails> ToolTipInformation
        //{
        //    get { return toolTipInformation; }

        //    set
        //    {
        //        toolTipInformation = value;
        //        this.OnPropertyChanged("ToolTipInformation");
        //    }
        //}

        public string ToolTipInformation
        {
            get { return sbTooTipInfo.ToString(); }

            set
            {
                sbTooTipInfo.Append(value);
                OnPropertyChanged("ToolTipInformation");
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }

            set
            {
                _isEnabled = value;
                OnPropertyChanged("IsEnabled");
            }
        }


    }
}
