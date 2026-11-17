using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Net.Mail;
using System.Windows.Forms;

namespace Practica
{
    public partial class Proveedores : Form
    {
        private readonly DataGridView dgvProveedores;

        private TextBox txtBusqueda = null!;
        private TextBox txtNombreEmpresa = null!;
        private TextBox txtRfc = null!;
        private TextBox txtTelefono = null!;
        private TextBox txtCorreo = null!;

        private const string ConnectionString =
            "Data Source=LAPTOP-HP-DAVID\\SQLEXPRESS;" +
            "Initial Catalog=prueba;" +
            "Integrated Security=True;" +
            "TrustServerCertificate=True";

        public Proveedores()
        {
            InitializeComponent();

            // Se reemplazan los controles antiguos por los nuevos campos.
            groupBox1.Controls.Clear();
            groupBox1.Size = new Size(700, 210);

            CrearCampos();

            dgvProveedores = new DataGridView
            {
                Name = "dgvProveedores",
                Location = new Point(21, 245),
                Size = new Size(700, 300),
                Anchor = AnchorStyles.Top |
                         AnchorStyles.Bottom |
                         AnchorStyles.Left |
                         AnchorStyles.Right,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoGenerateColumns = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false
            };

            dgvProveedores.CellClick += DgvProveedores_CellClick;

            Controls.Add(dgvProveedores);

            ClientSize = new Size(760, 580);
        }

        private void CrearCampos()
        {
            Label CrearEtiqueta(string texto, int x, int y)
            {
                return new Label
                {
                    Text = texto,
                    AutoSize = true,
                    Location = new Point(x, y)
                };
            }

            TextBox CrearTextBox(int x, int y, int ancho = 230)
            {
                return new TextBox
                {
                    Location = new Point(x, y),
                    Size = new Size(ancho, 23)
                };
            }

            groupBox1.Text = "Datos del proveedor";

            Label lblBusqueda = CrearEtiqueta("Buscar ID:", 20, 25);
            txtBusqueda = CrearTextBox(140, 21, 180);

            Button btnBuscar = new()
            {
                Text = "&Buscar",
                Location = new Point(335, 20),
                Size = new Size(85, 28)
            };
            btnBuscar.Click += BuscarClick;

            Label lblNombre = CrearEtiqueta("Nombre empresa:", 20, 65);
            txtNombreEmpresa = CrearTextBox(140, 61);

            Label lblRfc = CrearEtiqueta("RFC/NIT:", 20, 100);
            txtRfc = CrearTextBox(140, 96);

            Label lblTelefono = CrearEtiqueta("Teléfono:", 390, 65);
            txtTelefono = CrearTextBox(485, 61, 180);

            Label lblCorreo = CrearEtiqueta("Correo:", 390, 100);
            txtCorreo = CrearTextBox(485, 96, 180);

            txtNombreEmpresa.MaxLength = 100;
            txtRfc.MaxLength = 13;
            txtTelefono.MaxLength = 15;
            txtCorreo.MaxLength = 100;

            Button btnInsertar = new()
            {
                Text = "&Insertar",
                Location = new Point(220, 145),
                Size = new Size(90, 28)
            };
            btnInsertar.Click += InsertarClick;

            Button btnLimpiar = new()
            {
                Text = "&Limpiar",
                Location = new Point(325, 145),
                Size = new Size(90, 28)
            };
            btnLimpiar.Click += (_, _) => LimpiarCampos();

            Button btnEliminar = new()
            {
                Text = "&Eliminar",
                Location = new Point(430, 145),
                Size = new Size(90, 28)
            };
            btnEliminar.Click += deleteClick;

            groupBox1.Controls.AddRange(
            [
                lblBusqueda,
                txtBusqueda,
                btnBuscar,
                lblNombre,
                txtNombreEmpresa,
                lblRfc,
                txtRfc,
                lblTelefono,
                txtTelefono,
                lblCorreo,
                txtCorreo,
                btnInsertar,
                btnLimpiar,
                btnEliminar
            ]);
        }

        private void Proveedores_Load(object sender, EventArgs e)
        {
            CargarProveedores();
        }

        private void BuscarClick(object? sender, EventArgs e)
        {
            CargarProveedores();
        }

