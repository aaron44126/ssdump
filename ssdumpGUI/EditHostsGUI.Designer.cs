namespace ssdump
{
    partial class EditHostsGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ltbHosts = new System.Windows.Forms.ListBox();
            this.lblHosts = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAddHost = new System.Windows.Forms.Button();
            this.btnEditHost = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ltbHosts
            // 
            this.ltbHosts.FormattingEnabled = true;
            this.ltbHosts.Location = new System.Drawing.Point(12, 37);
            this.ltbHosts.Name = "ltbHosts";
            this.ltbHosts.Size = new System.Drawing.Size(164, 147);
            this.ltbHosts.TabIndex = 0;
            // 
            // lblHosts
            // 
            this.lblHosts.AutoSize = true;
            this.lblHosts.Location = new System.Drawing.Point(13, 18);
            this.lblHosts.Name = "lblHosts";
            this.lblHosts.Size = new System.Drawing.Size(37, 13);
            this.lblHosts.TabIndex = 1;
            this.lblHosts.Text = "Hosts:";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(182, 201);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAddHost
            // 
            this.btnAddHost.Location = new System.Drawing.Point(182, 37);
            this.btnAddHost.Name = "btnAddHost";
            this.btnAddHost.Size = new System.Drawing.Size(90, 23);
            this.btnAddHost.TabIndex = 3;
            this.btnAddHost.Text = "Add Host";
            this.btnAddHost.UseVisualStyleBackColor = true;
            this.btnAddHost.Click += new System.EventHandler(this.btnAddHost_Click);
            // 
            // btnEditHost
            // 
            this.btnEditHost.Location = new System.Drawing.Point(182, 66);
            this.btnEditHost.Name = "btnEditHost";
            this.btnEditHost.Size = new System.Drawing.Size(90, 23);
            this.btnEditHost.TabIndex = 4;
            this.btnEditHost.Text = "Edit Host";
            this.btnEditHost.UseVisualStyleBackColor = true;
            this.btnEditHost.Click += new System.EventHandler(this.btnEditHost_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(182, 95);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Remove Host";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // EditHostsGUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 235);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnEditHost);
            this.Controls.Add(this.btnAddHost);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblHosts);
            this.Controls.Add(this.ltbHosts);
            this.Name = "EditHostsGUI";
            this.Text = "Edit Hosts";
            this.Load += new System.EventHandler(this.EditHostsGUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ltbHosts;
        private System.Windows.Forms.Label lblHosts;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnAddHost;
        private System.Windows.Forms.Button btnEditHost;
        private System.Windows.Forms.Button button1;
    }
}