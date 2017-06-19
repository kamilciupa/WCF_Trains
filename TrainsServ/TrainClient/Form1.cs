using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrainClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            maskedTextBox1.Mask = "0000-00-00 00:00";
        }

        private void buttonZatwierdz_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceReference2.Service1Client client = new ServiceReference2.Service1Client();


                listBox1.Items.Clear();
                string[] outputStrings;          

                if (!string.IsNullOrEmpty(boxSkad.Text) && !string.IsNullOrEmpty(boxDokad.Text))
                {
                    if(maskedTextBox1.MaskCompleted)
                    {
                        outputStrings = client.GetTripWithTime(boxSkad.Text, boxDokad.Text, Convert.ToDateTime(maskedTextBox1.Text));
                    }
                    else
                        outputStrings = client.GetTripWithoutTime(boxSkad.Text, boxDokad.Text);


                    if (outputStrings.Length != 0)
                    {
                        foreach (string row in outputStrings)
                        {
                            listBox1.Items.Add(row);

                        }
                    }
                    else { listBox1.Items.Add("Brak polaczen"); }
                } else
                {
                    listBox1.Items.Add("Podaj dane");
                }
            }
            catch (Exception ex)
            {

            }


        }
    }
}
