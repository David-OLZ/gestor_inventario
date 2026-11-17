using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Practica
{
    public class Productos : Form
    {
        private readonly DataGridView dgvProductos;
        public static event EventHandler? ProductoCambiado;

        private TextBox txtBuscarId = null!;
        private TextBox txtSku = null!;
        private TextBox txtNombre = null!;
        private TextBox txtDescripcion = null!;
        private TextBox txtPrecio = null!;
        private TextBox txtStockActual = null!;
        private TextBox txtStockMinimo = null!;
        private TextBox txtProveedor = null!;
        private TextBox txtCategoria = null!;

        public Productos()
        {
            Text = "Productos";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(1120, 650);
            MinimumSize = new Size(900, 550);

            CrearControles();

            dgvProductos = new DataGridView
            {
                Name = "dgvProductos",
                Location = new Point(20, 270),
                Size = new Size(1060, 350),
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

            dgvProductos.CellClick += DgvProductos_CellClick;
            dgvProductos.DataBindingComplete += DgvProductos_DataBindingComplete;

            Controls.Add(dgvProductos);
            Load += Productos_Load;
        }

        private void CrearControles()
        {
            GroupBox grupo = new()
            {
                Text = "Datos del producto",
                Location = new Point(20, 15),
                Size = new Size(1060, 235),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };

            Label Etiqueta(string texto, int x, int y) => new()
            {
                Text = texto,
                AutoSize = true,
                Location = new Point(x, y)
            };

            TextBox Campo(int x, int y, int ancho = 210) => new()
            {
                Location = new Point(x, y),
                Size = new Size(ancho, 23)
            };

            Label lblBuscar = Etiqueta("Buscar ID:", 20, 27);
            txtBuscarId = Campo(95, 23, 130);

            Button btnBuscar = new()
            {
                Text = "&Buscar",
                Location = new Point(240, 20),
                Size = new Size(85, 28)
            };
            btnBuscar.Click += (_, _) => CargarProductos();

            Label lblSku = Etiqueta("SKU:", 20, 70);
            txtSku = Campo(95, 66, 210);
            txtSku.MaxLength = 30;

            Label lblNombre = Etiqueta("Nombre:", 20, 108);
            txtNombre = Campo(95, 104, 210);
            txtNombre.MaxLength = 100;

            Label lblDescripcion = Etiqueta("Descripción:", 20, 146);
            txtDescripcion = Campo(95, 142, 210);
            txtDescripcion.Height = 45;
            txtDescripcion.Multiline = true;
            txtDescripcion.ScrollBars = ScrollBars.Vertical;
            txtDescripcion.MaxLength = 250;

            Label lblPrecio = Etiqueta("Precio:", 350, 70);
            txtPrecio = Campo(430, 66, 150);

            Label lblStockActual = Etiqueta("Stock actual:", 350, 108);
            txtStockActual = Campo(430, 104, 150);

            Label lblStockMinimo = Etiqueta("Stock mínimo:", 350, 146);
            txtStockMinimo = Campo(430, 142, 150);

            Label lblProveedor = Etiqueta("ID proveedor:", 650, 70);
            txtProveedor = Campo(750, 66, 150);

            Label lblCategoria = Etiqueta("ID categoría:", 650, 108);
            txtCategoria = Campo(750, 104, 150);

            Button btnInsertar = new()
            {
                Text = "&Insertar",
                Location = new Point(650, 160),
                Size = new Size(90, 30)
            };
            btnInsertar.Click += InsertarClick;

            Button btnLimpiar = new()
            {
                Text = "&Limpiar",
                Location = new Point(755, 160),
                Size = new Size(90, 30)
            };
            btnLimpiar.Click += (_, _) => LimpiarCampos();

            Button btnEliminar = new()
            {
                Text = "&Eliminar",
                Location = new Point(860, 160),
                Size = new Size(90, 30)
            };
            btnEliminar.Click += EliminarClick;

            Button btnModificar = new()
            {
                Text = "&Modificar",
                Location = new Point(965, 160),
                Size = new Size(90, 30)
            };
            btnModificar.Click += ModificarClick;

            grupo.Controls.AddRange(
            [
                lblBuscar, txtBuscarId, btnBuscar,
                lblSku, txtSku, lblNombre, txtNombre,
                lblDescripcion, txtDescripcion, lblPrecio, txtPrecio,
                lblStockActual, txtStockActual, lblStockMinimo, txtStockMinimo,
                lblProveedor, txtProveedor, lblCategoria, txtCategoria,
                btnInsertar, btnLimpiar, btnEliminar, btnModificar
            ]);

            Controls.Add(grupo);
        }

        private void Productos_Load(object? sender, EventArgs e)
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
                    p.ID_Proveedor AS [ID proveedor],
                    pr.Nombre_Empresa AS [Proveedor],
                    p.ID_Categoria AS [ID categoría],
                    c.Nombre AS [Categoría]
                FROM PRODUCTO p
                INNER JOIN PROVEEDORES pr ON p.ID_Proveedor = pr.ID_Proveedor
                INNER JOIN CATEGORIA c ON p.ID_Categoria = c.ID_Categoria
                WHERE (@id = '' OR CONVERT(varchar(20), p.ID_Producto) LIKE '%' + @id + '%')
                  AND (@sku = '' OR p.Codigo_SKU LIKE '%' + @sku + '%')
                  AND (@nombre = '' OR p.Nombre LIKE '%' + @nombre + '%')
                  AND (@descripcion = '' OR ISNULL(p.Descripcion, '') LIKE '%' + @descripcion + '%')
                  AND (@precio = '' OR CONVERT(varchar(30), p.Precio) LIKE '%' + @precio + '%')
                  AND (@stockActual = '' OR CONVERT(varchar(20), p.Stock_Actual) = @stockActual)
                  AND (@stockMinimo = '' OR CONVERT(varchar(20), p.Stock_Minimo) = @stockMinimo)
                  AND (@proveedor = '' OR CONVERT(varchar(20), p.ID_Proveedor) = @proveedor)
                  AND (@categoria = '' OR CONVERT(varchar(20), p.ID_Categoria) = @categoria)
                ORDER BY p.ID_Producto;
                """;

            try
            {
                using SqlConnection conexion = new(DatabaseConfig.ConnectionString);
                using SqlCommand comando = new(sql, conexion);

                comando.Parameters.Add("@id", SqlDbType.VarChar, 20).Value = txtBuscarId.Text.Trim();
                comando.Parameters.Add("@sku", SqlDbType.VarChar, 30).Value = txtSku.Text.Trim();
                comando.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = txtNombre.Text.Trim();
                comando.Parameters.Add("@descripcion", SqlDbType.VarChar, 250).Value = txtDescripcion.Text.Trim();
                comando.Parameters.Add("@precio", SqlDbType.VarChar, 30).Value = txtPrecio.Text.Trim();
                comando.Parameters.Add("@stockActual", SqlDbType.VarChar, 20).Value = txtStockActual.Text.Trim();
                comando.Parameters.Add("@stockMinimo", SqlDbType.VarChar, 20).Value = txtStockMinimo.Text.Trim();
                comando.Parameters.Add("@proveedor", SqlDbType.VarChar, 20).Value = txtProveedor.Text.Trim();
                comando.Parameters.Add("@categoria", SqlDbType.VarChar, 20).Value = txtCategoria.Text.Trim();

                using SqlDataAdapter adaptador = new(comando);
                DataTable tabla = new();
                adaptador.Fill(tabla);
                dgvProductos.DataSource = tabla;

                if (dgvProductos.Columns.Contains("ID categoría"))
                    dgvProductos.Columns["ID categoría"].Visible = false;

                ColorearProductosConStockBajo();

                if (tabla.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "No se encontraron productos con los filtros introducidos.",
                        "Buscar",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (SqlException ex)
            {
                MostrarErrorSql("consultar los productos", ex);
            }
        }

        private void ColorearProductosConStockBajo()
        {
            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {
                if (fila.IsNewRow)
                    continue;

                int stockActual = Convert.ToInt32(fila.Cells["Stock actual"].Value);
                int stockMinimo = Convert.ToInt32(fila.Cells["Stock mínimo"].Value);
                bool stockBajo = stockActual < stockMinimo;

                fila.DefaultCellStyle.BackColor = stockBajo
                    ? Color.Red
                    : Color.Empty;
                fila.DefaultCellStyle.ForeColor = stockBajo
                    ? Color.White
                    : Color.Empty;
                fila.DefaultCellStyle.SelectionBackColor = stockBajo
                    ? Color.DarkRed
                    : SystemColors.Highlight;
                fila.DefaultCellStyle.SelectionForeColor = Color.White;
            }
        }

        private void DgvProductos_DataBindingComplete(
            object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            ColorearProductosConStockBajo();
        }

        private void DgvProductos_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvProductos.Rows.Count)
                return;

            DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];

            txtBuscarId.Text = ObtenerValorCelda(fila, "ID");
            txtSku.Text = ObtenerValorCelda(fila, "SKU");
            txtNombre.Text = ObtenerValorCelda(fila, "Nombre");
            txtDescripcion.Text = ObtenerValorCelda(fila, "Descripción");
            txtPrecio.Text = ObtenerValorCelda(fila, "Precio");
            txtStockActual.Text = ObtenerValorCelda(fila, "Stock actual");
            txtStockMinimo.Text = ObtenerValorCelda(fila, "Stock mínimo");
            txtProveedor.Text = ObtenerValorCelda(fila, "ID proveedor");
            txtCategoria.Text = ObtenerValorCelda(fila, "ID categoría");
        }

        private static string ObtenerValorCelda(DataGridViewRow fila, string nombreColumna)
        {
            object? valor = fila.Cells[nombreColumna].Value;
            return valor == null || valor == DBNull.Value
                ? string.Empty
                : Convert.ToString(valor) ?? string.Empty;
        }

        private void InsertarClick(object? sender, EventArgs e)
        {
            if (!ValidarDatos(out decimal precio, out int stockActual,
                out int stockMinimo, out int idProveedor, out int idCategoria))
            {
                return;
            }

            const string sql = """
                INSERT INTO PRODUCTO
                    (Codigo_SKU, Nombre, Descripcion, Precio, Stock_Actual,
                     Stock_Minimo, ID_Proveedor, ID_Categoria)
                VALUES
                    (@sku, @nombre, @descripcion, @precio, @stockActual,
                     @stockMinimo, @proveedor, @categoria);
                SELECT CAST(SCOPE_IDENTITY() AS int);
                """;

            try
            {
                using SqlConnection conexion = new(DatabaseConfig.ConnectionString);
                using SqlCommand comando = new(sql, conexion);

                comando.Parameters.Add("@sku", SqlDbType.VarChar, 30).Value = txtSku.Text.Trim();
                comando.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = txtNombre.Text.Trim();
                comando.Parameters.Add("@descripcion", SqlDbType.VarChar, 250).Value =
                    string.IsNullOrWhiteSpace(txtDescripcion.Text) ? DBNull.Value : txtDescripcion.Text.Trim();
                comando.Parameters.Add("@precio", SqlDbType.Decimal).Value = precio;
                comando.Parameters.Add("@stockActual", SqlDbType.Int).Value = stockActual;
                comando.Parameters.Add("@stockMinimo", SqlDbType.Int).Value = stockMinimo;
                comando.Parameters.Add("@proveedor", SqlDbType.Int).Value = idProveedor;
                comando.Parameters.Add("@categoria", SqlDbType.Int).Value = idCategoria;

                conexion.Open();
                using SqlTransaction transaccion = conexion.BeginTransaction();
                comando.Transaction = transaccion;

                int idProducto = Convert.ToInt32(comando.ExecuteScalar());
                RegistrarMovimiento(
                    conexion,
                    transaccion,
                    idProducto,
                    txtNombre.Text.Trim(),
                    idProveedor,
                    precio,
                    stockActual,
                    "insertar",
                    "Producto insertado desde el catálogo");

                transaccion.Commit();
                ProductoCambiado?.Invoke(null, EventArgs.Empty);

                MessageBox.Show("Producto insertado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                CargarProductos();
            }
            catch (SqlException ex)
            {
                MostrarErrorSql("guardar el producto", ex);
            }
        }

        private void ModificarClick(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtBuscarId.Text.Trim(), out int idProducto) || idProducto <= 0)
            {
                MostrarError("Selecciona un producto o escribe un ID válido para modificarlo.", txtBuscarId);
                return;
            }

            if (!ValidarDatos(out decimal precio, out int stockActual,
                out int stockMinimo, out int idProveedor, out int idCategoria))
            {
                return;
            }

            const string sql = """
                UPDATE PRODUCTO
                SET Codigo_SKU = @sku,
                    Nombre = @nombre,
                    Descripcion = @descripcion,
                    Precio = @precio,
                    Stock_Actual = @stockActual,
                    Stock_Minimo = @stockMinimo,
                    ID_Proveedor = @proveedor,
                    ID_Categoria = @categoria
                WHERE ID_Producto = @id;
                """;

            try
            {
                using SqlConnection conexion = new(DatabaseConfig.ConnectionString);
                conexion.Open();
                using SqlTransaction transaccion = conexion.BeginTransaction();
                using SqlCommand comando = new(sql, conexion, transaccion);

                comando.Parameters.Add("@id", SqlDbType.Int).Value = idProducto;
                comando.Parameters.Add("@sku", SqlDbType.VarChar, 30).Value = txtSku.Text.Trim();
                comando.Parameters.Add("@nombre", SqlDbType.VarChar, 100).Value = txtNombre.Text.Trim();
                comando.Parameters.Add("@descripcion", SqlDbType.VarChar, 250).Value =
                    string.IsNullOrWhiteSpace(txtDescripcion.Text) ? DBNull.Value : txtDescripcion.Text.Trim();
                comando.Parameters.Add("@precio", SqlDbType.Decimal).Value = precio;
                comando.Parameters.Add("@stockActual", SqlDbType.Int).Value = stockActual;
                comando.Parameters.Add("@stockMinimo", SqlDbType.Int).Value = stockMinimo;
                comando.Parameters.Add("@proveedor", SqlDbType.Int).Value = idProveedor;
                comando.Parameters.Add("@categoria", SqlDbType.Int).Value = idCategoria;

                if (comando.ExecuteNonQuery() == 0)
                {
                    transaccion.Rollback();
                    MessageBox.Show("No existe un producto con ese ID.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                RegistrarMovimiento(
                    conexion,
                    transaccion,
                    idProducto,
                    txtNombre.Text.Trim(),
                    idProveedor,
                    precio,
                    stockActual,
                    "modificar",
                    "Producto modificado desde el catálogo");

                transaccion.Commit();
                ProductoCambiado?.Invoke(null, EventArgs.Empty);

                MessageBox.Show("Producto modificado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
            }
            catch (SqlException ex)
            {
                MostrarErrorSql("modificar el producto", ex);
            }
        }

        private bool ValidarDatos(out decimal precio, out int stockActual,
            out int stockMinimo, out int idProveedor, out int idCategoria)
        {
            precio = 0;
            stockActual = 0;
            stockMinimo = 0;
            idProveedor = 0;
            idCategoria = 0;

            if (string.IsNullOrWhiteSpace(txtSku.Text))
                return MostrarError("El código SKU es obligatorio.", txtSku);

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
                return MostrarError("El nombre del producto es obligatorio.", txtNombre);

            if (!decimal.TryParse(txtPrecio.Text.Trim(), NumberStyles.Number,
                CultureInfo.CurrentCulture, out precio) || precio < 0)
                return MostrarError("El precio debe ser un número mayor o igual que cero.", txtPrecio);

            if (!int.TryParse(txtStockActual.Text.Trim(), out stockActual) || stockActual < 0)
                return MostrarError("El stock actual debe ser un entero mayor o igual que cero.", txtStockActual);

            if (!int.TryParse(txtStockMinimo.Text.Trim(), out stockMinimo) || stockMinimo < 0)
                return MostrarError("El stock mínimo debe ser un entero mayor o igual que cero.", txtStockMinimo);

            if (!int.TryParse(txtProveedor.Text.Trim(), out idProveedor) || idProveedor <= 0)
                return MostrarError("El ID del proveedor debe ser un entero positivo.", txtProveedor);

            if (!int.TryParse(txtCategoria.Text.Trim(), out idCategoria) || idCategoria <= 0)
                return MostrarError("El ID de la categoría debe ser un entero positivo.", txtCategoria);

            return true;
        }

        private void EliminarClick(object? sender, EventArgs e)
        {
            if (!int.TryParse(txtBuscarId.Text.Trim(), out int idProducto) || idProducto <= 0)
            {
                MostrarError("Escribe un ID de producto válido en el campo 'Buscar ID'.", txtBuscarId);
                return;
            }

            if (MessageBox.Show("¿Deseas eliminar el producto seleccionado?", "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using SqlConnection conexion = new(DatabaseConfig.ConnectionString);
                conexion.Open();
                using SqlTransaction transaccion = conexion.BeginTransaction();

                const string consultaNombre =
                    "SELECT Nombre, ID_Proveedor, Precio FROM PRODUCTO WHERE ID_Producto = @id;";

                using SqlCommand consulta = new(consultaNombre, conexion, transaccion);
                consulta.Parameters.Add("@id", SqlDbType.Int).Value = idProducto;
                using SqlDataReader lector = consulta.ExecuteReader();
                if (!lector.Read())
                {
                    lector.Close();
                    transaccion.Rollback();
                    MessageBox.Show("No existe un producto con ese ID.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string nombreProducto = lector.GetString(0);
                int idProveedor = lector.GetInt32(1);
                decimal precio = lector.GetDecimal(2);
                lector.Close();

                RegistrarMovimiento(
                    conexion,
                    transaccion,
                    idProducto,
                    nombreProducto,
                    idProveedor,
                    precio,
                    0,
                    "eliminar",
                    "Producto eliminado desde el catálogo");

                const string sql = "DELETE FROM PRODUCTO WHERE ID_Producto = @id;";
                using SqlCommand comando = new(sql, conexion, transaccion);
                comando.Parameters.Add("@id", SqlDbType.Int).Value = idProducto;
                comando.ExecuteNonQuery();
                transaccion.Commit();
                ProductoCambiado?.Invoke(null, EventArgs.Empty);

                MessageBox.Show("Producto eliminado correctamente.", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
                CargarProductos();
            }
            catch (SqlException ex)
            {
                MostrarErrorSql("eliminar el producto", ex);
            }
        }

        private static void RegistrarMovimiento(
            SqlConnection conexion,
            SqlTransaction transaccion,
            int idProducto,
            string nombreProducto,
            int idProveedor,
            decimal precio,
            int stockActual,
            string tipoMovimiento,
            string motivo)
        {
            const string sql = """
                INSERT INTO MOVIMIENTO_INVENTARIO
                    (Cantidad, Motivo, ID_Producto, Nombre_Producto, Nombre_Proveedor,
                     Precio_Movimiento, Stock_Actual_Movimiento, ID_Empleado, ID_Tipo)
                SELECT
                    1, @motivo, @idProducto, @nombreProducto, pr.Nombre_Empresa,
                    @precio, @stockActual, 1, tm.ID_Tipo
                FROM TIPO_MOVIMIENTO tm
                INNER JOIN PROVEEDORES pr ON pr.ID_Proveedor = @idProveedor
                WHERE tm.Nombre_Tipo = @tipoMovimiento;
                """;

            using SqlCommand comando = new(sql, conexion, transaccion);
            comando.Parameters.Add("@motivo", SqlDbType.VarChar, 200).Value = motivo;
            comando.Parameters.Add("@idProducto", SqlDbType.Int).Value = idProducto;
            comando.Parameters.Add("@nombreProducto", SqlDbType.VarChar, 100).Value = nombreProducto;
            comando.Parameters.Add("@idProveedor", SqlDbType.Int).Value = idProveedor;
            comando.Parameters.Add("@precio", SqlDbType.Decimal).Value = precio;
            comando.Parameters.Add("@stockActual", SqlDbType.Int).Value = stockActual;
            comando.Parameters.Add("@tipoMovimiento", SqlDbType.VarChar, 20).Value = tipoMovimiento;

            if (comando.ExecuteNonQuery() == 0)
                throw new InvalidOperationException($"No existe el tipo de movimiento '{tipoMovimiento}'.");
        }

        private void LimpiarCampos()
        {
            txtBuscarId.Clear();
            txtSku.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtPrecio.Clear();
            txtStockActual.Clear();
            txtStockMinimo.Clear();
            txtProveedor.Clear();
            txtCategoria.Clear();
            CargarProductos();
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
                2601 or 2627 => "El código SKU ya existe. Introduce un SKU diferente.",
                547 => "El ID del proveedor o de la categoría no existe, o algún dato viola una restricción.",
                515 => "Falta un dato obligatorio para guardar el producto.",
                _ => $"Error al {operacion}:\n{ex.Message}"
            };

            MessageBox.Show(mensaje, "Error de SQL Server",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
