using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private async void bStart_ClickAsync(object sender, EventArgs e)
        {
            bStart.Enabled = false;
            await StartAsync();
            bStart.Enabled = true;
        }
    }
}
