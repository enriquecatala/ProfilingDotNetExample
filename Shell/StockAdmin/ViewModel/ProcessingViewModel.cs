using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using StockAdmin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAdmin.ViewModel
{
    public class ProcessingViewModel : ViewModelBase
    {

        private readonly IDataService _dataService;

        private readonly System.Windows.Threading.DispatcherTimer dispatcherTimerMalo = new System.Windows.Threading.DispatcherTimer();
        private readonly System.Windows.Threading.DispatcherTimer dispatcherTimerBueno = new System.Windows.Threading.DispatcherTimer();

        public ProcessingViewModel(IDataService dataService)
        {
            _dataService = dataService;

            
                CleanValues();
            
        }

        void CleanValues()
        {
            TiempoFilaAFila = TiempoBCPParalelo= TiempoTVPParalelo = "00:00:00.000";
        }


        #region TiempoTVPParalelo
        /// <summary>
        /// The <see cref="TiempoTVPParalelo" /> property's name.
        /// </summary>
        public const string TiempoTVPParaleloPropertyName = "TiempoTVPParalelo";

        private string _TiempoTVPParalelo = "0.0";
        private DateTime _inicioiempoMalo ;

        /// <summary>
        /// Sets and gets the TiempoTVPParalelo property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TiempoTVPParalelo
        {
            get
            {
                return _TiempoTVPParalelo;
            }

            set
            {
                if (_TiempoTVPParalelo == value)
                {
                    return;
                }

                _TiempoTVPParalelo = value;
                RaisePropertyChanged(TiempoTVPParaleloPropertyName);
            }
        }
        #endregion

        #region TiempoBCPParalelo

        /// <summary>
        /// The <see cref="TiempoBCPParalelo" /> property's name.
        /// </summary>
        public const string TiempoBCPParaleloPropertyName = "TiempoBCPParalelo";

        private string _TiempoBCPParalelo = "0.0";
        private DateTime _inicioTiempoBCPParalelo;

        /// <summary>
        /// Sets and gets the TiempoBCPParalelo property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TiempoBCPParalelo
        {
            get
            {
                return _TiempoBCPParalelo;
            }

            set
            {
                if (_TiempoBCPParalelo == value)
                {
                    return;
                }

                _TiempoBCPParalelo = value;
                RaisePropertyChanged(TiempoBCPParaleloPropertyName);
            }
        }
        #endregion

        #region TiempoFilaAFila

        /// <summary>
        /// The <see cref="TiempoFilaAFila" /> property's name.
        /// </summary>
        public const string TiempoFilaAFilaPropertyName = "TiempoFilaAFila";

        private string _tiempoFilaAFila = String.Empty;

        /// <summary>
        /// Sets and gets the TiempoFilaAFila property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string TiempoFilaAFila
        {
            get
            {
                return _tiempoFilaAFila;
            }

            set
            {
                if (_tiempoFilaAFila == value)
                {
                    return;
                }

                _tiempoFilaAFila = value;
                RaisePropertyChanged(TiempoFilaAFilaPropertyName);
            }
        }

        #endregion

        #region ActivarTiempoTVPParalelo

        private RelayCommand _startProcessingBadWay;

        /// <summary>
        /// Gets the StartProcessingBadWay.
        /// </summary>
        public RelayCommand StartProcessingBadWay
        {
            get
            {
                return _startProcessingBadWay
                    ?? (_startProcessingBadWay = new RelayCommand(ExecuteStartProcessingBadWay));
            }
        }

        private void ExecuteStartProcessingBadWay()
        {
            _inicioiempoMalo = DateTime.Now;
                                     
            _dataService.ProcesarMultithreadLockTVP();

            TiempoTVPParalelo = (DateTime.Now - _inicioiempoMalo).ToString();

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            
            TiempoTVPParalelo = (DateTime.Now-_inicioiempoMalo).ToString();
        }

        #endregion

        #region ActivarTiempoBCPParalelo

        private RelayCommand _activarTiempoBCPParalelo;

        /// <summary>
        /// Gets the ActivarTiempoBCPParalelo.
        /// </summary>
        public RelayCommand StartProcessingGoodWay
        {
            get
            {
                return _activarTiempoBCPParalelo
                    ?? (_activarTiempoBCPParalelo = new RelayCommand(ExecuteActivarTiempoBCPParalelo));
            }
        }

        private void ExecuteActivarTiempoBCPParalelo()
        {
            _inicioTiempoBCPParalelo = DateTime.Now;           

            _dataService.ProcesarMultithreadLockFreeBulkInsert();

            TiempoBCPParalelo = (DateTime.Now - _inicioTiempoBCPParalelo).ToString();


        }


        private void dispatcherTimerBueno_Tick(object sender, EventArgs e)
        {
            
            TiempoBCPParalelo = (DateTime.Now - _inicioTiempoBCPParalelo).ToString();
        }
        #endregion

        #region ActivarTiempoFilaAFilaMonohilo

        private RelayCommand _procesarFilaAFilaMonohilo;

        /// <summary>
        /// Gets the ProcesarFilaAFilaMonohilo.
        /// </summary>
        public RelayCommand ProcesarFilaAFilaMonohilo
        {
            get
            {
                return _procesarFilaAFilaMonohilo
                    ?? (_procesarFilaAFilaMonohilo = new RelayCommand(ExecuteProcesarFilaAFilaMonohilo));
            }
        }

        private void ExecuteProcesarFilaAFilaMonohilo()
        {
            DateTime tmp = DateTime.Now;

            _dataService.ProcesarMonoHiloDeLaMuerte();

            TiempoFilaAFila = (DateTime.Now - tmp).ToString();
        }

        #endregion

    }
}
