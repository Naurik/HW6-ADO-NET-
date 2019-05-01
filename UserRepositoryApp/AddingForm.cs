using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data.Entity.Migrations;

namespace UserRepositoryApp
{
    public partial class AddingForm : Form
    {
        private readonly UsersTableDataService service;
        private User user;
        public AddingForm()
        {
            InitializeComponent();
            service = new UsersTableDataService();
            user = new User();
        }
        
        private void buttonSave_Click(object sender, EventArgs e)
        {
            AddingMethod();
        }
        
        private void AddingMethod(bool check = true)
        {
            if (listBoxErrors.Items != null)
                listBoxErrors.Items.Clear();

            user.Login = textBoxAddLogin.Text;

            Regex regex = new Regex(@"\W|[А-Яа-я]+");
            MatchCollection matches = regex.Matches(user.Login);
            if (matches.Count > 0)
            {
                listBoxErrors.Items.Add("Логин содержит недопустимые символы!");
            }

            var results = new List<ValidationResult>();
            var validationContext = new ValidationContext(user);
            if (!Validator.TryValidateObject(user, validationContext, results, true))
            {
                foreach (var error in results)
                {
                    listBoxErrors.Items.Add(error.ErrorMessage);
                }
            }
            if (service.CheckForAvailability("Login", user.Login, listBoxErrors) && check)
            {
                listBoxErrors.Items.Add("Логин уже занят!");
            }

            string pattern = @"(?=^.{6,32}$)((?=.*\d)(?=.*\W+))(?![.\n])(?=.*[A-Z])(?=.*[a-z]).*$";

            if (textBoxPassword.Text.Length < 6 || textBoxPassword.Text.Length > 32)
                listBoxErrors.Items.Add("\nДлина пароля должна быть не меньше 6 символов и не больше 32 символов.");
            else if (!(Regex.IsMatch(textBoxPassword.Text, pattern)))
                listBoxErrors.Items.Add("\nПароль должен содержать цифровой и спец символы, а также буквы верхнего, нижнего регистра.");

            user.Password = textBoxPassword.Text.GetHashCode().ToString();
            string PasswordCopy = textBoxConfirm.Text.GetHashCode().ToString();

            if (user.Password != PasswordCopy)
            {
                listBoxErrors.Items.Add("Пароли не совпадают.");
            }

            user.Address = textBoxAddress.Text;
            if (string.IsNullOrEmpty(user.Address) || string.IsNullOrWhiteSpace(user.Address))
                listBoxErrors.Items.Add("Поле адреса пустое, заполните пожалуйста.");

            user.Phone = textBoxPhone.Text;

            string pattern2 = @"^\+?[7]\d{10}$";

            if (!(Regex.IsMatch(user.Phone, pattern2, RegexOptions.IgnoreCase)))
            {
                listBoxErrors.Items.Add("Телефонный номер содержит недопустимые символы или введен не корректно.");
            }
            if (service.CheckForAvailability("Phone", user.Phone, listBoxErrors) && check)
            {
                listBoxErrors.Items.Add("Номер в базе существует.");
            }

            string isAdmin = textBoxAdmin.Text;
            if (isAdmin == "Да")
                user.IsAdmin = true;
            else if (isAdmin == "Нет")
                user.IsAdmin = false;
            else
                listBoxErrors.Items.Add("Признак админа указан не корректно.");

            if (listBoxErrors.Items.Count == 0)
            {
                try
                {
                    using (ContextUser context = new ContextUser())
                    {
                        context.Users.AddOrUpdate(user);
                        context.SaveChanges();
                    };
                    MessageBox.Show("Успешно сохранено", "Информация", MessageBoxButtons.OK);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Close();
                }
                
            }
        }
        public void EditingMethod(User user)
        {
            this.user = user;
            textBoxAddLogin.Text = user.Login;
            textBoxPhone.Text = user.Phone;
            textBoxAddress.Text = user.Address;
            if (user.IsAdmin)
            {
                textBoxAdmin.Text = "Да";
            }
            else
            {
                textBoxAdmin.Text = "Нет";
            }
            buttonEditing.Visible = true;
            buttonSave.Visible = false;
        }

        private void AddingForm_Load(object sender, EventArgs e)
        {
            
        }

        private void buttonEditing_Click(object sender, EventArgs e)
        {
            AddingMethod(false);
        }
    }
}
