using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using StockAdmin.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAdmin.ViewModel
{
    public class CustomersViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        public CustomersViewModel(IDataService dataService)
        {
            _dataService = dataService;

            if (IsInDesignMode)
            {
                Customers = dataService.GetCustomersPaged(1, 1);
                NameToSearch = "Texto ejemplo";
            }
        }

        #region Properties

        #region PageSize

        /// <summary>
        /// The <see cref="PageSize" /> property's name.
        /// </summary>
        public const string PageSizePropertyName = "PageSize";

        private String _pageSize = "8";

        /// <summary>
        /// Sets and gets the PageSize property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String PageSize
        {
            get
            {
                return _pageSize;
            }

            set
            {
                if (_pageSize == value)
                {
                    return;
                }

                _pageSize = value;
                RaisePropertyChanged(PageSizePropertyName);
            }
        }

        #endregion

        #region PageNumber

        /// <summary>
        /// The <see cref="PageNumber" /> property's name.
        /// </summary>
        public const string PageNumberPropertyName = "PageNumber";

        private String _pageNumber = "8";

        /// <summary>
        /// Sets and gets the PageNumber property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String PageNumber
        {
            get
            {
                return _pageNumber;
            }

            set
            {
                if (_pageNumber == value)
                {
                    return;
                }

                _pageNumber = value;
                RaisePropertyChanged(PageNumberPropertyName);
            }
        }

        #endregion

        #region CustomersBig

        /// <summary>
        /// The <see cref="Customers" /> property's name.
        /// </summary>
        public const string CustomersPropertyName = "Customers";

        private ObservableCollection<CustomersBig> _customers = new ObservableCollection<CustomersBig>();

        /// <summary>
        /// Sets and gets the Customers property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<CustomersBig> Customers
        {
            get
            {
                return _customers;
            }

            set
            {
                if (_customers == value)
                {
                    return;
                }

                _customers = value;
                RaisePropertyChanged(CustomersPropertyName);
            }
        }

        #endregion

        #region NameToSearch

        /// <summary>
        /// The <see cref="NameToSearch" /> property's name.
        /// </summary>
        public const string NameToSearchPropertyName = "NameToSearch";

        private string _nameToSearch = String.Empty;

        /// <summary>
        /// Sets and gets the NameToSearch property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string NameToSearch
        {
            get
            {
                return _nameToSearch;
            }

            set
            {
                if (_nameToSearch == value)
                {
                    return;
                }

                _nameToSearch = value;
                RaisePropertyChanged(NameToSearchPropertyName);
            }
        }

        #endregion

        #region CustomerIdsSeparatedByCommas

        /// <summary>
        /// The <see cref="CustomerIdsSeparatedByCommas" /> property's name.
        /// </summary>
        public const string CustomerIdsSeparatedByCommasPropertyName = "CustomerIdsSeparatedByCommas";

        private string _CustomerIdsSeparatedByCommas = String.Empty;

        /// <summary>
        /// Sets and gets the CustomerIdsSeparatedByCommas property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string CustomerIdsSeparatedByCommas
        {
            get
            {
                return _CustomerIdsSeparatedByCommas;
            }

            set
            {
                if (_CustomerIdsSeparatedByCommas == value)
                {
                    return;
                }

                _CustomerIdsSeparatedByCommas = value;
                RaisePropertyChanged(CustomerIdsSeparatedByCommasPropertyName);

                _customersIds = new List<int>();
                foreach (string v in _CustomerIdsSeparatedByCommas.Split(','))
                {
                    _customersIds.Add(Int32.Parse(v));
                }

            }
        }

        #endregion

        #region CustomersIds

        /// <summary>
        /// The <see cref="CustomersIds" /> property's name.
        /// </summary>
        public const string CustomersIdsPropertyName = "CustomersIds";

        private List<int> _customersIds = new List<int>();

        /// <summary>
        /// Sets and gets the CustomersIds property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<int> CustomersIds
        {
            get
            {
                return _customersIds;
            }

            set
            {
                if (_customersIds == value)
                {
                    return;
                }

                _customersIds = value;
                RaisePropertyChanged(CustomersIdsPropertyName);
            }
        }

        #endregion

        #endregion

        #region Commands

        #region GetCustomersPaged
        private RelayCommand _getCustomersPaged;

        /// <summary>
        /// Gets the GetCustomersPaged.
        /// </summary>
        public RelayCommand GetCustomersPaged
        {
            get
            {
                return _getCustomersPaged
                    ?? (_getCustomersPaged = new RelayCommand(ExecuteGetCustomersPaged));
            }
        }

        private void ExecuteGetCustomersPaged()
        {
            Customers = _dataService.GetCustomersPaged(Int32.Parse(PageNumber), Int32.Parse(PageSize));
        }
        #endregion

        #region GetCustomersPagedWithEFExtension
        private RelayCommand _getCustomersPagedWithEFEextension;

        /// <summary>
        /// Gets the GetCustomersPagedWithEFExtension.
        /// </summary>
        public RelayCommand GetCustomersPagedWithEFExtension
        {
            get
            {
                return _getCustomersPagedWithEFEextension
                    ?? (_getCustomersPagedWithEFEextension = new RelayCommand(ExecuteGetCustomersPagedWithEFExtension));
            }
        }

        private void ExecuteGetCustomersPagedWithEFExtension()
        {
            Customers = _dataService.GetCustomersPagedWithEFExtension(Int32.Parse(PageNumber), Int32.Parse(PageSize));
        }
        #endregion

        #region GetFirstCustomers
        private RelayCommand _getFirstCustomers;

        /// <summary>
        /// Gets the GetFirstCustomers.
        /// </summary>
        public RelayCommand GetFirstCustomers
        {
            get
            {
                return _getFirstCustomers
                    ?? (_getFirstCustomers = new RelayCommand(ExecuteGetFirstCustomers));
            }
        }

        private void ExecuteGetFirstCustomers()
        {
            Customers = _dataService.GetFirstCustomers(Int32.Parse(PageSize));
        }
        #endregion

        #region GetCustomersPagedWithStoredProcedure

        private RelayCommand _getcustomersPagedWithStoredProcedure;

        /// <summary>
        /// Gets the GetCustomersPagedWithStoredProcedure.
        /// </summary>
        public RelayCommand GetCustomersPagedWithStoredProcedure
        {
            get
            {
                return _getcustomersPagedWithStoredProcedure
                    ?? (_getcustomersPagedWithStoredProcedure = new RelayCommand(ExecuteGetCustomersPagedWithStoredProcedure));
            }
        }

        private void ExecuteGetCustomersPagedWithStoredProcedure()
        {
            Customers = _dataService.GetCustomersPagedWithStoredProcedure(Int32.Parse(PageNumber), Int32.Parse(PageSize));
        }

        #endregion

        #region SearchWithNameUsingNotMappedObject

        private RelayCommand _searchWithNameUsingNotMappedObject;

        /// <summary>
        /// Gets the SearchWithNameUsingNotMappedObject.
        /// </summary>
        public RelayCommand SearchWithNameUsingNotMappedObject
        {
            get
            {
                return _searchWithNameUsingNotMappedObject
                    ?? (_searchWithNameUsingNotMappedObject = new RelayCommand(ExecuteSearchWithNameUsingNotMappedObject));
            }
        }

        private void ExecuteSearchWithNameUsingNotMappedObject()
        {
            CustomerNotMappedInEF cnm = new CustomerNotMappedInEF();

            cnm.CustomerName = NameToSearch;

            Customers = _dataService.SearchWithNameUsingNotMappedObject(cnm);

        }

        #endregion

        #region GetCustomersByIds

        private RelayCommand _getCustomersByIds;

        /// <summary>
        /// Gets the GetCustomersByIds.
        /// </summary>
        public RelayCommand GetCustomersByIds
        {
            get
            {
                return _getCustomersByIds
                    ?? (_getCustomersByIds = new RelayCommand(ExecuteGetCustomersByIds));
            }
        }

        private void ExecuteGetCustomersByIds()
        {
            Customers = _dataService.GetCustomersByIdsWithContains(CustomersIds);
        }

        #endregion

        #region GetCustomersByIdsWithPostProcessing

        private RelayCommand _getCustomersByIdsWithPostProcessing;

        /// <summary>
        /// Gets the GetCustomersByIdsWithPostProcessing.
        /// </summary>
        public RelayCommand GetCustomersByIdsWithPostProcessing
        {
            get
            {
                return _getCustomersByIdsWithPostProcessing
                    ?? (_getCustomersByIdsWithPostProcessing = new RelayCommand(ExecuteGetCustomersByIdsWithPostProcessing));
            }
        }

        private void ExecuteGetCustomersByIdsWithPostProcessing()
        {
            Customers = _dataService.GetCustomersByIdsWithPostProcessing(CustomersIds);
        }

        #endregion

        #region TestQueryingResultsInMemory

        private RelayCommand _testQueryingResultsInMemory;

        /// <summary>
        /// Gets the TestQueryingResultsInMemory.
        /// </summary>
        public RelayCommand TestQueryingResultsInMemory
        {
            get
            {
                return _testQueryingResultsInMemory
                    ?? (_testQueryingResultsInMemory = new RelayCommand(ExecuteTestQueryingResultsInMemory));
            }
        }

        private void ExecuteTestQueryingResultsInMemory()
        {
            _dataService.TestQueryingResultsInMemory();
            var testc = new ObservableCollection<CustomersBig>();
            testc.Add(new CustomersBig() { ID_Customer = -1, Name = "Proceso FINALIZADO" });
            Customers = testc;
        }

        #endregion

        #endregion

    }
}