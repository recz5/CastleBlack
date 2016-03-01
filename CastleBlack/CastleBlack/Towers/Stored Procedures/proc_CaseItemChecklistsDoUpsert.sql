CREATE PROCEDURE [Towers].[proc_CaseItemChecklistsDoUpsert]
	(@tvpTable [Towers].[CaseItemChecklistsTypeUpsert] readonly)
AS
BEGIN
;WITH existingrows
  (
	[ContextKey]          	
	,[SalesForce Case ID]   	
	,[Category]				
	,[ChecklistName] 		
	,[Error_Comment] 		
	,[Error] 				
	,[LastUpdatedTime]   	
  ) AS (SELECT 
	T.[ContextKey]   
	,T.[SalesForce Case ID]   	
	,T.[Category]				
	,T.[ChecklistName] 		
	,T.[Error_Comment] 		
	,T.[Error] 				
	,T.[LastUpdatedTime] 			
         FROM   [Towers].[CaseItemChecklists] T
                INNER JOIN @TvpTable C
                        ON T.[ContextKey] = C.[ContextKey])
MERGE existingrows AS target
using (SELECT * FROM   @TvpTable) AS source ON target.[ContextKey] = source.[ContextKey]
WHEN matched AND source.[LastUpdatedTime] > target.[LastUpdatedTime] 
THEN
  UPDATE SET 	
				target.[ContextKey]				= source.[ContextKey]
				,target.[SalesForce Case ID]	= source.[SalesForce Case ID]
				,target.[Category]				= source.[Category]			
				,target.[ChecklistName] 		= source.[ChecklistName] 	
				,target.[Error_Comment] 		= source.[Error_Comment] 	
				,target.[Error] 				= source.[Error] 			
				,target.[LastUpdatedTime]		= source.[LastUpdatedTime]  
WHEN NOT matched BY target 
THEN
  INSERT
  (
	[ContextKey]          	
	,[SalesForce Case ID]   	
	,[Category]				
	,[ChecklistName] 		
	,[Error_Comment] 		
	,[Error] 				
	,[LastUpdatedTime]
  )
  VALUES
  (
	source.[ContextKey]
	,source.[SalesForce Case ID] 	
	,source.[Category]				
	,source.[ChecklistName] 	
	,source.[Error_Comment] 	
	,source.[Error] 			
	,source.[LastUpdatedTime]   
  ); 
END