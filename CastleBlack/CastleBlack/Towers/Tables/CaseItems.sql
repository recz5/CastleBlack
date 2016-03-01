﻿CREATE TABLE [Towers].[CaseItems]
(
	[Id] BIGINT IDENTITY (1, 1) NOT NULL,
	[SalesForce Case ID] NVARCHAR(400) NOT NULL,
	[SalesForce Due Date] DATE NOT NULL,
	[Developer ID] INT NOT NULL,
	[QA ID] INT NOT NULL,
	[Create Dttm] DATETIME NOT NULL,
	[LastUpdateDttm] DATETIME NOT NULL
CONSTRAINT [PK_CaseItems] PRIMARY KEY CLUSTERED ([Id] ASC));
