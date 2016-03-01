CREATE VIEW [NightsWatch].[BlankWorkItem]
	AS SELECT [SalesForce Case ID] = NULL
			  ,[Category]
			  ,[ChecklistName]
			  ,[Error_Comment]  = CONVERT(NVARCHAR(400),'')
			  ,[Error]  = CONVERT(NVARCHAR(400),'???')
	FROM [NightsWatch].[Checklists]