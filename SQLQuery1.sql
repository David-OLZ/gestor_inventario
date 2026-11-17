-- ==========================================================
-- SCRIPT DE CREACION DE BASE DE DATOS: prueba
-- ==========================================================

IF DB_ID('prueba') IS NOT NULL
BEGIN
    ALTER DATABASE prueba SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE prueba;
END
GO

CREATE DATABASE prueba;
GO

USE prueba;
GO

-- ==========================================================
-- 1. CREACION DE TABLAS (EN TERCERA FORMA NORMAL - 3FN)
-- ==========================================================

-- Tabla ROL: Almacena los puestos del personal.
-- Forma Normal: 3FN (llave primaria simple, atributos atomicos y sin dependencias transitivas).
CREATE TABLE ROL (
    ID_Rol INT IDENTITY(1,1) PRIMARY KEY,
    Nombre_Rol VARCHAR(50) NOT NULL UNIQUE
);
GO

-- Tabla CATEGORIA: Agrupa los productos del almacen.
-- Forma Normal: 3FN (cada atributo depende unicamente de ID_Categoria, sin grupos repetitivos).
CREATE TABLE CATEGORIA (
    ID_Categoria INT PRIMARY KEY, -- Sin IDENTITY para poder demostrar ON UPDATE CASCADE
    Nombre VARCHAR(80) NOT NULL UNIQUE,
    Descripcion VARCHAR(200) NULL
);
GO

-- Tabla PROVEEDORES: Registra las empresas que surten mercancia.
-- Forma Normal: 3FN (todos los datos de contacto y el RFC dependen directamente de ID_Proveedor).
CREATE TABLE PROVEEDORES (
    ID_Proveedor INT IDENTITY(1,1) PRIMARY KEY,
    Nombre_Empresa VARCHAR(100) NOT NULL,
    RFC_NIT VARCHAR(13) NOT NULL UNIQUE,
    Telefono VARCHAR(15) NOT NULL,
    Correo VARCHAR(100) NOT NULL,
    CONSTRAINT CHK_Proveedor_RFC CHECK (LEN(RFC_NIT) >= 12)
);
GO

-- Tabla TIPO_MOVIMIENTO: Catalogo de tipos de operacion (Entrada, Salida, Ajuste, Devolucion).
-- Forma Normal: 3FN (llave primaria simple y descripcion unica sin dependencias entre columnas no clave).
CREATE TABLE TIPO_MOVIMIENTO (
    ID_Tipo INT IDENTITY(1,1) PRIMARY KEY,
    Nombre_Tipo VARCHAR(30) NOT NULL UNIQUE
);
GO

-- Tabla EMPLEADO: Usuarios autorizados del sistema.
-- Forma Normal: 3FN (el nombre del rol se separa en la tabla ROL para evitar dependencia transitiva).
CREATE TABLE EMPLEADO (
    ID_Empleado INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(80) NOT NULL,
    Apellido VARCHAR(80) NOT NULL,
    Correo VARCHAR(100) NOT NULL UNIQUE,
    Contrasena VARCHAR(255) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1, -- 1 = Activo, 0 = Inactivo
    ID_Rol INT NOT NULL,
    CONSTRAINT FK_Empleado_Rol FOREIGN KEY (ID_Rol)
        REFERENCES ROL(ID_Rol)
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);
GO

-- Tabla PRODUCTO: Catalogo general de articulos en inventario.
-- Forma Normal: 3FN (datos de proveedor y categoria se referencian por FK sin duplicar sus atributos).
CREATE TABLE PRODUCTO (
    ID_Producto INT IDENTITY(1,1) PRIMARY KEY,
    Codigo_SKU VARCHAR(30) NOT NULL UNIQUE,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(250) NULL,
    Precio DECIMAL(10,2) NOT NULL,
    Stock_Actual INT NOT NULL DEFAULT 0,
    Stock_Minimo INT NOT NULL DEFAULT 5,
    ID_Proveedor INT NOT NULL,
    ID_Categoria INT NOT NULL,
    CONSTRAINT CHK_Producto_Precio CHECK (Precio >= 0),
    CONSTRAINT CHK_Producto_StockActual CHECK (Stock_Actual >= 0),
    CONSTRAINT CHK_Producto_StockMinimo CHECK (Stock_Minimo >= 0),
    CONSTRAINT FK_Producto_Proveedor FOREIGN KEY (ID_Proveedor)
        REFERENCES PROVEEDORES(ID_Proveedor)
        ON UPDATE NO ACTION
        ON DELETE NO ACTION, -- Protege el historial: no borra proveedores con productos asociados
    CONSTRAINT FK_Producto_Categoria FOREIGN KEY (ID_Categoria)
        REFERENCES CATEGORIA(ID_Categoria)
        ON UPDATE CASCADE
        ON DELETE CASCADE -- Ejemplo de actualizacion y eliminacion en cascada
);
GO

