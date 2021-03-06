USE [SocietyManagement]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 13-09-2017 10:11:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 13-09-2017 10:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 13-09-2017 10:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 13-09-2017 10:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 13-09-2017 10:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Gender] [nvarchar](10) NOT NULL,
	[DOB] [datetime] NULL,
	[Profession] [nvarchar](500) NULL,
	[Occupation] [nvarchar](500) NULL,
	[UnitID] [int] NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Audits]    Script Date: 13-09-2017 10:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Audits](
	[AuditID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[TableName] [nvarchar](50) NOT NULL,
	[FieldName] [nvarchar](50) NOT NULL,
	[ID] [numeric](18, 0) NOT NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[Action] [nvarchar](50) NULL,
	[Details] [nvarchar](max) NULL,
	[ActionDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Audits] PRIMARY KEY CLUSTERED 
(
	[AuditID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Buildings]    Script Date: 13-09-2017 10:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Buildings](
	[BuildingID] [int] IDENTITY(1,1) NOT NULL,
	[BuildingName] [nvarchar](500) NOT NULL,
	[BuildingTypeID] [int] NOT NULL,
	[Details] [nvarchar](max) NULL,
	[UDK1] [nvarchar](max) NULL,
	[UDK2] [nvarchar](max) NULL,
	[UDK3] [nvarchar](max) NULL,
	[UDK4] [nvarchar](max) NULL,
	[UDK5] [nvarchar](max) NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_Buildings_CreatedDate]  DEFAULT (getdate()),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_Buildings_ModifiedDate]  DEFAULT (getdate()),
	[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_Buildings_IsDeleted]  DEFAULT ((0)),
 CONSTRAINT [PK_Buildings] PRIMARY KEY CLUSTERED 
(
	[BuildingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BuildingUnits]    Script Date: 13-09-2017 10:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BuildingUnits](
	[UnitID] [int] IDENTITY(1,1) NOT NULL,
	[BuildingID] [int] NOT NULL,
	[UnitName] [nvarchar](500) NOT NULL,
	[UnitTypeID] [int] NOT NULL,
	[Details] [nvarchar](max) NULL,
	[OwnerID] [nvarchar](128) NULL,
	[OneTimeMaintenance] [numeric](18, 2) NOT NULL CONSTRAINT [DF_BuildingUnits_OneTimeMaintenance]  DEFAULT ((0)),
	[UnitArea] [numeric](18, 0) NOT NULL CONSTRAINT [DF_BuildingUnits_UnitArea]  DEFAULT ((0)),
	[UDK1] [nvarchar](max) NULL,
	[UDK2] [nvarchar](max) NULL,
	[UDK3] [nvarchar](max) NULL,
	[UDK4] [nvarchar](max) NULL,
	[UDK5] [nvarchar](max) NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_BuildingUnits_CreatedDate]  DEFAULT (getdate()),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_BuildingUnits_ModifiedDate]  DEFAULT (getdate()),
	[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_BuildingUnits_IsDeleted]  DEFAULT ((0)),
 CONSTRAINT [PK_BuildingUnits] PRIMARY KEY CLUSTERED 
(
	[UnitID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Collections]    Script Date: 13-09-2017 10:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Collections](
	[CollectionID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[CollectionDate] [date] NOT NULL,
	[UnitID] [int] NOT NULL,
	[Amount] [numeric](18, 2) NOT NULL,
	[LatePaymentCharges] [numeric](18, 2) NOT NULL,
	[ReceiptNumber] [nvarchar](100) NOT NULL,
	[PaymentModeID] [int] NOT NULL,
	[Reference] [nvarchar](max) NULL,
	[ChequeBank] [nvarchar](max) NULL,
	[ChequeDate] [date] NULL,
	[ChequeNumber] [nvarchar](100) NULL,
	[ChequeName] [nvarchar](255) NULL,
	[ChequeCleared] [bit] NOT NULL,
	[ChequeEncashmentDate] [date] NULL,
	[Details] [nvarchar](max) NULL,
	[UDK1] [nvarchar](max) NULL,
	[UDK2] [nvarchar](max) NULL,
	[UDK3] [nvarchar](max) NULL,
	[UDK4] [nvarchar](max) NULL,
	[UDK5] [nvarchar](max) NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Collections] PRIMARY KEY CLUSTERED 
(
	[CollectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ComplaintComments]    Script Date: 13-09-2017 10:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ComplaintComments](
	[CommentID] [int] IDENTITY(1,1) NOT NULL,
	[ComplaintID] [int] NOT NULL,
	[AuthorID] [nvarchar](128) NOT NULL,
	[AssignToID] [nvarchar](128) NULL,
	[Status] [int] NOT NULL,
	[Comment] [nvarchar](max) NOT NULL,
	[UDK1] [nvarchar](max) NULL,
	[UDK2] [nvarchar](max) NULL,
	[UDK3] [nvarchar](max) NULL,
	[UDK4] [nvarchar](max) NULL,
	[UDK5] [nvarchar](max) NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_ComplaintComments] PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Complaints]    Script Date: 13-09-2017 10:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Complaints](
	[ComplaintID] [int] IDENTITY(1,1) NOT NULL,
	[AuthorID] [nvarchar](128) NOT NULL,
	[ComplaintDate] [datetime] NOT NULL CONSTRAINT [DF_Complaints_ComplaintDate]  DEFAULT (getdate()),
	[ComplaintTypeID] [int] NOT NULL,
	[AssignToID] [nvarchar](128) NULL,
	[Title] [nvarchar](500) NOT NULL,
	[Details] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_Complaints_IsDeleted]  DEFAULT ((0)),
	[UDK1] [nvarchar](max) NULL,
	[UDK2] [nvarchar](max) NULL,
	[UDK3] [nvarchar](max) NULL,
	[UDK4] [nvarchar](max) NULL,
	[UDK5] [nvarchar](max) NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_Complaints_CreatedDate]  DEFAULT (getdate()),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_Complaints_ModifiedDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_Complaints] PRIMARY KEY CLUSTERED 
