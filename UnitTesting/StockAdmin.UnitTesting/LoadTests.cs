using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockAdmin.Model;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.SqlServer.Server;
using System.Data;

namespace StockAdmin.UnitTesting
{
    [TestClass]
    public class LoadTests
    {
        #region InitParameters
        private readonly int NumTests = 100;

        /// <summary>
        /// Hay 286 elementos
        /// </summary>
        public string[] CustomerNames = new string[] { "Aaron", "Abadia", "Acacio", "Adalto", "Adao", "Adriana", "Aecio", "Afonso", "Afrodite", "Agnes", "Ailson", "Airton", "Alan", "Alanis", "Alessandro", "Alex", "Alexandre", "Alice", "Alicia", "Aline", "Aloisio", "Alonso", "Alvaro", "Amanda", "Ana", "Ana Claudia", "Ana Paula", "Analy", "Anastacia", "Anderson", "Andre", "Andreia", "Anisio", "Anita", "Antonio", "Aparecida", "Apolo", "Arlin", "Atila", "Barbara", "Batistuta", "Beatriz", "Bernard", "Bernardo", "Bianca", "Boris", "Boromir", "Bruna", "Brunisa", "Bruno", "Camila", "Camille", "Carla", "Carlos", "Carmen", "Carolina", "Caterine", "Celine", "Cesar", "Charles", "Cicero", "Cinthia", "Cirilo", "Claire", "Clark", "Claudia", "Colin", "Conan", "Cristiane", "Daisy", "Dalila", "Daniel", "Daniela", "Darcy", "Davi", "David", "Denis", "Denise", "Diana", "Diego", "Dilbert", "Douglas", "Dumbledore", "Edson", "Eduarda", "Eduardo", "Edward", "Elaine", "Elber", "Elder", "Eliane", "Elias", "Elis", "Eloi", "Emerson", "Erasto", "Erminio", "Erodes", "Euclides", "Eulalia", "Fabiana", "Fabio", "Fabiola", "Fabricio", "Fani", "Felipe", "Fernanda", "Fernando", "Firmino", "Flavia", "Francielder", "Franco", "Frodo", "Gabriel", "Gandalf", "Geraldo", "Gilberto", "Gilmar", "Gimli", "Gloria", "Golias", "Goreti", "Grazielle", "Guilherme", "Guinevere", "Gustavo", "Harry", "Helen", "Helio", "Hermione", "Hiro", "Hugo", "Ian", "Icaro", "Ieda", "Igor", "Iris", "Italo", "Ivo", "Jack", "Jackson", "Janaina", "Jessica", "Jin", "Joana", "João Paulo", "Joilson", "José", "José Ricardo", "Joshua", "Juca", "Julia", "Juliana", "Jussara", "Kadija", "Karina", "Kate", "Katia", "Katy", "Kenia", "Kristy", "Lais", "Lancelot", "Larissa", "Leandro", "Legolas", "Liana", "Lilian", "Lourival", "Luanna", "Lucas", "Luciana", "Luciano", "Ludmila", "Luis", "Luiza", "Maome", "Maradonna", "Marcelo", "Márcia", "Marco Antonio", "Marcos", "Marcy", "Maria Lina", "Mariana", "Marilia", "Marisa", "Marli", "Marta", "Matheus", "Mauricio", "Mauro", "Max", "Melissa", "Merlin", "Merry", "Michael", "Michele", "Milene", "Minerva", "Miriam", "Moises", "Monica", "Natalia", "Nataly", "Nathan", "Nelson", "Nereu", "Noel", "Norton", "Nubia", "Orlando", "Patricia", "Paula", "Paulo", "Pedro", "Penelope", "Peter", "Pippin", "Prista", "Quesia", "Rafael", "Reginaldo", "Regis", "Renata", "Renato", "Rener", "Ricardo", "Rita", "Rivaldo", "Roberto", "Robson", "Ronaldo", "Rony", "Rosalia", "Rosana", "Sabrina", "Salvina", "Sam", "Samantha", "Samir", "Sandra", "Sansao", "Sarah", "Saulo", "Savio", "Sean", "Serafim", "Silas", "Silvio", "Simas", "Simone", "Snape", "Soraia", "Sorato", "Suzy", "Tais", "Taisa", "Talita", "Tania", "Tatiana", "Telmo", "Tereza", "Terry", "Tess", "Thomas", "Thor", "Ticiane", "Tie", "Tom", "Tuco", "Tulio", "Uiara", "Umberto", "Valeria", "Vanessa", "Vinicius", "Vitor", "Vladmir", "Voldemort", "Walter", "Wellington", "Wesley", "William", "Wilson", "Zeus" };

