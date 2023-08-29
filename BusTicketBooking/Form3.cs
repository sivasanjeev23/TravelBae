using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BusTicketBooking
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (BusUser.getName()!="" && BusUser.getEmail()!="" &&
                BusUser.getName() != null && BusUser.getEmail() != null)
            {
                Form4 f4 = new Form4();
                f4.Show();
                this.Hide(); 
            }
            else
            {
                MessageBox.Show("Update Profile");
            }
        }
        SqlConnection con = BusUser.getCon();
        private void button3_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            textBox1.Text = BusUser.getName();
            textBox2.Text = BusUser.getEmail();
            textBox3.Text = BusUser.getMob().ToString();
            if (BusUser.getGen() == "M") radioButton1.Checked = true;
            else if (BusUser.getGen() == "F") radioButton2.Checked = true;
            textBox4.Text = BusUser.getDob();
            textBox5.Text = BusUser.getCity();
            groupBox1.Visible = false;
            dataGridView1.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int k=0;bool c = false, d = false, f = true;
            if (BusUser.getEmail() != "" && BusUser.getEmail() != null)
            {
                if (BusUser.getEmail() == textBox2.Text) c = true;
                else
                {
                    MessageBox.Show("You Can't Change Registered Email");
                    f = false;
                }
            }
            if (BusUser.getMob() != null)
            {
                long n;
                long.TryParse(textBox3.Text, out n);
                if (BusUser.getMob() == n) d = true;
                else 
                { 
                    MessageBox.Show("You Can't Change Registered Mobile Number");
                    f = false;
                }
            }
            long? a = null;
            string gen = null, str1 = null;
            con.Open();
            if (c && BusUser.getMob()==null)
            {
                a = long.Parse(textBox3.Text);
                if (a > 1000000000 && a <= 9999999999)
                {
                    SqlCommand cmd2 = new SqlCommand("sp_pinsMbl", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@mbl", a);
                    cmd2.Parameters.AddWithValue("@em", BusUser.getEmail());
                    cmd2.ExecuteNonQuery();
                    BusUser.setMob(a);
                }
                else
                {
                    MessageBox.Show("Invalid Mobile Number");
                    c=false;
                }
            }
            if (d && BusUser.getEmail()=="" || BusUser.getEmail()==null)
            {
                str1 = textBox2.Text;
                try
                {
                    SqlCommand cmd4 = new SqlCommand("sp_pinsEm", con);
                    cmd4.CommandType = CommandType.StoredProcedure;
                    cmd4.Parameters.AddWithValue("@em", str1);
                    cmd4.Parameters.AddWithValue("@mbl", BusUser.getMob());
                    cmd4.ExecuteNonQuery();
                    BusUser.setEmail(str1);
                }
                catch
                {
                    MessageBox.Show("Invalid email");
                    d = false;
                    this.Close();
                    Form3 f3=   new Form3();
                    f3.Show();
                }
            }
            if (c&&f || d&&f)
            {
                try
                {
                    if (radioButton1.Checked) gen = "M";
                    else gen = "F";
                    SqlCommand cmd5 = new SqlCommand
                    ("Sp_upPro", con);
                    cmd5.CommandType = CommandType.StoredProcedure;
                    cmd5.Parameters.AddWithValue("@nam", textBox1.Text);
                    cmd5.Parameters.AddWithValue("@gen", gen);
                    cmd5.Parameters.AddWithValue("@dob", textBox4.Text);
                    cmd5.Parameters.AddWithValue("@city", textBox5.Text);
                    cmd5.Parameters.AddWithValue("@mbl", BusUser.getMob());
                    cmd5.Parameters.AddWithValue("@em", BusUser.getEmail());
                    k = cmd5.ExecuteNonQuery();
                }
                catch 
                { 
                    MessageBox.Show("Invalid Date of birth");
                    this.Close();
                    Form3 f3 = new Form3();
                    f3.Show();
                }
            }
            con.Close();
            if (k > 0)
            {
                BusUser.setName(textBox1.Text);
                BusUser.setGen(gen);
                BusUser.setDob(textBox4.Text);
                BusUser.setCity(textBox5.Text);
                MessageBox.Show("Profile Updated");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (BusUser.getMob() != null)
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_view", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mblno", BusUser.getMob());
                SqlDataAdapter sdr = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                int i = sdr.Fill(dt);
                if (i > 0)
                {
                    dataGridView1.Visible = true;
                    dataGridView1.DataSource = dt;
                }
                else
                    MessageBox.Show("No Booking Available to show");
            }
            else
                MessageBox.Show("No Booking Available to show");
            con.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '-'&& !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }   
}