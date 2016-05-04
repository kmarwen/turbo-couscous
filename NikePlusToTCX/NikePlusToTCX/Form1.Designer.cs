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
            this.tb_url = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btn_Convert = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dateTimePicker_startDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_endDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_count = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txb_Token
            // 
            this.txb_Token.Location = new System.Drawing.Point(622, 221);
            this.txb_Token.Name = "txb_Token";
            this.txb_Token.Size = new System.Drawing.Size(327, 20);
            this.txb_Token.TabIndex = 0;
            this.txb_Token.Text = "lRIY5Q5VG53HGbjVGao8D3pr9WWB";
            // 
            // lbl_token
            // 
            this.lbl_token.AutoSize = true;
            this.lbl_token.Location = new System.Drawing.Point(12, 224);
            this.lbl_token.Name = "lbl_token";
            this.lbl_token.Size = new System.Drawing.Size(105, 13);
            this.lbl_token.TabIndex = 2;
            this.lbl_token.Text = "AccessToken from : ";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(15, 585);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox1.Size = new System.Drawing.Size(936, 209);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // btn_GetAllActivities
            // 
            this.btn_GetAllActivities.Location = new System.Drawing.Point(15, 247);
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
            this.lbl_URL.Location = new System.Drawing.Point(15, 9);
            this.lbl_URL.Name = "lbl_URL";
            this.lbl_URL.Size = new System.Drawing.Size(29, 13);
            this.lbl_URL.TabIndex = 6;
            this.lbl_URL.Text = "URL";
            // 
            // tb_url
            // 
            this.tb_url.Location = new System.Drawing.Point(73, 6);
            this.tb_url.Name = "tb_url";
            this.tb_url.Size = new System.Drawing.Size(700, 20);
            this.tb_url.TabIndex = 5;
            this.tb_url.Text = "https://api.nike.com/v1/me/sport/activities/running?access_token={access_token}&c" +
    "ount={count}&startDate={startDate}&endDate={endDate}";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(15, 291);
            this.listBox1.Name = "listBox1";
            this.listBox1.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBox1.Size = new System.Drawing.Size(934, 238);
            this.listBox1.TabIndex = 8;
            // 
            // btn_Convert
            // 
            this.btn_Convert.Location = new System.Drawing.Point(15, 539);
            this.btn_Convert.Name = "btn_Convert";
            this.btn_Convert.Size = new System.Drawing.Size(934, 40);
            this.btn_Convert.TabIndex = 9;
            this.btn_Convert.Text = "Try Convert";
            this.btn_Convert.UseVisualStyleBackColor = true;
            this.btn_Convert.Click += new System.EventHandler(this.btn_Convert_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(123, 221);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(413, 20);
            this.textBox1.TabIndex = 10;
            this.textBox1.Text = "https://developer.nike.com/documentation/api-docs/test-console.html";
            // 
            // dateTimePicker_startDate
            // 
            this.dateTimePicker_startDate.Location = new System.Drawing.Point(73, 32);
            this.dateTimePicker_startDate.Name = "dateTimePicker_startDate";
            this.dateTimePicker_startDate.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker_startDate.TabIndex = 11;
            this.dateTimePicker_startDate.ValueChanged += new System.EventHandler(this.dateTimePicker_startDate_ValueChanged);
            // 
            // dateTimePicker_endDate
            // 
            this.dateTimePicker_endDate.Location = new System.Drawing.Point(73, 58);
            this.dateTimePicker_endDate.Name = "dateTimePicker_endDate";
            this.dateTimePicker_endDate.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker_endDate.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Start Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "End Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Nombre d\'activités";
            // 
            // tb_count
            // 
            this.tb_count.Location = new System.Drawing.Point(123, 87);
            this.tb_count.Name = "tb_count";
            this.tb_count.Size = new System.Drawing.Size(97, 20);
            this.tb_count.TabIndex = 5;
            this.tb_count.Text = "10";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(542, 224);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "access_token";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(961, 806);
            this.Controls.Add(this.dateTimePicker_endDate);
            this.Controls.Add(this.dateTimePicker_startDate);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btn_Convert);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbl_URL);
            this.Controls.Add(this.tb_count);
            this.Controls.Add(this.tb_url);
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
        private System.Windows.Forms.TextBox tb_url;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btn_Convert;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker_startDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker_endDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_count;
        private System.Windows.Forms.Label label4;
    }
}