        private void CargarProveedores()
        {
            const string sql = """
                SELECT
                    ID_Proveedor AS [ID],
                    Nombre_Empresa AS [Nombre de empresa],
                    RFC_NIT AS [RFC/NIT],
                    Telefono AS [Teléfono],
                    Correo
                FROM PROVEEDORES
                WHERE (@busqueda = ''
                       OR CONVERT(varchar(20), ID_Proveedor) LIKE '%' + @busqueda + '%')
                  AND (@nombre = ''
                       OR Nombre_Empresa LIKE '%' + @nombre + '%')
                  AND (@rfc = ''
                       OR RFC_NIT LIKE '%' + @rfc + '%')
                  AND (@telefono = ''
                       OR Telefono LIKE '%' + @telefono + '%')
                  AND (@correo = ''
                       OR Correo LIKE '%' + @correo + '%')
                ORDER BY ID_Proveedor;
                """;

            try
            {
                using SqlConnection cnx = new(ConnectionString);
                using SqlCommand cmd = new(sql, cnx);

                cmd.Parameters.Add("@busqueda", SqlDbType.VarChar, 100)
                    .Value = txtBusqueda.Text.Trim();

                cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 100)
                    .Value = txtNombreEmpresa.Text.Trim();

                cmd.Parameters.Add("@rfc", SqlDbType.VarChar, 13)
                    .Value = txtRfc.Text.Trim();

                cmd.Parameters.Add("@telefono", SqlDbType.VarChar, 15)
                    .Value = txtTelefono.Text.Trim();

                cmd.Parameters.Add("@correo", SqlDbType.VarChar, 100)
                    .Value = txtCorreo.Text.Trim();

                using SqlDataAdapter adapter = new(cmd);
                DataTable tabla = new();
                adapter.Fill(tabla);

                dgvProveedores.DataSource = tabla;

                if (tabla.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "No se encontraron proveedores.",
                        "Buscar",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Error al consultar los proveedores:\n" + ex.Message,
                    "Error de SQL Server",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void DgvProveedores_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvProveedores.Rows.Count)
                return;

            DataGridViewRow fila = dgvProveedores.Rows[e.RowIndex];

            txtBusqueda.Text = ObtenerValorCelda(fila, "ID");
            txtNombreEmpresa.Text = ObtenerValorCelda(fila, "Nombre de empresa");
            txtRfc.Text = ObtenerValorCelda(fila, "RFC/NIT");
            txtTelefono.Text = ObtenerValorCelda(fila, "Teléfono");
            txtCorreo.Text = ObtenerValorCelda(fila, "Correo");
        }

        private static string ObtenerValorCelda(DataGridViewRow fila, string nombreColumna)
        {
            object? valor = fila.Cells[nombreColumna].Value;
            return valor == null || valor == DBNull.Value
                ? string.Empty
                : Convert.ToString(valor) ?? string.Empty;
        }

