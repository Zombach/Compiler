CREATE PROCEDURE [dbo].[TemplatesChange_Insert]
	@Guid			UNIQUEIDENTIFIER,
	@Description	NVARCHAR (MAX)
AS

DECLARE @Number INT;
BEGIN
	SET @Number =
	(
		SELECT MAX([Number])
		FROM [dbo].[TemplatesChange]
		WHERE [Guid] = @Guid
	)
		
	IF @Number IS NULL
		SET @Number = 1
	ELSE
		SET @Number += 1
END

BEGIN
	INSERT INTO [dbo].[TemplatesChange]
	(
		[Guid],
		[Number],
		[Description]
	)
	VALUES
	(
		@Guid,
		@Number,
		@Description
	)
END