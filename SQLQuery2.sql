/****** Object:  Table [dbo].[Task]    Script Date: 1/18/2022 6:10:48 PM ******/


SET ANSI_NULLS ON

GO


SET QUOTED_IDENTIFIER ON

GO


CREATE TABLE [dbo].[Task](


	[Id] [int] IDENTITY(1,1) NOT NULL,


	[UserId] [int] NOT NULL,


	[Name] [nvarchar](100) NOT NULL,


	[IsCompleted] [bit] NOT NULL,


	[TS] [smalldatetime] NOT NULL,


 CONSTRAINT [PK_Task] PRIMARY KEY CLUSTERED 

(


	[Id] ASC


)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]


) ON [PRIMARY]

GO