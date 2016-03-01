CREATE TYPE [Towers].[CaseItemsTypeUpsert] AS TABLE
(
	[SalesForce Case ID]	INT NOT NULL,
	[SalesForce Due Date]	DATE NOT NULL,
	[Developer ID]			INT NOT NULL,
	[QA ID]					INT NOT NULL,
	[Creation Dttm]			DATETIME NOT NULL,
	[LastUpdateDttm]		DATETIME NOT NULL
);
