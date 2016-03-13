using System;
using System.Globalization;
using System.Windows.Data;

namespace Analyzer.ViewModels
{
    public class ElementTypeToImageConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path=string.Empty;
            switch (value.ToString())
            {
                case "Drive":
                    break;
                case "Directory":
                    path= "/Analyzer.Views;component/Icons/Folder.png";
                    break;
                case "Hidden":
                    path = "/Analyzer.Views;component/Icons/Question-mark-icon.png";
                    break;
                case ".txt":
                    path = "/Analyzer.Views;component/Icons/file-txt-icon.png";
                    break;
                case ".pdf":
                    path = "/Analyzer.Views;component/Icons/PDF-icon.png";
                    break;
                case ".jpg":
                    path = "/Analyzer.Views;component/Icons/jpg-icon.png";
                    break;
                case ".exe":
                    path= "/Analyzer.Views;component/Icons/File.png";
                    break;
                case ".dll":
                    path = "/Analyzer.Views;component/Icons/dll-icon.png";
                    break;
                case ".doc":
                    path = "/Analyzer.Views;component/Icons/Word-icon.png";
                    break;
                case ".rar":
                    path = "/Analyzer.Views;component/Icons/Zip-icon.png";
                    break;
                case ".zip":
                    path = "/Analyzer.Views;component/Icons/Zip-icon.png";
                    break;
                case "StatusError":
                    path = "/DART;component/Icons/StatusError.png";
                    break;
                case "StatusInformation":
                    path = "/DART;component/Icons/StatusInformation.png";
                    break;
                case "StatusSuccess":
                    path = "/DART;component/Icons/StatusSuccess.png";
                    break;
                default:
                    path = "/Analyzer.Views;component/Icons/Documents-icon.png";
                    break;
            }

            return path;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