-- Tabla MOVIMIENTO_INVENTARIO: Historial de entradas, salidas, ajustes y devoluciones.
-- Forma Normal: 3FN (todas las columnas dependen unicamente de ID_Movimiento; entidades referenciadas por FK).
CREATE TABLE MOVIMIENTO_INVENTARIO (
    ID_Movimiento INT IDENTITY(1,1) PRIMARY KEY,
    Cantidad INT NOT NULL,
    Fecha_Hora DATETIME NOT NULL DEFAULT GETDATE(),
    Motivo VARCHAR(200) NOT NULL,
    ID_Producto INT NOT NULL,
    ID_Empleado INT NOT NULL,
    ID_Tipo INT NOT NULL,
    CONSTRAINT CHK_Movimiento_Cantidad CHECK (Cantidad > 0),
    CONSTRAINT FK_Movimiento_Producto FOREIGN KEY (ID_Producto)
        REFERENCES PRODUCTO(ID_Producto)
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT FK_Movimiento_Empleado FOREIGN KEY (ID_Empleado)
        REFERENCES EMPLEADO(ID_Empleado)
        ON UPDATE NO ACTION
        ON DELETE NO ACTION, -- Protege el historial: no permite eliminar empleados con movimientos
    CONSTRAINT FK_Movimiento_Tipo FOREIGN KEY (ID_Tipo)
        REFERENCES TIPO_MOVIMIENTO(ID_Tipo)
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);
GO

-- ==========================================================
-- 2. TRIGGER: CONTROL AUTOMATICO DE EXISTENCIAS Y STOCK NEGATIVO
-- ==========================================================

