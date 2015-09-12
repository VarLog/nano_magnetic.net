namespace WinNano
{
    partial class Form2
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
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.rDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eTADataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hdipDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qintDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clusterBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mxDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.myDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mzDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.atomBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.clusterBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.atomBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.rDataGridViewTextBoxColumn1,
            this.xDataGridViewTextBoxColumn1,
            this.yDataGridViewTextBoxColumn1,
            this.zDataGridViewTextBoxColumn1,
            this.vDataGridViewTextBoxColumn1,
            this.mxDataGridViewTextBoxColumn,
            this.myDataGridViewTextBoxColumn,
            this.mzDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.atomBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(13, 109);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(899, 418);
            this.dataGridView1.TabIndex = 0;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AutoGenerateColumns = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.rDataGridViewTextBoxColumn,
            this.xDataGridViewTextBoxColumn,
            this.yDataGridViewTextBoxColumn,
            this.zDataGridViewTextBoxColumn,
            this.vDataGridViewTextBoxColumn,
            this.eTADataGridViewTextBoxColumn,
            this.hdipDataGridViewTextBoxColumn,
            this.qintDataGridViewTextBoxColumn});
            this.dataGridView2.DataSource = this.clusterBindingSource;
            this.dataGridView2.Location = new System.Drawing.Point(13, 31);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(899, 72);
            this.dataGridView2.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(94, 2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(176, 2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 4;
            // 
            // rDataGridViewTextBoxColumn
            // 
            this.rDataGridViewTextBoxColumn.DataPropertyName = "R";
            this.rDataGridViewTextBoxColumn.HeaderText = "R";
            this.rDataGridViewTextBoxColumn.Name = "rDataGridViewTextBoxColumn";
            // 
            // xDataGridViewTextBoxColumn
            // 
            this.xDataGridViewTextBoxColumn.DataPropertyName = "X";
            this.xDataGridViewTextBoxColumn.HeaderText = "X";
            this.xDataGridViewTextBoxColumn.Name = "xDataGridViewTextBoxColumn";
            // 
            // yDataGridViewTextBoxColumn
            // 
            this.yDataGridViewTextBoxColumn.DataPropertyName = "Y";
            this.yDataGridViewTextBoxColumn.HeaderText = "Y";
            this.yDataGridViewTextBoxColumn.Name = "yDataGridViewTextBoxColumn";
            // 
            // zDataGridViewTextBoxColumn
            // 
            this.zDataGridViewTextBoxColumn.DataPropertyName = "Z";
            this.zDataGridViewTextBoxColumn.HeaderText = "Z";
            this.zDataGridViewTextBoxColumn.Name = "zDataGridViewTextBoxColumn";
            // 
            // vDataGridViewTextBoxColumn
            // 
            this.vDataGridViewTextBoxColumn.DataPropertyName = "V";
            this.vDataGridViewTextBoxColumn.HeaderText = "V";
            this.vDataGridViewTextBoxColumn.Name = "vDataGridViewTextBoxColumn";
            // 
            // eTADataGridViewTextBoxColumn
            // 
            this.eTADataGridViewTextBoxColumn.DataPropertyName = "ETA";
            this.eTADataGridViewTextBoxColumn.HeaderText = "ETA";
            this.eTADataGridViewTextBoxColumn.Name = "eTADataGridViewTextBoxColumn";
            // 
            // hdipDataGridViewTextBoxColumn
            // 
            this.hdipDataGridViewTextBoxColumn.DataPropertyName = "Hdip";
            this.hdipDataGridViewTextBoxColumn.HeaderText = "Hdip";
            this.hdipDataGridViewTextBoxColumn.Name = "hdipDataGridViewTextBoxColumn";
            // 
            // qintDataGridViewTextBoxColumn
            // 
            this.qintDataGridViewTextBoxColumn.DataPropertyName = "Qint";
            this.qintDataGridViewTextBoxColumn.HeaderText = "Qint";
            this.qintDataGridViewTextBoxColumn.Name = "qintDataGridViewTextBoxColumn";
            // 
            // clusterBindingSource
            // 
            this.clusterBindingSource.DataSource = typeof(WinNano.Cluster);
            // 
            // rDataGridViewTextBoxColumn1
            // 
            this.rDataGridViewTextBoxColumn1.DataPropertyName = "R";
            this.rDataGridViewTextBoxColumn1.HeaderText = "R";
            this.rDataGridViewTextBoxColumn1.Name = "rDataGridViewTextBoxColumn1";
            // 
            // xDataGridViewTextBoxColumn1
            // 
            this.xDataGridViewTextBoxColumn1.DataPropertyName = "X";
            this.xDataGridViewTextBoxColumn1.HeaderText = "X";
            this.xDataGridViewTextBoxColumn1.Name = "xDataGridViewTextBoxColumn1";
            // 
            // yDataGridViewTextBoxColumn1
            // 
            this.yDataGridViewTextBoxColumn1.DataPropertyName = "Y";
            this.yDataGridViewTextBoxColumn1.HeaderText = "Y";
            this.yDataGridViewTextBoxColumn1.Name = "yDataGridViewTextBoxColumn1";
            // 
            // zDataGridViewTextBoxColumn1
            // 
            this.zDataGridViewTextBoxColumn1.DataPropertyName = "Z";
            this.zDataGridViewTextBoxColumn1.HeaderText = "Z";
            this.zDataGridViewTextBoxColumn1.Name = "zDataGridViewTextBoxColumn1";
            // 
            // vDataGridViewTextBoxColumn1
            // 
            this.vDataGridViewTextBoxColumn1.DataPropertyName = "V";
            this.vDataGridViewTextBoxColumn1.HeaderText = "V";
            this.vDataGridViewTextBoxColumn1.Name = "vDataGridViewTextBoxColumn1";
            // 
            // mxDataGridViewTextBoxColumn
            // 
            this.mxDataGridViewTextBoxColumn.DataPropertyName = "Mx";
            this.mxDataGridViewTextBoxColumn.HeaderText = "Mx";
            this.mxDataGridViewTextBoxColumn.Name = "mxDataGridViewTextBoxColumn";
            // 
            // myDataGridViewTextBoxColumn
            // 
            this.myDataGridViewTextBoxColumn.DataPropertyName = "My";
            this.myDataGridViewTextBoxColumn.HeaderText = "My";
            this.myDataGridViewTextBoxColumn.Name = "myDataGridViewTextBoxColumn";
            // 
            // mzDataGridViewTextBoxColumn
            // 
            this.mzDataGridViewTextBoxColumn.DataPropertyName = "Mz";
            this.mzDataGridViewTextBoxColumn.HeaderText = "Mz";
            this.mzDataGridViewTextBoxColumn.Name = "mzDataGridViewTextBoxColumn";
            // 
            // atomBindingSource
            // 
            this.atomBindingSource.DataSource = typeof(WinNano.Atom);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(837, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 571);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form2";
            this.Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.clusterBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.atomBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn rDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn xDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn yDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn zDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn vDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eTADataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hdipDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn qintDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource clusterBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn rDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn xDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn yDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn zDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn vDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn mxDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn myDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mzDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource atomBindingSource;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button3;
    }
}