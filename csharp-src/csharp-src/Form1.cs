using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace csharp_src
{
    public partial class form1 : Form
    {
        double temperatura = 0, umidita = 0;
        bool checkData = false;

        public form1()
        {
            InitializeComponent();
        }

        private void form1_Load(object sender, EventArgs e)
        {
            btnOpen.Enabled = true;
            btnClose.Enabled = false;

            chart1.Series["Temperatura"].Points.AddXY(1, 1);
            chart1.Series["Umiditá"].Points.AddXY(1, 1);
        }

        private void comboBoxPorte_DropDown(object sender, EventArgs e)
        {
            string[] listaPorte = SerialPort.GetPortNames();
            comboBoxPorte.Items.Clear();
            comboBoxPorte.Items.AddRange(listaPorte);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = comboBoxPorte.Text;
                serialPort1.BaudRate = Convert.ToInt32(comboBoxBaudRate.Text);
                serialPort1.Open();
                
                btnOpen.Enabled = false;
                btnClose.Enabled = true;

                chart1.Series["Temperatura"].Points.Clear();
                chart1.Series["Umiditá"].Points.Clear();

                MessageBox.Show("Connessione effettuata!");

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Close();

                btnOpen.Enabled = true;
                btnClose.Enabled = false;

                MessageBox.Show("Disconnessione effettuata!");

            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                serialPort1.Close();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = serialPort1.ReadTo("\n");
            Data_Parsing(data);
            this.BeginInvoke(new EventHandler(ShowData));
        }

        private void ShowData(object sender, EventArgs e)
        {
            if (checkData)
            {
                label_temperatura.Text = string.Format("Temperatura = {0} °C", temperatura.ToString());
                label_umidita.Text = string.Format("Umiditá = {0} %", umidita.ToString());

                chart1.Series["Temperatura"].Points.Add(temperatura);
                chart1.Series["Umiditá"].Points.Add(umidita);
            }
        }

        private void Data_Parsing(string data)
        {
            char carattereIniziale = (char)data.IndexOf("@");
            char indexA = (char)data.IndexOf("A");
            char indexB = (char)data.IndexOf("B");

            if(indexA != -1 && indexB != -1 && carattereIniziale != -1)
            {
                try
                {
                    string str_temperatura = data.Substring(carattereIniziale + 1, (indexA - carattereIniziale) - 1);
                    string str_umidita = data.Substring(indexA + 1, (indexB - indexA) - 1);

                    temperatura = Convert.ToDouble(str_temperatura);
                    umidita = Convert.ToDouble(str_umidita);

                    checkData = true;
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
            else
            {
                checkData = false;
            }
        }
    }
}
