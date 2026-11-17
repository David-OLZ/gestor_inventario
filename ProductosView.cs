using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Practica
{
    public class ProductosView : Form
    {
        private DataGridView dgv;

        public ProductosView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Ver Productos";
            this.ClientSize = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterParent;

            dgv = new DataGridView()
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            this.Controls.Add(dgv);
            this.Load += ProductosView_Load;
        }

        private void ProductosView_Load(object? sender, EventArgs e)
        {
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Nombre", typeof(string));
            table.Columns.Add("Descripción", typeof(string));
            table.Columns.Add("Precio", typeof(decimal));

            table.Rows.Add(1, "Producto A", "Descripción A", 10.5m);
            table.Rows.Add(2, "Producto B", "Descripción B", 20.0m);
            table.Rows.Add(3, "Producto C", "Descripción C", 15.75m);

            dgv.DataSource = table;
        }
    }
}
