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
    public partial class UrunlerKayıt : Form
    {
        SqlConnection con = new SqlConnection("Data Source = localhost\\SQLEXPRESS; Initial Catalog = kuru_temizleme; Integrated Security = True");
        public UrunlerKayıt()
        {
            InitializeComponent();
        }

        private void Goster()
        {
            con.Open();
            string query = "select * from Urun_Kayit";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            SqlCommandBuilder scb = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            liste_dataGridView.DataSource = ds.Tables[0];
            con.Close();
        }
        int tus = 0;
        private void Temizle()
        {
            urun_adı_tb.Text = "";
            islem_tb.Text = "";
            alim_tarih_tb.Text = "";
            teslim_tarih_tb.Text = "";
            musteri_adi_tb.Text = "";
            musteri_no_tb.Text = "";

            tus = 0;
        }
        private void kaydet_button_Click(object sender, EventArgs e)
        {
            if (urun_adı_tb.Text == "" || islem_tb.Text == "" || alim_tarih_tb.Text == "" || teslim_tarih_tb.Text == "" || musteri_adi_tb.Text == "" || musteri_no_tb.Text == "")
            {
                MessageBox.Show("Bilgileriniz eksik");
            }
            else
            {
                try
                {
                    con.Open();
                    string Query = "insert into Urun_Kayit values ('" + urun_adı_tb.Text + "','" + islem_tb.Text + "','" + alim_tarih_tb.Text + "','" + teslim_tarih_tb.Text + "','" + musteri_adi_tb.Text + "','" + musteri_no_tb.Text + "')";
                    SqlCommand cmd = new SqlCommand(Query, con);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Ürün başarılı bir şekilde kaydedildi");

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

        private void liste_dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = e.RowIndex;
            urun_adı_tb.Text = liste_dataGridView.Rows[index].Cells[1].Value.ToString();
            islem_tb.Text = liste_dataGridView.Rows[index].Cells[2].Value.ToString();
            alim_tarih_tb.Text = liste_dataGridView.Rows[index].Cells[3].Value.ToString();
            teslim_tarih_tb.Text = liste_dataGridView.Rows[index].Cells[4].Value.ToString();
            musteri_adi_tb.Text = liste_dataGridView.Rows[index].Cells[5].Value.ToString();
            musteri_no_tb.Text = liste_dataGridView.Rows[index].Cells[6].Value.ToString();

            if (urun_adı_tb.Text == "")
            {
                tus = 0;
            }

            else
            {
                tus = Convert.ToInt32(liste_dataGridView.Rows[index].Cells[0].Value.ToString());
            }
        }

        private void sil_button_Click(object sender, EventArgs e)
        {
            if (tus == 0)
            {
                MessageBox.Show("Silmek için önce ürün seçiniz");
            }
            else
            {
                try
                {
                    con.Open();
                    string Query = "delete from Urun_Kayit where UrunId=" + tus + ";";
                    SqlCommand cmd = new SqlCommand(Query, con);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Ürün başarılı bir şekilde silindi");
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
            if (urun_adı_tb.Text == "" || islem_tb.Text == "" || alim_tarih_tb.Text == "" || teslim_tarih_tb.Text == "" || musteri_adi_tb.Text == "" || musteri_no_tb.Text == "")
            {
                MessageBox.Show("Bilgileriniz eksik");
            }
            else
            {
                try
                {
                    con.Open();
                    string Query = "update Urun_Kayit set UrunAdi='" + urun_adı_tb.Text + "',Islem='" + islem_tb.Text + "',AlimTarih='" + alim_tarih_tb.Text + "',TeslimTarih='" + teslim_tarih_tb.Text + "',MusteriAdi='" + musteri_adi_tb.Text + "',MusteriNo='" + musteri_no_tb.Text + "' where UrunId=" + tus + ";";
                    SqlCommand cmd = new SqlCommand(Query, con);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Ürün başarılı bir şekilde düzenlendi");

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

        private void durum_label_Click(object sender, EventArgs e)
        {
            DurumKayıt dk = new DurumKayıt();
            dk.Show();
            this.Hide();
        }

        public void verilergoster(string veriler)
        {
            SqlDataAdapter da = new SqlDataAdapter(veriler, con);
            DataSet ds = new DataSet();
            da.Fill(ds);

            liste_dataGridView.DataSource = ds.Tables[0];
        }

        private void goster_button_Click(object sender, EventArgs e)
        {
            verilergoster("Select * From  Urun_Kayit");
            
        }
    }
}
