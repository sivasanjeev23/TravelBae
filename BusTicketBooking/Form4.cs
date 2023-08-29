using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace BusTicketBooking
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        SqlConnection con = BusUser.getCon();
        private void Form4_Load(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd1 = new SqlCommand("sp_board", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            SqlCommand cmd2 = new SqlCommand("sp_drop", con);
            cmd2.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter dr1 = new SqlDataAdapter(cmd1);
            SqlDataAdapter dr2 = new SqlDataAdapter(cmd2);
            DataTable dt1 = new DataTable();
            DataTable dt2=new DataTable();
            dr1.Fill(dt1);
            dr2.Fill(dt2);
            comboBox1.DataSource = dt1;
            comboBox1.DisplayMember = "Boarding";
            comboBox2.DataSource = dt2;
            comboBox2.DisplayMember = "Dropping";
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand
                ("sp_viewbus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@board", comboBox1.Text);
            cmd.Parameters.AddWithValue("@drop", comboBox2.Text);
            SqlDataAdapter dr = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dr.Fill(dt);
            comboBox3.DataSource = dt;
            comboBox3.ValueMember= "Bid";
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox3.Text != "")
            {
                int i = 0;
                con.Open();
                SqlCommand cmd1 = new SqlCommand("sp_checkseat", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@bid", int.Parse(comboBox3.Text));
                SqlDataReader sdr = cmd1.ExecuteReader(); sdr.Read();
                int rs = (int)sdr["remseat"];
                sdr.Close();
                if (rs > 0)
                {
                    SqlCommand cmd2 = new SqlCommand
                        ("sp_picUser", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@em", BusUser.getEmail());
                    SqlDataReader dr = cmd2.ExecuteReader(); int id = 0; string nam = null;
                    if (dr.Read())
                    {
                        id = (int)dr["uid"]; nam = (string)dr["name"];
                    }
                    dr.Close();
                    SqlCommand cmd3 = new SqlCommand
                    ("sp_insbook", con);
                    cmd3.CommandType = CommandType.StoredProcedure;
                    cmd3.Parameters.AddWithValue("@bid", comboBox3.Text);
                    cmd3.Parameters.AddWithValue("@nam", nam);
                    cmd3.Parameters.AddWithValue("@uid", id);
                    cmd3.ExecuteNonQuery();
                    SqlCommand cmd4 = new SqlCommand
                   ("sp_setRemseat", con);
                    cmd4.CommandType = CommandType.StoredProcedure;
                    cmd4.Parameters.AddWithValue("@bid", int.Parse(comboBox3.Text));
                    i = cmd4.ExecuteNonQuery();
                }
                con.Close();
                if (i > 0)
                {
                    MessageBox.Show("Your Journey confirmed in Bus No: "
                    + comboBox3.Text + "\nTicket Details shared to your Registered Mobile Number/email");
                }
                else MessageBox.Show("Tickets are sold out");
            }
            else MessageBox.Show("No bus Selected");
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
            Form3 f3 = new Form3();
            f3.Show();
        }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void comboBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
