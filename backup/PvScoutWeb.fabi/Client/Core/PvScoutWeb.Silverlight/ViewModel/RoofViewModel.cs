using GalaSoft.MvvmLight;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace PvScoutWeb.Silverlight.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm/getstarted
    /// </para>
    /// </summary>
    public class RoofViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the RoofViewModel class.
        /// </summary>
        public RoofViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real": Connect to service, etc...
            ////}
        }

        // todo: remove the default -> only for test
        // set a default value
        private int _roofWidth = 500;
        public int RoofWidth
        {
            get
            {
                return _roofWidth;
            }
            set
            {
                _roofWidth = value;
            }
        }

        // todo: remove the default -> only for test
        // set a default value
        private int _roofHeight = 500;
        public int RoofHeight
        {
            get
            {
                return _roofHeight;
            }
            set
            {
                _roofHeight = value;
            }
        }

        /// <summary>
        /// The <see cref="RoofWidthPixel" /> property's name.
        /// </summary>
        public const string RoofWidthPixelPropertyName = "RoofWidthPixel";

        private int _roofWidthPixel;

        /// <summary>
        /// Gets the RoofWidthPixel property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public int RoofWidthPixel
        {
            get
            {
                return _roofWidthPixel;
            }

            set
            {
                if (_roofWidthPixel == value)
                {
                    return;
                }

                var oldValue = _roofWidthPixel;
                _roofWidthPixel = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(RoofWidthPixelPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="RoofHeightPixel" /> property's name.
        /// </summary>
        public const string RoofHeightPixelPropertyName = "RoofHeightPixel";

        private int _roofHeightPixel;

        /// <summary>
        /// Gets the RoofHeightPixel property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public int RoofHeightPixel
        {
            get
            {
                return _roofHeightPixel; ;
            }

            set
            {
                if (_roofHeightPixel == value)
                {
                    return;
                }

                var oldValue = _roofHeightPixel; ;
                _roofHeightPixel = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(RoofHeightPixelPropertyName);

            }
        }

        #region Commands
        public ICommand CreateNewRoofCommand
        {
            get { return new RelayCommand(CreateNewRoof); }
        }

        public event ZoomUpdateRequired UpdateZoom;
        public delegate void ZoomUpdateRequired(double width, double height);


        private void CreateNewRoof()
        {
            if (RoofHeight > 0 && RoofWidth > 0)
            {
                RoofHeightPixel = RoofHeight;
                RoofWidthPixel = RoofWidth;
                UpdateZoom(RoofHeight, RoofWidth);

                //ZoomWidth = RoofWidth + 20;
                //ZoomHeight = RoofHeight + 20; // add the borderThickness to the roofsize so that the scrollbars will complete go down
            }
        }

      
        #endregion

        ////public override void Cleanup()
        ////{
        ////    // Clean own resources if needed

        ////    base.Cleanup();
        ////}
    }
}