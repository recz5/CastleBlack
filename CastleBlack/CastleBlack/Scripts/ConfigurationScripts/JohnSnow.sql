USE [$(CastleBlack)]

DECLARE @responseMessage NVARCHAR(250)

IF NOT EXISTS(SELECT * FROM [$(CastleBlack)].[NightsWatch].[QAs] WHERE 	[UserName] = 'Snowj')
BEGIN
	EXEC [NightsWatch].[addUser]
			  @userName = N'Snowj',
			  @password = N'bastard',
			  @firstName = N'John',
			  @lastName = N'Snow',
			  @responseMessage=@responseMessage OUTPUT
END