CREATE TRIGGER trg_ControlExistencias
ON MOVIMIENTO_INVENTARIO
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Evitar que una salida deje el inventario en negativo
    IF EXISTS (
        SELECT 1
        FROM inserted i
        INNER JOIN TIPO_MOVIMIENTO tm ON i.ID_Tipo = tm.ID_Tipo
        INNER JOIN PRODUCTO p ON i.ID_Producto = p.ID_Producto
        WHERE tm.Nombre_Tipo = 'Salida' AND p.Stock_Actual < i.Cantidad
    )
    BEGIN
        RAISERROR('Stock insuficiente para realizar la salida solicitada.', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- Sumar existencias en Entrada o Devolucion
    UPDATE p
    SET p.Stock_Actual = p.Stock_Actual + i.Cantidad
    FROM PRODUCTO p
    INNER JOIN inserted i ON p.ID_Producto = i.ID_Producto
    INNER JOIN TIPO_MOVIMIENTO tm ON i.ID_Tipo = tm.ID_Tipo
    WHERE tm.Nombre_Tipo IN ('Entrada', 'Devolucion');

    -- Restar existencias en Salida
    UPDATE p
    SET p.Stock_Actual = p.Stock_Actual - i.Cantidad
    FROM PRODUCTO p
    INNER JOIN inserted i ON p.ID_Producto = i.ID_Producto
    INNER JOIN TIPO_MOVIMIENTO tm ON i.ID_Tipo = tm.ID_Tipo
    WHERE tm.Nombre_Tipo = 'Salida';

    -- Reemplazar existencias en Ajuste tras revision fisica
    UPDATE p
    SET p.Stock_Actual = i.Cantidad
    FROM PRODUCTO p
    INNER JOIN inserted i ON p.ID_Producto = i.ID_Producto
    INNER JOIN TIPO_MOVIMIENTO tm ON i.ID_Tipo = tm.ID_Tipo
    WHERE tm.Nombre_Tipo = 'Ajuste';
END;
GO

-- ==========================================================
-- 3. FUNCION DEFINIDA POR EL USUARIO (UDF)
-- ==========================================================

-- Calcula el valor monetario total en almacen de un producto especifico
CREATE FUNCTION fn_ValorInventarioProducto (@ID_Producto INT)
RETURNS DECIMAL(12,2)
AS
BEGIN
    DECLARE @ValorTotal DECIMAL(12,2);

    SELECT @ValorTotal = ISNULL(Precio * Stock_Actual, 0)
    FROM PRODUCTO
    WHERE ID_Producto = @ID_Producto;

    RETURN ISNULL(@ValorTotal, 0);
END;
GO

-- ==========================================================
-- 4. PROCEDIMIENTO ALMACENADO CON CURSOR
-- ==========================================================

-- Recorre los productos con stock menor o igual al minimo para generar alertas de resurtido
CREATE PROCEDURE sp_ReporteStockBajo
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ID_Producto INT;
    DECLARE @Codigo_SKU VARCHAR(30);
    DECLARE @Nombre VARCHAR(100);
    DECLARE @Stock_Actual INT;
    DECLARE @Stock_Minimo INT;
    DECLARE @Proveedor VARCHAR(100);

    CREATE TABLE #AlertasStock (
        ID_Producto INT,
        Codigo_SKU VARCHAR(30),
        Producto VARCHAR(100),
        Stock_Actual INT,
        Stock_Minimo INT,
        Unidades_Faltantes INT,
        Proveedor VARCHAR(100)
    );

    DECLARE cur_StockBajo CURSOR FOR
        SELECT p.ID_Producto, p.Codigo_SKU, p.Nombre, p.Stock_Actual, p.Stock_Minimo, pr.Nombre_Empresa
        FROM PRODUCTO p
        INNER JOIN PROVEEDORES pr ON p.ID_Proveedor = pr.ID_Proveedor
        WHERE p.Stock_Actual <= p.Stock_Minimo;

    OPEN cur_StockBajo;
    FETCH NEXT FROM cur_StockBajo INTO @ID_Producto, @Codigo_SKU, @Nombre, @Stock_Actual, @Stock_Minimo, @Proveedor;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        INSERT INTO #AlertasStock
        VALUES (
            @ID_Producto,
            @Codigo_SKU,
            @Nombre,
            @Stock_Actual,
            @Stock_Minimo,
            (@Stock_Minimo - @Stock_Actual),
            @Proveedor
        );

        FETCH NEXT FROM cur_StockBajo INTO @ID_Producto, @Codigo_SKU, @Nombre, @Stock_Actual, @Stock_Minimo, @Proveedor;
    END

    CLOSE cur_StockBajo;
    DEALLOCATE cur_StockBajo;

    SELECT * FROM #AlertasStock;
    DROP TABLE #AlertasStock;
END;
GO

-- ==========================================================
-- 5. DATOS DE PRUEBA
-- ==========================================================

INSERT INTO ROL (Nombre_Rol) VALUES
('Administrador'),
('Almacenista');

INSERT INTO TIPO_MOVIMIENTO (Nombre_Tipo) VALUES
('Entrada'),
('Salida'),
('Ajuste'),
('Devolucion');

INSERT INTO CATEGORIA (ID_Categoria, Nombre, Descripcion) VALUES
(1, 'Herramientas Manuales', 'Martillos, desarmadores, llaves y pinzas'),
(2, 'Material Electrico', 'Cableado, interruptores y contactos'),
(3, 'Tornilleria y Fijacion', 'Tornillos, taquetes y tuercas'),
(4, 'Categoria Prueba Cascada', 'Categoria temporal para demostrar ON UPDATE y ON DELETE CASCADE');
GO

INSERT INTO PROVEEDORES (Nombre_Empresa, RFC_NIT, Telefono, Correo) VALUES
('Ferretera Industrial del Norte S.A.', 'FIN180512AB1', '6561112233', 'ventas@ferrenorte.mx'),
('Distribuidora Electrica Juarez', 'DEJ200903CD4', '6564445566', 'contacto@electricajuarez.mx'),
('Tornillos y Herrajes Chihuahua', 'THC150220EF7', '6147778899', 'pedidos@tornilloschih.mx');

