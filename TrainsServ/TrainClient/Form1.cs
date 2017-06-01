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
        }

        private void buttonZatwierdz_Click(object sender, EventArgs e)
        {
            ServiceReference2.Service1Client client = new ServiceReference2.Service1Client();
            listBox1.Items.Clear();
            string[] outputStrings;

            if (!string.IsNullOrEmpty(boxData.Text))
                outputStrings = client.GetTripWithTime(boxSkad.Text, boxDokad.Text, Convert.ToDateTime(boxData.Text));
            else
                outputStrings = client.GetTripWithoutTime(boxSkad.Text, boxDokad.Text);
            
            foreach (string row in outputStrings)
            {
                listBox1.Items.Add(row);
            }
        }
    }
}
