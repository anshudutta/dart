using System;

namespace Analyzer.ViewModels
{
    public class WorkspaceViewModel:ViewModelBase
    {
        public event EventHandler RequestClose;

        protected virtual void OnRequestClose()
        {
            if (RequestClose != null)
                RequestClose(this, new EventArgs());
        }

        protected virtual bool OnRequestCanClose()
        {
            return true;
        }        

    }
}
