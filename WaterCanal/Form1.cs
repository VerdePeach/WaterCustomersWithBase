using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace WaterCanal
{
    public partial class Form1 : Form
    {
        Base wateBase = new Base();

        public Form1()
        { 
            InitializeComponent();
            dataGridView2.DataSource = dataGridView1.DataSource = Base.upDateBase();
        }
        
        //Clear page Add and edit
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear(); //id
            textBox5.Clear(); //last_name
            textBox3.Clear(); //summery
            textBox4.Clear(); //address
        }

        //Insert new customer
        private void button1_Click(object sender, EventArgs e)
        {
            string id;
            string lastName;
            string address;
            int summery;

            id = textBox1.Text;
            lastName = textBox5.Text;
            address = textBox4.Text;


            if (id.Equals("") || lastName.Equals("") || address.Equals("") || textBox3.Text.Equals(""))
            {
                MessageBox.Show("Incorrect values!\nAll fields must be correctly filled.");
            }
            else
            {
                try
                {
                    summery = Convert.ToInt32(textBox3.Text);
                    Base.insertCustomer(id, lastName, address, summery);
                }
                catch
                {
                    MessageBox.Show("Incorrect value for summery.");
                    textBox3.Clear();
                }
                dataGridView2.DataSource = dataGridView1.DataSource = Base.upDateBase();
            }
        }

        //Update information about customer
        private void button4_Click(object sender, EventArgs e)
        {
            string id;
            string lastName;
            string address;
            int summery;

            id = textBox1.Text;
            lastName = textBox5.Text;
            address = textBox4.Text;


            if (id.Equals("") || lastName.Equals("") || address.Equals("") || textBox3.Text.Equals(null))
            {
                MessageBox.Show("Incorrect values!\nAll fields must be correctly filled.");
            }
            else
            {
                try
                {
                    summery = Convert.ToInt32(textBox3.Text);
                    Base.updateCustomer(id, lastName, address, summery);
                }
                catch
                {
                    MessageBox.Show("Incorrect value for summery.");
                    textBox3.Clear();
                }
                dataGridView2.DataSource = dataGridView1.DataSource = Base.upDateBase();
            }
        }

        //Clear page search and remove
        private void button6_Click(object sender, EventArgs e)
        {
            textBox7.Clear(); //id
            textBox9.Clear(); //last name
            textBox8.Clear(); //address
            textBox6.Clear(); //summery
            textBox2.Clear(); //id for remove
            radioButton3.Select();//=
            //radioButton1//<
            //radioButton2//>
        }

        //Remove
        private void button3_Click(object sender, EventArgs e)
        {
            string id;
            id = textBox2.Text;

            if (id.Equals(""))
            {
                MessageBox.Show("Incorrect values of id!");
            }
            else
            {
                Base.removeCustomer(id);
                dataGridView2.DataSource = dataGridView1.DataSource = Base.upDateBase();
            }
        }


        //Search
        private void button5_Click(object sender, EventArgs e)
        {
            string id;
            string lastName;
            string address;
            string summery;
            string signSummery;
            
            id = textBox7.Text;
            lastName = textBox9.Text;
            address = textBox8.Text;
            summery = textBox6.Text;

            if (radioButton1.Checked)
            {
                signSummery = "<";
            }
            else if (radioButton2.Checked)
            {
                signSummery = ">";
            }
            else
            {
                signSummery = "=";
            }
            dataGridView2.DataSource = dataGridView1.DataSource = Base.searchCustomer(id, lastName, address, summery, signSummery);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dataGridView3.DataSource = Base.customersWithOutDebt();

            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document doc = app.Documents.Open(System.Windows.Forms.Application.StartupPath.ToString() + "\\goodCustomersWith0.docx");
            object missing = System.Reflection.Missing.Value;
            string sum = "                                                               Customers who has no debt!  \n\n";
            try
            {
                string[] array = wateBase.getBaseContent("SELECT * FROM customers WHERE SUMMERY < 1");
                for (int i = 0; i < array.Length; i++)
                {
                    string a = array[i];
                    sum +=""+ a + "\n";
                }
                doc.Content.Text = sum;
                doc.Save();
                doc.Close(ref missing);
                app.Quit(ref missing);
                MessageBox.Show("File successfully created\n" + Application.StartupPath.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("Imposible to create file");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dataGridView3.DataSource = Base.customersWithAdvance(!textBox10.Text.Equals("") ? textBox10.Text : "0");

            Microsoft.Office.Interop.Word.Application app = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document doc = app.Documents.Open(Application.StartupPath.ToString() + "\\goodCustomersMore0.docx");
            object missing = System.Reflection.Missing.Value;
            string sum = "                                               Customers who have money on the bill! \n\n";
            try
            {
                string[] array = wateBase.getBaseContent("SELECT * FROM customers WHERE SUMMERY < " + (!textBox10.Text.Equals("")?textBox10.Text:"0"));
                for (int i = 0; i < array.Length; i++)
                {
                    string a = array[i];
                    sum += "" + a + "\n";
                }
                doc.Content.Text = sum;
                doc.Save();
                doc.Close(ref missing);
                app.Quit(ref missing);
                MessageBox.Show("File successfully created\n" + Application.StartupPath.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("Imposible to create file");
            }
            
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }
    }
}
