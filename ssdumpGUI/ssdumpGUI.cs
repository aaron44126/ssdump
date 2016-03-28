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
        public ssdumpGUI()
        {
            InitializeComponent();
            cboHostList.DataSource = Properties.Settings.Default.Hosts;
            txtUsername.Enabled = !ckbUseWinAuth.Checked;
            txtPassword.Enabled = !ckbUseWinAuth.Checked;
            lblVersion.Text = "v" + DumpProcessor.ProgramVersion;
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

        private void btnExecute_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "sql files (*.sql)|*.sql|All files(*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = txtDatabase.Text + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                DumpProcessor processor = GetProcessorWithValues();
                processor.FilePath = saveFileDialog1.FileName;
                Form processing = new ProcessingForm();
                processing.Show(this);
                processing.Refresh();
                try
                {
                    processor.Execute();
                    processing.Close();
                    MessageBox.Show("SQL Server Dump Complete", "Complete", MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
                }
                catch (Exception ex)
                {
                    processing.Close();
                    MessageBox.Show("ERORR: " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
                
            }
        }

        private DumpProcessor GetProcessorWithValues()
        {
            DumpProcessor _processor = new DumpProcessor();

            _processor.Host = cboHostList.Text;
            _processor.UseWindowsAuthentication = ckbUseWinAuth.Checked;
            _processor.Username = txtUsername.Text;
            _processor.Password = txtPassword.Text;
            _processor.DatabaseName = txtDatabase.Text;

            _processor.Tables = new List<string>();
            _processor.Tables.AddRange(txtTables.Lines);

            _processor.NoData = ckbNoData.Checked;
            _processor.IncludeCreateDatabase = ckbCreateDatabase.Checked;
            _processor.IncludeUsers = ckbCreateUser.Checked;

            _processor.UseEncryption = ckbEncryption.Checked;
            int timeout;
            int.TryParse(txtTimeout.Text, out timeout);
            _processor.Timeout = timeout;

            if (txtMaxPacket.Text.Length == 0)
            {
                _processor.MaxPacket = -1;
            }
            else {
                int maxPacket;
                int.TryParse(txtMaxPacket.Text, out maxPacket);
                _processor.MaxPacket = maxPacket;
            }

            return _processor;
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
