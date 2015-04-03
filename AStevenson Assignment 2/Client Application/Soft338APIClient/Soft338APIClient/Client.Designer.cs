namespace Soft338APIClient
{
    partial class Client
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Client));
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lstAllEvents = new System.Windows.Forms.ListView();
            this.event_id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.event_name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.event_address = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.event_postcode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.event_date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.event_time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtResponseCode = new System.Windows.Forms.TextBox();
            this.txtResponseMessage = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtResponseObjects = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboxUrl = new System.Windows.Forms.ComboBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtQueryString = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cboxVerbs = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label2.Location = new System.Drawing.Point(495, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(195, 24);
            this.label2.TabIndex = 9;
            this.label2.Text = "SOFT338 API Client";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(48, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1168, 32);
            this.label1.TabIndex = 8;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lstAllEvents);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(603, 151);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(602, 502);
            this.groupBox3.TabIndex = 7;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Response Events";
            // 
            // lstAllEvents
            // 
            this.lstAllEvents.AllowColumnReorder = true;
            this.lstAllEvents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.event_id,
            this.event_name,
            this.event_address,
            this.event_postcode,
            this.event_date,
            this.event_time});
            this.lstAllEvents.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lstAllEvents.LabelEdit = true;
            this.lstAllEvents.Location = new System.Drawing.Point(21, 19);
            this.lstAllEvents.Name = "lstAllEvents";
            this.lstAllEvents.Size = new System.Drawing.Size(564, 460);
            this.lstAllEvents.TabIndex = 0;
            this.lstAllEvents.UseCompatibleStateImageBehavior = false;
            this.lstAllEvents.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lstAllEvents_ItemSelectionChanged);
            // 
            // event_id
            // 
            this.event_id.Text = "event_id";
            this.event_id.Width = 92;
            // 
            // event_name
            // 
            this.event_name.Text = "name";
            this.event_name.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.event_name.Width = 94;
            // 
            // event_address
            // 
            this.event_address.Text = "address";
            this.event_address.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.event_address.Width = 94;
            // 
            // event_postcode
            // 
            this.event_postcode.Text = "postcode";
            this.event_postcode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.event_postcode.Width = 94;
            // 
            // event_date
            // 
            this.event_date.Text = "date";
            this.event_date.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.event_date.Width = 94;
            // 
            // event_time
            // 
            this.event_time.Text = "time";
            this.event_time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.event_time.Width = 92;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtResponseCode);
            this.groupBox2.Controls.Add(this.txtResponseMessage);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtResponseObjects);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(51, 400);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(514, 253);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Response Details";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(41, 39);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 16);
            this.label8.TabIndex = 5;
            this.label8.Text = "Status";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(41, 92);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 16);
            this.label7.TabIndex = 4;
            this.label7.Text = "Message";
            // 
            // txtResponseCode
            // 
            this.txtResponseCode.Location = new System.Drawing.Point(150, 39);
            this.txtResponseCode.Name = "txtResponseCode";
            this.txtResponseCode.Size = new System.Drawing.Size(348, 22);
            this.txtResponseCode.TabIndex = 3;
            // 
            // txtResponseMessage
            // 
            this.txtResponseMessage.Location = new System.Drawing.Point(150, 89);
            this.txtResponseMessage.Multiline = true;
            this.txtResponseMessage.Name = "txtResponseMessage";
            this.txtResponseMessage.Size = new System.Drawing.Size(348, 49);
            this.txtResponseMessage.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 155);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 48);
            this.label5.TabIndex = 1;
            this.label5.Text = "Unedited \r\nResponse\r\n(Json)";
            // 
            // txtResponseObjects
            // 
            this.txtResponseObjects.Location = new System.Drawing.Point(150, 155);
            this.txtResponseObjects.Multiline = true;
            this.txtResponseObjects.Name = "txtResponseObjects";
            this.txtResponseObjects.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResponseObjects.Size = new System.Drawing.Size(348, 75);
            this.txtResponseObjects.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox1.Controls.Add(this.cboxUrl);
            this.groupBox1.Controls.Add(this.btnSend);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtQueryString);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cboxVerbs);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(51, 151);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(514, 224);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parameters";
            // 
            // cboxUrl
            // 
            this.cboxUrl.FormattingEnabled = true;
            this.cboxUrl.Items.AddRange(new object[] {
            "events",
            "events/show",
            "events/create",
            "events/delete",
            "events/update",
            "search_places"});
            this.cboxUrl.Location = new System.Drawing.Point(150, 87);
            this.cboxUrl.Name = "cboxUrl";
            this.cboxUrl.Size = new System.Drawing.Size(348, 24);
            this.cboxUrl.TabIndex = 8;
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnSend.Location = new System.Drawing.Point(415, 173);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(83, 35);
            this.btnSend.TabIndex = 7;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(41, 142);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 16);
            this.label6.TabIndex = 6;
            this.label6.Text = "Query String";
            // 
            // txtQueryString
            // 
            this.txtQueryString.Location = new System.Drawing.Point(150, 142);
            this.txtQueryString.Name = "txtQueryString";
            this.txtQueryString.Size = new System.Drawing.Size(348, 22);
            this.txtQueryString.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(25, 16);
            this.label4.TabIndex = 2;
            this.label4.Text = "Url";
            // 
            // cboxVerbs
            // 
            this.cboxVerbs.AllowDrop = true;
            this.cboxVerbs.FormattingEnabled = true;
            this.cboxVerbs.Items.AddRange(new object[] {
            "GET",
            "POST",
            "PUT",
            "DELETE"});
            this.cboxVerbs.Location = new System.Drawing.Point(150, 43);
            this.cboxVerbs.Name = "cboxVerbs";
            this.cboxVerbs.Size = new System.Drawing.Size(121, 24);
            this.cboxVerbs.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Http verb";
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(1264, 682);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Client";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Client_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListView lstAllEvents;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboxVerbs;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ColumnHeader event_id;
        private System.Windows.Forms.ColumnHeader event_name;
        private System.Windows.Forms.ColumnHeader event_address;
        private System.Windows.Forms.ColumnHeader event_postcode;
        private System.Windows.Forms.ColumnHeader event_date;
        private System.Windows.Forms.ColumnHeader event_time;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtQueryString;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtResponseCode;
        private System.Windows.Forms.TextBox txtResponseMessage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtResponseObjects;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ComboBox cboxUrl;
    }
}

