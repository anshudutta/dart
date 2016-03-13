using System;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using System.Windows;

namespace NotesListBox
{
    public class NotesAdornerDecorator : AdornerDecorator
    {
        #region Data
        /// <summary>
        /// You MUST have an AdornerLayer to show the NotesListBoxControl
        /// as it was designed with the AdornerLayer in mind
        /// </summary>
        private AdornerLayer _layer;
        /// <summary>
        /// A adorner to show the NotesListBoxControl in the AdornerLayer
        /// </summary>
        private NoteAdorner _adorner;
        private ObservableCollection<Note> _notes = new ObservableCollection<Note>();
        private FrameworkElement _adornedElement;
        #endregion

        #region Ctor
        public NotesAdornerDecorator()
        {
            Loaded += NotesAdornerDecoratorLoaded;
        }
        #endregion

        #region DPs

        /// <summary>
        /// DisplayNotes Dependency Property
        /// </summary>
        public static readonly DependencyProperty DisplayNotesProperty =
            DependencyProperty.Register("DisplayNotes", typeof(ObservableCollection<Note>), 
            typeof(NotesAdornerDecorator),
                new FrameworkPropertyMetadata(OnDisplayNotesChanged));

        /// <summary>
        /// Gets or sets the DisplayNotes property.  
        /// </summary>
        public ObservableCollection<Note> DisplayNotes
        {
            get { return (ObservableCollection<Note>)GetValue(DisplayNotesProperty); }
            set { SetValue(DisplayNotesProperty, value); }
        }

        /// <summary>
        /// Handles changes to the DisplayNotes property.
        /// </summary>
        private static void OnDisplayNotesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            var notesAdornerDecorator = (NotesAdornerDecorator)d;

            ((NotesAdornerDecorator)d)._notes = (ObservableCollection<Note>)e.NewValue;
            
            if (notesAdornerDecorator._adorner != null & notesAdornerDecorator._layer != null)
                notesAdornerDecorator._layer.Remove(notesAdornerDecorator._adorner);

            notesAdornerDecorator._adorner = 
                new NoteAdorner(notesAdornerDecorator._adornedElement, notesAdornerDecorator._notes);
            if (notesAdornerDecorator._layer != null) notesAdornerDecorator._layer.Add(notesAdornerDecorator._adorner);
        }


        #endregion



        #region Private Methods

        private void NotesAdornerDecoratorLoaded(object sender, RoutedEventArgs e)
        {
            _layer = AdornerLayer;

            //I am assuming that I need to create an actual Child element
            _adornedElement = new FrameworkElement { Height = Height, Width = Width };
            Child = _adornedElement;

            #region Wire up the actual NotesListBoxControl

            //Wire up the Close Notes Event, which will come from the 
            //NotesListBoxControl on the AdornerLayer
            EventManager.RegisterClassHandler(
                typeof(NotesListBoxControl),
                NotesListBoxControl.CloseNotesEvent,
                new EventHandler(
                    (s, ea) =>
                    {
                        if (_adorner != null && _layer != null)
                        {
                            _layer.Remove(_adorner);
                            _adorner = null;
                        }
                    }));
            #endregion
        }
        #endregion

        #region Overrides
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            if (_adornedElement != null)
            {
                _adornedElement.Height = sizeInfo.NewSize.Height;
                _adornedElement.Width = sizeInfo.NewSize.Width;
            }
 
            if (_adorner!=null)
                _adorner.ResizeToFillAvailableSpace(sizeInfo.NewSize);
        }
        #endregion
    }
}
