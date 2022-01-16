using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktopapplication
{
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection("Data Source=.\\SQLEXPRESS;Initial Catalog=simpleform;Integrated Security=true;");
        SqlCommand cmd;
        SqlDataAdapter adapt;


        public Form1()
        {
            InitializeComponent();
        }
        public void ClearData()
        {
            txt_Name.Text = "";
            txt_State.Text = "";
            ibl_id.Text = "";
        }
        public void DisplayData()
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select * from simpletable", con);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txt_Name.Text != "" && txt_State.Text != "")
            {
                cmd = new SqlCommand("insert into simpletable(name,location) values(@name,@location)", con);
                con.Open();
                cmd.Parameters.AddWithValue("@name", txt_Name.Text);
                cmd.Parameters.AddWithValue("@location", txt_State.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Record Inserted Successfully");
                DisplayData();
                ClearData();
            }
            else
            {
                MessageBox.Show("Please Provide Details!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txt_Name.Text != "" && txt_State.Text != "")
            {
                cmd = new SqlCommand("update simpletable set name=@name,location=@location where id=@id", con);
                con.Open();
                cmd.Parameters.AddWithValue("@id", ibl_id.Text);
                cmd.Parameters.AddWithValue("@name", txt_Name.Text);
                cmd.Parameters.AddWithValue("@location", txt_State.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Updated Successfully");
                con.Close();
                DisplayData();
                ClearData();
            }
            else
            {
                MessageBox.Show("Please Select Record to Update");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand("delete simpletable where id=@id", con);
            con.Open();
            cmd.Parameters.AddWithValue("@id", ibl_id.Text);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Record Deleted Successfully!");
            DisplayData();
            ClearData();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                ibl_id.Text = row.Cells[0].Value.ToString();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateDatabase();
            DisplayData();
        }
        //Initization of the connectionString
        private string ConnectionString = "Data Source=.\\SQLEXPRESS;initial Catalog=master;Integrated Security=true;";
        private SqlCommand comd = null;
        private SqlConnection conn = null;
        //create the function for database
        public void CreateDatabase()
        {
            string dir = "C:\\simpleform";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                conn = new SqlConnection(ConnectionString);
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                string sql = @"create database simpleform on primary (Name=simpleform_data,filename='C:\\simpleform\\simpleform_data.mdf',size=5,maxsize=20,filegrowth=1)";
                comd = new SqlCommand(sql, conn);
                comd.ExecuteNonQuery();
                conn.Close();
                conn.ConnectionString = "Data Source=.\\SQLEXPRESS;initial Catalog=simpleform;Integrated Security=true;";
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                sql= @"CREATE TABLE [dbo].[simpletable](
                         [id] [int] IDENTITY(1,1) NOT NULL,
                         [name] [varchar](50) NOT NULL,
                         [location] [varchar](50) NOT NULL,
                          CONSTRAINT [PK_simpletable] PRIMARY KEY CLUSTERED
                         (
                         [id] ASC
                         )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                         ) ON [PRIMARY]";
                comd = new SqlCommand(sql, conn);
                comd.ExecuteNonQuery();

            }
        }
        
    }
}
