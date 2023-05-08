using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace KuruTemizleme
{
    public partial class DurumKayıt : Form
    {
        SqlConnection con = new SqlConnection("Data Source = localhost\\SQLEXPRESS; Initial Catalog = kuru_temizleme; Integrated Security = True");
        public DurumKayıt()
        {
            InitializeComponent();
            EkleUrunId();
            Goster();
        }

        private void urun_label_Click(object sender, EventArgs e)
        {
            UrunlerKayıt uk = new UrunlerKayıt();
            uk.Show();
            this.Hide();
        }

        private void EkleUrunId()
        {
            con.Open();

            SqlCommand com = new SqlCommand("select UrunId from Urun_Kayit", con);
            SqlDataReader rdr;
            rdr = com.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("UrunId", typeof(int));
            dt.Load(rdr);
            urun_id_cb.ValueMember = "UrunId";
            urun_id_cb.DataSource = dt;

            con.Close();
        }

        private void Goster()
        {
            con.Open();
            string query = "select * from Durum_Kayit_Ekran";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            SqlCommandBuilder scb = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            liste2_dataGridView.DataSource = ds.Tables[0];
            con.Close();
        }

        private void GetUrunAdi()
        {
            con.Open();

            string query = "select * from Urun_Kayit where UrunId=" + urun_id_cb.SelectedValue.ToString() + "";
            SqlCommand com = new SqlCommand(query, con);
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(com);
            sda.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                urun_adi_tb.Text = dr["UrunAdi"].ToString();
            }

            con.Close();
        }
        private void Temizle()
        {
            urun_adi_tb.Text = "";
            yıkama_durum_tb.Text = "";
            fiyat_tb.Text = "";
            teslim_durum_tb.Text = "";
            temsilci_id_tb.Text = "";

            tus = 0;
        }

        private void kaydet_button_Click(object sender, EventArgs e)
        {
            if (urun_id_cb.SelectedIndex == -1 || urun_adi_tb.Text == "" || yıkama_durum_tb.Text == "" || fiyat_tb.Text == "" || teslim_durum_tb.Text == "")
            {
                MessageBox.Show("Bilgileriniz eksik");
            }
            else
            {
                try
                {
                    con.Open();
                    string Query = "insert into Durum_Kayit_Ekran values ('" + urun_id_cb.SelectedValue.ToString() + "','" + urun_adi_tb.Text + "','" + yıkama_durum_tb.Text + "','" + fiyat_tb.Text + "','" + teslim_durum_tb.Text + "')";
                    SqlCommand cmd = new SqlCommand(Query, con);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Ürün durumu başarılı bir şekilde kaydedildi.");

                    con.Close();

                    Goster();
                    Temizle();

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }


        private void temizle_button_Click(object sender, EventArgs e)
        {
            Temizle();
        }

        int tus = 0;
        private void liste2_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            urun_id_cb.SelectedValue = liste2_dataGridView.Rows[index].Cells[0].Value.ToString();
            urun_adi_tb.Text = liste2_dataGridView.Rows[index].Cells[1].Value.ToString();
            yıkama_durum_tb.Text = liste2_dataGridView.Rows[index].Cells[2].Value.ToString();
            fiyat_tb.Text = liste2_dataGridView.Rows[index].Cells[3].Value.ToString();
            teslim_durum_tb.Text = liste2_dataGridView.Rows[index].Cells[4].Value.ToString();
            temsilci_id_tb.Text = liste2_dataGridView.Rows[index].Cells[5].Value.ToString();


            if (urun_adi_tb.Text == "")
            {
                tus = 0;
            }

            else
            {
                tus = Convert.ToInt32(liste2_dataGridView.Rows[index].Cells[0].Value.ToString());
            }
        }


        private void urun_id_cb_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetUrunAdi();
        }

        private void sil_button_Click(object sender, EventArgs e)
        {
            if (tus == 0)
            {
                MessageBox.Show("Silmek için önce kayıtlı bir ürün seçiniz");
            }
            else
            {
                try
                {
                    con.Open();
                    string Query = "delete from Durum_Kayit_Ekran where TemsilciId=" + tus + ";";
                    SqlCommand cmd = new SqlCommand(Query, con);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Başarıyla silindi");
                    con.Close();

                    Goster();
                    Temizle();

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void duzenle_button_Click(object sender, EventArgs e)
        {
            if (urun_id_cb.SelectedIndex == -1 || urun_adi_tb.Text == "" || yıkama_durum_tb.Text == "" || fiyat_tb.Text == "" || teslim_durum_tb.Text == "" || temsilci_id_tb.Text == "" )
            {
                MessageBox.Show("Bilgileriniz eksik");
            }
            else
            {
                try
                {
                    con.Open();
                    string Query = "update Durum_Kayit_Ekran set UrunId=@UrunId, UrunAdi=@UrunAdi, YikamaDurum=@YikamaDurum, Fiyat=@Fiyat, TeslimDurum=@TeslimDurum where TemsilciId=@TemsilciId";

                    SqlCommand cmd = new SqlCommand(Query, con);


                    cmd.Parameters.AddWithValue("@UrunId", urun_id_cb.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@UrunAdi", urun_adi_tb.Text);
                    cmd.Parameters.AddWithValue("@YıkamaDurum", yıkama_durum_tb.Text);
                    cmd.Parameters.AddWithValue("@TeslimDurum", teslim_durum_tb.Text);
                    cmd.Parameters.AddWithValue("@Fiyat", fiyat_tb.Text);
                    cmd.Parameters.AddWithValue("@TemsilciId", temsilci_id_tb.Text); 

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Ürünler başarılı bir şekilde düzenlendi");

                    con.Close();

                    Goster();
                    Temizle();

                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
        }

        private void DurumKayıt_Load(object sender, EventArgs e)
        {

        }
    }
}
