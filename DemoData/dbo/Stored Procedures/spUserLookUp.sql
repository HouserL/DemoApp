CREATE PROCEDURE [dbo].[spUserLookUp]
	@Id NVARCHAR(128)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id], [FirstName], [LastName], [EmailAddress], [CreateDate]
	FROM [dbo].[User] 
	WHERE [Id] = @Id
END