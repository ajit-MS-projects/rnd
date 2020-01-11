using GalaSoft.MvvmLight;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using PvScoutWeb.Silverlight.Model;

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
    public class PvModulViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the PvModulViewModel class.
        /// </summary>
        public PvModulViewModel()
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
        
        ////public override void Cleanup()
        ////{
        ////    // Clean own resources if needed

        ////    base.Cleanup();
        ////}
        PvModul modul = new PvModul();

        /// <summary>
        /// Current product model
        /// </summary>
        public PvModul PvModul
        {
            get
            {
                if (modul == null)
                    modul = new PvModul();
                return modul;
            }
            set
            {
                modul = value;
                //RaisePropertyChanged(ModulWidthPropertyName);
                //RaisePropertyChanged(ModulHeightPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="ModulWidth" /> property's name.
        /// </summary>
        public const string ModulWidthPropertyName = "ModulWidth";

        private int _modWidth ;

        /// <summary>
        /// Gets the ModulWidth property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public int ModulWidth
        {
            get
            {
                return _modWidth;
            }

            set
            {
                if (_modWidth == value)
                {
                    return;
                }

                var oldValue = _modWidth;
                _modWidth = value;
                
                // Update bindings, no broadcast
                RaisePropertyChanged(ModulWidthPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="ModulHeight" /> property's name.
        /// </summary>
        public const string ModulHeightPropertyName = "ModulHeight";

        private int _modHeight;

        /// <summary>
        /// Gets the ModulHeight property.
        /// TODO Update documentation:
        /// Changes to that property's value raise the PropertyChanged event. 
        /// This property's value is broadcasted by the Messenger's default instance when it changes.
        /// </summary>
        public int ModulHeight
        {
            get
            {
                return _modHeight;
            }

            set
            {
                if (_modHeight == value)
                {
                    return;
                }

                var oldValue = _modHeight;
                _modHeight = value;

                // Update bindings, no broadcast
                RaisePropertyChanged(ModulHeightPropertyName);
            }
        }

        public ICommand FillRoofCommand
        {
            get { return new RelayCommand(FillRoof); }
        }

        private void FillRoof()
        {

        }
    }
}