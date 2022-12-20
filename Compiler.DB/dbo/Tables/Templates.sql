﻿CREATE TABLE [dbo].[Templates]
(
	[Guid]		UNIQUEIDENTIFIER	NOT NULL,
	[Name]		NVARCHAR (100)		NOT NULL,
	[Source]	NVARCHAR (MAX)		NOT NULL,
	[Type]		INT					NOT NULL,
	[Create]	DATE				NOT NULL,
	CONSTRAINT [PK_Templates_Guid]
	PRIMARY KEY CLUSTERED 
	([Guid] ASC)
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
ALTER TABLE [dbo].[Templates]
ADD DEFAULT (NEWID())
FOR [Guid]

GO
ALTER TABLE [dbo].[Templates]
ADD DEFAULT (GETDATE())
FOR [Create]