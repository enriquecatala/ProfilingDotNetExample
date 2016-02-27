using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using Dapper;
using Dapper.Tvp;
using Microsoft.SqlServer.Server;
using System.Data.SqlClient;
using System.Data.Entity;
using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using FastMember;
using Microsoft.ConcurrencyVisualizer.Instrumentation;

namespace StockAdmin.Model
{
    public class DataService : IDataService
    {
        /// <summary>
        /// Muy mala práctica poner como public, estas permitiendo acceso libre a tus developers sobre tu BBDD
        /// </summary>
        public TrainingEntities context = new TrainingEntities();


        private int maxRows = 50000;
        private readonly string query_batch = @"select n id, 
                                                'cualquier cosa, me da igual' texto 
                                                from fn_Nums(100000)";

        /// <summary>
        /// Obtiene el nº de items por cliente
        /// </summary>
        /// <param name="id_customers"></param>
        /// <returns></returns>
        public ObservableCollection<CustomersBig> GetCustomersByIdsWithContains(List<int> ids)
        {

            var retorno = context
                .CustomersBigs
                .Where(c => ids.Contains(c.ID_Customer));

            return new ObservableCollection<CustomersBig>(retorno);
        }

        public ObservableCollection<CustomersBig> GetCustomersByIdsWithPostProcessing(List<int> id_customers)
        {

            var customersMaterializados = context.CustomersBigs.ToList();
            var retorno = customersMaterializados.Where(x => id_customers.Contains(x.ID_Customer));
            return (new ObservableCollection<CustomersBig>(retorno));

            #region SolucionEntreComillada
            /*
            /// Cuando tu forma de proceder requiere que postproceses resultados, intenta "indexar" como lo harias en BBDD creandote una tabla Hash
            /// Esto realmente no es solución porque el nº de elementos a buscar no compensa el coste de creación del diccionario
            var customersMaterializados = context.CustomersBigs.ToDictionary(t=>t.ID_Customer);
            ObservableCollection<CustomersBig> retorno = new ObservableCollection<CustomersBig>();
            foreach (var id in id_customers)
            {
                retorno.Add(customersMaterializados.Where(x => x.Key == id).Select(x => x.Value).FirstOrDefault());
            }
            return (retorno);
            */
            #endregion
        }

        public bool TestQueryingResultsInMemory()
        {
            /// Caso1: Materializar en Lista y buscar elementos dentro de una lista
            ///        O(n)
            var customersMaterializados = context.CustomersBigs.ToList();
            ///Lanzamos ahora 1000 peticiones 
            ///
            for (int i = 0; i < 1000; i++)
            {
                var retorno = customersMaterializados
                    .Where(x => x.ID_Customer == i).FirstOrDefault();  //sabemos que nos devuelve una
            }

            /// Caso2: Materializar en diccionario y buscar elementos dentro del mismo
            ///        O(1)
            var customersMaterializadosDiccionario = context.CustomersBigs.ToDictionary(t => t.ID_Customer);

            for (int i = 0; i < 1000; i++)
            {
                CustomersBig retorno;
                //Esta es la forma buena de pedir al diccionario
                customersMaterializadosDiccionario.TryGetValue(i, out retorno);

                /// Ojo con esto, que no vale :)
                retorno = customersMaterializadosDiccionario
                    .Where(x => x.Key == i).Select(x => x.Value).FirstOrDefault();
            }
            ///Ahora lo mismo pero con el tipo de datos Diccionario
            return (true);
        }

        /// <summary>
        /// Es el mísmo código que GetCustomersByIds, pero no listo los resultados, los cuento
        /// Solo para ver performance de .NET sin preocuparme de renderizado
        /// </summary>
        /// <param name="id_customers"></param>
        /// <returns></returns>
        public int TestContainsMethod(List<int> id_customers)
        {
            var query = context.CustomersBigs.Where(c => id_customers.Contains(c.ID_Customer));

            return (query.Count());
        }

        public int TestContainsMethodWithStoredProcedure(List<SqlDataRecord> id_customers)
        {
            IEnumerable<int> resultado;
            using (SqlConnection con = new SqlConnection(context.Database.Connection.ConnectionString.ToString()))
            {
                con.Open();

                resultado = con.Query<int>("[dbo].[ContainsCustomersByIds]", new TableValueParameter("@ids", "IDsTableType", id_customers), commandType: CommandType.StoredProcedure);
                //resultado = con.Execute("[dbo].[ContainsCustomersByIds]", new TableValueParameter("@ids", "IDsTableType", id_customers), null,null,CommandType.StoredProcedure);


            }
            return (resultado.First());
        }