        #endregion


        [TestMethod]
        public void TestGetCustomersByIds()
        {
           
            /// CAMBIAR False por True
            var retorno =  LanzarProceso(usar_dapper: false);

            /// me da igual, quiero que devuelva OK
            Assert.AreEqual(1, 1);

        }
        
        private int LanzarProceso(bool usar_dapper)
        { 
            DataService ds = new DataService();
            int retorno = 0;

            if (usar_dapper)
            {
                #region DAPPER
                // Create the metadata once.  The metadata can contain multiple columns.
                // In this example there's just a single Id (int) column.
                // Basically this just matches the column name and data type of the 
                // SQL TableType variable you created in the database.
                var myMetaData = new SqlMetaData[] { new SqlMetaData("Id", SqlDbType.Int) };

                for (int pruebas = 0; pruebas < NumTests; pruebas++)
                {
                    int rand = new Random(System.DateTime.Now.Millisecond).Next(500, 1000);
                    Random r = new Random(System.DateTime.Now.Millisecond);

                    var ids = new List<SqlDataRecord>();

                    for (int i = 0; i < rand; i++)
                    {
                        var record = new SqlDataRecord(myMetaData);
                        // Set the 1st colunm, i.e., position 0 with the correcponding value:
                        record.SetInt32(0, r.Next(1, 81796));

                        // Add the new row to the table rows array:
                        ids.Add(record);
                    }

                    retorno = ds.TestContainsMethodWithStoredProcedure(ids);
                }
                #endregion
            }
            else // NO USAR DAPPER
            {
                #region ENTITY_FRAMEWORK
                for (int pruebas = 0; pruebas < NumTests; pruebas++)
                {
                    int rand = new Random(System.DateTime.Now.Millisecond).Next(500, 1000);
                    Random r = new Random(System.DateTime.Now.Millisecond);

                    List<int> ids = new List<int>();
                    for (int i = 0; i < rand; i++)
                        ids.Add(r.Next(1, 81796));

                     retorno = ds.TestContainsMethod(ids);
                }
                #endregion
            }

            return (retorno);
        }










        /*

        [TestMethod]
        public void TestGetCustomersPaged()
        {
            DataService ds = new DataService();
            Random r = new Random(System.DateTime.Now.Millisecond);
            for (int test = 0; test < NumTests; test++)
            {
               ds.GetCustomersPaged(r.Next(1, 50000), 8);
               //ds.GetCustomersPagedWithStoredProcedure(r.Next(1, 50000), 8);
            }

                /// me da igual, quiero que devuelva OK
                Assert.AreEqual(1, 1);
        }

        [TestMethod]
        public void TestSearchWithNameUsingNotMappedObject()
        {
            DataService ds = new DataService();
            Random r = new Random(System.DateTime.Now.Millisecond);
            for (int test = 0; test < NumTests; test++)
            {
                CustomerNotMappedInEF c = new CustomerNotMappedInEF();
                c.CustomerName = CustomerNames[r.Next(0, 286)];

                ds.SearchWithNameUsingNotMappedObject(c);
            }

            /// me da igual, quiero que devuelva OK
            Assert.AreEqual(1, 1);
        }
          */
    }
}