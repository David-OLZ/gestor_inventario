using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Practica
{
    public class CategoriasView : Form
    {
        private readonly DataGridView dgvCategorias;
        private TextBox txtId = null!;
        private TextBox txtNombre = null!;
        private TextBox txtDescripcion = null!;

        public CategoriasView()
        {
            Text = "Categorías - Solo lectura";
            ClientSize = new Size(900, 560);
            MinimumSize = new Size(700, 450);
            StartPosition = FormStartPosition.CenterParent;

            GroupBox filtros = new()
            {
                Text = "Buscar categorías",
                Location = new Point(20, 15),
                Size = new Size(840, 120),
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

            txtId = Campo(85, 22, 100);
            txtNombre = Campo(285, 22, 210);
            txtDescripcion = Campo(125, 62, 370);

            Button btnBuscar = new()
            {
                Text = "&Buscar",
                Location = new Point(530, 38),
                Size = new Size(90, 30)
            };
            btnBuscar.Click += (_, _) => CargarCategorias();

            Button btnLimpiar = new()
            {
                Text = "&Limpiar",
                Location = new Point(630, 38),
                Size = new Size(90, 30)
            };
            btnLimpiar.Click += (_, _) => LimpiarFiltros();

            filtros.Controls.AddRange(
            [
                Etiqueta("ID:", 20, 26), txtId,
                Etiqueta("Nombre:", 210, 26), txtNombre,
                Etiqueta("Descripción:", 20, 66), txtDescripcion,
                btnBuscar, btnLimpiar
            ]);

            dgvCategorias = new DataGridView
            {
                Location = new Point(20, 155),
                Size = new Size(840, 365),
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
            Controls.Add(dgvCategorias);
            Load += CategoriasView_Load;
        }

        private void CategoriasView_Load(object? sender, EventArgs e)
        {
            CargarCategorias();
        }

        private void CargarCategorias()
        {
            const string sql = """
                SELECT
                    ID_Categoria AS [ID],
                    Nombre,
                    Descripcion AS [Descripción]
                FROM CATEGORIA
                WHERE (@id = '' OR CONVERT(varchar(20), ID_Categoria) LIKE '%' + @id + '%')
                  AND (@nombre = '' OR Nombre LIKE '%' + @nombre + '%')
                  AND (@descripcion = '' OR ISNULL(Descripcion, '') LIKE '%' + @descripcion + '%')
                ORDER BY ID_Categoria;
                """;

            try
            {
                using SqlConnection conexion = new(DatabaseConfig.ConnectionString);
                using SqlCommand comando = new(sql, conexion);
                comando.Parameters.Add("@id", SqlDbType.VarChar, 20).Value = txtId.Text.Trim();
                comando.Parameters.Add("@nombre", SqlDbType.VarChar, 80).Value = txtNombre.Text.Trim();
                comando.Parameters.Add("@descripcion", SqlDbType.VarChar, 200).Value = txtDescripcion.Text.Trim();

                using SqlDataAdapter adaptador = new(comando);
                DataTable tabla = new();
                adaptador.Fill(tabla);
                dgvCategorias.DataSource = tabla;

                if (tabla.Rows.Count == 0)
                {
                    MessageBox.Show("No se encontraron categorías con los filtros introducidos.",
                        "Buscar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al consultar las categorías:\n" + ex.Message,
                    "Error de SQL Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarFiltros()
        {
            txtId.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            CargarCategorias();
        }
    }
}
