using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ssdump
{
    public partial class EditHostsGUI : Form
    {
        public EditHostsGUI()
        {
            InitializeComponent();
        }

        private void EditHostsGUI_Load(object sender, EventArgs e)
        {
            ltbHosts.DataSource = Properties.Settings.Default.Hosts;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddHost_Click(object sender, EventArgs e)
        {
            string newHost = ShowStringDialogBox("Add Host", "Enter new host:");
            if(newHost.Length > 0)
            {
                if(Properties.Settings.Default.Hosts == null)
                {
                    Properties.Settings.Default.Hosts = new System.Collections.Specialized.StringCollection();
                }
                Properties.Settings.Default.Hosts.Add(newHost);
                Properties.Settings.Default.Save();
                ltbHosts.DataSource = null;
                ltbHosts.DataSource = Properties.Settings.Default.Hosts;
            }
        }

        private void btnEditHost_Click(object sender, EventArgs e)
        {
            string oldHost = (string)ltbHosts.SelectedValue;
            string newHost = ShowStringDialogBox("Edit Host", "Edit host:", oldHost);
            if (newHost.Length > 0 && oldHost != newHost)
            {
                int oldValueIndex = Properties.Settings.Default.Hosts.IndexOf(oldHost);
                Properties.Settings.Default.Hosts.RemoveAt(oldValueIndex);
                Properties.Settings.Default.Hosts.Insert(oldValueIndex, newHost);
                Properties.Settings.Default.Save();
                ltbHosts.DataSource = null;
                ltbHosts.DataSource = Properties.Settings.Default.Hosts;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string oldHost = (string)ltbHosts.SelectedValue;
            DialogResult ConfirmRemove = MessageBox.Show(
                "Are you sure you would like to remove the host: " + oldHost,
                "Are you sure?",
                MessageBoxButtons.OKCancel
            );

            if (ConfirmRemove == DialogResult.OK)
            {
                int oldValueIndex = Properties.Settings.Default.Hosts.IndexOf(oldHost);
                if (oldValueIndex >= 0)
                {
                    Properties.Settings.Default.Hosts.RemoveAt(oldValueIndex);
                    Properties.Settings.Default.Save();
                    ltbHosts.DataSource = null;
                    ltbHosts.DataSource = Properties.Settings.Default.Hosts;
                }
            }
        }

        public static string ShowStringDialogBox(string title, string text, string defaultText = "")
        {
            Form StringDialogBox = new Form()
            {
                Width = 400,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = title,
                StartPosition = FormStartPosition.CenterScreen
            };

            Label txtAddHost = new Label()
            {
                Left = 10,
                Top = 20,
                Text = text
            };

            TextBox textBox = new TextBox()
            {
                Left = 10,
                Top = 40,
                Width = 360,
                Text = defaultText
            };

            Button btnAddHost = new Button()
            {
                Text = "Save",
                Left = 270,
                Width = 100,
                Top = 75,
                DialogResult = DialogResult.OK,
            };
            btnAddHost.Click += (sender, e) => { StringDialogBox.Close(); };

            Button btnCancel = new Button()
            {
                Text = "Cancel",
                Left = 10,
                Width = 100,
                Top = 75,
                DialogResult = DialogResult.Cancel
            };
            btnCancel.Click += (sender, e) => { StringDialogBox.Close(); };

            StringDialogBox.Controls.Add(textBox);
            StringDialogBox.Controls.Add(txtAddHost);
            StringDialogBox.Controls.Add(btnAddHost);
            StringDialogBox.Controls.Add(btnCancel);

            StringDialogBox.AcceptButton = btnAddHost;

            return StringDialogBox.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
