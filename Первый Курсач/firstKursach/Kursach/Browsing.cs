using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using Bunifu.Framework.UI;
using Microsoft.Office.Interop.Excel;

namespace Kursach
{
    public partial class Browsing : Form
    {
        object SqlNameUser = null;

        SqlConnection sqlConnection = null;
        public object sqlNameUser_
        {
            get { return SqlNameUser; }
            set { SqlNameUser = value; }
        }

        public SqlConnection sqlConnection_
        {
            get { return sqlConnection; }
            set { sqlConnection = value; }
        }

        public Browsing()
        {
            InitializeComponent();

        }

        private void Browser_Load(object sender, EventArgs e)
        {
            //TODO: данная строка кода позволяет загрузить данные в таблицу "kursachDataSet.PhoneBook".При необходимости она может быть перемещена или удалена.
            this.phoneBookTableAdapter.Fill(this.kursachDataSet.PhoneBook);
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void BackWorkForm_Click(object sender, EventArgs e)
        {
            if (SqlNameUser != null)
            {
                this.Hide();
                WorkForm NewWorkForm = new WorkForm();
                NewWorkForm.sqlNameUser_ = SqlNameUser;
                NewWorkForm.Show();
            }
            else
            {
                this.Hide();
                MainForm NewMainForm = new MainForm();
                NewMainForm.Show();
            }
        }


        private void PrintButton_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook workbook = ExcelApp.Workbooks.Add(XlSheetType.xlWorksheet);
            Worksheet ws = (Worksheet)ExcelApp.ActiveSheet;
            ws.StandardWidth = 15;
            ExcelApp.Visible = true;
            ws.Cells[1, 1] = "First name";
            ws.Cells[1, 2] = "Last name";
            ws.Cells[1, 3] = "Telephone number";
            ws.Cells[1, 4] = "City";
            ws.Cells[1, 5] = "Street";
            ws.Cells[1, 6] = "House";
            ws.Cells[1, 7] = "Apartment";
            for (int i = 2; i <= bunifuCustomDataGrid1.Rows.Count + 1; i++)
            {
                for (int j = 1; j <= 7; j++)
                {
                    ws.Cells[i, j] = bunifuCustomDataGrid1.Rows[i - 2].Cells[j - 1].Value;
                }
            }
        }

        private void SerchNumber_Click(object sender, EventArgs e)
        {
            phoneBookBindingSource.Filter = "[TelephoneNumber] LIKE '%" + TelephoneNumberTxt.Text + "%'";
            TelephoneNumberTxt.Text = null;
        }

        private void SerchCity_Click(object sender, EventArgs e)
        {
            phoneBookBindingSource.Filter = "[City] LIKE '%" + CityTxtBox.Text + "%'";
            CityTxtBox.Text = null;
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            phoneBookBindingSource.Filter = "[City] LIKE '%%'";
        }

        private void TelephoneNumberTxt_Click(object sender, EventArgs e)
        {
            TelephoneNumberTxt.SelectionStart = TelephoneNumberTxt.Text.Length;
        }
    }
}