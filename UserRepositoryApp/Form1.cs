using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserRepositoryApp
{
    public partial class Form1 : Form
    {
        private readonly UsersTableDataService service;
        private List<User> users;

        public Form1()
        {
            InitializeComponent();
            service = new UsersTableDataService();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddingForm form = new AddingForm();
            form.ShowDialog();
        }

        private void tBoxLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !(string.IsNullOrEmpty(tBoxLogin.Text)) && !(string.IsNullOrWhiteSpace(tBoxLogin.Text)))
            {
                users = service.ShowAllUsers(login : tBoxLogin.Text);
                listBoxUsers.DataSource = users;
                listBoxUsers.DisplayMember = "Login";
            }
        }

        private void buttonShowAll_Click(object sender, EventArgs e)
        {
            if (checkBoxAdmin.Checked)
            {
                users = service.ShowAllUsers(true);
                if (users.Count == 0)
                    MessageBox.Show("В базе нету администраторов.");
            }
            else
            {
                users = service.ShowAllUsers();
                if (users.Count == 0)
                    MessageBox.Show("В базе нету пользователей.");
            }
            listBoxUsers.DataSource = users;
            listBoxUsers.DisplayMember = "Login";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBoxUsers.SelectedIndex < 0)
            {
                MessageBox.Show("Выберите пользователя.");
                return;
            }
            User user = users[listBoxUsers.SelectedIndex];
            try
            {
                using (ContextUser context = new ContextUser())
                {
                    context.Users.Attach(user);
                    context.Users.Remove(user);
                    context.SaveChanges();
                    MessageBox.Show("Успешно удален!");
                };
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                listBoxUsers.DataSource = null;
                listBoxUsers.Items.Clear();
            }
        }

        private void listBoxUsers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            AddingForm form = new AddingForm();
            User user = users[listBoxUsers.SelectedIndex];
            form.EditingMethod(user);
            form.ShowDialog();
        }
    }
}