(
	[ComplaintID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Dues]    Script Date: 13-09-2017 10:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dues](
	[DueID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[BillDate] [date] NOT NULL,
	[UnitID] [int] NOT NULL,
	[DueTypeID] [int] NOT NULL,
	[DueAmount] [numeric](18, 2) NOT NULL,
	[Details] [nvarchar](max) NULL,
	[DueDate] [date] NOT NULL,
	[RecurringID] [numeric](18, 0) NULL,
	[UDK1] [nvarchar](max) NULL,
	[UDK2] [nvarchar](max) NULL,
	[UDK3] [nvarchar](max) NULL,
	[UDK4] [nvarchar](max) NULL,
	[UDK5] [nvarchar](max) NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Dues] PRIMARY KEY CLUSTERED 
(
	[DueID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ForumComments]    Script Date: 13-09-2017 10:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ForumComments](
	[CommentID] [int] IDENTITY(1,1) NOT NULL,
	[ForumID] [int] NOT NULL,
	[AuthorID] [nvarchar](128) NOT NULL,
	[Comment] [nvarchar](max) NOT NULL,
	[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_ForumComments_IsDeleted]  DEFAULT ((0)),
	[UDK1] [nvarchar](max) NULL,
	[UDK2] [nvarchar](max) NULL,
	[UDK3] [nvarchar](max) NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_ForumComments_CreatedDate]  DEFAULT (getdate()),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_ForumComments_ModifiedDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_ForumComments] PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Forums]    Script Date: 13-09-2017 10:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Forums](
	[ForumID] [int] IDENTITY(1,1) NOT NULL,
	[AuthorID] [nvarchar](128) NOT NULL,
	[ForumTitle] [nvarchar](500) NOT NULL,
	[Details] [nvarchar](max) NULL,
	[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_Forums_IsDeleted]  DEFAULT ((0)),
	[UDK1] [nvarchar](max) NULL,
	[UDK2] [nvarchar](max) NULL,
	[UDK3] [nvarchar](max) NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_Forums_CreatedDate]  DEFAULT (getdate()),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_Forums_ModifiedDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_Forums] PRIMARY KEY CLUSTERED 
