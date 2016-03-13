using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Collections.ObjectModel;

namespace NotesListBox
{
    /// <summary>
    /// Hosts an single NotesListBoxControl 
    /// in the AdornerLayer
    /// </summary>
    public class NoteAdorner : Adorner
    {
        #region Data
        private ArrayList _logicalChildren;
        private NotesListBoxControl notesListBoxControl;
        private readonly Grid _host = new Grid();
        #endregion // Data

        #region Constructor



        public NoteAdorner(FrameworkElement adornedCtrl, ObservableCollection<Note> notes)
            : base(adornedCtrl)
        {


            _host.Width = 250;
            _host.Height = adornedCtrl.ActualHeight;
            _host.VerticalAlignment = VerticalAlignment.Stretch;
            _host.HorizontalAlignment = HorizontalAlignment.Right;
            _host.Margin = new Thickness(0);
            notesListBoxControl = new NotesListBoxControl {Notes = notes, Margin = new Thickness(0)};
            _host.Children.Add(notesListBoxControl);
            AddLogicalChild(_host);
            AddVisualChild(_host);
        }


        public void ResizeToFillAvailableSpace(Size availableSize)
        {
            _host.Width = 250;
            _host.Height = availableSize.Width;
            notesListBoxControl.ResizeToFillAvailableSpace(_host.Height);
        }



        #endregion // Constructor



        #region Measure/Arrange

        /// <summary>
        /// Allows the control to determine how big it wants to be.
        /// </summary>
        /// <param name="constraint">A limiting size for the control.</param>
        protected override Size MeasureOverride(Size constraint)
        {
            return constraint;
        }

        /// <summary>
        /// Positions and sizes the control.
        /// </summary>
        /// <param name="finalSize">The actual size of the control.</param>		
        protected override Size ArrangeOverride(Size finalSize)
        {
            var rect = new Rect(new Point(), finalSize);

            _host.Arrange(rect);
            return finalSize;
        }

        #endregion // Measure/Arrange

        #region Visual Children

        /// <summary>
        /// Required for the element to be rendered.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        /// <summary>
        /// Required for the element to be rendered.
        /// </summary>
        protected override Visual GetVisualChild(int index)
        {
            if (index != 0)
                throw new ArgumentOutOfRangeException("index");

            return _host;
        }

        #endregion // Visual Children

        #region Logical Children

        /// <summary>
        /// Required for the displayed element to inherit property values
        /// from the logical tree, such as FontSize.
        /// </summary>
        protected override IEnumerator LogicalChildren
        {
            get
            {
                if (_logicalChildren == null)
                {
                    _logicalChildren = new ArrayList {_host};
                }

                return _logicalChildren.GetEnumerator();
            }
        }

        #endregion // Logical Children
    }
}