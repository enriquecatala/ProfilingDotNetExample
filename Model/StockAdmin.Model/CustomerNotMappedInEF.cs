using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAdmin.Model
{
    /// <summary>
    /// When a query uses the properties of a non-mapped object type as a parameter then the query will not get cached
    /// </summary>
    public class CustomerNotMappedInEF
    {
        #region IdCustomer
        /// <summary>
        /// The <see cref="IdCustomer" /> property's name.
        /// </summary>
        public const string IdCustomerPropertyName = "IdCustomer";

        private int _idCustomer = 0;

        /// <summary>
        /// Sets and gets the IdCustomer property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int IdCustomer
        {
            get
            {
                return _idCustomer;
            }

            set
            {
                if (_idCustomer == value)
                {
                    return;
                }

                _idCustomer = value;
                }
        }

        #endregion

        #region CustomerName

        /// <summary>
        /// The <see cref="CustomerName" /> property's name.
        /// </summary>
        public const string CustomerNamePropertyName = "CustomerName";

        private string _CustomerName = string.Empty;

        /// <summary>
        /// Sets and gets the CustomerName property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string CustomerName
        {
            get
            {
                return _CustomerName;
            }

            set
            {
                if (_CustomerName == value)
                {
                    return;
                }

                _CustomerName = value;

            }
        }

        #endregion
    }
}
