CREATE TABLE [Towers].[CaseItemChecklists]
(
	[Id]					BIGINT IDENTITY (1, 1) NOT NULL,
	[ContextKey]			NVARCHAR(500) NOT NULL,
	[SalesForce Case ID]	INT NOT NULL,
	[Category]				NVARCHAR(80) NOT NULL,
	[ChecklistName]			NVARCHAR(320) NOT NULL,
	[Error_Comment]			NVARCHAR(400) NULL,
	[Error]					NVARCHAR (5) NOT NULL,
	[LastUpdatedTime]   	DATETIME NOT NULL
CONSTRAINT [PK_CaseItemChecklists] PRIMARY KEY CLUSTERED ([Id] ASC));
