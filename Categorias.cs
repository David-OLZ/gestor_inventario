using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Practica
{
    public class Categorias : Form
    {
        private readonly DataGridView dgvCategorias;

        private TextBox txtBuscarId = null!;
        private TextBox txtId = null!;
        private TextBox txtNombre = null!;
        private TextBox txtDescripcion = null!;

        private const string ConnectionString =
            "Data Source=LAPTOP-HP-DAVID\\SQLEXPRESS;" +
            "Initial Catalog=prueba;" +
            "Integrated Security=True;" +
            "TrustServerCertificate=True";

        public Categorias()
        {
            Text = "Categorías";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(900, 560);
            MinimumSize = new Size(750, 450);

            CrearControles();

            dgvCategorias = new DataGridView
            {
                Name = "dgvCategorias",
                Location = new Point(20, 220),
                Size = new Size(840, 300),
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

            dgvCategorias.CellClick += DgvCategorias_CellClick;
            Controls.Add(dgvCategorias);
            Load += Categorias_Load;
        }

        private void CrearControles()
        {
            GroupBox grupo = new()
            {
                Text = "Datos de la categoría",
                Location = new Point(20, 15),
                Size = new Size(840, 185),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            Label Etiqueta(string texto, int x, int y) => new()
            {
                Text = texto,
                AutoSize = true,
                Location = new Point(x, y)
            };

            TextBox Campo(int x, int y, int ancho = 230) => new()
            {
                Location = new Point(x, y),
                Size = new Size(ancho, 23)
            };

            Label lblBuscar = Etiqueta("Buscar ID:", 20, 27);
            txtBuscarId = Campo(105, 23, 130);

            Button btnBuscar = new()
            {
                Text = "&Buscar",
                Location = new Point(250, 20),
                Size = new Size(85, 28)
            };
            btnBuscar.Click += (_, _) => CargarCategorias();

            Label lblId = Etiqueta("ID categoría:", 20, 70);
            txtId = Campo(105, 66, 130);
            txtId.MaxLength = 10;

            Label lblNombre = Etiqueta("Nombre:", 300, 70);
            txtNombre = Campo(370, 66, 230);
            txtNombre.MaxLength = 80;

            Label lblDescripcion = Etiqueta("Descripción:", 20, 108);
            txtDescripcion = Campo(105, 104, 495);
            txtDescripcion.MaxLength = 200;

            Button btnInsertar = new()
            {
                Text = "&Insertar",
                Location = new Point(620, 62),
                Size = new Size(90, 30)
            };
            btnInsertar.Click += InsertarClick;

            Button btnLimpiar = new()
            {
                Text = "&Limpiar",
                Location = new Point(620, 102),
                Size = new Size(90, 30)
            };
            btnLimpiar.Click += (_, _) => LimpiarCampos();

            Button btnEliminar = new()
            {
                Text = "&Eliminar",
                Location = new Point(720, 82),
                Size = new Size(90, 30)
            };
            btnEliminar.Click += EliminarClick;

            grupo.Controls.AddRange(
            [
                lblBuscar, txtBuscarId, btnBuscar,
                lblId, txtId, lblNombre, txtNombre,
                lblDescripcion, txtDescripcion,
                btnInsertar, btnLimpiar, btnEliminar
            ]);

            Controls.Add(grupo);
        }

        private void Categorias_Load(object? sender, EventArgs e)
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
                using SqlConnection conexion = new(ConnectionString);
                using SqlCommand comando = new(sql, conexion);

                comando.Parameters.Add("@id", SqlDbType.VarChar, 20).Value = txtBuscarId.Text.Trim();
                comando.Parameters.Add("@nombre", SqlDbType.VarChar, 80).Value = txtNombre.Text.Trim();
                comando.Parameters.Add("@descripcion", SqlDbType.VarChar, 200).Value = txtDescripcion.Text.Trim();

                using SqlDataAdapter adaptador = new(comando);
                DataTable tabla = new();
                adaptador.Fill(tabla);
                dgvCategorias.DataSource = tabla;

                if (tabla.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "No se encontraron categorías con los filtros introducidos.",
                        "Buscar",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (SqlException ex)
            {
                MostrarErrorSql("consultar las categorías", ex);
            }
        }

        private void DgvCategorias_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvCategorias.Rows.Count)
                return;

            DataGridViewRow fila = dgvCategorias.Rows[e.RowIndex];
            txtBuscarId.Text = ObtenerValorCelda(fila, "ID");
            txtId.Text = ObtenerValorCelda(fila, "ID");
            txtNombre.Text = ObtenerValorCelda(fila, "Nombre");
            txtDescripcion.Text = ObtenerValorCelda(fila, "Descripción");
        }

        private void InsertarClick(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtId.Text.Trim(), out int idCategoria) || idCategoria <= 0)
            {
                MostrarError("El ID de la categoría debe ser un entero positivo.", txtId);
                return;
            }

            string nombre = txtNombre.Text.Trim();
            string descripcion = txtDescripcion.Text.Trim();

            if (nombre.Length == 0)
            {
                MostrarError("El nombre de la categoría es obligatorio.", txtNombre);
                return;
            }

            if (descripcion.Length > 200)
            {
                MostrarError("La descripción no puede tener más de 200 caracteres.", txtDescripcion);
                return;
            }

            const string sql = """
                INSERT INTO CATEGORIA (ID_Categoria, Nombre, Descripcion)
                VALUES (@id, @nombre, @descripcion);
                """;

            try
            {
                using SqlConnection conexion = new(ConnectionString);
                using SqlCommand comando = new(sql, conexion);

                comando.Parameters.Add("@id", SqlDbType.Int).Value = idCategoria;
                comando.Parameters.Add("@nombre", SqlDbType.VarChar, 80).Value = nombre;
                comando.Parameters.Add("@descripcion", SqlDbType.VarChar, 200).Value =
                    descripcion.Length == 0 ? DBNull.Value : descripcion;

                conexion.Open();
                comando.ExecuteNonQuery();

                MessageBox.Show("Categoría insertada correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
            }
            catch (SqlException ex)
            {
                MostrarErrorSql("guardar la categoría", ex);
            }
        }

        private void EliminarClick(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtBuscarId.Text.Trim(), out int idCategoria) || idCategoria <= 0)
            {
                MostrarError("Escribe un ID válido en el campo 'Buscar ID' para eliminar la categoría.", txtBuscarId);
                return;
            }

            if (MessageBox.Show(
                "¿Deseas eliminar la categoría seleccionada? Si tiene productos asociados, también se eliminarán por la relación en cascada.",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            const string sql = "DELETE FROM CATEGORIA WHERE ID_Categoria = @id;";

            try
            {
                using SqlConnection conexion = new(ConnectionString);
                using SqlCommand comando = new(sql, conexion);
                comando.Parameters.Add("@id", SqlDbType.Int).Value = idCategoria;
                conexion.Open();

                if (comando.ExecuteNonQuery() == 0)
                {
                    MessageBox.Show("No existe una categoría con ese ID.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show("Categoría eliminada correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
            }
            catch (SqlException ex)
            {
                MostrarErrorSql("eliminar la categoría", ex);
            }
        }

        private void LimpiarCampos()
        {
            txtBuscarId.Clear();
            txtId.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            CargarCategorias();
        }

        private static string ObtenerValorCelda(DataGridViewRow fila, string nombreColumna)
        {
            object? valor = fila.Cells[nombreColumna].Value;
            return valor == null || valor == DBNull.Value
                ? string.Empty
                : Convert.ToString(valor) ?? string.Empty;
        }

        private static bool MostrarError(string mensaje, Control control)
        {
            MessageBox.Show(mensaje, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
            return false;
        }

        private static void MostrarErrorSql(string operacion, SqlException ex)
        {
            string mensaje = ex.Number switch
            {
                2601 or 2627 => "El ID o el nombre de la categoría ya existe. Introduce valores diferentes.",
                547 => "No se puede eliminar la categoría porque existe una relación que lo impide.",
                515 => "Falta un dato obligatorio para guardar la categoría.",
                _ => $"Error al {operacion}:\n{ex.Message}"
            };

            MessageBox.Show(mensaje, "Error de SQL Server",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