(
	[ForumID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[KeyValues]    Script Date: 13-09-2017 10:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KeyValues](
	[KeyID] [int] IDENTITY(1,1) NOT NULL,
	[KeyName] [nvarchar](500) NOT NULL,
	[KeyValues] [nvarchar](500) NOT NULL,
	[KeyOrder] [int] NOT NULL CONSTRAINT [DF_Table_1_KeyOrder]  DEFAULT ((0)),
	[Details] [nvarchar](max) NULL,
	[UDK1] [nvarchar](max) NULL,
	[UDK2] [nvarchar](max) NULL,
	[UDK3] [nvarchar](max) NULL,
 CONSTRAINT [PK_KeyValues] PRIMARY KEY CLUSTERED 
(
	[KeyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NoticeBoard]    Script Date: 13-09-2017 10:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoticeBoard](
	[NoticeID] [int] IDENTITY(1,1) NOT NULL,
	[NoticeDate] [datetime] NOT NULL,
	[NoticeHeading] [nvarchar](500) NOT NULL,
	[Notice] [nvarchar](max) NULL,
	[ExpiryDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_NoticeBoard_IsDeleted]  DEFAULT ((0)),
	[UDK1] [nvarchar](max) NULL,
	[UDK2] [nvarchar](max) NULL,
	[UDK3] [nvarchar](max) NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_NoticeBoard_CreatedDate]  DEFAULT (getdate()),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_NoticeBoard_UpdatedDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_NoticeBoard] PRIMARY KEY CLUSTERED 
(
	[NoticeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PollOptions]    Script Date: 13-09-2017 10:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PollOptions](
	[PollOptionID] [int] IDENTITY(1,1) NOT NULL,
	[PollID] [int] NOT NULL,
	[PollOption] [nvarchar](max) NULL,
	[OptionOrder] [int] NOT NULL,
	[UDK1] [nvarchar](max) NULL,
	[UDK2] [nvarchar](max) NULL,
	[UDK3] [nvarchar](max) NULL,
 CONSTRAINT [PK_PollOptions] PRIMARY KEY CLUSTERED 
(
	[PollOptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Polls]    Script Date: 13-09-2017 10:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Polls](
	[PollID] [int] IDENTITY(1,1) NOT NULL,
	[PollTitle] [nvarchar](500) NULL,
	[Details] [nvarchar](max) NULL,
	[StartDate] [datetime] NOT NULL CONSTRAINT [DF_Polls_PollStartDate]  DEFAULT (getdate()),
	[EndDate] [datetime] NOT NULL CONSTRAINT [DF_Polls_PollEndDate]  DEFAULT (getdate()),
	[PollTypeID] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_Polls_IsDeleted]  DEFAULT ((0)),
	[UDK1] [nvarchar](max) NULL,
	[UDK2] [nvarchar](max) NULL,
	[UDK3] [nvarchar](max) NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[CreatedDate] [datetime] NOT NULL CONSTRAINT [DF_Polls_CreatedDate]  DEFAULT (getdate()),
	[ModifiedDate] [datetime] NOT NULL CONSTRAINT [DF_Polls_UpdatedDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_Polls] PRIMARY KEY CLUSTERED 
(
	[PollID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PollVotes]    Script Date: 13-09-2017 10:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PollVotes](
	[VoteID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[PollOptionID] [int] NOT NULL,
	[VoteByID] [nvarchar](128) NOT NULL,
	[VoteDate] [datetime] NOT NULL,
	[UDK1] [nvarchar](max) NOT NULL,
	[UDK2] [nvarchar](max) NOT NULL,
	[UDK3] [nvarchar](max) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_PollVotes] PRIMARY KEY CLUSTERED 
(
	[VoteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RecurringDues]    Script Date: 13-09-2017 10:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RecurringDues](
	[RecurringID] [numeric](18, 0) IDENTITY(1,1) NOT NULL,
	[UnitID] [int] NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[RecurringTypeID] [int] NOT NULL,
	[DueTypeID] [int] NOT NULL,
	[Amount] [numeric](18, 2) NOT NULL,
	[DueDays] [int] NOT NULL,
	[LastRunDate] [datetime] NULL,
	[UDK1] [nvarchar](max) NULL,
	[UDK2] [nvarchar](max) NULL,
	[UDK3] [nvarchar](max) NULL,
	[UDK4] [nvarchar](max) NULL,
	[UDK5] [nvarchar](max) NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_RecurringDues] PRIMARY KEY CLUSTERED 
(
	[RecurringID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tenants]    Script Date: 13-09-2017 10:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tenants](
	[TenantID] [int] IDENTITY(1,1) NOT NULL,
	[UnitID] [int] NOT NULL,
	[TenantTypeID] [int] NOT NULL,
	[TenantName] [nvarchar](500) NOT NULL,
	[DateOfJoining] [date] NOT NULL,
	[PermanentAddress] [nvarchar](max) NULL,
	[Email] [nvarchar](500) NULL,
	[Mobile] [nvarchar](50) NULL,
	[Details] [nvarchar](max) NULL,
	[DateOfLeaving] [date] NULL,
	[IsActive] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[UDK1] [nvarchar](max) NULL,
	[UDK2] [nvarchar](max) NULL,
	[UDK3] [nvarchar](max) NULL,
	[UDK4] [nvarchar](max) NULL,
	[UDK5] [nvarchar](max) NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Tenants] PRIMARY KEY CLUSTERED 
(
	[TenantID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserDefineFields]    Script Date: 13-09-2017 10:11:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserDefineFields](
	[UDFID] [int] IDENTITY(1,1) NOT NULL,
	[TableName] [nvarchar](500) NOT NULL,
	[FieldName] [nvarchar](500) NOT NULL,
	[IsDispaly] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[FieldLable] [nvarchar](500) NOT NULL,
	[ControlType] [nvarchar](500) NOT NULL,
	[FieldType] [nvarchar](500) NOT NULL,
	[DefaultValue] [nvarchar](max) NULL,
	[IsRequired] [bit] NOT NULL,
	[IsRangeValidator] [bit] NOT NULL,
	[MinimumValue] [nvarchar](500) NULL,
	[MaximumValue] [nvarchar](500) NULL,
	[IsRegularExpression] [bit] NOT NULL,
	[RegularExpression] [nvarchar](max) NULL,
	[UserID] [nvarchar](128) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_UserDefineFields] PRIMARY KEY CLUSTERED 
(
	[UDFID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'05558045-b5b4-4b23-9f23-dbe10eb423e8', N'User')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'aa6c91a0-dde1-4fad-ac9a-102a165cba32', N'Manager')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'f228bf6b-21d1-474e-ad56-3fb4705f502f', N'Admin')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'81cd3d3b-848f-4300-bb37-02b230537eae', N'f228bf6b-21d1-474e-ad56-3fb4705f502f')
GO
INSERT [dbo].[AspNetUsers] ([Id], [FirstName], [LastName], [Gender], [DOB], [Profession], [Occupation], [UnitID], [Email], [EmailConfirmed], [PasswordHash], [SecurityStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEndDateUtc], [LockoutEnabled], [AccessFailedCount], [UserName]) VALUES (N'81cd3d3b-848f-4300-bb37-02b230537eae', N'Mukesh', N'Patel', N'Male', NULL, NULL, NULL, NULL, N'mukeshbpatel@gmail.com', 0, N'AL0ryORX4tYTYmhjZQBCL+xT3s72Wiernr2RbiC4E0mpO0sLEnYC6xLuuVpWC6BV6w==', N'67257eed-ebaf-4cb7-addd-f6d93468c111', NULL, 0, 0, NULL, 1, 0, N'9860002040')
GO
SET IDENTITY_INSERT [dbo].[Buildings] ON 

GO
INSERT [dbo].[Buildings] ([BuildingID], [BuildingName], [BuildingTypeID], [Details], [UDK1], [UDK2], [UDK3], [UDK4], [UDK5], [UserID], [CreatedDate], [ModifiedDate], [IsDeleted]) VALUES (1, N'A Wing', 5, N'Total 24 Flats 1/1.5 BHK', NULL, NULL, NULL, NULL, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(N'2017-09-06 09:42:46.700' AS DateTime), CAST(N'2017-09-06 09:42:46.700' AS DateTime), 0)
GO
INSERT [dbo].[Buildings] ([BuildingID], [BuildingName], [BuildingTypeID], [Details], [UDK1], [UDK2], [UDK3], [UDK4], [UDK5], [UserID], [CreatedDate], [ModifiedDate], [IsDeleted]) VALUES (2, N'B Wing', 5, N'Total 24 Flats 1/1.5 BHK', NULL, NULL, NULL, NULL, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(N'2017-09-06 09:43:55.557' AS DateTime), CAST(N'2017-09-06 09:43:55.557' AS DateTime), 0)
GO
INSERT [dbo].[Buildings] ([BuildingID], [BuildingName], [BuildingTypeID], [Details], [UDK1], [UDK2], [UDK3], [UDK4], [UDK5], [UserID], [CreatedDate], [ModifiedDate], [IsDeleted]) VALUES (3, N'C Wing', 5, N'Total 24 Flats 1/1.5 BHK', NULL, NULL, NULL, NULL, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(N'2017-09-06 09:44:09.890' AS DateTime), CAST(N'2017-09-06 09:44:09.890' AS DateTime), 0)
GO
INSERT [dbo].[Buildings] ([BuildingID], [BuildingName], [BuildingTypeID], [Details], [UDK1], [UDK2], [UDK3], [UDK4], [UDK5], [UserID], [CreatedDate], [ModifiedDate], [IsDeleted]) VALUES (4, N'Shops', 6, N'Total 100 Shops', NULL, NULL, NULL, NULL, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(N'2017-09-06 09:45:00.830' AS DateTime), CAST(N'2017-09-06 09:45:00.830' AS DateTime), 0)
GO
SET IDENTITY_INSERT [dbo].[Buildings] OFF
GO
SET IDENTITY_INSERT [dbo].[BuildingUnits] ON 

GO
INSERT [dbo].[BuildingUnits] ([UnitID], [BuildingID], [UnitName], [UnitTypeID], [Details], [OwnerID], [OneTimeMaintenance], [UnitArea], [UDK1], [UDK2], [UDK3], [UDK4], [UDK5], [UserID], [CreatedDate], [ModifiedDate], [IsDeleted]) VALUES (1, 1, N'A-101', 9, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(95000.00 AS Numeric(18, 2)), CAST(985 AS Numeric(18, 0)), NULL, NULL, NULL, NULL, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(N'2017-09-06 10:00:46.773' AS DateTime), CAST(N'2017-09-06 10:00:46.773' AS DateTime), 0)
GO
INSERT [dbo].[BuildingUnits] ([UnitID], [BuildingID], [UnitName], [UnitTypeID], [Details], [OwnerID], [OneTimeMaintenance], [UnitArea], [UDK1], [UDK2], [UDK3], [UDK4], [UDK5], [UserID], [CreatedDate], [ModifiedDate], [IsDeleted]) VALUES (2, 1, N'A-102', 9, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(95000.00 AS Numeric(18, 2)), CAST(985 AS Numeric(18, 0)), NULL, NULL, NULL, NULL, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(N'2017-09-06 10:01:09.823' AS DateTime), CAST(N'2017-09-06 10:01:09.823' AS DateTime), 0)
GO
INSERT [dbo].[BuildingUnits] ([UnitID], [BuildingID], [UnitName], [UnitTypeID], [Details], [OwnerID], [OneTimeMaintenance], [UnitArea], [UDK1], [UDK2], [UDK3], [UDK4], [UDK5], [UserID], [CreatedDate], [ModifiedDate], [IsDeleted]) VALUES (3, 1, N'A-103', 10, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(95000.00 AS Numeric(18, 2)), CAST(985 AS Numeric(18, 0)), NULL, NULL, NULL, NULL, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(N'2017-09-06 10:01:28.777' AS DateTime), CAST(N'2017-09-06 10:01:28.777' AS DateTime), 0)
GO
SET IDENTITY_INSERT [dbo].[BuildingUnits] OFF
GO
SET IDENTITY_INSERT [dbo].[Complaints] ON 

GO
INSERT [dbo].[Complaints] ([ComplaintID], [AuthorID], [ComplaintDate], [ComplaintTypeID], [AssignToID], [Title], [Details], [Status], [IsDeleted], [UDK1], [UDK2], [UDK3], [UDK4], [UDK5], [UserID], [CreatedDate], [ModifiedDate]) VALUES (1, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(N'2017-09-13 00:00:00.000' AS DateTime), 18, N'81cd3d3b-848f-4300-bb37-02b230537eae', N'Tatasky Not Working', N'Hi Manager,

My Tata sky connection is not working from last 5 days. 
Please do some thing.
Thanks.', 1, 0, NULL, NULL, NULL, NULL, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(N'2017-09-13 09:56:15.497' AS DateTime), CAST(N'2017-09-13 09:56:15.497' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Complaints] OFF
GO
SET IDENTITY_INSERT [dbo].[ForumComments] ON 

GO
INSERT [dbo].[ForumComments] ([CommentID], [ForumID], [AuthorID], [Comment], [IsDeleted], [UDK1], [UDK2], [UDK3], [UserID], [CreatedDate], [ModifiedDate]) VALUES (2, 1, N'81cd3d3b-848f-4300-bb37-02b230537eae', N'<p>this is test</p><p>wht is this</p>', 1, NULL, NULL, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(N'2017-09-08 11:06:02.000' AS DateTime), CAST(N'2017-09-11 11:36:46.643' AS DateTime))
GO
INSERT [dbo].[ForumComments] ([CommentID], [ForumID], [AuthorID], [Comment], [IsDeleted], [UDK1], [UDK2], [UDK3], [UserID], [CreatedDate], [ModifiedDate]) VALUES (3, 1, N'81cd3d3b-848f-4300-bb37-02b230537eae', N'<p>good test&nbsp;</p><p>let me check.</p><p>For what is good</p>', 0, NULL, NULL, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(N'2017-09-08 11:06:30.000' AS DateTime), CAST(N'2017-09-08 11:34:16.600' AS DateTime))
GO
INSERT [dbo].[ForumComments] ([CommentID], [ForumID], [AuthorID], [Comment], [IsDeleted], [UDK1], [UDK2], [UDK3], [UserID], [CreatedDate], [ModifiedDate]) VALUES (4, 1, N'81cd3d3b-848f-4300-bb37-02b230537eae', N'<p>let me check one more</p><p>this will be good test</p>', 0, NULL, NULL, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(N'2017-09-08 11:07:03.140' AS DateTime), CAST(N'2017-09-08 11:07:03.140' AS DateTime))
GO
INSERT [dbo].[ForumComments] ([CommentID], [ForumID], [AuthorID], [Comment], [IsDeleted], [UDK1], [UDK2], [UDK3], [UserID], [CreatedDate], [ModifiedDate]) VALUES (6, 1, N'81cd3d3b-848f-4300-bb37-02b230537eae', N'<p>What is foing on to much time</p><p><br></p>', 1, NULL, NULL, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(N'2017-09-08 11:20:09.280' AS DateTime), CAST(N'2017-09-08 11:36:01.150' AS DateTime))
GO
INSERT [dbo].[ForumComments] ([CommentID], [ForumID], [AuthorID], [Comment], [IsDeleted], [UDK1], [UDK2], [UDK3], [UserID], [CreatedDate], [ModifiedDate]) VALUES (7, 1, N'81cd3d3b-848f-4300-bb37-02b230537eae', N'Let Have Fun
With Whatt', 1, NULL, NULL, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(N'2017-09-09 09:48:10.727' AS DateTime), CAST(N'2017-09-09 10:00:24.963' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[ForumComments] OFF
GO
SET IDENTITY_INSERT [dbo].[Forums] ON 

GO
INSERT [dbo].[Forums] ([ForumID], [AuthorID], [ForumTitle], [Details], [IsDeleted], [UDK1], [UDK2], [UDK3], [UserID], [CreatedDate], [ModifiedDate]) VALUES (1, N'81cd3d3b-848f-4300-bb37-02b230537eae', N'Test Script', N'<p>This is test</p><p>Let''s have a fun</p><p>Thanks and Regards,</p><span style="font-size:16px;"><p style="font-size: 16px;"></p></span><p style="font-size: 16px;"><span style="font-size:16px;">Mukesh Patel</span></p><p></p>', 0, NULL, NULL, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(N'2017-09-08 10:09:25.000' AS DateTime), CAST(N'2017-09-12 10:19:40.237' AS DateTime))
GO
INSERT [dbo].[Forums] ([ForumID], [AuthorID], [ForumTitle], [Details], [IsDeleted], [UDK1], [UDK2], [UDK3], [UserID], [CreatedDate], [ModifiedDate]) VALUES (2, N'81cd3d3b-848f-4300-bb37-02b230537eae', N'Test For Delete', N'Lets Delete this Post', 1, NULL, NULL, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(N'2017-09-08 10:44:13.170' AS DateTime), CAST(N'2017-09-08 10:44:25.067' AS DateTime))
GO
INSERT [dbo].[Forums] ([ForumID], [AuthorID], [ForumTitle], [Details], [IsDeleted], [UDK1], [UDK2], [UDK3], [UserID], [CreatedDate], [ModifiedDate]) VALUES (3, N'81cd3d3b-848f-4300-bb37-02b230537eae', N'Second Forum Post For Test more thena 50 character stinrg', N'<p>This will be a good fun.</p><a href="http://google.com"><p>Lets make it good.</p></a><ol><li><i>Nice to do a good coindg</i></li><li>Link to out of the world.</li></ol>', 0, NULL, NULL, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(N'2017-09-12 10:48:56.000' AS DateTime), CAST(N'2017-09-12 11:20:05.123' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Forums] OFF
GO
SET IDENTITY_INSERT [dbo].[KeyValues] ON 

GO
INSERT [dbo].[KeyValues] ([KeyID], [KeyName], [KeyValues], [KeyOrder], [Details], [UDK1], [UDK2], [UDK3]) VALUES (1, N'Gender', N'Male', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[KeyValues] ([KeyID], [KeyName], [KeyValues], [KeyOrder], [Details], [UDK1], [UDK2], [UDK3]) VALUES (2, N'Gender', N'Female', 2, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[KeyValues] ([KeyID], [KeyName], [KeyValues], [KeyOrder], [Details], [UDK1], [UDK2], [UDK3]) VALUES (3, N'PollType', N'Single Selection', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[KeyValues] ([KeyID], [KeyName], [KeyValues], [KeyOrder], [Details], [UDK1], [UDK2], [UDK3]) VALUES (4, N'PollType', N'Multiple Selection', 2, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[KeyValues] ([KeyID], [KeyName], [KeyValues], [KeyOrder], [Details], [UDK1], [UDK2], [UDK3]) VALUES (5, N'BuildingType', N'Residential Building', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[KeyValues] ([KeyID], [KeyName], [KeyValues], [KeyOrder], [Details], [UDK1], [UDK2], [UDK3]) VALUES (6, N'BuildingType', N'Commercial Building', 2, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[KeyValues] ([KeyID], [KeyName], [KeyValues], [KeyOrder], [Details], [UDK1], [UDK2], [UDK3]) VALUES (7, N'BuildingType', N'Common Facility', 3, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[KeyValues] ([KeyID], [KeyName], [KeyValues], [KeyOrder], [Details], [UDK1], [UDK2], [UDK3]) VALUES (8, N'BuildingType', N'Parking', 4, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[KeyValues] ([KeyID], [KeyName], [KeyValues], [KeyOrder], [Details], [UDK1], [UDK2], [UDK3]) VALUES (9, N'BuildingUnitType', N'Residential Flat', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[KeyValues] ([KeyID], [KeyName], [KeyValues], [KeyOrder], [Details], [UDK1], [UDK2], [UDK3]) VALUES (10, N'BuildingUnitType', N'Rented Flat', 2, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[KeyValues] ([KeyID], [KeyName], [KeyValues], [KeyOrder], [Details], [UDK1], [UDK2], [UDK3]) VALUES (11, N'BuildingUnitType', N'Owner Shop', 3, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[KeyValues] ([KeyID], [KeyName], [KeyValues], [KeyOrder], [Details], [UDK1], [UDK2], [UDK3]) VALUES (12, N'BuildingUnitType', N'Rented Shop', 4, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[KeyValues] ([KeyID], [KeyName], [KeyValues], [KeyOrder], [Details], [UDK1], [UDK2], [UDK3]) VALUES (13, N'BuildingUnitType', N'Owner Parking', 5, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[KeyValues] ([KeyID], [KeyName], [KeyValues], [KeyOrder], [Details], [UDK1], [UDK2], [UDK3]) VALUES (14, N'BuildingUnitType', N'Common Parking', 6, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[KeyValues] ([KeyID], [KeyName], [KeyValues], [KeyOrder], [Details], [UDK1], [UDK2], [UDK3]) VALUES (15, N'DueType', N'Maintenance', 1, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[KeyValues] ([KeyID], [KeyName], [KeyValues], [KeyOrder], [Details], [UDK1], [UDK2], [UDK3]) VALUES (16, N'DueType', N'Sinking Fund', 2, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[KeyValues] ([KeyID], [KeyName], [KeyValues], [KeyOrder], [Details], [UDK1], [UDK2], [UDK3]) VALUES (17, N'DueType', N'Festival Fund', 3, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[KeyValues] ([KeyID], [KeyName], [KeyValues], [KeyOrder], [Details], [UDK1], [UDK2], [UDK3]) VALUES (18, N'ComplaintType', N'Personal', 1, N'Personal Complaint For Flat and Shops', NULL, NULL, NULL)
GO
INSERT [dbo].[KeyValues] ([KeyID], [KeyName], [KeyValues], [KeyOrder], [Details], [UDK1], [UDK2], [UDK3]) VALUES (19, N'ComplaintType', N'General', 2, N'General Complaint', NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[KeyValues] OFF
GO
SET IDENTITY_INSERT [dbo].[NoticeBoard] ON 

GO
INSERT [dbo].[NoticeBoard] ([NoticeID], [NoticeDate], [NoticeHeading], [Notice], [ExpiryDate], [IsDeleted], [UDK1], [UDK2], [UDK3], [UserID], [CreatedDate], [ModifiedDate]) VALUES (1, CAST(N'2017-09-07 00:00:00.000' AS DateTime), N'Test Notice', N'Hi All,

Subject : Test Notice to Present in meeting.

This is test notice to all members of ygc.

Thanks,
Charmain', CAST(N'2017-12-08 00:00:00.000' AS DateTime), 0, NULL, NULL, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(N'2017-09-07 09:49:36.000' AS DateTime), CAST(N'2017-09-08 08:54:00.937' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[NoticeBoard] OFF
GO
SET IDENTITY_INSERT [dbo].[PollOptions] ON 

GO
INSERT [dbo].[PollOptions] ([PollOptionID], [PollID], [PollOption], [OptionOrder], [UDK1], [UDK2], [UDK3]) VALUES (1, 2, N'Option 1', 1, NULL, NULL, NULL)
GO
INSERT [dbo].[PollOptions] ([PollOptionID], [PollID], [PollOption], [OptionOrder], [UDK1], [UDK2], [UDK3]) VALUES (2, 2, N'Option 2', 1, NULL, NULL, NULL)
GO
INSERT [dbo].[PollOptions] ([PollOptionID], [PollID], [PollOption], [OptionOrder], [UDK1], [UDK2], [UDK3]) VALUES (3, 2, N'Option 3', 1, NULL, NULL, NULL)
GO
INSERT [dbo].[PollOptions] ([PollOptionID], [PollID], [PollOption], [OptionOrder], [UDK1], [UDK2], [UDK3]) VALUES (4, 2, N'Option 4', 1, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[PollOptions] OFF
GO
SET IDENTITY_INSERT [dbo].[Polls] ON 

GO
INSERT [dbo].[Polls] ([PollID], [PollTitle], [Details], [StartDate], [EndDate], [PollTypeID], [IsDeleted], [UDK1], [UDK2], [UDK3], [UserID], [CreatedDate], [ModifiedDate]) VALUES (1, N'Purchase of LED Flud Light', N'This is test
what is this', CAST(N'2017-09-01 00:00:00.000' AS DateTime), CAST(N'2017-09-30 00:00:00.000' AS DateTime), 3, 0, NULL, NULL, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(N'2017-09-11 10:43:15.577' AS DateTime), CAST(N'2017-09-11 10:43:15.577' AS DateTime))
GO
INSERT [dbo].[Polls] ([PollID], [PollTitle], [Details], [StartDate], [EndDate], [PollTypeID], [IsDeleted], [UDK1], [UDK2], [UDK3], [UserID], [CreatedDate], [ModifiedDate]) VALUES (2, N'Purchase of LED Flud Light', N'I don''t know
How this will work', CAST(N'2017-09-01 00:00:00.000' AS DateTime), CAST(N'2017-09-13 00:00:00.000' AS DateTime), 3, 0, NULL, NULL, NULL, N'81cd3d3b-848f-4300-bb37-02b230537eae', CAST(N'2017-09-09 11:02:02.420' AS DateTime), CAST(N'2017-09-09 11:02:02.420' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[Polls] OFF
GO
ALTER TABLE [dbo].[Audits] ADD  CONSTRAINT [DF_Audits_ActionDate]  DEFAULT (getdate()) FOR [ActionDate]
GO
ALTER TABLE [dbo].[Collections] ADD  CONSTRAINT [DF_Collections_CollectionDate]  DEFAULT (getdate()) FOR [CollectionDate]
GO
ALTER TABLE [dbo].[Collections] ADD  CONSTRAINT [DF_Collections_Amount]  DEFAULT ((0)) FOR [Amount]
GO
ALTER TABLE [dbo].[Collections] ADD  CONSTRAINT [DF_Table_1_Penalty]  DEFAULT ((0)) FOR [LatePaymentCharges]
GO
ALTER TABLE [dbo].[Collections] ADD  CONSTRAINT [DF_Collections_ChequeCleared]  DEFAULT ((0)) FOR [ChequeCleared]
GO
ALTER TABLE [dbo].[Collections] ADD  CONSTRAINT [DF_Collections_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Collections] ADD  CONSTRAINT [DF_Collections_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[Collections] ADD  CONSTRAINT [DF_Collections_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ComplaintComments] ADD  CONSTRAINT [DF_ComplaintComments_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ComplaintComments] ADD  CONSTRAINT [DF_ComplaintComments_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[ComplaintComments] ADD  CONSTRAINT [DF_ComplaintComments_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Dues] ADD  CONSTRAINT [DF_Dues_DueAmount]  DEFAULT ((0)) FOR [DueAmount]
GO
ALTER TABLE [dbo].[Dues] ADD  CONSTRAINT [DF_Dues_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Dues] ADD  CONSTRAINT [DF_Dues_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[Dues] ADD  CONSTRAINT [DF_Dues_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[PollVotes] ADD  CONSTRAINT [DF_PollVotes_VoteDate]  DEFAULT (getdate()) FOR [VoteDate]
GO
ALTER TABLE [dbo].[PollVotes] ADD  CONSTRAINT [DF_PollVotes_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[PollVotes] ADD  CONSTRAINT [DF_PollVotes_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[PollVotes] ADD  CONSTRAINT [DF_PollVotes_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[RecurringDues] ADD  CONSTRAINT [DF_RecurringDues_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[RecurringDues] ADD  CONSTRAINT [DF_RecurringDues_ModifiedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[RecurringDues] ADD  CONSTRAINT [DF_RecurringDues_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Tenants] ADD  CONSTRAINT [DF_Tenants_DateOfJoining]  DEFAULT (getdate()) FOR [DateOfJoining]
GO
ALTER TABLE [dbo].[Tenants] ADD  CONSTRAINT [DF_Tenants_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Tenants] ADD  CONSTRAINT [DF_Tenants_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[UserDefineFields] ADD  CONSTRAINT [DF_UserDefineFields_IsDispaly]  DEFAULT ((0)) FOR [IsDispaly]
GO
ALTER TABLE [dbo].[UserDefineFields] ADD  CONSTRAINT [DF_UserDefineFields_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[UserDefineFields] ADD  CONSTRAINT [DF_UserDefineFields_IsRequired]  DEFAULT ((0)) FOR [IsRequired]
GO
ALTER TABLE [dbo].[UserDefineFields] ADD  CONSTRAINT [DF_UserDefineFields_IsRangeValidator]  DEFAULT ((0)) FOR [IsRangeValidator]
GO
ALTER TABLE [dbo].[UserDefineFields] ADD  CONSTRAINT [DF_UserDefineFields_IsRegularExpression]  DEFAULT ((0)) FOR [IsRegularExpression]
GO
ALTER TABLE [dbo].[UserDefineFields] ADD  CONSTRAINT [DF_UserDefineFields_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[UserDefineFields] ADD  CONSTRAINT [DF_UserDefineFields_UpdatedDate]  DEFAULT (getdate()) FOR [ModifiedDate]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Buildings]  WITH CHECK ADD  CONSTRAINT [FK_Buildings_BuildingType] FOREIGN KEY([BuildingTypeID])
REFERENCES [dbo].[KeyValues] ([KeyID])
GO
ALTER TABLE [dbo].[Buildings] CHECK CONSTRAINT [FK_Buildings_BuildingType]
GO
ALTER TABLE [dbo].[BuildingUnits]  WITH CHECK ADD  CONSTRAINT [FK_BuildingUnits_Buildings] FOREIGN KEY([BuildingID])
REFERENCES [dbo].[Buildings] ([BuildingID])
GO
ALTER TABLE [dbo].[BuildingUnits] CHECK CONSTRAINT [FK_BuildingUnits_Buildings]
GO
ALTER TABLE [dbo].[BuildingUnits]  WITH CHECK ADD  CONSTRAINT [FK_BuildingUnits_Owner] FOREIGN KEY([OwnerID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[BuildingUnits] CHECK CONSTRAINT [FK_BuildingUnits_Owner]
GO
ALTER TABLE [dbo].[BuildingUnits]  WITH CHECK ADD  CONSTRAINT [FK_BuildingUnits_UnitType] FOREIGN KEY([UnitTypeID])
REFERENCES [dbo].[KeyValues] ([KeyID])
GO
ALTER TABLE [dbo].[BuildingUnits] CHECK CONSTRAINT [FK_BuildingUnits_UnitType]
GO
ALTER TABLE [dbo].[Collections]  WITH CHECK ADD  CONSTRAINT [FK_Collections_BuildingUnits] FOREIGN KEY([UnitID])
REFERENCES [dbo].[BuildingUnits] ([UnitID])
GO
ALTER TABLE [dbo].[Collections] CHECK CONSTRAINT [FK_Collections_BuildingUnits]
GO
ALTER TABLE [dbo].[Collections]  WITH CHECK ADD  CONSTRAINT [FK_Collections_KeyValues] FOREIGN KEY([PaymentModeID])
REFERENCES [dbo].[KeyValues] ([KeyID])
GO
ALTER TABLE [dbo].[Collections] CHECK CONSTRAINT [FK_Collections_KeyValues]
GO
ALTER TABLE [dbo].[ComplaintComments]  WITH CHECK ADD  CONSTRAINT [FK_ComplaintComments_Complaints] FOREIGN KEY([ComplaintID])
REFERENCES [dbo].[Complaints] ([ComplaintID])
GO
ALTER TABLE [dbo].[ComplaintComments] CHECK CONSTRAINT [FK_ComplaintComments_Complaints]
GO
ALTER TABLE [dbo].[ComplaintComments]  WITH CHECK ADD  CONSTRAINT [FK_ComplaintComments_UserProfile] FOREIGN KEY([AssignToID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[ComplaintComments] CHECK CONSTRAINT [FK_ComplaintComments_UserProfile]
GO
ALTER TABLE [dbo].[Complaints]  WITH CHECK ADD  CONSTRAINT [FK_Complaints_AssignTo] FOREIGN KEY([AssignToID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Complaints] CHECK CONSTRAINT [FK_Complaints_AssignTo]
GO
ALTER TABLE [dbo].[Complaints]  WITH CHECK ADD  CONSTRAINT [FK_Complaints_Author] FOREIGN KEY([AuthorID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Complaints] CHECK CONSTRAINT [FK_Complaints_Author]
GO
ALTER TABLE [dbo].[Complaints]  WITH CHECK ADD  CONSTRAINT [FK_Complaints_ComplaintType] FOREIGN KEY([ComplaintTypeID])
REFERENCES [dbo].[KeyValues] ([KeyID])
GO
ALTER TABLE [dbo].[Complaints] CHECK CONSTRAINT [FK_Complaints_ComplaintType]
GO
ALTER TABLE [dbo].[Dues]  WITH CHECK ADD  CONSTRAINT [FK_Dues_BuildingUnits] FOREIGN KEY([UnitID])
REFERENCES [dbo].[BuildingUnits] ([UnitID])
GO
ALTER TABLE [dbo].[Dues] CHECK CONSTRAINT [FK_Dues_BuildingUnits]
GO
ALTER TABLE [dbo].[Dues]  WITH CHECK ADD  CONSTRAINT [FK_Dues_KeyValues] FOREIGN KEY([DueTypeID])
REFERENCES [dbo].[KeyValues] ([KeyID])
GO
ALTER TABLE [dbo].[Dues] CHECK CONSTRAINT [FK_Dues_KeyValues]
GO
ALTER TABLE [dbo].[Dues]  WITH CHECK ADD  CONSTRAINT [FK_Dues_RecurringDues] FOREIGN KEY([RecurringID])
REFERENCES [dbo].[RecurringDues] ([RecurringID])
GO
ALTER TABLE [dbo].[Dues] CHECK CONSTRAINT [FK_Dues_RecurringDues]
GO
ALTER TABLE [dbo].[ForumComments]  WITH CHECK ADD  CONSTRAINT [FK_ForumComments_Author] FOREIGN KEY([AuthorID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[ForumComments] CHECK CONSTRAINT [FK_ForumComments_Author]
GO
ALTER TABLE [dbo].[ForumComments]  WITH CHECK ADD  CONSTRAINT [FK_ForumComments_Forums] FOREIGN KEY([ForumID])
REFERENCES [dbo].[Forums] ([ForumID])
GO
ALTER TABLE [dbo].[ForumComments] CHECK CONSTRAINT [FK_ForumComments_Forums]
GO
ALTER TABLE [dbo].[Forums]  WITH CHECK ADD  CONSTRAINT [FK_Forums_Author] FOREIGN KEY([AuthorID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Forums] CHECK CONSTRAINT [FK_Forums_Author]
GO
ALTER TABLE [dbo].[PollOptions]  WITH CHECK ADD  CONSTRAINT [FK_PollOptions_Polls] FOREIGN KEY([PollID])
REFERENCES [dbo].[Polls] ([PollID])
GO
ALTER TABLE [dbo].[PollOptions] CHECK CONSTRAINT [FK_PollOptions_Polls]
GO
ALTER TABLE [dbo].[Polls]  WITH CHECK ADD  CONSTRAINT [FK_Polls_PollType] FOREIGN KEY([PollTypeID])
REFERENCES [dbo].[KeyValues] ([KeyID])
GO
ALTER TABLE [dbo].[Polls] CHECK CONSTRAINT [FK_Polls_PollType]
GO
ALTER TABLE [dbo].[PollVotes]  WITH CHECK ADD  CONSTRAINT [FK_PollVotes_AspNetUsers] FOREIGN KEY([VoteByID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[PollVotes] CHECK CONSTRAINT [FK_PollVotes_AspNetUsers]
GO
ALTER TABLE [dbo].[PollVotes]  WITH CHECK ADD  CONSTRAINT [FK_PollVotes_PollOptions] FOREIGN KEY([PollOptionID])
REFERENCES [dbo].[PollOptions] ([PollOptionID])
GO
ALTER TABLE [dbo].[PollVotes] CHECK CONSTRAINT [FK_PollVotes_PollOptions]
GO
ALTER TABLE [dbo].[RecurringDues]  WITH CHECK ADD  CONSTRAINT [FK_RecurringDues_BuildingUnits] FOREIGN KEY([UnitID])
REFERENCES [dbo].[BuildingUnits] ([UnitID])
GO
ALTER TABLE [dbo].[RecurringDues] CHECK CONSTRAINT [FK_RecurringDues_BuildingUnits]
GO
ALTER TABLE [dbo].[RecurringDues]  WITH CHECK ADD  CONSTRAINT [FK_RecurringDues_DueType] FOREIGN KEY([DueTypeID])
REFERENCES [dbo].[KeyValues] ([KeyID])
GO
ALTER TABLE [dbo].[RecurringDues] CHECK CONSTRAINT [FK_RecurringDues_DueType]
GO
ALTER TABLE [dbo].[RecurringDues]  WITH CHECK ADD  CONSTRAINT [FK_RecurringDues_RecurringType] FOREIGN KEY([RecurringTypeID])
REFERENCES [dbo].[KeyValues] ([KeyID])
GO
ALTER TABLE [dbo].[RecurringDues] CHECK CONSTRAINT [FK_RecurringDues_RecurringType]
GO
ALTER TABLE [dbo].[Tenants]  WITH CHECK ADD  CONSTRAINT [FK_Tenants_BuildingUnits] FOREIGN KEY([UnitID])
REFERENCES [dbo].[BuildingUnits] ([UnitID])
GO
ALTER TABLE [dbo].[Tenants] CHECK CONSTRAINT [FK_Tenants_BuildingUnits]
GO
ALTER TABLE [dbo].[Tenants]  WITH CHECK ADD  CONSTRAINT [FK_Tenants_TenantType] FOREIGN KEY([TenantTypeID])
REFERENCES [dbo].[KeyValues] ([KeyID])
GO
ALTER TABLE [dbo].[Tenants] CHECK CONSTRAINT [FK_Tenants_TenantType]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 - Cash, 2 - Online, 3 - Cheque' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Collections', @level2type=N'COLUMN',@level2name=N'PaymentModeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Open, Inprogress, Completed, Closed, Deleted, Decline' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Complaints', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Maintenance, Sinking Fund, Festival Event Contribution' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Dues', @level2type=N'COLUMN',@level2name=N'DueTypeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Textbox, Dropdown, Textarea, Checkbox, Radiobutton' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserDefineFields', @level2type=N'COLUMN',@level2name=N'ControlType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Integer, Decimal, Alphnumeric, Date, Boolen' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserDefineFields', @level2type=N'COLUMN',@level2name=N'FieldType'
GO
