using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WaterCanal
{
    class Base
    {
        private static string connStr = "server=localhost;user=vladbright;database=water;password=vladbright;";
        private static MySqlConnection connectionToBase;
        private static MySqlCommand command;
        private MySqlDataReader readerBaseContent;
        private string[] customersList;

        public static void connection() {
            connectionToBase = new MySqlConnection(connStr);
            connectionToBase.Open();
        }

        public static string connectionString()
        {
            return connStr;
        }

        public static void breakConnection()
        {
            if(connectionToBase != null)
                connectionToBase.Close();
        }

        //deprecated
        
        public string[] getBaseContent(string requestContentSQL)
        {
            connection();
            if (connectionToBase != null)
            {
                command = new MySqlCommand(requestContentSQL, connectionToBase);
                
                readerBaseContent = command.ExecuteReader();
                int count = 0;
                while (readerBaseContent.Read())
                {
                    count++;
                }

                readerBaseContent.Close();
                readerBaseContent = command.ExecuteReader();
                customersList = new string[count];
                count = 0;
                while (readerBaseContent.Read())
                {
                    customersList[count] = readerBaseContent[0].ToString() + ". " + readerBaseContent[1].ToString() 
                        + ". Address: \"" + readerBaseContent[2].ToString() + "\". Need to pay: " + readerBaseContent[3].ToString() + " $";
                    Console.WriteLine(customersList[count]);
                    count++;
                }
                readerBaseContent.Close();
            }
            return customersList;
        }

        public string[] getCustomerList()
        {
            return customersList;
        }
        

        public static void insertCustomer(string id, string lastName, string address, int summery)
        {
            try
            {
                connection();
                string insertSQL = "INSERT INTO customers(id, LAST_NAME, ADRESS, SUMMERY) VALUES (@id, @LAST_NAME, @ADRESS, @SUMMERY)";
                command = new MySqlCommand(insertSQL, connectionToBase);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@LAST_NAME", lastName);
                command.Parameters.AddWithValue("@ADRESS", address);
                command.Parameters.AddWithValue("@SUMMERY", summery);
                command.ExecuteNonQuery();
                MessageBox.Show("Customer have successfully added");
            }
            catch(Exception) {
                MessageBox.Show("Customer did not add! Incorrect data");
            }
            finally {
                breakConnection();
            }
            
        }

        public static void updateCustomer(string id, string lastName, string address, int summery)
        {
            string updateSQL = "UPDATE water.customers SET LAST_NAME = '" + lastName + "', ADRESS = '" + address + "', SUMMERY = " + summery + " WHERE id =" + id;
           
            try
            {
                connection();
                command = new MySqlCommand(updateSQL, connectionToBase);
                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Information have successfully updated.");
                }
                else
                {
                    MessageBox.Show("Update is failed!\n No such element at the base.");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Customer did not edit! Incorrect data.");
            }
            finally
            {
                breakConnection();
            }
        }

        public static void removeCustomer(string id)
        {
            string removeSQL = "DELETE FROM water.customers WHERE id =" + id;

            try
            {
                connection();
                command = new MySqlCommand(removeSQL, connectionToBase);
                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Customer with id "+ id +" successfully removed.");
                }
                else
                {
                    MessageBox.Show("Removing is failed!\nNo such element at the base.");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Customer did not remove! Incorrect data.");
            }
            finally
            {
                breakConnection();
            }

        }

        public static DataTable searchCustomer(string id, string lastName, string address, string summery, string signSummery = "=")
        {
            string searchSQL = "SELECT * FROM customers ";
            int[] mas = new int[4] {0,0,0,0};
            string[] masStr = new string[4] {id,lastName,address,""+summery};

            if (!id.Equals(""))
                mas[0] = 1;
            if (!lastName.Equals(""))
                mas[1] = 1;
            if (!address.Equals(""))
                mas[2] = 1;
            if (!summery.Equals(""))
                mas[3] = 1;

            bool flag = false;
            for(int i = 0; i < mas.Length; i++)
            {
                if(mas[i] == 1)
                {
                    if (flag)
                    {
                        searchSQL += " AND ";
                    }
                    else
                    {
                        searchSQL += "WHERE ";
                    }
                    if(i == 0)
                        searchSQL += "id = " + masStr[i];
                    if (i == 1)
                        searchSQL += "LAST_NAME = \'" + masStr[i] + "\'";
                    if (i == 2)
                        searchSQL += "ADRESS = \'" + masStr[i] + "\'";
                    if (i == 3)
                        if(signSummery.Equals("="))
                            searchSQL += "SUMMERY = " + Convert.ToInt32(masStr[i]);
                        else if(signSummery.Equals("<"))
                            searchSQL += "SUMMERY < " + Convert.ToInt32(masStr[i]);
                        else if(signSummery.Equals(">"))
                            searchSQL += "SUMMERY > " + Convert.ToInt32(masStr[i]);
                    flag = true;
                }
            }

            Console.WriteLine(searchSQL);

            
            DataTable dataTable = new DataTable();
            try
            {
                connection();
                MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(searchSQL, connectionString());
                sqlDataAdapter.Fill(dataTable);
            }
            catch (Exception)
            {
                MessageBox.Show("Customer(s) did not find! Incorrect data.");
            }
            finally
            {
                breakConnection();
            }
            return dataTable;
        }
        
        public static DataTable upDateBase()
        {
            connection();
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter("SELECT * FROM customers", connectionString());
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            breakConnection();
            return dataTable;
        }

        public static DataTable customersWithAdvance(string str) //с авансом
        {
            connection();
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter("SELECT * FROM customers WHERE SUMMERY < " + str, connectionString());
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            breakConnection();
            return dataTable;
        }

        public static DataTable customersWithOutDebt() //без долга
        {
            connection();
            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter("SELECT * FROM customers WHERE SUMMERY < 1", connectionString());
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            breakConnection();
            return dataTable;
        }
    }
}
