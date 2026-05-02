CREATE PROCEDURE sp_ObtenerProductosPorCategoria
    @CategoryId int
AS
BEGIN
    SELECT * FROM Products 
    WHERE CategoryId = @CategoryId 
    ORDER BY Nombre
END


CREATE PROCEDURE sp_BuscarProductos
    @SearchTerm nvarchar(100)
AS
BEGIN
    SELECT * FROM Products 
    WHERE Nombre LIKE '%' + @SearchTerm + '%' 
       OR Descripcion LIKE '%' + @SearchTerm + '%'
    ORDER BY Nombre
END


CREATE PROCEDURE sp_ObtenerPedidosUsuarioConDetalles
    @UserId nvarchar(450)
AS
BEGIN
    SELECT * FROM Orders 
    WHERE UserId = @UserId 
    ORDER BY Fecha DESC
END

CREATE PROCEDURE sp_ObtenerVentasPorUsuario
    @UserName nvarchar(100)
AS
BEGIN
    SELECT * FROM Orders 
    WHERE UserName LIKE '%' + @UserName + '%'
    ORDER BY Fecha DESC
END