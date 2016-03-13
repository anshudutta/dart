using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Events;
using Microsoft.Practices.Composite.Presentation.Events;

namespace Analyzer.ViewModels
{
    public class MessageFromAnalyzerViewModel : PresentationEvent<StatusBar> { }
    
}
