using System;
using StockAdmin.Model;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace StockAdmin.Model.Design
{
    public class DesignDataService : IDataService
    {

        public System.Collections.ObjectModel.ObservableCollection<CustomersBig> GetCustomersByIdsWithContains(List<int> id_customers)
        {
            throw new NotImplementedException();
        }

        public bool DoesPurchadedTheItem(int id_customer, int id_product)
        {
            throw new NotImplementedException();
        }

   

        public System.Collections.ObjectModel.ObservableCollection<CustomersBig> GetCustomersPaged(int numPage, int pageSize)
        {
            ObservableCollection<CustomersBig> retorno = new ObservableCollection<CustomersBig>();

            for (int i = 0; i < 10; i++)
                retorno.Add(new CustomersBig() { Name = "Customer" + i, ID_Customer = i });

            return (retorno);
        }


        public System.Collections.ObjectModel.ObservableCollection<CustomersBig> GetFirstCustomers(int pageSize)
        {
            throw new NotImplementedException();
        }




        public ObservableCollection<CustomersBig> GetCustomersPagedWithStoredProcedure(int numPage, int pageSize)
        {
            throw new NotImplementedException();
        }


        public ObservableCollection<CustomersBig> SearchWithNameUsingNotMappedObject(CustomerNotMappedInEF c)
        {
            throw new NotImplementedException();
        }


        public int TestContainsMethod(List<int> id_customers)
        {
            throw new NotImplementedException();
        }


        public int TestContainsMethodWithStoredProcedure(List<Microsoft.SqlServer.Server.SqlDataRecord> id_customers)
        {
            throw new NotImplementedException();
        }


        public ObservableCollection<CustomersBig> GetCustomersByIdsWithPostProcessing(List<int> id_customers)
        {
            throw new NotImplementedException();
        }


        public bool TestQueryingResultsInMemory()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<CustomersBig> GetCustomersPagedWithEFExtension(int numPage, int pageSize)
        {
            throw new NotImplementedException();
        }

        public void ProcesarMultithreadLockTVP()
        {
            throw new NotImplementedException();
        }

        public void ProcesarMultithreadLockFreeBulkInsert()
        {
            throw new NotImplementedException();
        }

        public void ProcesarMonoHiloDeLaMuerte()
        {
            throw new NotImplementedException();
        }
    }
}