using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Collections.ObjectModel;
      
namespace StockAdmin.Model
{

    /// <summary>
    /// https://msdn.microsoft.com/en-us/data/hh949853.aspx?f=255&MSPPError=-2147217396
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// Obtiene el nº de items de una serie de customers
        /// Para usar .Contains y ve rque no se cachea
        /// </summary>
        /// <param name="id_customer"></param>
        /// <returns></returns>
        ObservableCollection<CustomersBig> GetCustomersByIdsWithContains(List<int> id_customers);

        ObservableCollection<CustomersBig> GetCustomersByIdsWithPostProcessing(List<int> id_customers);

        /// <summary>
        /// Metodo para que veamos con profiler lo que implica utilizar el tipo de datos correcto
        /// </summary>
        /// <returns></returns>
        bool TestQueryingResultsInMemory();

        int TestContainsMethod(List<int> id_customers);


        int TestContainsMethodWithStoredProcedure(List<SqlDataRecord> id_customers);
        
         
        
        /// <summary>
         /// The Skip(), Take(), Contains() and DefautIfEmpty() LINQ operators do not produce SQL queries with parameters but instead put the values passed to them as constants
        /// </summary>
        /// <param name="numPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
         ObservableCollection<CustomersBig> GetCustomersPaged(int numPage, int pageSize);

        /// <summary>
        /// The Skip(), Take(), Contains() and DefautIfEmpty() LINQ operators in EntityFramework.Extensions does produce SQL queries with parameters
        /// </summary>
        /// <param name="numPage"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        ObservableCollection<CustomersBig> GetCustomersPagedWithEFExtension(int numPage, int pageSize);
         
         ObservableCollection<CustomersBig> GetCustomersPagedWithStoredProcedure(int numPage, int pageSize);
       
         ObservableCollection<CustomersBig> GetFirstCustomers(int pageSize);


         /// <summary>
         /// When a query uses the properties of a non-mapped object type as a parameter then the query will not get cached
         /// </summary>
         /// <param name="c"></param>
         /// <returns></returns>
         ObservableCollection<CustomersBig> SearchWithNameUsingNotMappedObject(CustomerNotMappedInEF c);

        void ProcesarMultithreadLockTVP();

        void ProcesarMultithreadLockFreeBulkInsert();

        void ProcesarMonoHiloDeLaMuerte();

    }
}
