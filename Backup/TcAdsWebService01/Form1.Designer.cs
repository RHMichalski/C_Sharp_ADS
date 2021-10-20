namespace TcAdsWebService01
{
    partial class Form1
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
            this.lblWebServiceUrl = new System.Windows.Forms.Label();
            this.txtWebServiceUrl = new System.Windows.Forms.TextBox();
            this.lblAdsPort = new System.Windows.Forms.Label();
            this.lblAmsNetID = new System.Windows.Forms.Label();
            this.txtAdsPort = new System.Windows.Forms.TextBox();
            this.txtAmsNetId = new System.Windows.Forms.TextBox();
            this.lblBool = new System.Windows.Forms.Label();
            this.lblInt = new System.Windows.Forms.Label();
            this.lblString = new System.Windows.Forms.Label();
            this.txtBool = new System.Windows.Forms.TextBox();
            this.txtInt = new System.Windows.Forms.TextBox();
            this.txtString = new System.Windows.Forms.TextBox();
            this.btnRead = new System.Windows.Forms.Button();
            this.btnWrite = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblWebServiceUrl
            // 
            this.lblWebServiceUrl.Location = new System.Drawing.Point(8, 12);
            this.lblWebServiceUrl.Name = "lblWebServiceUrl";
            this.lblWebServiceUrl.Size = new System.Drawing.Size(96, 16);
            this.lblWebServiceUrl.TabIndex = 27;
            this.lblWebServiceUrl.Text = "WebServiceUrl:";
            // 
            // txtWebServiceUrl
            // 
            this.txtWebServiceUrl.Location = new System.Drawing.Point(107, 9);
            this.txtWebServiceUrl.Name = "txtWebServiceUrl";
            this.txtWebServiceUrl.Size = new System.Drawing.Size(314, 20);
            this.txtWebServiceUrl.TabIndex = 26;
            // 
            // lblAdsPort
            // 
            this.lblAdsPort.Location = new System.Drawing.Point(8, 65);
            this.lblAdsPort.Name = "lblAdsPort";
            this.lblAdsPort.Size = new System.Drawing.Size(96, 16);
            this.lblAdsPort.TabIndex = 25;
            this.lblAdsPort.Text = "ADS Port:";
            // 
            // lblAmsNetID
            // 
            this.lblAmsNetID.Location = new System.Drawing.Point(8, 41);
            this.lblAmsNetID.Name = "lblAmsNetID";
            this.lblAmsNetID.Size = new System.Drawing.Size(96, 16);
            this.lblAmsNetID.TabIndex = 24;
            this.lblAmsNetID.Text = "Ams Net ID:";
            // 
            // txtAdsPort
            // 
            this.txtAdsPort.Location = new System.Drawing.Point(108, 62);
            this.txtAdsPort.Name = "txtAdsPort";
            this.txtAdsPort.Size = new System.Drawing.Size(115, 20);
            this.txtAdsPort.TabIndex = 23;
            // 
            // txtAmsNetId
            // 
            this.txtAmsNetId.Location = new System.Drawing.Point(108, 38);
            this.txtAmsNetId.Name = "txtAmsNetId";
            this.txtAmsNetId.Size = new System.Drawing.Size(115, 20);
            this.txtAmsNetId.TabIndex = 22;
            // 
            // lblBool
            // 
            this.lblBool.Location = new System.Drawing.Point(8, 115);
            this.lblBool.Name = "lblBool";
            this.lblBool.Size = new System.Drawing.Size(100, 23);
            this.lblBool.TabIndex = 14;
            this.lblBool.Text = "VarBool:";
            // 
            // lblInt
            // 
            this.lblInt.Location = new System.Drawing.Point(8, 141);
            this.lblInt.Name = "lblInt";
            this.lblInt.Size = new System.Drawing.Size(100, 17);
            this.lblInt.TabIndex = 15;
            this.lblInt.Text = "VarInt:";
            // 
            // lblString
            // 
            this.lblString.Location = new System.Drawing.Point(8, 165);
            this.lblString.Name = "lblString";
            this.lblString.Size = new System.Drawing.Size(100, 16);
            this.lblString.TabIndex = 16;
            this.lblString.Text = "VarString:";
            // 
            // txtBool
            // 
            this.txtBool.Location = new System.Drawing.Point(108, 112);
            this.txtBool.Name = "txtBool";
            this.txtBool.Size = new System.Drawing.Size(115, 20);
            this.txtBool.TabIndex = 17;
            // 
            // txtInt
            // 
            this.txtInt.Location = new System.Drawing.Point(108, 138);
            this.txtInt.Name = "txtInt";
            this.txtInt.Size = new System.Drawing.Size(115, 20);
            this.txtInt.TabIndex = 18;
            // 
            // txtString
            // 
            this.txtString.Location = new System.Drawing.Point(108, 162);
            this.txtString.Name = "txtString";
            this.txtString.Size = new System.Drawing.Size(115, 20);
            this.txtString.TabIndex = 19;
            // 
            // btnRead
            // 
            this.btnRead.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnRead.Location = new System.Drawing.Point(292, 115);
            this.btnRead.Name = "btnRead";
            this.btnRead.Size = new System.Drawing.Size(75, 23);
            this.btnRead.TabIndex = 20;
            this.btnRead.Text = "&Read";
            this.btnRead.Click += new System.EventHandler(this.btnRead_Click);
            // 
            // btnWrite
            // 
            this.btnWrite.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnWrite.Location = new System.Drawing.Point(295, 158);
            this.btnWrite.Name = "btnWrite";
            this.btnWrite.Size = new System.Drawing.Size(72, 23);
            this.btnWrite.TabIndex = 21;
            this.btnWrite.Text = "&Write";
            this.btnWrite.Click += new System.EventHandler(this.btnWrite_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 201);
            this.Controls.Add(this.lblWebServiceUrl);
            this.Controls.Add(this.txtWebServiceUrl);
            this.Controls.Add(this.lblAdsPort);
            this.Controls.Add(this.lblAmsNetID);
            this.Controls.Add(this.txtAdsPort);
            this.Controls.Add(this.txtAmsNetId);
            this.Controls.Add(this.lblBool);
            this.Controls.Add(this.lblInt);
            this.Controls.Add(this.lblString);
            this.Controls.Add(this.txtBool);
            this.Controls.Add(this.txtInt);
            this.Controls.Add(this.txtString);
            this.Controls.Add(this.btnRead);
            this.Controls.Add(this.btnWrite);
            this.Name = "Form1";
            this.Text = "C# Sample for TcAdsWebservice";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblWebServiceUrl;
        private System.Windows.Forms.TextBox txtWebServiceUrl;
        private System.Windows.Forms.Label lblAdsPort;
        private System.Windows.Forms.Label lblAmsNetID;
        private System.Windows.Forms.TextBox txtAdsPort;
        private System.Windows.Forms.TextBox txtAmsNetId;
        private System.Windows.Forms.Label lblBool;
        private System.Windows.Forms.Label lblInt;
        private System.Windows.Forms.Label lblString;
        private System.Windows.Forms.TextBox txtBool;
        private System.Windows.Forms.TextBox txtInt;
        private System.Windows.Forms.TextBox txtString;
        private System.Windows.Forms.Button btnRead;
        private System.Windows.Forms.Button btnWrite;
    }
}

