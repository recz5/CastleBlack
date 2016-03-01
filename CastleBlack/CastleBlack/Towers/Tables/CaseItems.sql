CREATE TABLE [Towers].[CaseItems]
(
	[Id]					BIGINT IDENTITY (1, 1) NOT NULL,
	[SalesForce Case ID]	INT NOT NULL,
	[SalesForce Due Date]	DATE NULL,
	[Developer ID]			INT NOT NULL,
	[QA ID]					INT NOT NULL,
	[Creation Dttm]			DATETIME NOT NULL,
	[LastUpdateDttm]		DATETIME NOT NULL
CONSTRAINT [PK_CaseItems]	PRIMARY KEY CLUSTERED ([Id] ASC));
