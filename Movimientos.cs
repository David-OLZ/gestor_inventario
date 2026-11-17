using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Practica
{
    public class Movimientos : Form
    {
        private DataGridView dgv;

        public Movimientos()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Movimientos";
            this.ClientSize = new Size(700, 400);
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

            Load += Movimientos_Load;
        }

        private void Movimientos_Load(object? sender, EventArgs e)
        {
            // Ejemplo: crear una tabla con movimientos y enlazarla al DataGridView.
            var table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Fecha", typeof(DateTime));
            table.Columns.Add("Tipo", typeof(string));
            table.Columns.Add("Producto", typeof(string));
            table.Columns.Add("Cantidad", typeof(decimal));

            // Filas de ejemplo
            table.Rows.Add(1, DateTime.Now.AddDays(-2), "Entrada", "Producto A", 10);
            table.Rows.Add(2, DateTime.Now.AddDays(-1), "Salida", "Producto B", 3);
            table.Rows.Add(3, DateTime.Now, "Entrada", "Producto C", 7);

            dgv.DataSource = table;
        }
    }
}
