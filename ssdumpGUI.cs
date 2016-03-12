using Mono.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ssdump
{
    public partial class ssdumpGUI : Form
    {
        DumpProcessor processor = new DumpProcessor();

        public ssdumpGUI()
        {
            InitializeComponent();
            cboHostList.DataSource = Properties.Settings.Default.Hosts;
        }

        private void ssdumpGUI_Activated(object sender, System.EventArgs e)
        {
            cboHostList.DataSource = null;
            cboHostList.DataSource = Properties.Settings.Default.Hosts;
        }

        private void editHostsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditHostsGUI HostEditForm = new EditHostsGUI();
            HostEditForm.StartPosition = FormStartPosition.CenterScreen;
            HostEditForm.Show(this);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ckbNoData_CheckedChanged(object sender, EventArgs e)
        {
            processor.NoData = ckbNoData.Checked;
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            Stream myStream;

            saveFileDialog1.Filter = "sql files (*.sql)|*.sql|All files(*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    myStream.Close();
                }
            }
        }

        /// <summary>
        /// Set Windows Authentication flag based on if there's something in the username field.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            processor.Username = txtUsername.Text;
            processor.UseWindowsAuthentication = (txtUsername.TextLength == 0);
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            processor.Password = txtPassword.Text;
        }

        private void txtDatabase_TextChanged(object sender, EventArgs e)
        {
            processor.DatabaseName = txtDatabase.Text;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (processor.Tables == null)
            {
                processor.Tables = new List<string>();
            } else
            {
                processor.Tables.Clear();
            }
            processor.Tables.AddRange(txtTables.Lines);
        }
    }
}
