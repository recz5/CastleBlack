CREATE PROCEDURE [NightsWatch].[addUser]
	@username NVARCHAR(50), 
    @password NVARCHAR(50), 
    @firstName NVARCHAR(40) = NULL, 
    @lastName NVARCHAR(40) = NULL,
    @responseMessage NVARCHAR(250) OUTPUT
AS
BEGIN
	SET NOCOUNT ON

	BEGIN TRY
		INSERT INTO [NightsWatch].[QAs] ([UserName], [PasswordHash], [First Name], [Last Name])
		VALUES(@username, HASHBYTES('SHA2_512', @password), @firstName, @lastName)

		SET @responseMessage='Success'

	END TRY
	BEGIN CATCH
		SET @responseMessage=ERROR_MESSAGE() 
	END CATCH
END
