using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Kursach
{
    public partial class MainForm : Form
    {

        SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-E9694FA\SQLEXPRESS;Initial Catalog=Kursach;Integrated Security=True;");
        object sqlNameUser = null;
        object sqlRightsUser = 0;

        public object sqlNameUser_
        {
            get { return sqlNameUser; }
            set { sqlNameUser = value; }
        }

        //int LoginLength = 0;
        //int PasswordLength = 0;

        public MainForm()
        {
            InitializeComponent();
            //GetLength();
        }

        //private async Task GetLength()
        //{
        //    await Task.Run(() =>
        //    {
        //        while (true)
        //        {
        //            LoginLength = LoginTextBox.Text.Length;
        //            PasswordLength = PasswordTextBox.Text.Length;
        //            Thread.Sleep(100);
        //        }
        //    });
        //}

        //private async Task Check()
        //{
        //    await Task.Run(() =>
        //    {
        //        while(true)//this.LoginTextBox.Text == String.Empty)
        //        {
        //            if (this.LoginTextBox.Text == String.Empty)
        //            {
        //                this.IsItEmpty.BeginInvoke((MethodInvoker)(() => this.IsItEmpty.Text = "Поле не может быть пустым"));
        //            }
        //            else
        //            {
        //                this.IsItEmpty.BeginInvoke((MethodInvoker)(() => this.IsItEmpty.Text = "*"));
        //            }
        //            if (LoginLength < 3)
        //            {
        //                this.IsItLong.BeginInvoke((MethodInvoker)(() => this.IsItLong.Text = "Количество символов < 3"));
        //            }
        //            else
        //            {
        //                this.IsItLong.BeginInvoke((MethodInvoker)(() => this.IsItLong.Text = "*"));
        //            }
        //        }
        //    });
        //}

        private void SignInButton_Click(object sender, EventArgs e)
        {
            sqlConnection.Open();
            try
            {
                if (!String.IsNullOrWhiteSpace(LoginTextBox.Text) && !String.IsNullOrWhiteSpace(PasswordTextBox.Text))
                {
                    string command = @"INSERT INTO Users (Login, Password, Rights) " + " VALUES (@Login, @Password, @Rights)";
                    //SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO dbo.Users (Login, Password, Rights) VALUE (@Login, @Password, @Rights)");
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandText = command;

                    sqlCommand.Parameters.Add("@Login", SqlDbType.Char).Value = LoginTextBox.Text;
                    sqlCommand.Parameters.Add("@Password", SqlDbType.Char).Value = PasswordTextBox.Text;
                    sqlCommand.Parameters.Add("@Rights", SqlDbType.Int).Value = 0;
                    sqlCommand.ExecuteNonQuery();
                    MessageBox.Show("Пользователь добавлен");
                }
                else
                {
                    MessageBox.Show("Поле(-я) содержат пустые строки");
                }
            }
            catch(System.Data.SqlClient.SqlException)
            {
                MessageBox.Show("Такой пользователь уже существует");
            }
            sqlConnection.Close();
        }

        private void LoginInButton_Click(object sender, EventArgs e)
        {
            sqlConnection.Open();
            string command = @"SELECT Login, Password, Rights FROM Users WHERE Login = '" + LoginTextBox.Text + "' AND Password = '" + PasswordTextBox.Text + "'";
            //SqlCommand sqlCommand = new SqlCommand(@"SELECT Login, Password FROM Users WHERE Login = " + LoginTextBox.Text + "AND Password = " + PasswordTextBox.Text);
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = command;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            sqlDataReader.Read();
            if (sqlDataReader.HasRows)
            {
                sqlNameUser = sqlDataReader.GetValue(sqlDataReader.GetOrdinal("Login"));
                sqlRightsUser = sqlDataReader.GetValue(sqlDataReader.GetOrdinal("Rights"));
                this.Hide();
                WorkForm NewWorkForm = new WorkForm();
                NewWorkForm.sqlNameUser_ = sqlNameUser;
                NewWorkForm.sqlConnection_ = sqlConnection;
                NewWorkForm.sqlRightsUser_ = sqlRightsUser;
                NewWorkForm.Show();
            }
            else
            {
                MessageBox.Show("Пользователь не найден");
            }
            sqlDataReader.Close();
            sqlConnection.Close();
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Browsing_Click(object sender, EventArgs e)
        {
            this.Hide();
            Browsing NewBrowser = new Browsing();
            NewBrowser.sqlConnection_ = sqlConnection;
            NewBrowser.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            PasswordTextBox.PasswordChar = '*';
        }

        //private void LoginTextBox_MouseClick(object sender, MouseEventArgs e)
        //{
        //    Check();
        //}
    }
}
