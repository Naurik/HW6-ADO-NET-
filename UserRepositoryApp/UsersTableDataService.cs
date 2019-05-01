using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserRepositoryApp
{
    public class UsersTableDataService
    {
        private readonly string connectionString;
        private readonly string providerName;
        private readonly DbProviderFactory providerFactory;
        public UsersTableDataService()
        {
            connectionString = ConfigurationManager.ConnectionStrings["UserRepositoryApp.Properties.Settings.UserRepositoryApp_DataContextConnectionString"].ConnectionString;
            providerName = ConfigurationManager.ConnectionStrings["UserRepositoryApp.Properties.Settings.UserRepositoryApp_DataContextConnectionString"].ProviderName;
            providerFactory = DbProviderFactories.GetFactory(providerName);
        }

        #region Проверка на доступность введенных данных
        public bool CheckForAvailability(string property, object data, ListBox listBox)
        {
            using (var connection = providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    command.CommandText = $"Select {property} from Users where {property} = '{data}'";

                    if (command.ExecuteScalar() == null)
                        return false;
                }
                catch (DbException exception)
                {
                    listBox.Items.Add(exception.Message);
                }
                catch (Exception exception)
                {
                    listBox.Items.Add(exception.Message);
                }
            }
            return true;
        }
        #endregion

        #region Вывод всех пользователей
        public List<User> ShowAllUsers(bool isAdmin = false, string login = "")
        {
            List<User> data = new List<User>();

            //if (listBox.Items != null)
            //    listBox.Items.Clear();
            using (var connection = providerFactory.CreateConnection())
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.ConnectionString = connectionString;
                    connection.Open();
                    if (login.Length > 0)
                        command.CommandText = $"SELECT * FROM Users where Login = '{login}'";
                    else
                        command.CommandText = $"SELECT * FROM Users where IsAdmin = '{isAdmin}'";

                    if (command.ExecuteScalar() == null && login.Length > 0)
                    {
                        MessageBox.Show("Такого пользователя не существует.");
                    }

                    var dataReader = command.ExecuteReader();
                    
                    while (dataReader.Read())
                    {
                        Guid id = (Guid)dataReader["Id"];
                        string login_ = dataReader["Login"].ToString();
                        string password = dataReader["Password"].ToString();
                        string address = dataReader["Address"].ToString();
                        string phone = dataReader["Phone"].ToString();
                        bool isAdmin_ = (bool)dataReader["IsAdmin"];
                        
                        data.Add(new User
                        {
                            Id = id,
                            Login = login_,
                            Password = password,
                            Address = address,
                            Phone = phone,
                            IsAdmin = isAdmin_
                        });
                    }
                }
                catch (DbException exception)
                {
                    MessageBox.Show(exception.Message, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return data;
        }
        #endregion
    }
}