        private void InsertarClick(object sender, EventArgs e)
        {
            if (!ValidarDatosProveedor())
                return;

            try
            {
                using SqlConnection cnx = new(ConnectionString);
                cnx.Open();

                const string consultaExistencia = """
                    SELECT ID_Proveedor, Nombre_Empresa, RFC_NIT, Telefono, Correo
                    FROM PROVEEDORES
                    WHERE RFC_NIT = @rfc;
                    """;

                using SqlCommand cmdExistencia = new(consultaExistencia, cnx);
                cmdExistencia.Parameters.Add("@rfc", SqlDbType.VarChar, 20)
                    .Value = txtRfc.Text.Trim();

                using SqlDataReader reader = cmdExistencia.ExecuteReader();

                if (reader.Read())
                {
                    int idProveedorExistente = reader.GetInt32(0);

                    DialogResult respuesta = MessageBox.Show(
                        "Ya existe un proveedor con ese RFC/NIT.\n\n" +
                        "ID: " + idProveedorExistente + "\n" +
                        "¿Deseas actualizar sus datos?",
                        "Proveedor existente",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (respuesta == DialogResult.Yes)
                    {
                        reader.Close();

                        const string sqlActualizar = """
                            UPDATE PROVEEDORES
                            SET Nombre_Empresa = @nombre,
                                Telefono = @telefono,
                                Correo = @correo
                            WHERE ID_Proveedor = @id;
                            """;

                        using SqlCommand cmdActualizar = new(sqlActualizar, cnx);

                        cmdActualizar.Parameters.Add("@id", SqlDbType.Int)
                            .Value = idProveedorExistente;

                        cmdActualizar.Parameters.Add("@nombre", SqlDbType.VarChar, 100)
                            .Value = txtNombreEmpresa.Text.Trim();

                        cmdActualizar.Parameters.Add("@telefono", SqlDbType.VarChar, 20)
                            .Value = txtTelefono.Text.Trim();

                        cmdActualizar.Parameters.Add("@correo", SqlDbType.VarChar, 100)
                            .Value = txtCorreo.Text.Trim();

                        cmdActualizar.ExecuteNonQuery();

                        MessageBox.Show(
                            "Los datos del proveedor fueron actualizados.",
                            "Actualización exitosa",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    return;
                }

                reader.Close();

                const string sqlInsertar = """
                    INSERT INTO PROVEEDORES
                        (Nombre_Empresa, RFC_NIT, Telefono, Correo)
                    VALUES
                        (@nombre, @rfc, @telefono, @correo);
                    """;

                using SqlCommand cmdInsertar = new(sqlInsertar, cnx);

                cmdInsertar.Parameters.Add("@nombre", SqlDbType.VarChar, 100)
                    .Value = txtNombreEmpresa.Text.Trim();

                cmdInsertar.Parameters.Add("@rfc", SqlDbType.VarChar, 20)
                    .Value = txtRfc.Text.Trim();

                cmdInsertar.Parameters.Add("@telefono", SqlDbType.VarChar, 20)
                    .Value = txtTelefono.Text.Trim();

                cmdInsertar.Parameters.Add("@correo", SqlDbType.VarChar, 100)
                    .Value = txtCorreo.Text.Trim();

                cmdInsertar.ExecuteNonQuery();

                MessageBox.Show(
                    "Proveedor registrado correctamente.",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Error al guardar los datos del proveedor:\n" + ex.Message,
                    "Error de SQL Server",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                CargarProveedores();
            }
        }

        private bool ValidarDatosProveedor()
        {
            string nombre = txtNombreEmpresa.Text.Trim();
            string rfc = txtRfc.Text.Trim();
            string telefono = txtTelefono.Text.Trim();
            string correo = txtCorreo.Text.Trim();

            if (nombre.Length == 0)
                return MostrarError("El nombre de la empresa es obligatorio.", txtNombreEmpresa);

            if (nombre.Length > 100)
                return MostrarError("El nombre de la empresa no puede tener más de 100 caracteres.", txtNombreEmpresa);

            if (rfc.Length == 0)
                return MostrarError("El RFC/NIT es obligatorio.", txtRfc);

            if (rfc.Length < 12 || rfc.Length > 13)
                return MostrarError("El RFC/NIT debe tener entre 12 y 13 caracteres.", txtRfc);

            foreach (char caracter in rfc)
            {
                if (!char.IsLetterOrDigit(caracter))
                    return MostrarError("El RFC/NIT solo puede contener letras y números.", txtRfc);
            }

            if (telefono.Length == 0)
                return MostrarError("El teléfono es obligatorio.", txtTelefono);

            if (telefono.Length < 7 || telefono.Length > 15)
                return MostrarError("El teléfono debe tener entre 7 y 15 dígitos.", txtTelefono);

            foreach (char caracter in telefono)
            {
                if (!char.IsDigit(caracter))
                    return MostrarError("El teléfono solo puede contener números.", txtTelefono);
            }

            if (correo.Length == 0)
                return MostrarError("El correo electrónico es obligatorio.", txtCorreo);

            if (correo.Length > 100)
                return MostrarError("El correo electrónico no puede tener más de 100 caracteres.", txtCorreo);

            try
            {
                _ = new MailAddress(correo);
            }
            catch (FormatException)
            {
                return MostrarError("Escribe un correo electrónico válido, por ejemplo: usuario@dominio.com.", txtCorreo);
            }

            return true;
        }

        private static bool MostrarError(string mensaje, Control control)
        {
            MessageBox.Show(
                mensaje,
                "Dato inválido",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);

            control.Focus();
            return false;
        }

        private void deleteClick(object sender, EventArgs e)
        {
            if (!int.TryParse(txtBusqueda.Text.Trim(), out int idProveedor))
            {
                MessageBox.Show(
                    "Escribe un ID válido en el campo 'Buscar ID' para eliminar el proveedor.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtBusqueda.Focus();
                return;
            }

            DialogResult respuesta = MessageBox.Show(
                "¿Está seguro de eliminar el proveedor?",
                "Advertencia",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
                return;

            const string sql = """
                DELETE FROM PROVEEDORES
                WHERE ID_Proveedor = @id;
                """;

            try
            {
                using SqlConnection cnx = new(ConnectionString);
                using SqlCommand cmd = new(sql, cnx);

                cmd.Parameters.Add("@id", SqlDbType.Int)
                    .Value = idProveedor;

                cnx.Open();

                int filasAfectadas = cmd.ExecuteNonQuery();

                if (filasAfectadas > 0)
                {
                    MessageBox.Show(
                        "Proveedor eliminado correctamente.",
                        "Información",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    LimpiarCampos();
                    CargarProveedores();
                }
                else
                {
                    MessageBox.Show(
                        "No se encontró el proveedor.",
                        "Aviso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(
                    "Error al eliminar el proveedor:\n" + ex.Message,
                    "Error de SQL Server",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtBusqueda.Clear();
            txtNombreEmpresa.Clear();
            txtRfc.Clear();
            txtTelefono.Clear();
            txtCorreo.Clear();
            CargarProveedores();
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void fecha_registro_ValueChanged(object sender, EventArgs e)
        {
        }
    }
}
