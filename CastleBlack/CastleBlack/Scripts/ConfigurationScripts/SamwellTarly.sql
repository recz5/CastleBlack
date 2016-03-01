USE [$(CastleBlack)]

IF NOT EXISTS(SELECT * FROM [$(CastleBlack)].[NightsWatch].[Developers] WHERE [First Name] = N'Samwell' AND [Last Name] = N'Tarly')
BEGIN
	INSERT INTO  [$(CastleBlack)].[NightsWatch].[Developers] ([First Name], [Last Name]) VALUES(N'Samwell', N'Tarly')
END