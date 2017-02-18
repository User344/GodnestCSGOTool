using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace GodnestCSGOTool
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void materialRaisedButton20_Click(object sender, EventArgs e)
        {
            string[] lines = textBox1.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            int i = 0;
            while (i != lines.Length)
            {
                lobby.InviteMessage(lines[i].ToString());
                System.Threading.Thread.Sleep(100);
                i = i + 1;
            }
        }
    }
}
