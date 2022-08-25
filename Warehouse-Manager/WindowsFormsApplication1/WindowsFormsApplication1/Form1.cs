using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        SqlConnection connection;
        SqlDataAdapter DataAdapter1, DataAdapter2, DataAdapter3, sqlAdapter, sqlAdapter2;
        DataSet Dataset1, Dataset2, Dataset3;
        BindingSource BindingSource1, BindingSource2, BindingSource3;
        String database, host , query;
        SqlCommand command;

        public Form1()
        {
            InitializeComponent();
            connection = new SqlConnection(@"Data Source=GIWRGOS;Initial Catalog=APOTHIKI_4679;Integrated Security=True");
            connection.Open();

            DataAdapter1 = new SqlDataAdapter("Select * from PELATHS", connection);
            DataTable dt1 = new DataTable();
            DataAdapter1.Fill(dt1);
            comboBox1.DataSource = dt1;
            comboBox1.DisplayMember = "EPITHETO";


            DataAdapter3 = new SqlDataAdapter("Select * from APOTHIKI", connection);
            DataTable dt3 = new DataTable();
            DataAdapter3.Fill(dt3);
            comboBox2.DataSource = dt3;
            comboBox2.DisplayMember = "KATHGORIA";



            database = "APOTHIKI_4679";
            host = "GIWRGOS";
            bindingNavigator1.BindingSource = BindingSource2;
            bindingNavigator2.BindingSource = BindingSource2;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'aPOTHIKI_4679DataSet2.APOTHIKI' table. You can move, or remove it, as needed.
            this.aPOTHIKITableAdapter1.Fill(this.aPOTHIKI_4679DataSet2.APOTHIKI);
            // TODO: This line of code loads data into the 'aPOTHIKI_4679DataSet.PROIONTA_PARAGELIAS' table. You can move, or remove it, as needed.
            this.pROIONTA_PARAGELIASTableAdapter.Fill(this.aPOTHIKI_4679DataSet.PROIONTA_PARAGELIAS);
            // TODO: This line of code loads data into the 'aPOTHIKI_4679DataSet.PARAGELIA' table. You can move, or remove it, as needed.
            this.pARAGELIATableAdapter.Fill(this.aPOTHIKI_4679DataSet.PARAGELIA);
            // TODO: This line of code loads data into the 'aPOTHIKI_4679DataSet.APOTHIKI' table. You can move, or remove it, as needed.
            this.aPOTHIKITableAdapter.Fill(this.aPOTHIKI_4679DataSet.APOTHIKI);
            // TODO: This line of code loads data into the 'aPOTHIKI_4679DataSet.PELATHS' table. You can move, or remove it, as needed.
            this.pELATHSTableAdapter.Fill(this.aPOTHIKI_4679DataSet.PELATHS);



            connection = new SqlConnection("Data Source=" + host + ";Initial Catalog=" + database + ";Integrated Security=True");
            connection.Open();

            if (connection.State == ConnectionState.Open)
            {
                MessageBox.Show("Connection Established!");
            }
            else
            {
                MessageBox.Show("Connection Error!");
                Application.Exit();
            }
            
            query = "Select * from PELATHS";
            query = "Select * from APOTHIKI";


            
            sqlAdapter = new SqlDataAdapter(query, connection);
            Dataset3 = new DataSet();
            sqlAdapter.Fill(Dataset3);


            sqlAdapter2 = new SqlDataAdapter(query, connection);
            Dataset3 = new DataSet();
            sqlAdapter.Fill(Dataset3);
                             
            
            BindingSource2.DataSource = Dataset3.Tables[0];
            bindingNavigator1.Refresh();


            BindingSource2.DataSource = Dataset3.Tables[0];
            bindingNavigator2.Refresh();

            refreshImage();

        }

        //ιστορικο παραγγελιων

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillDataSet();
        }

        public void fillDataSet()  
        {
            DataAdapter2 = new SqlDataAdapter("SELECT EPONYMIA,AFM,EIDOS,KATHGORIA,TIMH_POLHSHS,FPA,POSOTHTA FROM PELATHS INNER JOIN (PARAGELIA INNER JOIN (PROIONTA_PARAGELIAS INNER JOIN APOTHIKI ON PROIONTA_PARAGELIAS.K_E=APOTHIKI.KE) ON PROIONTA_PARAGELIAS.K_PA=PARAGELIA.KOD_PAR)ON PELATHS.KOD_PELATH=PARAGELIA.K_PEL  WHERE PELATHS.EPITHETO='" + comboBox1.Text.ToString() + "'", connection);
            Dataset2 = new DataSet();
            DataAdapter2.Fill(Dataset2);
            BindingSource2 = new BindingSource();
            DataTable dt = new DataTable();
            BindingSource2.DataSource = Dataset2.Tables[0].DefaultView;
            dataGridView1.DataSource = BindingSource2;
            double tel = 0;
            double tim;
            double fpa;
            double pos;
           
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                tim = Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value);
                fpa = Convert.ToDouble(dataGridView1.Rows[i].Cells[5].Value);
                pos = Convert.ToDouble(dataGridView1.Rows[i].Cells[6].Value);
                tel += (tim*pos)+(fpa/100)*(tim*pos);
            }
            label31.Text = tel.ToString();

        }


        //πελατης φωτο


        
        public void refreshImage()
        {
            String photoPath = textBox12.Text.Trim();
            if (photoPath != null && File.Exists(photoPath))
            {
                pictureBox1.Image = Image.FromFile(photoPath);
            }
            else
            {
                pictureBox1.Image = Image.FromFile("C:/Photos/error.jpg");
            }

        }

        private void bindingNavigator1_RefreshItems(object sender, EventArgs e)
        {
            refreshImage();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String openPath;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openPath = openFileDialog1.InitialDirectory + openFileDialog1.FileName;
                textBox12.Text = openPath;
                pictureBox1.Image = Image.FromFile(openPath);
                command = new SqlCommand("update PELATHS set FOTO='"+openPath+"' where KOD_PELATH="+textBox1.Text+";",connection);
                command.ExecuteNonQuery();
            }
        }

        


        //αποθηκη φωτο
 
        public void refreshImage2()
        {
            String photoPath = textBox12.Text.Trim();
            if (photoPath != null && File.Exists(photoPath))
            {
                pictureBox2.Image = Image.FromFile(photoPath);
            }
            else
            {
                pictureBox2.Image = Image.FromFile("C:/Photos/error.jpg");
            }

        }
  
        private void button2_Click(object sender, EventArgs e)
        {
            String openPath;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openPath = openFileDialog1.InitialDirectory + openFileDialog1.FileName;
                textBox28.Text = openPath;
                pictureBox2.Image = Image.FromFile(openPath);
                command = new SqlCommand("update APOTHIKI set FOTO='" + openPath + "' where KE=" + textBox14.Text + ";", connection);
                command.ExecuteNonQuery();
            }
        }

       

        private void bindingNavigator1_RefreshItems_1(object sender, EventArgs e)
        {
            refreshImage2();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            connection.Close();
        }





        //ιστορικο προιοντος

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillDataSet2();
        }

        public void fillDataSet2()
        {
            DataAdapter3 = new SqlDataAdapter("SELECT KE,EIDOS,KATHGORIA,APOTHEMA,TIMH_POLHSHS FROM APOTHIKI WHERE APOTHIKI.KATHGORIA='" + comboBox2.Text.ToString() + "'", connection);
            Dataset3 = new DataSet();
            DataAdapter3.Fill(Dataset3);
            BindingSource2 = new BindingSource();
            DataTable dt = new DataTable();
            BindingSource2.DataSource = Dataset3.Tables[0].DefaultView;
            dataGridView2.DataSource = BindingSource2;
            double tel = 0;
            
           
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
               
            }
            label37.Text = tel.ToString();

        }









    }
}
