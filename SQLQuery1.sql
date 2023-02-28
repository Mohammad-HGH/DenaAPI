USE [Denadb]

GO

/****** Object:  Table [dbo].[RefreshToken]    Script Date: 1/18/2022 6:10:48 PM ******/


SET ANSI_NULLS ON

GO


SET QUOTED_IDENTIFIER ON

GO


CREATE TABLE [dbo].[RefreshToken](


	[Id] [int] IDENTITY(1,1) NOT NULL,


	[UserId] [int] NOT NULL,


	[TokenHash] [nvarchar](1000) NOT NULL,


	[TokenSalt] [nvarchar](50) NOT NULL,


	[TS] [smalldatetime] NOT NULL,


	[ExpiryDate] [smalldatetime] NOT NULL,


 CONSTRAINT [PK_RefreshToken] PRIMARY KEY CLUSTERED 

(


	[Id] ASC


)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]


) ON [PRIMARY]

GO