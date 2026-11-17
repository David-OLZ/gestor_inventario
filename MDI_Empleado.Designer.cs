namespace Practica
{
    partial class MDI_Empleado
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
            menuStrip1 = new MenuStrip();
            Menu_catalogos = new ToolStripMenuItem();
            salirToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { Menu_catalogos });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(5, 2, 0, 2);
            menuStrip1.Size = new Size(700, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // Menu_catalogos
            // 
            // Item_Catalogos_Categorias
            // 
            Item_Catalogos_Categorias = new ToolStripMenuItem();
            Item_Catalogos_Categorias.Name = "Item_Catalogos_Categorias";
            Item_Catalogos_Categorias.Size = new Size(180, 22);
            Item_Catalogos_Categorias.Text = "Categoría";
            Item_Catalogos_Categorias.Click += Item_Catalogos_Categorias_Click;
            // 
            // Item_Catalogos_Productos
            // 
            Item_Catalogos_Productos = new ToolStripMenuItem();
            Item_Catalogos_Productos.Name = "Item_Catalogos_Productos";
            Item_Catalogos_Productos.Size = new Size(180, 22);
            Item_Catalogos_Productos.Text = "Productos";
            Item_Catalogos_Productos.Click += Item_Catalogos_Productos_Click;
            // 
            // salirToolStripMenuItem
            // 
            salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            salirToolStripMenuItem.Size = new Size(139, 22);
            salirToolStripMenuItem.Text = "&Salir";
            salirToolStripMenuItem.Click += salirToolStripMenuItem_Click;

            Menu_catalogos.DropDownItems.AddRange(new ToolStripItem[] { Item_Catalogos_Categorias, Item_Catalogos_Productos, salirToolStripMenuItem });
            Menu_catalogos.Name = "Menu_catalogos";
            Menu_catalogos.Size = new Size(72, 20);
            Menu_catalogos.Text = "&Catalogos";
            // 
            // MDI_Empleado
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 338);
            Controls.Add(menuStrip1);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 2, 3, 2);
            Name = "MDI_Empleado";
            Text = "MDI_Empleado";
            Load += MDI_Empleado_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem Menu_catalogos;
        private ToolStripMenuItem Item_Catalogos_Categorias;
        private ToolStripMenuItem Item_Catalogos_Productos;
        private ToolStripMenuItem salirToolStripMenuItem;
    }
}
