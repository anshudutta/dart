using System;
using System.Windows.Threading;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace Analyzer.ViewModels
{
    public abstract class ViewModelBase:DispatcherObject,INotifyPropertyChanged, IDisposable
    {
        private bool _isSelected;
        private bool _isReadyToRemove;
        private StatusBar _statusBar;

        protected readonly IEventAggregator aggregator = new EventAggregator();
                
        public ViewModelBase()
        {            
            //SubscribeToAggregator();
            _statusBar = new StatusBar();
        }

        protected virtual void SubscribeToAggregator() { }
                      

        public bool IsSelected 
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            } 
        }

        public bool IsReadyToRemove 
        {
            get { return _isReadyToRemove; }
            set
            {
                _isReadyToRemove = value;
                OnPropertyChanged("IsReadyToRemove");
            }             
        }

        public StatusBar StatusBar
        {
            get { return _statusBar; }
            set
            {
                _statusBar = value;
                OnPropertyChanged("StatusBar");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ListenToSubscribedEvent(StatusBar s)
        {            
            UpdateStatusBar(s);
        }

        protected StatusBar CreateStatusMessage(string statusMessageType, string statusMessage, string detailedStatusMessage, string loadingAnimation)
        {            
            var status = new StatusBar();
            status.StatusMessage = statusMessage;
            status.StatusMessageType = statusMessageType;
            status.DetailedStatusMessage = detailedStatusMessage;

            status.StatusAnimation.Clear();

            if (loadingAnimation == "InWork")
                status.StatusAnimation.Add(new StatusbarLoadingViewModel());
            else
                status.StatusAnimation.Add(new StatusbarWorkCompleteViewModel());

            return status;
        }

        protected void PublishStatusMessage(string statusMessageType, string statusMessage,
            string detailedStatusMessage, string loadingAnimation)
        {
            _statusBar=CreateStatusMessage(statusMessageType, statusMessage,
                detailedStatusMessage, loadingAnimation);
            ListenToSubscribedEvent(StatusBar);
        }

        protected void UpdateStatusBar(StatusBar status)
        {
            
            StatusBar = status;
        }

        protected virtual void ItemCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action.ToString() == "Add")
            {
                if (e.NewItems != null)
                {
                    foreach (ViewModelBase item in e.NewItems)
                    {
                        item.PropertyChanged += ItemPropertyChanged;
                    }
                }
            }
            else
            {
                if (e.OldItems != null)
                {
                    foreach (ViewModelBase item in e.OldItems)
                    {
                        item.PropertyChanged -= ItemPropertyChanged;
                    }
                }
            }
            
        }

        protected virtual void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e != null)
                OnPropertyChanged(e.PropertyName);
        }
        
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged!=null)
                PropertyChanged(this,new PropertyChangedEventArgs(propertyName));
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected string XmlSerialize(object obj, System.Type type)
        {
            try
            {
                String XmlizedString = null;
                var xs = new XmlSerializer(type);
               
                var memoryStream = new MemoryStream();
               
                var xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                //Serialize emp in the xmlTextWriter
                xs.Serialize(xmlTextWriter, type);
                //Get the BaseStream of the xmlTextWriter in the Memory Stream
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                //Convert to array
                XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
                return XmlizedString;
            }
            catch (Exception ex)
            {                
                throw;
            }
        }

        private static String UTF8ByteArrayToString(Byte[] characters)
        {
                var encoding = new UTF8Encoding();
                var constructedString = encoding.GetString(characters);
                return (constructedString);
        }

        protected object XMLDeserialize(string xml, System.Type type)
        {
            var xs = new XmlSerializer(type);
            object obj = xs.Deserialize(new StringReader(xml));

            return obj;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
            }

                // dispose unmanaged resources
        }

        ~ViewModelBase()
        {
            Dispose(false);
        }        
    }
}
