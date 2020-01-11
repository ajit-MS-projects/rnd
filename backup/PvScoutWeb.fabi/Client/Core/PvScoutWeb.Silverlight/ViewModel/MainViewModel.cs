using GalaSoft.MvvmLight;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System;

namespace PvScoutWeb.Silverlight.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
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
    public class MainViewModel : ViewModelBase
    {

       

        #region properties
        

        /// <summary>
        /// The <see cref="ZoomWidth" /> property's name.
        /// </summary>
        public const string ZoomWidthPropertyName = "ZoomWidth";

        private int _zoomWidth;

        /// <summary>
        /// Gets the ZoomWidth property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public int ZoomWidth
        {
            get
            {
                return _zoomWidth;
            }

            set
            {
                if (_zoomWidth == value)
                {
                    return;
                }
                _zoomWidth = value;
               
                // Update bindings, no broadcast
                RaisePropertyChanged(ZoomWidthPropertyName);                
            }
        }

        /// <summary>
        /// The <see cref="ZoomRoofHeightPixel" /> property's name.
        /// </summary>
        public const string ZoomHeightPropertyName = "ZoomHeight";

        private int _zoomHeight;

        /// <summary>
        /// Gets the "ZoomRoofHeightPixel property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public int ZoomHeight
        {
            get
            {
                return _zoomHeight;
            }

            set
            {
                if (_zoomHeight == value)
                {
                    return;
                }                
                _zoomHeight = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(ZoomHeightPropertyName);

            }
        }

        
        
        #endregion

        

       

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                // Code runs "for real"
            }
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}


        #region commands
        public ICommand CreateNewRoofCommand
        {
            get { return new RelayCommand(CreateNewRoof); }
        }
        
        private void CreateNewRoof()
        {
            //if (RoofHeight > 0 && RoofWidth >0)
            //{
            //    RoofHeightPixel = RoofHeight;
            //    RoofWidthPixel = RoofWidth;
            //    ZoomWidth = RoofWidth + 20;
            //    ZoomHeight = RoofHeight + 20; // add the borderThickness to the roofsize so that the scrollbars will complete go down
            //}
        }

        public ICommand FillRoofCommand 
        {
            get { return new RelayCommand(FillRoof); }
        }

        private void FillRoof()
        { 
        
        }
        #endregion
    }
}