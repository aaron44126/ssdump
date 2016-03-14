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

            processor.Host = cboHostList.SelectedText;
            txtUsername.Enabled = !ckbUseWinAuth.Checked;
            txtPassword.Enabled = !ckbUseWinAuth.Checked;
            if (txtMaxPacket.Text.Length == 0)
            {
                processor.MaxPacket = -1;
            }
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
            saveFileDialog1.Filter = "sql files (*.sql)|*.sql|All files(*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                processor.FilePath = saveFileDialog1.FileName;
                try
                {
                    processor.Execute();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERORR: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void txtTimeout_TextChanged_1(object sender, EventArgs e)
        {
            int timeout;
            int.TryParse(txtTimeout.Text, out timeout);
            processor.Timeout = timeout;
        }

        private void ckbEncryption_CheckedChanged(object sender, EventArgs e)
        {
            processor.UseEncryption = ckbEncryption.Checked;
        }

        private void txtMaxPacket_TextChanged(object sender, EventArgs e)
        {
            if (txtMaxPacket.Text.Length == 0)
            {
                processor.MaxPacket = -1;
            }
            else {
                int maxPacket;
                int.TryParse(txtMaxPacket.Text, out maxPacket);
                processor.MaxPacket = maxPacket;
            }
        }

        private void ckbCreateDatabase_CheckedChanged(object sender, EventArgs e)
        {
            processor.IncludeCreateDatabase = ckbCreateDatabase.Checked;
        }

        private void ckbCreateUser_CheckedChanged(object sender, EventArgs e)
        {
            processor.IncludeUsers = ckbCreateUser.Checked;
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ckbUseWinAuth_CheckedChanged(object sender, EventArgs e)
        {
            txtUsername.Enabled = !ckbUseWinAuth.Checked;
            txtPassword.Enabled = !ckbUseWinAuth.Checked;
            txtUsername.Text = "";
            txtPassword.Text = "";
        }
    }
}
