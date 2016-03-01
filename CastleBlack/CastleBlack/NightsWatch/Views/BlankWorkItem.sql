CREATE VIEW [NightsWatch].[BlankWorkItem]
	AS SELECT [SalesForce Case ID] = NULL
			  ,[Category]
			  ,[ChecklistName]
			  ,[Error_Comment]  = ''
			  ,[Error]  = '???'
	FROM [NightsWatch].[Checklists]