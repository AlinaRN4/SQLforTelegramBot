using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace SQLforTelegramBot
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TestDB"].ConnectionString);

            sqlConnection.Open();

            if (sqlConnection.State == ConnectionState.Open)
            {
                MessageBox.Show("Подключение установлено!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand($"INSERT INTO [Clients] (Name, Surname, NumberOfPhone, StartDay, EndDay, GymMembership, NumberOfCard) VALUES (N'{textBox1.Text}', N'{textBox2.Text}', '{textBox3.Text}', '{textBox4.Text}', '{textBox5.Text}', N'{textBox6.Text}', '{textBox7.Text}')", sqlConnection);
            MessageBox.Show(command.ExecuteNonQuery().ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand($"INSERT INTO [NewsOfGym] (news) VALUES (N'{richTextBox1.Text}')", sqlConnection);
            MessageBox.Show(command.ExecuteNonQuery().ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string commandText = @"C:\Users\user\source\repos\SQLforTelegramBot\SQLforTelegramBot\Help.chm";
            var proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = commandText;
            proc.StartInfo.UseShellExecute = true;
            proc.Start();


        }
    }
}
