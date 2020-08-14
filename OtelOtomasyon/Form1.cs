using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtelOtomasyon
{
    
    public partial class Form1 : Form
    {
        

        public void DataGridGuncelle()
        {
            try
            {
                connect.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            dataSet.Reset();
            string sql = "select \"Insan\".\"Id\" as \"ID\",\"Insan\".\"Adi\" as \"AD\",\"Insan\".\"Soyadi\" as \"SOYAD\"," +
                "\"Insan\".\"TelNo\" as \"TEL NO\",\"Insan\".\"TcNo\" as \"TC\",\"Insan\".\"Adres\" as \"ADRES\"," +
                "\"Insan\".\"Cinsiyet\" as \"CİNSİYET\"," + "\"Insan\".\"DogumTarihi\" as \"DOĞUM TARİHİ\"," +
                "\"Musteriler\".\"MedeniDurum\" as \"MEDENİ DURUM\",\"Musteriler\".\"SirketAdi\" as \"ŞİRKET ADI\",\"Musteriler\".\"Aciklama\" as \"AÇIKLAMA\" " +
                "from \"Insan\" inner join \"Musteriler\"on \"Insan\".\"Id\" = \"Musteriler\".\"Id\"";
            NpgsqlDataAdapter add = new NpgsqlDataAdapter(sql, connect);
            add.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];
            connect.Close();
        }
        public void Temizle()
        {
            txtAd.Clear();
            txtSoyad.Clear();
            txtTelNo.Clear();
            txtTc.Clear();
            txtAdres.Clear();
            txtDogumTarihi.Clear();
            txtSirketAdi.Clear();
            txtAciklama.Clear();
            rdbErkek.Checked = false;
            rdbKadin.Checked = false;
            rdbEvli.Checked = false;
            rdbBekar.Checked = false;
        }
        NpgsqlConnection connect = new NpgsqlConnection("Server=localhost;Port=5432;Database=Otel;User Id=postgres;Password=05101990");
        DataSet dataSet = new DataSet();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtDogumTarihi.Text = "gg/aa/yyyy";
            txtDogumTarihi.ForeColor = Color.Gray;
            txtId.Enabled = false;
            DataGridGuncelle();
        }

        private void btnBul_Click(object sender, EventArgs e)
        {
            Temizle();
            if (txtId.Text == "") 
            {
                MessageBox.Show("Lütfen Id Giriniz.");
            }
            else
            {
                connect.Open();
                int id = Convert.ToInt32(txtId.Text);
                string sql= "select \"Insan\".\"Id\" as \"ID\",\"Insan\".\"Adi\" as \"AD\",\"Insan\".\"Soyadi\" as \"SOYAD\"," +
                "\"Insan\".\"TelNo\" as \"TEL NO\",\"Insan\".\"TcNo\" as \"TC\",\"Insan\".\"Adres\" as \"ADRES\"," +
                "\"Insan\".\"Cinsiyet\" as \"CİNSİYET\"," + "\"Insan\".\"DogumTarihi\" as \"DOĞUM TARİHİ\"," +
                "\"Musteriler\".\"MedeniDurum\" as \"MEDENİ DURUM\",\"Musteriler\".\"SirketAdi\" as \"ŞİRKET ADI\",\"Musteriler\".\"Aciklama\" as \"AÇIKLAMA\" " +
                "from \"Insan\" inner join \"Musteriler\"on \"Insan\".\"Id\" = \"Musteriler\".\"Id\"" +
                "where \"Insan\".\"Id\"="+id+"";
                NpgsqlCommand command = new NpgsqlCommand(sql, connect);
                NpgsqlDataReader read = command.ExecuteReader();
                if (read.HasRows == false)
                {
                    MessageBox.Show("Aranan Kayıt Bulunamadı");
                    txtId.Clear();
                }
                while (read.Read()) {
                    
                    txtAd.Text = read[1].ToString();
                    txtSoyad.Text = read[2].ToString();
                    txtTelNo.Text = read[3].ToString();
                    txtTc.Text = read[4].ToString();
                    txtAdres.Text = read[5].ToString();
                    txtDogumTarihi.Text = read[7].ToString();
                    txtSirketAdi.Text = read[9].ToString();
                    txtAciklama.Text = read[10].ToString();
                    if (read[6].ToString() == "1")
                    {
                        
                        rdbErkek.Checked = true;
                    }
                    else
                    {
                        rdbKadin.Checked = true;
                    }
                    if (read[8].ToString() == "1")
                        rdbEvli.Checked = true;
                    else
                        rdbBekar.Checked = true;
                    
                }
                connect.Close();
                txtId.Enabled = false;
            }
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            Temizle();
            txtId.Clear();
        }

        private void btnBul_MouseEnter(object sender, EventArgs e)
        {
            txtId.Enabled = true;
        }

        private void txtId_MouseLeave(object sender, EventArgs e)
        {
            txtId.Enabled = false;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (txtId.Text != "")
            {
                MessageBox.Show("Id alanı otomatik belirlenir. Lütfen boş bırakınız.");
            }
            else
            {
                if(txtAd.Text=="" || txtDogumTarihi.Text=="" || txtSoyad.Text=="" || txtTc.Text=="" || txtTelNo.Text == "")
                {
                    MessageBox.Show("Adres,Şirket Adı ve Açıklama Haricindeki bütün alanları doldurmak zorunludur!");
                }
                else
                {
                    string ad = txtAd.Text;
                    string soyad = txtSoyad.Text;
                    string telno = txtTelNo.Text;
                    string tc = txtTc.Text;
                    string adres = txtAdres.Text;
                    string dogumtarihi = txtDogumTarihi.Text;
                    string sirketadi = txtSirketAdi.Text;
                    string aciklama = txtAciklama.Text;
                    int cinsiyet = 0;
                    if (rdbErkek.Checked == true)
                        cinsiyet = 1;
                    int medeni = 0;
                    if (rdbEvli.Checked == true)
                        medeni = 1;
                    string sqlInsan= "INSERT INTO \"Insan\" ( \"Adi\", \"Soyadi\", \"TelNo\", \"TcNo\", \"Adres\", \"Cinsiyet\", \"DogumTarihi\") " +
                        "VALUES(\'"+ad+ "\',\'" + soyad+ "\',\'" + telno+ "\',\'" + tc+ "\',\'" + adres+ "\',\'" + cinsiyet+ "\',\'" + dogumtarihi+ "\')";
                    string sqlMusteri = "INSERT INTO \"Musteriler\" ( \"Id\", \"MedeniDurum\", \"SirketAdi\", \"Aciklama\") " +
                        "VALUES(\"sonInsanSayisi\"(),\'" + medeni + "\',\'" + sirketadi + "\',\'" + aciklama + "\')";
                    connect.Open();
                    NpgsqlCommand cmd = new NpgsqlCommand(sqlInsan, connect);
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = sqlMusteri;
                    cmd.ExecuteNonQuery();
                    connect.Close();
                    DataGridGuncelle();

                }
            }
        }

        private void txtDogumTarihi_MouseEnter(object sender, EventArgs e)
        {
            //txtDogumTarihi.Clear();
            txtDogumTarihi.ForeColor = Color.Black;
        }

        private void txtDogumTarihi_MouseLeave(object sender, EventArgs e)
        {
            //txtDogumTarihi.Text = "gg/aa/yyyy";
            //txtDogumTarihi.ForeColor = Color.Gray;
        }

        private void btnSil_MouseEnter(object sender, EventArgs e)
        {
            txtId.Enabled = true;
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (txtId.Text == "")
            {
                MessageBox.Show("Lütfen Id Giriniz.");
            }
            else
            {
                int id = Convert.ToInt32(txtId.Text);
                string sql = "DELETE FROM \"Insan\" WHERE \"Id\" =" + id + "";
                connect.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(sql, connect);
                cmd.ExecuteNonQuery();
                connect.Close();
                DataGridGuncelle();
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (txtId.Text == "")
            {
                MessageBox.Show("Önce Bul işlemi yapınız!");
            }
            else
            {
                int id = Convert.ToInt32(txtId.Text);
                string ad = txtAd.Text;
                string soyad = txtSoyad.Text;
                string telno = txtTelNo.Text;
                string tc = txtTc.Text;
                string adres = txtAdres.Text;
                string dogumtarihi = txtDogumTarihi.Text;
                string sirketadi = txtSirketAdi.Text;
                string aciklama = txtAciklama.Text;
                int cinsiyet = 0;
                if (rdbErkek.Checked == true)
                    cinsiyet = 1;
                int medeni = 0;
                if (rdbEvli.Checked == true)
                    medeni = 1;
                string sqlInsan = "UPDATE \"Insan\" SET \"Adi\" = \'" + ad + "\',\"Soyadi\" =\'" + soyad + "\' ," +
                    "\"TelNo\" = \'" + telno + "\',\"TcNo\" = \'" + tc + "\',\"Adres\" =\'" + adres + "\' ," +
                    "\"Cinsiyet\" =\'" + cinsiyet + "\' ,\"DogumTarihi\" =\'" + dogumtarihi + "\' WHERE \"Id\"=" + id + " ";
                string sqlMusteri = "UPDATE \"Musteriler\" SET \"MedeniDurum\" =\'" + medeni + "\' ,\"SirketAdi\" =" +
                    "\'" + sirketadi + "\' ,\"Aciklama\" =\'" + aciklama + "\' WHERE \"Id\" =" + id + " ";
                connect.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(sqlInsan, connect);
                cmd.ExecuteNonQuery();
                cmd.CommandText = sqlMusteri;
                cmd.ExecuteNonQuery();
                connect.Close();
                DataGridGuncelle();
                Temizle();
                txtId.Clear();
            }
            
        }
    }
}
