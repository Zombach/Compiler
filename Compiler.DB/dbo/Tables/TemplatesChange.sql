CREATE TABLE [dbo].[TemplatesChange]
(
	[Guid]			UNIQUEIDENTIFIER	NOT NULL,
	[Number]		INT					NOT NULL,
	[Change]		DATE				NOT NULL,
	[Description]	NVARCHAR (MAX)		NOT NULL			
	CONSTRAINT [PK_TemplatesChange_Cluster]
	PRIMARY KEY CLUSTERED 
	(
		[Guid] ASC,
		[Number] ASC
	)
	WITH
	(
		PAD_INDEX = OFF,
		STATISTICS_NORECOMPUTE = OFF,
		IGNORE_DUP_KEY = OFF,
		ALLOW_ROW_LOCKS = ON,
		ALLOW_PAGE_LOCKS = ON,
		OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
	)
	ON [PRIMARY]
)

GO
ALTER TABLE [dbo].[TemplatesChange]
ADD CONSTRAINT [FK_Guid]
FOREIGN KEY([Guid])
REFERENCES [dbo].[Templates] ([Guid])

GO
ALTER TABLE [dbo].[TemplatesChange]
ADD DEFAULT (GETDATE())
FOR [Change]