IF NOT EXISTS (SELECT * FROM master.dbo.syslogins WHERE loginname = 'EPRI') CREATE LOGIN [EPRI] WITH PASSWORD = '1234', CHECK_EXPIRATION = OFF, CHECK_POLICY = OFF

/****** Object:  User [EPRI]    Script Date: 10/19/2016 8:01:47 AM ******/
CREATE USER [EPRI] FOR LOGIN [EPRI] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_datareader] ADD MEMBER [EPRI]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [EPRI]
GO
ALTER ROLE [db_owner] ADD MEMBER [EPRI]
GO


/****** Object:  StoredProcedure [dbo].[UniversalCascadeDelete]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author, William Ernest/ Stephen Wills>
-- Create date: <Create Date,12/1/2016>
-- Description:	<Description, Calls usp_delete_cascade to perform cascading deletes for a table>
-- =============================================
create PROCEDURE [dbo].[UniversalCascadeDelete]
	-- Add the parameters for the stored procedure here
	@tableName VARCHAR(200),
	@baseCriteria NVARCHAR(1000)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @deleteSQL NVARCHAR(900)

	CREATE TABLE #DeleteCascade
	(
		DeleteSQL NVARCHAR(900)
	)

	INSERT INTO #DeleteCascade
	EXEC usp_delete_cascade @tableName, @baseCriteria

	DECLARE DeleteCursor CURSOR FOR
	SELECT *
	FROM #DeleteCascade

	OPEN DeleteCursor

	FETCH NEXT FROM DeleteCursor
	INTO @deleteSQL

	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC sp_executesql @deleteSQL

		FETCH NEXT FROM DeleteCursor
		INTO @deleteSQL
	END

	CLOSE DeleteCursor
	DEALLOCATE DeleteCursor

	DROP TABLE #DeleteCascade
END



GO
/****** Object:  StoredProcedure [dbo].[usp_delete_cascade]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_delete_cascade] (
	@base_table_name varchar(200), @base_criteria nvarchar(1000)
)
as begin
	-- Adapted from http://www.sqlteam.com/article/performing-a-cascade-delete-in-sql-server-7
	-- Expects the name of a table, and a conditional for selecting rows
	-- within that table that you want deleted.
	-- Produces SQL that, when run, deletes all table rows referencing the ones
	-- you initially selected, cascading into any number of tables,
	-- without the need for "ON DELETE CASCADE".
	-- Does not appear to work with self-referencing tables, but it will
	-- delete everything beneath them.
	-- To make it easy on the server, put a "GO" statement between each line.

	declare @to_delete table (
		id int identity(1, 1) primary key not null,
		criteria nvarchar(1000) not null,
		table_name varchar(200) not null,
		processed bit not null,
		delete_sql varchar(1000)
	)

	insert into @to_delete (criteria, table_name, processed) values (@base_criteria, @base_table_name, 0)

	declare @id int, @criteria nvarchar(1000), @table_name varchar(200)
	while exists(select 1 from @to_delete where processed = 0) begin
		select top 1 @id = id, @criteria = criteria, @table_name = table_name from @to_delete where processed = 0 order by id desc

		insert into @to_delete (criteria, table_name, processed)
			select referencing_column.name + ' in (select [' + referenced_column.name + '] from [' + @table_name +'] where ' + @criteria + ')',
				referencing_table.name,
				0
			from  sys.foreign_key_columns fk
				inner join sys.columns referencing_column on fk.parent_object_id = referencing_column.object_id 
					and fk.parent_column_id = referencing_column.column_id 
				inner join  sys.columns referenced_column on fk.referenced_object_id = referenced_column.object_id 
					and fk.referenced_column_id = referenced_column.column_id 
				inner join  sys.objects referencing_table on fk.parent_object_id = referencing_table.object_id 
				inner join  sys.objects referenced_table on fk.referenced_object_id = referenced_table.object_id 
				inner join  sys.objects constraint_object on fk.constraint_object_id = constraint_object.object_id
			where referenced_table.name = @table_name
				and referencing_table.name != referenced_table.name

		update @to_delete set
			processed = 1
		where id = @id
	end

	select 'print ''deleting from ' + table_name + '...''; delete from [' + table_name + '] where ' + criteria from @to_delete order by id desc
end

GO
/****** Object:  UserDefinedFunction [dbo].[RemoveNonAlphaCharacters]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Function [dbo].[RemoveNonAlphaCharacters](@Temp VarChar(1000))
Returns VarChar(1000)
AS
Begin

    Declare @KeepValues as varchar(50)
    Set @KeepValues = '%[^a-zA-Z0-9]%'
    While PatIndex(@KeepValues, @Temp) > 0
        Set @Temp = Stuff(@Temp, PatIndex(@KeepValues, @Temp), 1, '')

    Return @Temp
End
GO

/****** Object:  UserDefinedFunction [dbo].[GetJSONValueForProperty]    Script Date: 6/30/2017 8:32:57 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE Function [dbo].GetJSONValueForProperty(@data varchar(max), @columnName varchar(max))
Returns VarChar(max)
AS
Begin

	   Return Substring(@Data, 
				   CHARINDEX('"'+@columnName+'":"', @data) + LEN('"'+@columnName+'":"'),
				   CHARINDEX('"', @data, CHARINDEX('"'+@columnName+'":"', @data) + LEN('"'+@columnName+'":"')) - CHARINDEX('"'+@columnName+'":"', @data) - LEN('"'+@columnName+'":"'))
End
GO

/****** Object:  UserDefinedFunction [dbo].[ReverseMercatorLat]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Function [dbo].[ReverseMercatorLat](
	@x float,
	@y float
)
Returns float
AS
Begin

    DECLARE @lon float = (@x / 20037508.34) * 180;
    DECLARE @lat float = (@y / 20037508.34) * 180;
                       
    SET @lat = 180 /pi() * (2 * atan(exp(@lat * pi() / 180)) - pi() / 2);
    Return @lat
End

GO
/****** Object:  UserDefinedFunction [dbo].[ReverseMercatorLng]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Function [dbo].[ReverseMercatorLng](
	@x float,
	@y float
)
Returns float
AS
Begin


    Return (@x / 20037508.34) * 180
End

GO
/****** Object:  Table [dbo].[ApplicationRole]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ApplicationRole](
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_ApplicationRole_ID]  DEFAULT (newid()),
	[Name] [varchar](200) NOT NULL,
	[Description] [varchar](max) NULL,
	[NodeID] [uniqueidentifier] NOT NULL,
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_ApplicationRole_CreatedOn]  DEFAULT (getutcdate()),
	[CreatedBy] [varchar](200) NOT NULL CONSTRAINT [DF_ApplicationRole_CreatedBy]  DEFAULT (suser_name()),
	[UpdatedOn] [datetime] NOT NULL CONSTRAINT [DF_ApplicationRole_UpdatedOn]  DEFAULT (getutcdate()),
	[UpdatedBy] [varchar](200) NOT NULL CONSTRAINT [DF_ApplicationRole_UpdatedBy]  DEFAULT (getutcdate()),
 CONSTRAINT [PK_ApplicationRole] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ApplicationRoleSecurityGroup]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationRoleSecurityGroup](
	[ApplicationRoleID] [uniqueidentifier] NOT NULL,
	[SecurityGroupID] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ApplicationRoleUserAccount]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationRoleUserAccount](
	[ApplicationRoleID] [uniqueidentifier] NOT NULL,
	[UserAccountID] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ColorGradients]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ColorGradients](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](400) NOT NULL,
	[RangeValue] [varchar](500) NOT NULL,
	[HexColor] [varchar](10) NOT NULL,
	[LoadOrder] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[File]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[File](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FileTypeID] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Data] [varchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FileType]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FileType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Description] [varchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IndividualResult]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[IndividualResult](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ResultID] [int] NULL,
	[NodePointID] [int] NULL,
	[Data] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LineCharacteristicsToDisplay]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LineCharacteristicsToDisplay](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](400) NOT NULL,
	[Label] [varchar](500) NOT NULL,
	[FileTypeID] [int] NOT NULL,
	[Display] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LineSegment]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[LineSegment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ResultID] [int] NULL,
	[FromNodeID] [int] NULL,
	[ToNodeID] [int] NULL,
	[Data] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Node]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Node](
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Node_ID]  DEFAULT (newid()),
	[Name] [varchar](200) NOT NULL,
	[CompanyID] [int] NULL,
	[Longitude] [decimal](9, 6) NULL,
	[Latitude] [decimal](9, 6) NULL,
	[Description] [varchar](max) NULL,
	[ImagePath] [varchar](max) NULL,
	[Settings] [varchar](max) NULL,
	[MenuType] [varchar](200) NOT NULL CONSTRAINT [DF_Node_MenuType]  DEFAULT (N'File'),
	[MenuData] [varchar](max) NOT NULL CONSTRAINT [DF_Node_MenuData]  DEFAULT (N'Menu.xml'),
	[Master] [bit] NOT NULL CONSTRAINT [DF_Node_Master]  DEFAULT ((0)),
	[LoadOrder] [int] NOT NULL CONSTRAINT [DF_Node_LoadOrder]  DEFAULT ((0)),
	[Enabled] [bit] NOT NULL CONSTRAINT [DF_Node_Enabled]  DEFAULT ((0)),
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_Node_CreatedOn]  DEFAULT (getutcdate()),
	[CreatedBy] [varchar](200) NOT NULL CONSTRAINT [DF_Node_CreatedBy]  DEFAULT (suser_name()),
	[UpdatedOn] [datetime] NOT NULL CONSTRAINT [DF_Node_UpdatedOn]  DEFAULT (getutcdate()),
	[UpdatedBy] [varchar](200) NOT NULL CONSTRAINT [DF_Node_UpdatedBy]  DEFAULT (suser_name()),
 CONSTRAINT [PK_Node] PRIMARY KEY NONCLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[NodePoint]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[NodePoint](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[ResultID] [int] NULL,
	[XCoordinate] [float] NULL,
	[YCoordinate] [float] NULL,
	[Latitude] [float] NULL,
	[Longitude] [float] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Result]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[Result](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[NodeRFileID] [int] NULL,
	[NodeZFileID] [int] NULL,
	[NodeLDGFileID] [int] NULL,
	[LineFileID] [int] NULL,
	[Post] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ResultsToDisplay]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ResultsToDisplay](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](400) NOT NULL,
	[Label] [varchar](500) NOT NULL,
	[FileTypeID] [int] NOT NULL,
	[Display] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SecurityGroup]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SecurityGroup](
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_SecurityGroup_ID]  DEFAULT (newid()),
	[Name] [varchar](200) NOT NULL,
	[Description] [varchar](max) NULL,
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_SecurityGroup_CreatedOn]  DEFAULT (getutcdate()),
	[CreatedBy] [varchar](200) NOT NULL CONSTRAINT [DF_SecurityGroup_CreatedBy]  DEFAULT (suser_name()),
	[UpdatedOn] [datetime] NOT NULL CONSTRAINT [DF_SecurityGroup_UpdatedOn]  DEFAULT (getutcdate()),
	[UpdatedBy] [varchar](200) NOT NULL CONSTRAINT [DF_SecurityGroup_UpdatedBy]  DEFAULT (getutcdate()),
 CONSTRAINT [PK_SecurityGroup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SecurityGroupUserAccount]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityGroupUserAccount](
	[SecurityGroupID] [uniqueidentifier] NOT NULL,
	[UserAccountID] [uniqueidentifier] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Setting]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Setting](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Label] [varchar](300) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserAccount]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserAccount](
	[ID] [uniqueidentifier] NOT NULL CONSTRAINT [DF_UserAccount_ID]  DEFAULT (newid()),
	[Name] [varchar](200) NOT NULL,
	[Password] [varchar](200) NULL,
	[FirstName] [varchar](200) NULL,
	[LastName] [varchar](200) NULL,
	[DefaultNodeID] [uniqueidentifier] NOT NULL,
	[Phone] [varchar](200) NULL,
	[Email] [varchar](200) NULL,
	[LockedOut] [bit] NOT NULL CONSTRAINT [DF_UserAccount_LockedOut]  DEFAULT ((0)),
	[UseADAuthentication] [bit] NOT NULL CONSTRAINT [DF_UserAccount_UseADAuthentication]  DEFAULT ((1)),
	[ChangePasswordOn] [datetime] NULL CONSTRAINT [DF_UserAccount_ChangePasswordOn]  DEFAULT (dateadd(day,(90),getutcdate())),
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_UserAccount_CreatedOn]  DEFAULT (getutcdate()),
	[CreatedBy] [varchar](50) NOT NULL CONSTRAINT [DF_UserAccount_CreatedBy]  DEFAULT (suser_name()),
	[UpdatedOn] [datetime] NOT NULL CONSTRAINT [DF_UserAccount_UpdatedOn]  DEFAULT (getutcdate()),
	[UpdatedBy] [varchar](50) NOT NULL CONSTRAINT [DF_UserAccount_UpdatedBy]  DEFAULT (suser_name()),
 CONSTRAINT [PK_UserAccount] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  View [dbo].[IndividualResultView]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[IndividualResultView]
AS
SELECT        dbo.IndividualResult.ID, dbo.IndividualResult.ResultID, dbo.IndividualResult.NodePointID, dbo.IndividualResult.Data, dbo.NodePoint.Name AS Node
FROM            dbo.IndividualResult INNER JOIN
                         dbo.NodePoint ON dbo.NodePoint.ID = dbo.IndividualResult.NodePointID

GO
/****** Object:  View [dbo].[LineSegmentView]    Script Date: 6/27/2017 1:26:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[LineSegmentView]
AS
SELECT        dbo.LineSegment.ID, dbo.LineSegment.ResultID, dbo.LineSegment.FromNodeID, dbo.LineSegment.ToNodeID, dbo.LineSegment.Data, fromNode.Name AS FromNode, 
                         toNode.Name AS ToNode
FROM            dbo.LineSegment INNER JOIN
                         dbo.NodePoint AS fromNode ON dbo.LineSegment.FromNodeID = fromNode.ID INNER JOIN
                         dbo.NodePoint AS toNode ON dbo.LineSegment.ToNodeID = toNode.ID

GO
ALTER TABLE [dbo].[ApplicationRole]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationRole_Node] FOREIGN KEY([NodeID])
REFERENCES [dbo].[Node] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationRole] CHECK CONSTRAINT [FK_ApplicationRole_Node]
GO
ALTER TABLE [dbo].[ApplicationRoleSecurityGroup]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationRoleSecurityGroup_ApplicationRole] FOREIGN KEY([ApplicationRoleID])
REFERENCES [dbo].[ApplicationRole] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationRoleSecurityGroup] CHECK CONSTRAINT [FK_ApplicationRoleSecurityGroup_ApplicationRole]
GO
ALTER TABLE [dbo].[ApplicationRoleSecurityGroup]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationRoleSecurityGroup_SecurityGroup] FOREIGN KEY([SecurityGroupID])
REFERENCES [dbo].[SecurityGroup] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationRoleSecurityGroup] CHECK CONSTRAINT [FK_ApplicationRoleSecurityGroup_SecurityGroup]
GO
ALTER TABLE [dbo].[ApplicationRoleUserAccount]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationRoleUserAccount_ApplicationRole] FOREIGN KEY([ApplicationRoleID])
REFERENCES [dbo].[ApplicationRole] ([ID])
GO
ALTER TABLE [dbo].[ApplicationRoleUserAccount] CHECK CONSTRAINT [FK_ApplicationRoleUserAccount_ApplicationRole]
GO
ALTER TABLE [dbo].[ApplicationRoleUserAccount]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationRoleUserAccount_UserAccount] FOREIGN KEY([UserAccountID])
REFERENCES [dbo].[UserAccount] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationRoleUserAccount] CHECK CONSTRAINT [FK_ApplicationRoleUserAccount_UserAccount]
GO
ALTER TABLE [dbo].[IndividualResult]  WITH CHECK ADD FOREIGN KEY([NodePointID])
REFERENCES [dbo].[NodePoint] ([ID])
GO
ALTER TABLE [dbo].[IndividualResult]  WITH CHECK ADD FOREIGN KEY([ResultID])
REFERENCES [dbo].[Result] ([ID])
GO
ALTER TABLE [dbo].[LineCharacteristicsToDisplay]  WITH CHECK ADD FOREIGN KEY([FileTypeID])
REFERENCES [dbo].[FileType] ([ID])
GO
ALTER TABLE [dbo].[LineSegment]  WITH CHECK ADD FOREIGN KEY([FromNodeID])
REFERENCES [dbo].[NodePoint] ([ID])
GO
ALTER TABLE [dbo].[LineSegment]  WITH CHECK ADD FOREIGN KEY([ResultID])
REFERENCES [dbo].[Result] ([ID])
GO
ALTER TABLE [dbo].[LineSegment]  WITH CHECK ADD FOREIGN KEY([ToNodeID])
REFERENCES [dbo].[NodePoint] ([ID])
GO
ALTER TABLE [dbo].[NodePoint]  WITH CHECK ADD FOREIGN KEY([ResultID])
REFERENCES [dbo].[Result] ([ID])
GO
ALTER TABLE [dbo].[Result]  WITH CHECK ADD FOREIGN KEY([LineFileID])
REFERENCES [dbo].[File] ([ID])
GO
ALTER TABLE [dbo].[Result]  WITH CHECK ADD FOREIGN KEY([NodeLDGFileID])
REFERENCES [dbo].[File] ([ID])
GO
ALTER TABLE [dbo].[Result]  WITH CHECK ADD FOREIGN KEY([NodeRFileID])
REFERENCES [dbo].[File] ([ID])
GO
ALTER TABLE [dbo].[Result]  WITH CHECK ADD FOREIGN KEY([NodeZFileID])
REFERENCES [dbo].[File] ([ID])
GO
ALTER TABLE [dbo].[ResultsToDisplay]  WITH CHECK ADD FOREIGN KEY([FileTypeID])
REFERENCES [dbo].[FileType] ([ID])
GO
ALTER TABLE [dbo].[SecurityGroupUserAccount]  WITH CHECK ADD  CONSTRAINT [FK_SecurityGroupUserAccount_SecurityGroup] FOREIGN KEY([SecurityGroupID])
REFERENCES [dbo].[SecurityGroup] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SecurityGroupUserAccount] CHECK CONSTRAINT [FK_SecurityGroupUserAccount_SecurityGroup]
GO
ALTER TABLE [dbo].[SecurityGroupUserAccount]  WITH CHECK ADD  CONSTRAINT [FK_SecurityGroupUserAccount_UserAccount] FOREIGN KEY([UserAccountID])
REFERENCES [dbo].[UserAccount] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SecurityGroupUserAccount] CHECK CONSTRAINT [FK_SecurityGroupUserAccount_UserAccount]
GO
ALTER TABLE [dbo].[UserAccount]  WITH CHECK ADD  CONSTRAINT [FK_UserAccount_Node] FOREIGN KEY([DefaultNodeID])
REFERENCES [dbo].[Node] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserAccount] CHECK CONSTRAINT [FK_UserAccount_Node]
GO
