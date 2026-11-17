using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Practica
{
    public class Movimientos : Form
    {
        private readonly DataGridView dgv;

        private const string ConnectionString =
            "Data Source=LAPTOP-HP-DAVID\\SQLEXPRESS;" +
            "Initial Catalog=prueba;" +
            "Integrated Security=True;" +
            "TrustServerCertificate=True";

        public Movimientos()
        {
            Text = "Movimientos del inventario";
            ClientSize = new Size(800, 450);
            MinimumSize = new Size(650, 350);
            StartPosition = FormStartPosition.CenterParent;

            dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoGenerateColumns = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            Controls.Add(dgv);
            Load += Movimientos_Load;
            Productos.ProductoCambiado += Productos_ProductoCambiado;
            FormClosed += Movimientos_FormClosed;
        }

        private void Productos_ProductoCambiado(object? sender, EventArgs e)
        {
            if (!IsDisposed)
                CargarMovimientos();
        }

        private void Movimientos_FormClosed(object? sender, FormClosedEventArgs e)
        {
            Productos.ProductoCambiado -= Productos_ProductoCambiado;
        }

        private void Movimientos_Load(object? sender, EventArgs e)
        {
            CargarMovimientos();
        }

        private void CargarMovimientos()
        {
            const string sql = """
                SELECT
                    m.ID_Movimiento AS [ID movimiento],
                    m.Fecha_Hora AS [Fecha],
                    tm.Nombre_Tipo AS [Tipo de movimiento],
                    COALESCE(m.Nombre_Producto, p.Nombre) AS [Producto],
                    COALESCE(m.Nombre_Proveedor, pr.Nombre_Empresa, 'Proveedor no disponible') AS [Proveedor]
                FROM MOVIMIENTO_INVENTARIO m
                INNER JOIN TIPO_MOVIMIENTO tm ON m.ID_Tipo = tm.ID_Tipo
                LEFT JOIN PRODUCTO p ON m.ID_Producto = p.ID_Producto
                LEFT JOIN PROVEEDORES pr ON p.ID_Proveedor = pr.ID_Proveedor
                ORDER BY m.ID_Movimiento DESC;
                """;

            try
            {
                using SqlConnection conexion = new(ConnectionString);
                using SqlCommand comando = new(sql, conexion);
                using SqlDataAdapter adaptador = new(comando);

                DataTable tabla = new();
                adaptador.Fill(tabla);
                dgv.DataSource = tabla;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Error al consultar los movimientos:\n" + ex.Message,
                    "Error de SQL Server",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
