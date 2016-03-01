
USE [$(CastleBlack)]

BEGIN TRAN
	TRUNCATE TABLE [$(CastleBlack)].[NIghtsWatch].[Checklists]
COMMIT TRAN

BEGIN TRAN --INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'',N'')

	--New Implementation or Base Build
	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'New Implementation or Base Build',N'Typo or spelling error')
	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'New Implementation or Base Build',N'Error in functionality for making a report, asking a question')
	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'New Implementation or Base Build',N'Incorrect data privacy flow or restrictions')
	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'New Implementation or Base Build',N'Error in locking of locations on the issue package')
	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'New Implementation or Base Build',N'Error in use of PSO instructions with regard to locking locations ')
	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'New Implementation or Base Build',N'Incorrect attachment')
	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'New Implementation or Base Build',N'Incorrect phone number based on TOW (Telephony Order Form) attachment in SalesForce (if mentioned in case)')
	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'New Implementation or Base Build',N'Incorrect package on child tier (if relevant). If not copied to children, failed to add note in PSO instructions explaining why children have different package than parent.')
	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'New Implementation or Base Build',N'Incorrect screen pop greeting compared to greeting in workbook')
	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'New Implementation or Base Build',N'Follow up link or form did not have full path (for European server clients)')
	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'New Implementation or Base Build',N'Monetary value questions are inconsistent or non-functional')
	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'New Implementation or Base Build',N'Developer used incorrect template')
	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'New Implementation or Base Build',N'Browser - Checked in Firefox')
	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'New Implementation or Base Build',N'Browser - Checked in Chrome')
	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'New Implementation or Base Build',N'Browser - Checked in IE11')
	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'New Implementation or Base Build',N'Browser - Checked in IE10')
	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'New Implementation or Base Build',N'Browser - Checked in IE9')
	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'New Implementation or Base Build',N'Browser - Checked in IE8')
																						
	--Attachments																			 
	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'Attachments',N'Incorrect nav bar links')	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'Attachments',N'Incorrect body text embedded links')	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'Attachments',N'Attachment when checked from live link was incorrect')	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'Attachments',N'"Leaving EP" interim page missing for attachments to external resources')	INSERT INTO [$(CastleBlack)].[NightsWatch].[Checklists]([Category],[ChecklistName]) VALUES (N'Attachments',N'Attachment update not correct for all languages')

	--Web  Site Text Edit - English

	--Web Site Text Edit - non-English

	--Web Site Edits to Functionality or Look and Feel

	--Screen Pop / Contact Center Intake Site Text

	--Locations

	--Issue Types

	--Custom Questions

	--GoLive

	--Information Request or Scoping

	--LDB Pull

	--Translation Spreadsheet Pull

	--Translation Upload

	--Translation LANGUAGES

	--Web Site Redesign

	--Data Privacy/ Dialing Instructions

	--Redesign GoLive Check

	--Redesign GoLive

	--Discontinued Client

	--Old Dialing Instructions Update

	--Report Form Text Edit - English

	--Report Form Text Edit - non-English

	--Report Form Edits to Functionality or Look and Feel

	--
COMMIT TRAN