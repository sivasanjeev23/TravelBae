using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace BusTicketBooking
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        SqlConnection con = BusUser.getCon();
        private void Form2_Load(object sender, EventArgs e)
        {
            groupBox1.Visible = true;
            groupBox2.Visible = false;
        }
        int m;
        private void button1_Click(object sender, EventArgs e)
        {
            long a; int j = 0, l = 0; bool b = false, c = false;
            string str1 = textBox1.Text;
            try
            {
                con.Open();
                if (long.TryParse(textBox1.Text, out a))
                {
                    SqlCommand cmd1 = new SqlCommand("sp_pickuseMbl", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@mbl",a);
                    SqlDataReader sdr1 = cmd1.ExecuteReader();
                    b = sdr1.Read();
                    if (b)
                    {
                        BusUser.setName(sdr1["name"].ToString());
                        BusUser.setEmail(sdr1["email"].ToString());
                        BusUser.setGen(sdr1["gender"].ToString());
                        BusUser.setMob((long?)sdr1["mblno"]);
                        string[] db = sdr1["dob"].ToString().Split(' ').ToArray();
                        BusUser.setDob(db[0]);
                        BusUser.setCity(sdr1["city"].ToString());
                        sdr1.Close();
                    }
                    if (!b && a > 1000000000 && a <= 9999999999)
                    {
                        sdr1.Close();
                        SqlCommand cmd2 = new SqlCommand("sp_insMbl", con);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@mbl", a);
                        j = cmd2.ExecuteNonQuery();
                        BusUser.setMob(a);
                    }
                    else if (!b) MessageBox.Show("Invalid Mobile Number");
                }
                else
                {
                    SqlCommand cmd3 = new SqlCommand("sp_pickuseEm", con);
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@em", str1);
                    SqlDataReader sdr2 = cmd3.ExecuteReader();
                    c = sdr2.Read();
                    if (c)
                    {
                        BusUser.setName(sdr2["name"].ToString());
                        BusUser.setEmail(sdr2["email"].ToString());
                        BusUser.setGen(sdr2["gender"].ToString());
                        if (sdr2["mblno"].ToString() != "") BusUser.setMob((long?)sdr2["mblno"]);
                        string[] db = sdr2["dob"].ToString().Split(' ').ToArray();
                        BusUser.setDob(db[0]);
                        BusUser.setCity(sdr2["city"].ToString());
                        sdr2.Close();
                    }
                    else
                    {
                        sdr2.Close();
                        SqlCommand cmd4 = new SqlCommand("sp_insEm", con);
                        cmd4.CommandType = CommandType.StoredProcedure;
                        cmd4.Parameters.AddWithValue("@em", str1);
                        l = cmd4.ExecuteNonQuery();
                        BusUser.setEmail(str1);
                    }
                }
                con.Close();
            }
            catch
            {
                MessageBox.Show("Enter Valid email/Mobile");
                ResetFormState();
            }
            if (b || c || j > 0 || l > 0)
            {
                Random random = new Random();
                m = random.Next(100000, 999999);
                MessageBox.Show("OTP for Login/Sign up : " + m);
                groupBox2.Visible = true;
                groupBox1.Visible = false;
            }
        }

        private void ResetFormState()
        {
            this.Close();
            Form2 f2=new Form2();
            f2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int val;
            if (int.TryParse(textBox2.Text,out val))
            {
                if (val == m)
                {
                    Form3 form3 = new Form3();
                    form3.Show();
                    this.Hide();
                }
                else MessageBox.Show("Invalid");
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar==' ')
            {
                e.Handled = true;
            }
        }
    }
}
