using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Practica
{
    public class ProductosView : Form
    {
        private readonly DataGridView dgv;
        private TextBox txtId = null!;
        private TextBox txtSku = null!;
        private TextBox txtNombre = null!;
        private TextBox txtProveedor = null!;
        private TextBox txtCategoria = null!;

        private const string ConnectionString =
            "Data Source=LAPTOP-HP-DAVID\\SQLEXPRESS;" +
            "Initial Catalog=prueba;" +
            "Integrated Security=True;" +
            "TrustServerCertificate=True";

        public ProductosView()
        {
            Text = "Productos - Solo lectura";
            ClientSize = new Size(1120, 620);
            MinimumSize = new Size(850, 480);
            StartPosition = FormStartPosition.CenterParent;

            GroupBox filtros = new()
            {
                Text = "Buscar productos",
                Location = new Point(20, 15),
                Size = new Size(1060, 145),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            Label Etiqueta(string texto, int x, int y) => new()
            {
                Text = texto,
                AutoSize = true,
                Location = new Point(x, y)
            };

            TextBox Campo(int x, int y, int ancho) => new()
            {
                Location = new Point(x, y),
                Size = new Size(ancho, 23)
            };

            txtId = Campo(70, 22, 100);
            txtSku = Campo(235, 22, 180);
            txtNombre = Campo(500, 22, 220);
            txtProveedor = Campo(120, 67, 250);
            txtCategoria = Campo(495, 67, 250);

            Button btnBuscar = new()
            {
                Text = "&Buscar",
                Location = new Point(790, 45),
                Size = new Size(90, 30)
            };
            btnBuscar.Click += (_, _) => CargarProductos();

            Button btnLimpiar = new()
            {
                Text = "&Limpiar",
                Location = new Point(890, 45),
                Size = new Size(90, 30)
            };
            btnLimpiar.Click += (_, _) => LimpiarFiltros();

            filtros.Controls.AddRange(
            [
                Etiqueta("ID:", 20, 26), txtId,
                Etiqueta("SKU:", 190, 26), txtSku,
                Etiqueta("Nombre:", 430, 26), txtNombre,
                Etiqueta("Proveedor:", 20, 71), txtProveedor,
                Etiqueta("Categoría:", 400, 71), txtCategoria,
                btnBuscar, btnLimpiar
            ]);

            dgv = new DataGridView
            {
                Location = new Point(20, 180),
                Size = new Size(1060, 410),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom |
                         AnchorStyles.Left | AnchorStyles.Right,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoGenerateColumns = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            Controls.Add(filtros);
            Controls.Add(dgv);
            Load += ProductosView_Load;
        }

        private void ProductosView_Load(object? sender, EventArgs e)
        {
            CargarProductos();
        }

        private void CargarProductos()
        {
            const string sql = """
                SELECT
                    p.ID_Producto AS [ID],
                    p.Codigo_SKU AS [SKU],
                    p.Nombre,
                    p.Descripcion AS [Descripción],
                    p.Precio,
                    p.Stock_Actual AS [Stock actual],
                    p.Stock_Minimo AS [Stock mínimo],
                    pr.Nombre_Empresa AS [Proveedor],
                    c.Nombre AS [Categoría]
                FROM PRODUCTO p
                INNER JOIN PROVEEDORES pr ON p.ID_Proveedor = pr.ID_Proveedor
                INNER JOIN CATEGORIA c ON p.ID_Categoria = c.ID_Categoria
                WHERE (@id = '' OR CONVERT(varchar(20), p.ID_Producto) LIKE '%' + @id + '%')
                  AND (@sku = '' OR p.Codigo_SKU LIKE '%' + @sku + '%')
                  AND (@nombre = '' OR p.Nombre LIKE '%' + @nombre + '%')
                  AND (@proveedor = '' OR pr.Nombre_Empresa LIKE '%' + @proveedor + '%')
                  AND (@categoria = '' OR c.Nombre LIKE '%' + @categoria + '%')
                ORDER BY p.ID_Producto;
                """;

            try
            {
                using SqlConnection conexion = new(ConnectionString);
                using SqlCommand comando = new(sql, conexion);
                comando.Parameters.Add("@id", SqlDbType.VarChar, 20).Value = txtId.Text.Trim();
                comando.Parameters.Add("@sku", SqlDbType.VarChar, 30).Value = txtSku.Text.Trim();
                comando.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = txtNombre.Text.Trim();
                comando.Parameters.Add("@proveedor", SqlDbType.VarChar, 100).Value = txtProveedor.Text.Trim();
                comando.Parameters.Add("@categoria", SqlDbType.VarChar, 80).Value = txtCategoria.Text.Trim();

                using SqlDataAdapter adaptador = new(comando);
                DataTable tabla = new();
                adaptador.Fill(tabla);
                dgv.DataSource = tabla;

                if (tabla.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron productos con los filtros introducidos.",
                        "Buscar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al consultar los productos:\n" + ex.Message,
                    "Error de SQL Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFiltros()
        {
            txtId.Clear();
            txtSku.Clear();
            txtNombre.Clear();
            txtProveedor.Clear();
            txtCategoria.Clear();
            CargarProductos();
        }
    }
}
