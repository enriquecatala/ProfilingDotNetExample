using GalaSoft.MvvmLight;
using StockAdmin.Model;

namespace StockAdmin.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }

            set
            {
                if (_welcomeTitle == value)
                {
                    return;
                }

                _welcomeTitle = value;
                RaisePropertyChanged(WelcomeTitlePropertyName);
            }
        }

        #region StatusInterceptorText

        /// <summary>
        /// The <see cref="StatusInterceptorText" /> property's name.
        /// </summary>
        public const string StatusInterceptorTextPropertyName = "StatusInterceptorText";

        private string _statusInterceptorText = string.Empty;

        /// <summary>
        /// Sets and gets the StatusInterceptorText property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string StatusInterceptorText
        {
            get
            {
                return _statusInterceptorText;
            }

            set
            {
                if (_statusInterceptorText == value)
                {
                    return;
                }

                _statusInterceptorText = value;
                RaisePropertyChanged(StatusInterceptorTextPropertyName);
            }
        }


        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;

#if INTERCEPTOR_ON
            StatusInterceptorText = "El interceptor de Entity Framework está ACTIVADO";
#else
            StatusInterceptorText = "El interceptor de Entity Framework está DESACTIVADO";
#endif

            //_dataService.GetData(
            //    (item, error) =>
            //    {
            //        if (error != null)
            //        {
            //            // Report error here
            //            return;
            //        }

            //        WelcomeTitle = item.Title;
            //    });
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}