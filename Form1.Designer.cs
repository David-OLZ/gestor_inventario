namespace Practica
{
    partial class login
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            label5 = new Label();
            comboBox1 = new ComboBox();
            txtPassword = new TextBox();
            label4 = new Label();
            txtUser = new TextBox();
            label3 = new Label();
            txtDatabase = new TextBox();
            label2 = new Label();
            txtServer = new TextBox();
            btnBuscarMdf = new Button();
            label1 = new Label();
            cmdAceptar = new Button();
            cmdSalir = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Controls.Add(txtPassword);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(txtUser);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(txtDatabase);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(txtServer);
            groupBox1.Controls.Add(btnBuscarMdf);
            groupBox1.Controls.Add(label1);
            groupBox1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(395, 207);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Login";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(21, 98);
            label5.Name = "label5";
            label5.Size = new Size(43, 15);
            label5.TabIndex = 9;
            label5.Text = "Puesto";
            label5.Click += label5_Click;
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Administrador", "Empleado" });
            comboBox1.Location = new Point(67, 95);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(283, 23);
            comboBox1.TabIndex = 8;
            // 
            // txtPassword
            // 
            txtPassword.Enabled = false;
            txtPassword.Location = new Point(67, 153);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(283, 23);
            txtPassword.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F);
            label4.Location = new Point(4, 156);
            label4.Name = "label4";
            label4.Size = new Size(57, 15);
            label4.TabIndex = 6;
            label4.Text = "Password";
            // 
            // txtUser
            // 
            txtUser.Enabled = false;
            txtUser.Location = new Point(67, 124);
            txtUser.Name = "txtUser";
            txtUser.Size = new Size(283, 23);
            txtUser.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F);
            label3.Location = new Point(31, 127);
            label3.Name = "label3";
            label3.Size = new Size(30, 15);
            label3.TabIndex = 4;
            label3.Text = "User";
            // 
            // txtDatabase
            // 
            txtDatabase.Location = new Point(67, 66);
            txtDatabase.Name = "txtDatabase";
            txtDatabase.Size = new Size(283, 23);
            txtDatabase.TabIndex = 3;
            txtDatabase.Text = "prueba";
            txtDatabase.TextChanged += txtDatabase_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F);
            label2.Location = new Point(6, 69);
            label2.Name = "label2";
            label2.Size = new Size(55, 15);
            label2.TabIndex = 2;
            label2.Text = "Database";
            // 
            // txtServer
            // 
            txtServer.BackColor = Color.White;
            txtServer.BorderStyle = BorderStyle.FixedSingle;
            txtServer.Location = new Point(67, 37);
            txtServer.Name = "txtServer";
            txtServer.Size = new Size(283, 23);
            txtServer.TabIndex = 1;
            txtServer.Text = "LAPTOP-HP-DAVID\\SQLEXPRESS";
            txtServer.TextChanged += txtServer_TextChanged_1;
            // 
            // btnBuscarMdf
            // 
            btnBuscarMdf.Location = new Point(356, 35);
            btnBuscarMdf.Name = "btnBuscarMdf";
            btnBuscarMdf.Size = new Size(33, 23);
            btnBuscarMdf.TabIndex = 10;
            btnBuscarMdf.Text = "...";
            btnBuscarMdf.UseVisualStyleBackColor = true;
            btnBuscarMdf.Click += btnBuscarMdf_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F);
            label1.Location = new Point(22, 40);
            label1.Name = "label1";
            label1.Size = new Size(39, 15);
            label1.TabIndex = 0;
            label1.Text = "Server";
            // 
            // cmdAceptar
            // 
            cmdAceptar.Location = new Point(43, 225);
            cmdAceptar.Name = "cmdAceptar";
            cmdAceptar.Size = new Size(75, 23);
            cmdAceptar.TabIndex = 1;
            cmdAceptar.Text = "&Aceptar";
            cmdAceptar.UseVisualStyleBackColor = true;
            cmdAceptar.Click += cmdAceptar_Click;
            // 
            // cmdSalir
            // 
            cmdSalir.Location = new Point(287, 225);
            cmdSalir.Name = "cmdSalir";
            cmdSalir.Size = new Size(75, 23);
            cmdSalir.TabIndex = 2;
            cmdSalir.Text = "&Salir";
            cmdSalir.UseVisualStyleBackColor = true;
            cmdSalir.Click += cmdSalir_Click;
            // 
            // login
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(419, 260);
            Controls.Add(cmdSalir);
            Controls.Add(cmdAceptar);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Inicio de sesión";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private TextBox txtServer;
        private Button btnBuscarMdf;
        private Label label1;
        private Button cmdAceptar;
        private Button cmdSalir;
        private TextBox txtPassword;
        private Label label4;
        private TextBox txtUser;
        private Label label3;
        private TextBox txtDatabase;
        private Label label2;
        private Label label5;
        private ComboBox comboBox1;
    }
}
