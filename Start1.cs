using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MotionDetection
{
    public partial class Start1 : Form
    {
        public Start1()
        {
            InitializeComponent();
        }

             private void button1_Click(object sender, EventArgs e)
             {

                 //= new FormLogin();
                 //ff.Show(); FormLogin ff

                 LoadCamera ll = new LoadCamera();
                 ll.Show();

             }
    }
}