        public ObservableCollection<CustomersBig> GetCustomersPaged(int numPage, int pageSize)
        {
            var retorno = context.CustomersBigs.OrderBy(x => x.ID_Customer)
                                               .Skip(pageSize * numPage)
                                               .Take(pageSize);

            return (new ObservableCollection<CustomersBig>(retorno));
        }
              

        public ObservableCollection<CustomersBig> GetFirstCustomers(int pageSize)
        {
            var retorno = context.CustomersBigs.OrderBy(x => x.ID_Customer).Take(pageSize);
            return (new ObservableCollection<CustomersBig>(retorno));
        }


        public ObservableCollection<CustomersBig> GetCustomersPagedWithStoredProcedure(int numPage, int pageSize)
        {
            var retorno = context.GetCustomersPaged(numPage, pageSize);

            return (new ObservableCollection<CustomersBig>(retorno));
        }


        public ObservableCollection<CustomersBig> GetCustomersPagedWithEFExtension(int numPage, int pageSize)
        {
            // Fijate que ahora tienes expresiones lambda como parámetro de entrada de Skip y Take
            // (Requiere using System.Data.Entity)
            //
            int skip = pageSize * numPage;
            var retorno = context.CustomersBigs.OrderBy(x => x.ID_Customer)
                                               .Skip(() => skip)
                                               .Take(() => pageSize);

            return (new ObservableCollection<CustomersBig>(retorno));
        }

        public ObservableCollection<CustomersBig> SearchWithNameUsingNotMappedObject(CustomerNotMappedInEF c)
        {

            // var retorno = context.CustomersBigs.Where(cb => cb.Name.StartsWith(cnm.CustomerName));

            var query = from entity in context.CustomersBigs
                        where entity.Name.StartsWith(c.CustomerName)
                        select entity;

            return (new ObservableCollection<CustomersBig>(query.ToList()));
        }


