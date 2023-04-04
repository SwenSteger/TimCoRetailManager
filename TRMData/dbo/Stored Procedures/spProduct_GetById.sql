CREATE PROCEDURE [dbo].[spProduct_GetById]
	@Id INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id, ProductName, [Description], RetailPrice, QuantityInStock, IsTaxable, CreateDate, LastModified
	FROM [dbo].[Product]
	WHERE id = @Id;
END