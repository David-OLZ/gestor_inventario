namespace Practica
{
    partial class Proveedores
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
            groupBox1 = new GroupBox();
            fecha_registro = new DateTimePicker();
            delete = new Button();
            button2 = new Button();
            insert = new Button();
            txt_proveedores = new TextBox();
            txt_direccion = new TextBox();
            txt_id = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(fecha_registro);
            groupBox1.Controls.Add(delete);
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(insert);
            groupBox1.Controls.Add(txt_proveedores);
            groupBox1.Controls.Add(txt_direccion);
            groupBox1.Controls.Add(txt_id);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(21, 11);
            groupBox1.Margin = new Padding(3, 2, 3, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 2, 3, 2);
            groupBox1.Size = new Size(351, 214);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            // 
            // fecha_registro
            // 
            fecha_registro.AccessibleDescription = "";
            fecha_registro.Format = DateTimePickerFormat.Short;
            fecha_registro.Location = new Point(227, 23);
            fecha_registro.Name = "fecha_registro";
            fecha_registro.Size = new Size(99, 23);
            fecha_registro.TabIndex = 9;
            fecha_registro.ValueChanged += fecha_registro_ValueChanged;
            // 
            // delete
            // 
            delete.Location = new Point(244, 176);
            delete.Margin = new Padding(3, 2, 3, 2);
            delete.Name = "delete";
            delete.Size = new Size(82, 22);
            delete.TabIndex = 8;
            delete.Text = "&Eliminar";
            delete.UseVisualStyleBackColor = true;
            delete.Click += deleteClick;
            // 
            // button2
            // 
            button2.Location = new Point(145, 176);
            button2.Margin = new Padding(3, 2, 3, 2);
            button2.Name = "button2";
            button2.Size = new Size(82, 22);
            button2.TabIndex = 7;
            button2.Text = "&Buscar";
            button2.UseVisualStyleBackColor = true;
            // 
            // insert
            // 
            insert.Location = new Point(47, 176);
            insert.Margin = new Padding(3, 2, 3, 2);
            insert.Name = "insert";
            insert.Size = new Size(82, 22);
            insert.TabIndex = 6;
            insert.Text = "&Insertar";
            insert.UseVisualStyleBackColor = true;
            insert.Click += InsertarClick;
            // 
            // txt_proveedores
            // 
            txt_proveedores.Location = new Point(94, 62);
            txt_proveedores.Margin = new Padding(3, 2, 3, 2);
            txt_proveedores.Name = "txt_proveedores";
            txt_proveedores.Size = new Size(110, 23);
            txt_proveedores.TabIndex = 5;
            // 
            // txt_direccion
            // 
            txt_direccion.Location = new Point(94, 101);
            txt_direccion.Margin = new Padding(3, 2, 3, 2);
            txt_direccion.Name = "txt_direccion";
            txt_direccion.Size = new Size(110, 23);
            txt_direccion.TabIndex = 4;
            // 
            // txt_id
            // 
            txt_id.Location = new Point(94, 21);
            txt_id.Margin = new Padding(3, 2, 3, 2);
            txt_id.Name = "txt_id";
            txt_id.Size = new Size(110, 23);
            txt_id.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(18, 104);
            label3.Name = "label3";
            label3.Size = new Size(57, 15);
            label3.TabIndex = 2;
            label3.Text = "Direccion";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(2, 64);
            label2.Name = "label2";
            label2.Size = new Size(72, 15);
            label2.TabIndex = 1;
            label2.Text = "Proveedores";
            label2.Click += label2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(60, 23);
            label1.Name = "label1";
            label1.Size = new Size(18, 15);
            label1.TabIndex = 0;
            label1.Text = "ID";
            label1.Click += label1_Click;
            // 
            // Proveedores
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(393, 243);
            Controls.Add(groupBox1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Proveedores";
            Text = "Proveedores";
            Load += Proveedores_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button delete;
        private Button button2;
        private Button insert;
        private TextBox txt_proveedores;
        private TextBox txt_direccion;
        private TextBox txt_id;
        private DateTimePicker fecha_registro;
    }
}