        public void ProcesarMonoHiloDeLaMuerte()
        {
            Object thisLock = new object();
            DataTable dt = new DataTable("mitablita");
            dt.Columns.Add("id", typeof(Int64));
            dt.Columns.Add("texto", typeof(String));


            using (SqlConnection cn = new SqlConnection(context.Database.Connection.ConnectionString))
            using (SqlConnection cn2 = new SqlConnection(context.Database.Connection.ConnectionString))
            {
                cn.Open();
                cn2.Open();

                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandText = query_batch;
                    cm.CommandType = CommandType.Text;
                    cm.CommandTimeout = 0;  // esto puede potencialmente tardar horas

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                Int64 id = dr.GetInt64(0);
                                String texto = dr.GetString(1);

                                String texto_a_insertar = MetodoCostoso(texto);
                                using (SqlCommand cm2 = cn2.CreateCommand())
                                {
                                    cm2.CommandText = "dbo.InsertarFilaAFila";
                                    cm2.CommandType = CommandType.StoredProcedure;
                                    cm2.CommandTimeout = 0; // esto en ocasiones tarda bastante

                                    cm2.Parameters.AddWithValue("@id", id);
                                    cm2.Parameters.AddWithValue("@texto", texto_a_insertar);
                                    cm2.ExecuteNonQuery();

                                }
                            }
                        }
                    }
                }
            }
        }

        public void ProcesarMultithreadLockTVP()
        {
            Object thisLock = new object();
            DataTable dt = new DataTable("mitablita");
            dt.Columns.Add("id", typeof(Int64));
            dt.Columns.Add("texto", typeof(String));


            using (SqlConnection cn = new SqlConnection(context.Database.Connection.ConnectionString))
            using (SqlConnection cn2 = new SqlConnection(context.Database.Connection.ConnectionString))
            {
                cn.Open();
                cn2.Open();

                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandText = query_batch;
                    cm.CommandType = CommandType.Text;
                    cm.CommandTimeout = 0;  // esto puede potencialmente tardar horas

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {
                            int filas = 0;
                            while (dr.Read())
                            {
                                dt.Rows.Add(dr.GetInt64(0),
                                            dr.GetString(1));
                                filas++;

                                /// cada 1000 filas, proceso y hago push al servidor
                                /// 
                                if ((filas > 0) && (filas % maxRows == 0))
                                {
                                    using (Markers.EnterSpan("Procesamiento DataRows"))
                                    {
                                        Parallel.For(0, maxRows, i =>
                                                       {
                                                           string query_parsed = MetodoCostoso(dt.Rows[i].Field<String>(1));
                                                           lock (thisLock)
                                                           {
                                                               dt.Rows[i].BeginEdit();
                                                               dt.Rows[i].SetField<String>(1, query_parsed);
                                                           }
                                                       });

                                        dt.AcceptChanges();
                                    }

                                    using (Markers.EnterSpan("InsercionTVP"))
                                    {
                                        using (SqlCommand cm2 = cn2.CreateCommand())
                                        {
                                            cm2.CommandText = "dbo.InsertarConTVP";
                                            cm2.CommandType = CommandType.StoredProcedure;
                                            cm2.CommandTimeout = 0; // esto en ocasiones tarda bastante

                                            SqlParameter sp = cm2.Parameters.AddWithValue("@tbl", dt);
                                            sp.SqlDbType = SqlDbType.Structured;
                                            cm2.ExecuteNonQuery();

                                        }

                                        dt.Rows.Clear();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        public void ProcesarMultithreadLockFreeBulkInsert()
        {
            Object thisLock = new object();
            DataTable dt = new DataTable("mitablita");
            dt.Columns.Add("Id", typeof(Int64));
            dt.Columns.Add("Texto", typeof(String));


            using (SqlConnection cn = new SqlConnection(context.Database.Connection.ConnectionString))
            using (SqlConnection cn2 = new SqlConnection(context.Database.Connection.ConnectionString))
            {
                cn.Open();
                cn2.Open();

                using (SqlCommand cm = cn.CreateCommand())
                {
                    cm.CommandText = query_batch;
                    cm.CommandType = CommandType.Text;
                    cm.CommandTimeout = 0;  // esto puede potencialmente tardar horas

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.HasRows)
                        {


                            ConcurrentDictionary<int, MyDataRow> cdNew = new ConcurrentDictionary<int, MyDataRow>();

                            int filas = 0;
                            while (dr.Read())
                            {
                                MyDataRow mdr = new MyDataRow(dr.GetInt64(0),
                                                              dr.GetString(1));
                                cdNew.TryAdd(filas % maxRows, mdr);

                                filas++;

                                /// cada 1000 filas, proceso y hago push al servidor
                                /// 
                                if ((filas > 0) && (filas % maxRows == 0))
                                {
                                    using (Markers.EnterSpan("Procesamiento ConcurrentDictionary"))
                                    {
                                        Parallel.For(0, maxRows, i =>
                                                        {
                                                            MyDataRow mdrNew;
                                                            cdNew.TryGetValue(i, out mdrNew);

                                                            if (mdrNew != null)
                                                            {
                                                                string query_parsedNew = MetodoCostoso(mdrNew.Texto);
                                                                MyDataRow mdrn2 = new MyDataRow(mdrNew.Id, query_parsedNew);

                                                                cdNew.TryUpdate(i, mdrNew, mdrn2);
                                                            }
                                                        });
                                    }

                                    using (Markers.EnterSpan("Insercion bulkinsert"))
                                    {
                                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cn2))
                                        {
                                            bulkCopy.BulkCopyTimeout = 0; // Esto puede que tarde un buen rato cuando hay presion

                                            bulkCopy.ColumnMappings.Add("Id", "Id");
                                            bulkCopy.ColumnMappings.Add("Texto", "texto");

                                            ///Libreria FastMember (NuGet) para convertir al vuelo
                                            ///
                                            using (var reader = ObjectReader.Create(cdNew.Values, "Id", "Texto"))
                                            {
                                                bulkCopy.DestinationTableName = "InsercionesBatch";
                                                bulkCopy.WriteToServer(reader);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private string MetodoCostoso(string textoActual)
        {
            string tralari = textoActual + "me d aigual que poner" + "el caso es que haga algo añsldfasjlñfk";
            return tralari;
        }
    }
}

   
    