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
using System.Data.Common;
using System.Globalization;


namespace SQLforTelegramBot
{
    public partial class Form1 : Form
    {
        private SqlConnection sqlConnection = null;
        public Form1()
        {
            InitializeComponent();
        }

        private List<string[]> rows = new List<string[]>();
        private List<string[]> filteredList = null;

        private List<string[]> news = new List<string[]>();
        private List<string[]> messages = new List<string[]>();
        private List<string[]> trainers = new List<string[]>();

        private void Form1_Load_1(object sender, EventArgs e)
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["TestDB"].ConnectionString);

            sqlConnection.Open();

            if (sqlConnection.State == ConnectionState.Open)
            {
                MessageBox.Show("Подключение установлено!");
            }


            SqlDataReader dataReader = null;
            string[] row = null;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT Id, Name, Surname, NumberOfPhone, StartDay, EndDay, GymMembership, NumberOfCard FROM Clients", sqlConnection);

                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    row = new string[]
                    {
                    Convert.ToString(dataReader["Id"]) ,
                    Convert.ToString(dataReader["Name"]) ,
                    Convert.ToString(dataReader["Surname"]),
                    Convert.ToString(dataReader["NumberOfPhone"]),
                    Convert.ToString(dataReader["StartDay"]),
                    Convert.ToString(dataReader["EndDay"]),
                    Convert.ToString(dataReader["GymMembership"]),
                    Convert.ToString(dataReader["NumberOfCard"])};

