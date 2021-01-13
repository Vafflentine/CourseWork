using System;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
namespace PC_Monitor
{
    class SQLHelper
    {
        public SQLHelper() { }

        public void WriteToDataBase(string sensor, float current, float expected, DateTime date)
        {
            string connStr = @"Data Source=WIN-42VS6RNEHRF\SQLEXPRESS;
                            Initial Catalog=PC_Monitor;
                            Integrated Security=True";

            SqlConnection conn = new SqlConnection(connStr);
            try
            {
                conn.Open();
            }
            catch (SqlException se)
            {
                MessageBox.Show("Ошибка подключения:{0}", se.Message);
                return;
            }
            /*Создание экземпляра класса  SqlCommand и передача запрос на 
              добавление строки в таблицу Students и объект SqlConnection */
            SqlCommand cmd = new SqlCommand("Insert into CriticalInfo" +
                "(Sensor,CurrentValue,ExpectedValue,LoggedDate) Values (@Sensor,@CurrentValue,@ExpectedValue,@LoggedDate)", conn);
            /*Работа с параметрами(SqlParameter), эта техника позволяет 
		уменьшить количество ошибок и повысить быстродействие, но 
		требует и больших усилий в написании кода */
            //объявляем объект класса SqlParameter
            SqlParameter param = new SqlParameter();
            //имя параметра
            param.ParameterName = "@Sensor";
            //значение параметра
            param.Value = sensor;
            //тип параметра
            param.SqlDbType = SqlDbType.VarChar;
            //параметр объекту класса SqlCommand
            cmd.Parameters.Add(param);
            //переопределение объекта класса SqlParameter
            param = new SqlParameter();
            //задаем имя параметра
            param.ParameterName = "@CurrentValue";
            //задаем значение параметра
            param.Value = current;
            //задаем тип параметра
            param.SqlDbType = SqlDbType.Float;
            //передаем параметр объекту класса SqlCommand
            cmd.Parameters.Add(param);
            //переопределяем объект класса SqlParameter
            param = new SqlParameter();
            //задаем имя параметра
            param.ParameterName = "@ExpectedValue";
            //задаем значение параметра
            param.Value = expected;
            //задаем тип параметра
            param.SqlDbType = SqlDbType.Float;
            //передаем параметр объекту класса SqlCommand
            cmd.Parameters.Add(param);
            param = new SqlParameter();
            //задаем имя параметра
            param.ParameterName = "@LoggedDate";
            //задаем значение параметра
            param.Value = date;
            //задаем тип параметра
            param.SqlDbType = SqlDbType.SmallDateTime;
            //передаем параметр объекту класса SqlCommand
            cmd.Parameters.Add(param);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Ошибка запроса на добавление записи");
                return;
            }
            conn.Close();
            conn.Dispose();
        }
    }
}

