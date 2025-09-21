CREATE TABLE Usuarios (
    UsuarioId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    ContraseñaHash NVARCHAR(255) NOT NULL,
    Rol NVARCHAR(50) NOT NULL CHECK (Rol IN ('Cliente', 'Almacenero', 'Repartidor', 'Admin')),
    Telefono NVARCHAR(20),
    Direccion NVARCHAR(255),
    FechaRegistro DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1
);

CREATE TABLE Almacenes (
    AlmacenId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Direccion NVARCHAR(255) NOT NULL,
    Telefono NVARCHAR(20)
);

CREATE TABLE Paquetes (
    PaqueteId INT IDENTITY(1,1) PRIMARY KEY,
    ClienteId INT NOT NULL,
    CodigoSeguimiento NVARCHAR(50) UNIQUE NOT NULL,
    Peso DECIMAL(10,2) NOT NULL,
    Dimensiones NVARCHAR(100),
    Descripcion NVARCHAR(255),
    Estado NVARCHAR(50) NOT NULL CHECK (Estado IN ('En almacén','En reparto','Entregado','Devuelto')),
    FechaRegistro DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ClienteId) REFERENCES Usuarios(UsuarioId)
);

CREATE TABLE MovimientosPaquete (
    MovimientoId INT IDENTITY(1,1) PRIMARY KEY,
    PaqueteId INT NOT NULL,
    AlmacenId INT NOT NULL,
    UsuarioId INT NOT NULL,
    TipoMovimiento NVARCHAR(50) NOT NULL CHECK (TipoMovimiento IN ('Entrada','Salida','Transferencia')),
    FechaMovimiento DATETIME DEFAULT GETDATE(),
    Observaciones NVARCHAR(255),
    FOREIGN KEY (PaqueteId) REFERENCES Paquetes(PaqueteId),
    FOREIGN KEY (AlmacenId) REFERENCES Almacenes(AlmacenId),
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(UsuarioId)
);

CREATE TABLE Envios (
    EnvioId INT IDENTITY(1,1) PRIMARY KEY,
    PaqueteId INT NOT NULL,
    RepartidorId INT NOT NULL,
    FechaSalida DATETIME DEFAULT GETDATE(),
    FechaEntrega DATETIME NULL,
    Estado NVARCHAR(50) NOT NULL CHECK (Estado IN ('Pendiente','En ruta','Entregado','Fallido')),
    FOREIGN KEY (PaqueteId) REFERENCES Paquetes(PaqueteId),
    FOREIGN KEY (RepartidorId) REFERENCES Usuarios(UsuarioId)
);

CREATE TABLE Rutas (
    RutaId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255)
);

CREATE TABLE EnvioRutas (
    EnvioRutaId INT IDENTITY(1,1) PRIMARY KEY,
    EnvioId INT NOT NULL,
    RutaId INT NOT NULL,
    FOREIGN KEY (EnvioId) REFERENCES Envios(EnvioId),
    FOREIGN KEY (RutaId) REFERENCES Rutas(RutaId)
);

CREATE TABLE Incidencias (
    IncidenciaId INT IDENTITY(1,1) PRIMARY KEY,
    PaqueteId INT NOT NULL,
    UsuarioId INT NOT NULL,
    Descripcion NVARCHAR(255) NOT NULL,
    Fecha DATETIME DEFAULT GETDATE(),
    Resuelta BIT DEFAULT 0,
    FOREIGN KEY (PaqueteId) REFERENCES Paquetes(PaqueteId),
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(UsuarioId)
);

CREATE TABLE Pagos (
    PagoId INT IDENTITY(1,1) PRIMARY KEY,
    ClienteId INT NOT NULL,
    PaqueteId INT NOT NULL,
    Monto DECIMAL(10,2) NOT NULL,
    MetodoPago NVARCHAR(50) NOT NULL,
    FechaPago DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ClienteId) REFERENCES Usuarios(UsuarioId),
    FOREIGN KEY (PaqueteId) REFERENCES Paquetes(PaqueteId)
);