                    rows.Add(row);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
            RefreshList(rows, listView2);

        }
        private void RefreshList(List<string[]> data, ListView listView)
        {
            listView.Items.Clear();
            foreach (var item in data)
            {
                listView.Items.Add(new ListViewItem(item));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            SqlCommand command = new SqlCommand($"INSERT INTO [Clients] (Name, Surname, NumberOfPhone, StartDay, EndDay, GymMembership, NumberOfCard) VALUES (N'{textBox1.Text}', N'{textBox2.Text}', '{textBox3.Text}', '{DateTime.ParseExact(textBox4.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")}', '{DateTime.ParseExact(textBox5.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")}', N'{textBox6.Text}', '{textBox7.Text}')", sqlConnection);
            MessageBox.Show(command.ExecuteNonQuery().ToString());
            MessageBox.Show("Клиент успешно добавлен в базу данных!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand($"INSERT INTO [NewsOfGym] (news) VALUES (N'{richTextBox1.Text}')", sqlConnection);
            MessageBox.Show(command.ExecuteNonQuery().ToString());
            MessageBox.Show("Новости успешно добавлены!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string commandText = @"C:\Users\user\source\repos\SQLforTelegramBot\SQLforTelegramBot\Help.chm";
            var proc = new System.Diagnostics.Process();
            proc.StartInfo.FileName = commandText;
            proc.StartInfo.UseShellExecute = true;
            proc.Start();


        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand($"INSERT INTO [MotivationalMessages] (message) VALUES (N'{richTextBox2.Text}')", sqlConnection);
            MessageBox.Show(command.ExecuteNonQuery().ToString());
            MessageBox.Show("Цитата успешно добавлена!");
        }



        //private void button6_Click(object sender, EventArgs e)
        //{
        //    listView1.Items.Clear();

        //    SqlDataReader dataReader = null;

        //    try
        //    {
        //        SqlCommand sqlCommand = new SqlCommand("SELECT Name, Surname, NumberOfPhone, StartDay, EndDay, GymMembership, NumberOfCard FROM Clients", sqlConnection);

        //        dataReader = sqlCommand.ExecuteReader();

        //        ListViewItem item = null;
        //        while (dataReader.Read())
        //        {
        //            item = new ListViewItem(new string[] { Convert.ToString(dataReader["Name"]) ,
        //            Convert.ToString(dataReader["Surname"]),
        //            Convert.ToString(dataReader["NumberOfPhone"]),
        //            Convert.ToString(dataReader["StartDay"]),
        //            Convert.ToString(dataReader["EndDay"]),
        //            Convert.ToString(dataReader["GymMembership"]),
        //            Convert.ToString(dataReader["NumberOfCard"])});

        //            listView1.Items.Add(item);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {
        //        if (dataReader != null && !dataReader.IsClosed)
        //        {
        //            dataReader.Close();
        //        }
        //    }
        //}

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            filteredList = rows.Where((x) =>
            x[0].ToLower().Contains(textBox8.Text.ToLower()) || x[1].ToLower().Contains(textBox8.Text.ToLower())).ToList();

            RefreshList(filteredList, listView2);
        }

        private void button5_Click(object sender, EventArgs e)
        {

            // Получаем значения из текстовых полей
            string clientId = textBox9.Text;
            string name = textBox10.Text;
            string surname = textBox11.Text;
            string numberOfPhone = textBox12.Text;
            string startDay = textBox13.Text;
            string endDay = textBox14.Text;
            string gymMembership = textBox15.Text;
            string numberOfCard = textBox16.Text;

            // Формируем запрос на обновление данных в базе
            string updateQuery = "UPDATE Clients SET ";
            bool needComma = false;

            if (!string.IsNullOrEmpty(name))
            {
                updateQuery += "Name = N'" + name + "'";
                needComma = true;
            }
            if (!string.IsNullOrEmpty(surname))
            {
                if (needComma) updateQuery += ", ";
                updateQuery += "Surname = N'" + surname + "'";
                needComma = true;
            }
            if (!string.IsNullOrEmpty(numberOfPhone))
            {
                if (needComma) updateQuery += ", ";
                updateQuery += "NumberOfPhone = N'" + numberOfPhone + "'";
                needComma = true;
            }
            if (!string.IsNullOrEmpty(startDay))
            {
                if (needComma) updateQuery += ", ";
                updateQuery += "StartDay = N'" + startDay + "'";
                needComma = true;
            }
            if (!string.IsNullOrEmpty(endDay))
            {
                if (needComma) updateQuery += ", ";
                updateQuery += "EndDay = N'" + endDay + "'";
                needComma = true;
            }
            if (!string.IsNullOrEmpty(gymMembership))
            {
                if (needComma) updateQuery += ", ";
                updateQuery += "GymMembership = N'" + gymMembership + "'";
                needComma = true;
            }
            if (!string.IsNullOrEmpty(numberOfCard))
            {
                if (needComma) updateQuery += ", ";
                updateQuery += "NumberOfCard = N'" + numberOfCard + "'";
                needComma = true;
            }


            updateQuery += " WHERE Id = " + clientId;

            // Выполняем запрос на обновление данных в базе
            try
            {
                SqlCommand sqlCommand = new SqlCommand(updateQuery, sqlConnection);
                sqlCommand.ExecuteNonQuery();
                MessageBox.Show("Данные успешно обновлены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении данных: " + ex.Message);
            }
            RefreshList(rows, listView2);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            string clientId = textBox9.Text;

            if (MessageBox.Show("Вы действительно хотите удалить клиента?", "Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string deleteQuery = "DELETE FROM Clients WHERE Id = " + clientId;

                try
                {

                    SqlCommand sqlcommand = new SqlCommand(deleteQuery, sqlConnection);
                    int rowsAffected = sqlcommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Клиент с идентификатором " + clientId + " успешно удален.");
                    }
                    else
                    {
                        MessageBox.Show("Клиент с идентификатором " + clientId + " не найден.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении клиента: " + ex.Message);
                }
            }
        }



        private void button7_Click(object sender, EventArgs e)
        {
            SqlDataReader dataReader = null;
            string[] new1 = null;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT Id, news FROM NewsOfGym", sqlConnection);

                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    new1 = new string[]
                    {
                    Convert.ToString(dataReader["Id"]) ,
                    Convert.ToString(dataReader["news"]) ,
                   };

                    news.Add(new1);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
            RefreshList(news, listView1);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            SqlDataReader dataReader = null;
            string[] new1 = null;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT Id, message FROM MotivationalMessages", sqlConnection);

                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    new1 = new string[]
                    {
                    Convert.ToString(dataReader["Id"]),
                    Convert.ToString(dataReader["message"]),
                   };

                    messages.Add(new1);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
            RefreshList(messages, listView3);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string newsId = textBox17.Text;

            if (MessageBox.Show("Вы действительно хотите удалить новость?", "Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string deleteQuery = "DELETE FROM NewsOfGym WHERE Id = " + newsId;

                try
                {

                    SqlCommand sqlcommand = new SqlCommand(deleteQuery, sqlConnection);
                    int rowsAffected = sqlcommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Сообщение с идентификатором " + newsId + " успешно удалено.");
                    }
                    else
                    {
                        MessageBox.Show("Сообщение с идентификатором " + newsId + " не найдено.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении сообщения: " + ex.Message);
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string messageId = textBox18.Text;

            if (MessageBox.Show("Вы действительно хотите удалить цитату?", "Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string deleteQuery = "DELETE FROM MotivationalMessages WHERE Id = " + messageId;

                try
                {

                    SqlCommand sqlcommand = new SqlCommand(deleteQuery, sqlConnection);
                    int rowsAffected = sqlcommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Сообщение с идентификатором " + messageId + " успешно удалено.");
                    }
                    else
                    {
                        MessageBox.Show("Сообщение с идентификатором " + messageId + " не найдено.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении сообщения: " + ex.Message);
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            SqlDataReader dataReader = null;
            string[] trainer = null;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("SELECT ScheduleId, TrainerName, DayOfWeek, StartTime, EndTime FROM PersonalTrainersSchedule", sqlConnection);

                dataReader = sqlCommand.ExecuteReader();

                while (dataReader.Read())
                {
                    trainer = new string[]
                    {
                    Convert.ToString(dataReader["ScheduleId"]),
                    Convert.ToString(dataReader["TrainerName"]),
                     Convert.ToString(dataReader["DayOfWeek"]),
                    Convert.ToString(dataReader["StartTime"]),
                     Convert.ToString(dataReader["EndTime"]),
                   };

                    trainers.Add(trainer);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (dataReader != null && !dataReader.IsClosed)
                {
                    dataReader.Close();
                }
            }
            RefreshList(trainers, listView4);
        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {
            filteredList = trainers.Where((x) =>
            x[0].ToLower().Contains(textBox19.Text.ToLower()) || x[1].ToLower().Contains(textBox19.Text.ToLower())).ToList();

            RefreshList(filteredList, listView4);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand($"INSERT INTO [PersonalTrainersSchedule] (TrainerName, DayOfWeek, StartTime, EndTime, IdOfTrainer) VALUES (N'{textBox31.Text}', N'{textBox30.Text}', '{textBox29.Text}', '{textBox28.Text}', '{textBox25.Text}')", sqlConnection);
            MessageBox.Show(command.ExecuteNonQuery().ToString());
            MessageBox.Show("Расписание добавлено!");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // Получаем значения из текстовых полей
            string trainerId = textBox20.Text;
            string name = textBox21.Text;
            string day = textBox22.Text;
            string start = textBox23.Text;
            string end = textBox24.Text;
            string password = textBox26.Text;

            // Формируем запрос на обновление данных в базе
            string updateQuery = "UPDATE PersonalTrainersSchedule SET ";
            bool needComma = false;

            if (!string.IsNullOrEmpty(name))
            {
                updateQuery += "TrainerName = N'" + name + "'";
                needComma = true;
            }
            if (!string.IsNullOrEmpty(day))
            {
                if (needComma) updateQuery += ", ";
                updateQuery += "DayOfWeek = N'" + day + "'";
                needComma = true;
            }
            if (!string.IsNullOrEmpty(start))
            {
                if (needComma) updateQuery += ", ";
                updateQuery += "StartTime = N'" + start + "'";
                needComma = true;
            }
            if (!string.IsNullOrEmpty(end))
            {
                if (needComma) updateQuery += ", ";
                updateQuery += "EndTime = N'" + end + "'";
                needComma = true;
            }
            if (!string.IsNullOrEmpty(password))
            {
                if (needComma) updateQuery += ", ";
                updateQuery += "IdOfTrainer = N'" + password + "'";
                needComma = true;
            }


            updateQuery += " WHERE ScheduleId = " + trainerId;

            // Выполняем запрос на обновление данных в базе
            try
            {
                SqlCommand sqlCommand = new SqlCommand(updateQuery, sqlConnection);
                sqlCommand.ExecuteNonQuery();
                MessageBox.Show("Данные успешно обновлены!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обновлении данных: " + ex.Message);
            }
            RefreshList(trainers, listView4);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string trainerId = textBox20.Text;

            if (MessageBox.Show("Вы действительно хотите удалить расписание?", "Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string deleteQuery = "DELETE FROM PersonalTrainersSchedule WHERE ScheduleId = " + trainerId;

                try
                {

                    SqlCommand sqlcommand = new SqlCommand(deleteQuery, sqlConnection);
                    int rowsAffected = sqlcommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Клиент с идентификатором " + trainerId + " успешно удален.");
                    }
                    else
                    {
                        MessageBox.Show("Клиент с идентификатором " + trainerId + " не найден.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении клиента: " + ex.Message);
                }
            }
        }

        
    }
}
