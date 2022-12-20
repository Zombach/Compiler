CREATE PROCEDURE [dbo].[Templates_Insert]
	@Name		NVARCHAR (100),
	@Source		NVARCHAR (MAX),
	@Type		INT
AS
BEGIN
	INSERT INTO [dbo].[Templates]
	(
		[Name],
		[Source],
		[Type]
	)
	VALUES
	(
		@Name,
		@Source,
		@Type
	)
END