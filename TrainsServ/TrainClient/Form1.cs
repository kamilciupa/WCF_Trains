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
            ServiceReference1.Service1Client client = new ServiceReference1.Service1Client();
            if (!string.IsNullOrEmpty(boxData.Text))
            {
                string outputString = client.GetTripWithTime(boxSkad.Text, boxDokad.Text, Convert.ToDateTime(boxData.Text));
                labelOutput.Text = outputString;
            } else
            {
                string[] outputStrings = client.GetTripWithoutTime(boxSkad.Text, boxDokad.Text);
                labelOutput.Text = "There is so many";
            }
        }
    }
}
