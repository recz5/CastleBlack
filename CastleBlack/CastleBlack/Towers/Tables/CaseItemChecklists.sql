CREATE TABLE [Towers].[CaseItemChecklists]
(
	[Id] BIGINT IDENTITY (1, 1) NOT NULL,
	[SalesForce Case ID] NVARCHAR(400) NOT NULL,
	[Category] NVARCHAR(400) NOT NULL,
	[ChecklistName] NVARCHAR(400) NOT NULL,
	[Error_Comment] NVARCHAR(400) NOT NULL,
	[Error] CHAR(2) NOT NULL,
CONSTRAINT [PK_CaseItemChecklists] PRIMARY KEY CLUSTERED ([Id] ASC));