INSERT INTO EMPLEADO (Nombre, Apellido, Correo, Contrasena, Estado, ID_Rol) VALUES
('Gerardo', 'Ortega', 'gerardo.ortega@almacen.mx', 'Admin1234!', 1, 1),
('Carlos', 'Mendoza', 'carlos.mendoza@almacen.mx', 'Alma5678!', 1, 2),
('Andres', 'Ruiz', 'andres.ruiz@almacen.mx', 'Inactivo99!', 0, 2);

INSERT INTO PRODUCTO (Codigo_SKU, Nombre, Descripcion, Precio, Stock_Actual, Stock_Minimo, ID_Proveedor, ID_Categoria) VALUES
('SKU-HER-001', 'Martillo de Una 16 oz', 'Mango de fibra de vidrio antiderrapante', 185.50, 25, 10, 1, 1),
('SKU-ELE-002', 'Rollo Cable Calibre 12', 'Caja con 100 metros color negro', 1250.00, 12, 10, 2, 2),
('SKU-TOR-003', 'Caja Tornillo Tabla Roca 1 pulg', 'Caja con 1000 piezas punta fina', 210.00, 4, 15, 3, 3),
('SKU-HER-004', 'Juego Desarmadores 6 Piezas', 'Punta plana y cruz magnetica', 320.00, 18, 8, 1, 1),
('SKU-TMP-005', 'Producto Prueba Cascada', 'Articulo de prueba para eliminacion en cascada', 50.00, 10, 2, 1, 4);

-- Movimientos iniciales (el trigger actualiza Stock_Actual automaticamente)
INSERT INTO MOVIMIENTO_INVENTARIO (Cantidad, Motivo, ID_Producto, ID_Empleado, ID_Tipo) VALUES
(15, 'Compra mensual de reposicion', 1, 1, 1),     -- Entrada: stock pasa de 25 a 40
(7, 'Entrega a cuadrilla de obra', 2, 2, 2),      -- Salida: stock pasa de 12 a 5 (activa alerta de stock bajo)
(2, 'Devolucion por cliente', 4, 2, 4),           -- Devolucion: stock pasa de 18 a 20
(6, 'Conteo fisico semanal', 3, 1, 3);            -- Ajuste: stock pasa de 4 a 6 (sigue debajo del minimo de 15)
GO


-- Ver datos
USE prueba;
GO

-- Consultas directas de cada tabla
SELECT * FROM ROL;
SELECT * FROM CATEGORIA;
SELECT * FROM PROVEEDORES;
SELECT * FROM TIPO_MOVIMIENTO;
SELECT * FROM EMPLEADO;
SELECT * FROM PRODUCTO;
SELECT * FROM MOVIMIENTO_INVENTARIO;
GO

-- Productos con el nombre de su categoria y proveedor
SELECT 
    p.ID_Producto,
    p.Codigo_SKU,
    p.Nombre AS Producto,
    p.Precio,
    p.Stock_Actual,
    p.Stock_Minimo,
    c.Nombre AS Categoria,
    pr.Nombre_Empresa AS Proveedor
FROM PRODUCTO p
INNER JOIN CATEGORIA c ON p.ID_Categoria = c.ID_Categoria
INNER JOIN PROVEEDORES pr ON p.ID_Proveedor = pr.ID_Proveedor;

-- Historial de movimientos con nombres en lugar de IDs
SELECT 
    m.ID_Movimiento,
    m.Fecha_Hora,
    tm.Nombre_Tipo AS Tipo,
    p.Nombre AS Producto,
    m.Cantidad,
    m.Motivo,
    e.Nombre + ' ' + e.Apellido AS Empleado
FROM MOVIMIENTO_INVENTARIO m
INNER JOIN TIPO_MOVIMIENTO tm ON m.ID_Tipo = tm.ID_Tipo
INNER JOIN PRODUCTO p ON m.ID_Producto = p.ID_Producto
INNER JOIN EMPLEADO e ON m.ID_Empleado = e.ID_Empleado;
GO