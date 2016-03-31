namespace NikePlusToTCX
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.txb_Token = new System.Windows.Forms.TextBox();
            this.lbl_token = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btn_GetAllActivities = new System.Windows.Forms.Button();
            this.lbl_URL = new System.Windows.Forms.Label();
            this.txb_url = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btn_Convert = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txb_Token
            // 
            this.txb_Token.Location = new System.Drawing.Point(738, 32);
            this.txb_Token.Name = "txb_Token";
            this.txb_Token.Size = new System.Drawing.Size(211, 20);
            this.txb_Token.TabIndex = 0;
            this.txb_Token.Text = "Fa01gHgayZ48x3IhizGtSo4X8Gt7";
            // 
            // lbl_token
            // 
            this.lbl_token.AutoSize = true;
            this.lbl_token.Location = new System.Drawing.Point(12, 35);
            this.lbl_token.Name = "lbl_token";
            this.lbl_token.Size = new System.Drawing.Size(105, 13);
            this.lbl_token.TabIndex = 2;
            this.lbl_token.Text = "AccessToken from : ";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(15, 396);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox1.Size = new System.Drawing.Size(936, 209);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // btn_GetAllActivities
            // 
            this.btn_GetAllActivities.Location = new System.Drawing.Point(15, 58);
            this.btn_GetAllActivities.Name = "btn_GetAllActivities";
            this.btn_GetAllActivities.Size = new System.Drawing.Size(934, 40);
            this.btn_GetAllActivities.TabIndex = 4;
            this.btn_GetAllActivities.Text = "GetActivities";
            this.btn_GetAllActivities.UseVisualStyleBackColor = true;
            this.btn_GetAllActivities.Click += new System.EventHandler(this.btn_GetAllActivities_Click);
            // 
            // lbl_URL
            // 
            this.lbl_URL.AutoSize = true;
            this.lbl_URL.Location = new System.Drawing.Point(12, 9);
            this.lbl_URL.Name = "lbl_URL";
            this.lbl_URL.Size = new System.Drawing.Size(29, 13);
            this.lbl_URL.TabIndex = 6;
            this.lbl_URL.Text = "URL";
            // 
            // txb_url
            // 
            this.txb_url.Location = new System.Drawing.Point(56, 6);
            this.txb_url.Name = "txb_url";
            this.txb_url.Size = new System.Drawing.Size(893, 20);
            this.txb_url.TabIndex = 5;
            this.txb_url.Text = "https://api.nike.com/v1/me/sport/activities/running?access_token={access_token}&c" +
    "ount=10&startDate=2016-03-20&endDate=2016-03-31";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(15, 102);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(934, 238);
            this.listBox1.TabIndex = 8;
            // 
            // btn_Convert
            // 
            this.btn_Convert.Location = new System.Drawing.Point(15, 350);
            this.btn_Convert.Name = "btn_Convert";
            this.btn_Convert.Size = new System.Drawing.Size(934, 40);
            this.btn_Convert.TabIndex = 9;
            this.btn_Convert.Text = "Try Convert";
            this.btn_Convert.UseVisualStyleBackColor = true;
            this.btn_Convert.Click += new System.EventHandler(this.btn_Convert_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(123, 32);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(609, 20);
            this.textBox1.TabIndex = 10;
            this.textBox1.Text = "https://developer.nike.com/documentation/api-docs/test-console.html";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(961, 617);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btn_Convert);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.lbl_URL);
            this.Controls.Add(this.txb_url);
            this.Controls.Add(this.btn_GetAllActivities);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.lbl_token);
            this.Controls.Add(this.txb_Token);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txb_Token;
        private System.Windows.Forms.Label lbl_token;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btn_GetAllActivities;
        private System.Windows.Forms.Label lbl_URL;
        private System.Windows.Forms.TextBox txb_url;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btn_Convert;
        private System.Windows.Forms.TextBox textBox1;
    }
}

