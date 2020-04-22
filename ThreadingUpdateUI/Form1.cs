using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreadingUpdateUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Global collection of labels
        List<Label> threadLabels = new List<Label>();

        private void btnGo_Click(object sender, EventArgs e)
        {

            if (threadLabels.Count < 10)
            {
                int i = Convert.ToInt32(textBox1.Text); //Counter
                int sleep = Convert.ToInt32(textBox2.Text); //How long to pause between counting milliseconds

                Label l = new Label(); //Make a label
                l.Text = DateTime.Now.ToLongTimeString();

                threadLabels.Add(l);

                this.panel1.Controls.Add(l);

                l.Left = 5;
                l.Top = (this.panel1.Controls.Count - 1) * 35;

                //Spawn threads
                Task.Factory.StartNew(() =>
                {
                    //Counting process
                    for (int j = 0; j <= i; j++)
                    {
                        //Access the UI thread from another thread
                        this.Invoke((MethodInvoker)delegate
                        {
                            l.Text = j.ToString();
                        });

                        System.Threading.Thread.Sleep(sleep);//Currently sleeping the spawned thread
                    }
                });

            }
        }

        private void btnGoNoThread_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(textBox1.Text); //Counter
            int sleep = Convert.ToInt32(textBox2.Text); //How long to pause between counting milliseconds

            btnGoNoThread.Enabled = false;
            textBox1.Enabled = false;
            textBox2.Enabled = false;

            //Loop
            for (int j = 0; j <= i; j++)
            {
                lblCounter.Text = j.ToString();
                lblCounter.Refresh();//This will update the label text in the UI
                                     //Pause
                System.Threading.Thread.Sleep(sleep);
            }

            btnGoNoThread.Enabled = true;
            textBox1.Enabled = true;
            textBox2.Enabled = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //We need to kill any running threads
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
