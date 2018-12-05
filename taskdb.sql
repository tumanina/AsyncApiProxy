CREATE TABLE [dbo].[Task](
	[Id] [uniqueidentifier] NULL,
	[Data] [nvarchar](255) NOT NULL,
	[Result] [nvarchar](255) NULL,
	[Status] [int] NULL,
	[Error] [nvarchar](255) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[Type] [int] NULL
) ON [PRIMARY]
GO