USE [master]
GO
/****** Object:  Database [ExchangeStuff]    Script Date: 11/06/2024 19:49:29 ******/
CREATE DATABASE [ExchangeStuff]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ExchangeStuff', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\ExchangeStuff.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ExchangeStuff_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\ExchangeStuff_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [ExchangeStuff] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ExchangeStuff].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ExchangeStuff] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ExchangeStuff] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ExchangeStuff] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ExchangeStuff] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ExchangeStuff] SET ARITHABORT OFF 
GO
ALTER DATABASE [ExchangeStuff] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [ExchangeStuff] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ExchangeStuff] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ExchangeStuff] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ExchangeStuff] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ExchangeStuff] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ExchangeStuff] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ExchangeStuff] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ExchangeStuff] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ExchangeStuff] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ExchangeStuff] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ExchangeStuff] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ExchangeStuff] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ExchangeStuff] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ExchangeStuff] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ExchangeStuff] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [ExchangeStuff] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ExchangeStuff] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ExchangeStuff] SET  MULTI_USER 
GO
ALTER DATABASE [ExchangeStuff] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ExchangeStuff] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ExchangeStuff] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ExchangeStuff] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ExchangeStuff] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [ExchangeStuff] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [ExchangeStuff] SET QUERY_STORE = OFF
GO
USE [ExchangeStuff]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountPermissionGroup]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountPermissionGroup](
	[AccountsId] [uniqueidentifier] NOT NULL,
	[PermissionGroupsId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_AccountPermissionGroup] PRIMARY KEY CLUSTERED 
(
	[AccountsId] ASC,
	[PermissionGroupsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Accounts]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[Id] [uniqueidentifier] NOT NULL,
	[Username] [nvarchar](30) NULL,
	[Name] [nvarchar](30) NOT NULL,
	[Email] [nvarchar](50) NULL,
	[Password] [nvarchar](max) NULL,
	[Thumbnail] [nvarchar](max) NULL,
	[Discriminator] [nvarchar](13) NOT NULL,
	[StudentId] [nvarchar](10) NULL,
	[Address] [nvarchar](100) NULL,
	[Phone] [nvarchar](12) NULL,
	[Gender] [int] NULL,
	[CampusId] [uniqueidentifier] NULL,
	[IsActived] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Actions]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Actions](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Value] [int] NOT NULL,
	[Index] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Actions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Campuses]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Campuses](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[IsActived] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Campuses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[Thumbnail] [nvarchar](max) NOT NULL,
	[IsActived] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CategoryProduct]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CategoryProduct](
	[CategoriesId] [uniqueidentifier] NOT NULL,
	[ProductsId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_CategoryProduct] PRIMARY KEY CLUSTERED 
(
	[CategoriesId] ASC,
	[ProductsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[Id] [uniqueidentifier] NOT NULL,
	[Content] [nvarchar](500) NOT NULL,
	[AccountId] [uniqueidentifier] NOT NULL,
	[ProductId] [uniqueidentifier] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FinancialTickets]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FinancialTickets](
	[Id] [uniqueidentifier] NOT NULL,
	[Amount] [float] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[IsCredit] [bit] NOT NULL,
	[Status] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_FinancialTickets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ImageProduct]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ImageProduct](
	[ImagesId] [uniqueidentifier] NOT NULL,
	[ProductsId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ImageProduct] PRIMARY KEY CLUSTERED 
(
	[ImagesId] ASC,
	[ProductsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Images]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Images](
	[Id] [uniqueidentifier] NOT NULL,
	[Url] [nvarchar](max) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Images] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payments]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payments](
	[Id] [uniqueidentifier] NOT NULL,
	[Amount] [float] NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[PaymentDate] [datetime2](7) NOT NULL,
	[Status] [nvarchar](max) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Payments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PermissionGroups]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PermissionGroups](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](10) NOT NULL,
	[IsActived] [bit] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PermissionGroups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[Id] [uniqueidentifier] NOT NULL,
	[PermissionGroupId] [uniqueidentifier] NOT NULL,
	[ResourceId] [uniqueidentifier] NOT NULL,
	[PermissionValue] [int] NOT NULL,
 CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PostTickets]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PostTickets](
	[Id] [uniqueidentifier] NOT NULL,
	[Amount] [float] NOT NULL,
	[ProductId] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PostTickets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](30) NOT NULL,
	[Quantity] [int] NOT NULL,
	[Price] [float] NOT NULL,
	[Thumbnail] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[IsActived] [bit] NOT NULL,
	[ProductStatus] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PurchaseTickets]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PurchaseTickets](
	[Id] [uniqueidentifier] NOT NULL,
	[Amount] [float] NOT NULL,
	[ProductId] [uniqueidentifier] NOT NULL,
	[StudentId] [nvarchar](10) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Quantity] [int] NOT NULL,
	[CampusName] [nvarchar](30) NOT NULL,
	[Status] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PurchaseTickets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ratings]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ratings](
	[PurchaseTicketId] [uniqueidentifier] NOT NULL,
	[Content] [nvarchar](100) NOT NULL,
	[EvaluateType] [int] NOT NULL,
 CONSTRAINT [PK_Ratings] PRIMARY KEY CLUSTERED 
(
	[PurchaseTicketId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Resources]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Resources](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Resources] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tokens]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tokens](
	[Id] [uniqueidentifier] NOT NULL,
	[AccessToken] [nvarchar](max) NOT NULL,
	[RefreshToken] [nvarchar](max) NOT NULL,
	[AccountId] [uniqueidentifier] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Tokens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionHistories]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionHistories](
	[Id] [uniqueidentifier] NOT NULL,
	[UserId] [uniqueidentifier] NOT NULL,
	[Amount] [float] NOT NULL,
	[IsCredit] [bit] NOT NULL,
	[TransactionType] [int] NOT NULL,
	[CreatedOn] [datetime2](7) NOT NULL,
	[ModifiedOn] [datetime2](7) NOT NULL,
	[CreatedBy] [uniqueidentifier] NOT NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_TransactionHistories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserBalances]    Script Date: 11/06/2024 19:49:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserBalances](
	[UserId] [uniqueidentifier] NOT NULL,
	[Balance] [float] NOT NULL,
 CONSTRAINT [PK_UserBalances] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240605132051_initDb', N'8.0.6')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240608065532_update_auth', N'8.0.6')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240610024634_payment', N'8.0.6')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240610062723_email_admin', N'8.0.6')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20240610062933_email_admin', N'8.0.6')
GO
INSERT [dbo].[AccountPermissionGroup] ([AccountsId], [PermissionGroupsId]) VALUES (N'c2671565-9f56-49e6-8e2a-86a46abcde71', N'54673e96-1f8c-4e9d-94d3-373acf0eb100')
INSERT [dbo].[AccountPermissionGroup] ([AccountsId], [PermissionGroupsId]) VALUES (N'c2671565-9f56-49e6-8e2a-86a46abcde71', N'b4f26c1c-ed25-4ffd-8c70-55e4a05db493')
INSERT [dbo].[AccountPermissionGroup] ([AccountsId], [PermissionGroupsId]) VALUES (N'c2671565-9f56-49e6-8e2a-86a46abcde71', N'3a36be07-892f-45f6-b526-89e040dafeeb')
INSERT [dbo].[AccountPermissionGroup] ([AccountsId], [PermissionGroupsId]) VALUES (N'c2671565-9f56-49e6-8e2a-86a46abcde71', N'6b2ff683-926d-402b-b49b-e9e700f182de')
INSERT [dbo].[AccountPermissionGroup] ([AccountsId], [PermissionGroupsId]) VALUES (N'c11e2a85-c5b4-4b4b-85e0-5b30d7e3b595', N'ca8731a1-fd2c-49d6-ba25-f0c6e4d845a1')
INSERT [dbo].[AccountPermissionGroup] ([AccountsId], [PermissionGroupsId]) VALUES (N'c2671565-9f56-49e6-8e2a-86a46abcde71', N'0a105ee4-ff52-4424-884a-f431574ad90a')
GO
INSERT [dbo].[Accounts] ([Id], [Username], [Name], [Email], [Password], [Thumbnail], [Discriminator], [StudentId], [Address], [Phone], [Gender], [CampusId], [IsActived], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'c11e2a85-c5b4-4b4b-85e0-5b30d7e3b595', N'admin', N'string', NULL, N'd82494f05d6917ba02f7aaa29689ccb444bb73f20380876cb05d1f37537b7892', NULL, N'Admin', NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2024-06-10T13:35:11.8079430' AS DateTime2), CAST(N'2024-06-10T13:35:11.8306954' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
INSERT [dbo].[Accounts] ([Id], [Username], [Name], [Email], [Password], [Thumbnail], [Discriminator], [StudentId], [Address], [Phone], [Gender], [CampusId], [IsActived], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'c2671565-9f56-49e6-8e2a-86a46abcde71', NULL, N' userInfo.Name', N'nguyyenminhvu', NULL, NULL, N'User', NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2024-06-08T16:18:45.7857251' AS DateTime2), CAST(N'2024-06-11T12:02:34.6781671' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
INSERT [dbo].[Accounts] ([Id], [Username], [Name], [Email], [Password], [Thumbnail], [Discriminator], [StudentId], [Address], [Phone], [Gender], [CampusId], [IsActived], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'0f0a3d7c-428c-4cea-9ae2-c49e86b91399', NULL, N'Nguyen Minh Vu', N'vunmtse161207@fpt.edu.vn', NULL, N'https://lh3.googleusercontent.com/a/ACg8ocLhFLiTZ18_LVDpzi0MXp-CenJPixIv0zW2xPRDJ5v0_WrZA343=s96-c', N'User', NULL, NULL, NULL, NULL, NULL, 1, CAST(N'2024-06-08T19:19:45.8892439' AS DateTime2), CAST(N'2024-06-10T21:54:47.3504932' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
GO
INSERT [dbo].[Actions] ([Id], [Name], [Value], [Index], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'adf45e8f-c443-47bf-bf64-64007e2c61c5', N'Write', 2, 1, CAST(N'2024-05-09T19:00:35.3866667' AS DateTime2), CAST(N'2024-05-09T19:00:35.3866667' AS DateTime2), N'5d1179aa-7385-4126-b5b6-eb6e1f06694c', N'5d1179aa-7385-4126-b5b6-eb6e1f06694c')
INSERT [dbo].[Actions] ([Id], [Name], [Value], [Index], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'b9fc22d9-a391-45b7-bbf2-72e58751a641', N'Delete', 8, 3, CAST(N'2024-05-09T19:00:35.4166667' AS DateTime2), CAST(N'2024-05-09T19:00:35.4166667' AS DateTime2), N'5d1179aa-7385-4126-b5b6-eb6e1f06694c', N'5d1179aa-7385-4126-b5b6-eb6e1f06694c')
INSERT [dbo].[Actions] ([Id], [Name], [Value], [Index], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'ad0d5553-81da-40b4-b8a7-b35527d2bf43', N'Overwrite', 4, 2, CAST(N'2024-05-09T19:00:35.4100000' AS DateTime2), CAST(N'2024-05-09T19:00:35.4100000' AS DateTime2), N'5d1179aa-7385-4126-b5b6-eb6e1f06694c', N'5d1179aa-7385-4126-b5b6-eb6e1f06694c')
INSERT [dbo].[Actions] ([Id], [Name], [Value], [Index], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'da84b5a8-3013-4094-9318-c7ac88c8c7bd', N'Read', 1, 0, CAST(N'2024-05-09T19:00:35.3800000' AS DateTime2), CAST(N'2024-05-09T19:00:35.3800000' AS DateTime2), N'5d1179aa-7385-4126-b5b6-eb6e1f06694c', N'5d1179aa-7385-4126-b5b6-eb6e1f06694c')
GO
INSERT [dbo].[PermissionGroups] ([Id], [Name], [IsActived], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'1e2f84c7-e718-459f-b293-25662d6dd51b', N'Fat', 0, CAST(N'2024-06-10T23:00:40.3596248' AS DateTime2), CAST(N'2024-06-10T23:00:40.3597597' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
INSERT [dbo].[PermissionGroups] ([Id], [Name], [IsActived], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'189d797e-b2be-401d-81eb-31f499376524', N'Pro', 0, CAST(N'2024-06-10T21:20:16.0230761' AS DateTime2), CAST(N'2024-06-10T21:20:16.0407637' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
INSERT [dbo].[PermissionGroups] ([Id], [Name], [IsActived], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'54673e96-1f8c-4e9d-94d3-373acf0eb100', N'Moderator', 1, CAST(N'2024-06-08T19:44:24.0933333' AS DateTime2), CAST(N'2024-06-08T19:44:24.0933333' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
INSERT [dbo].[PermissionGroups] ([Id], [Name], [IsActived], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'b4f26c1c-ed25-4ffd-8c70-55e4a05db493', N'Dog', 0, CAST(N'2024-06-11T12:02:22.4035063' AS DateTime2), CAST(N'2024-06-11T12:02:22.4036566' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
INSERT [dbo].[PermissionGroups] ([Id], [Name], [IsActived], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'3a36be07-892f-45f6-b526-89e040dafeeb', N'Vip', 0, CAST(N'2024-06-10T21:14:05.7336846' AS DateTime2), CAST(N'2024-06-10T21:14:05.7527811' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
INSERT [dbo].[PermissionGroups] ([Id], [Name], [IsActived], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'6b2ff683-926d-402b-b49b-e9e700f182de', N'Student', 1, CAST(N'2024-06-08T19:44:12.4733333' AS DateTime2), CAST(N'2024-06-08T19:44:12.4733333' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
INSERT [dbo].[PermissionGroups] ([Id], [Name], [IsActived], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'ca8731a1-fd2c-49d6-ba25-f0c6e4d845a1', N'Admin', 1, CAST(N'2024-06-08T19:44:28.5600000' AS DateTime2), CAST(N'2024-06-08T19:44:28.5600000' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
INSERT [dbo].[PermissionGroups] ([Id], [Name], [IsActived], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'0a105ee4-ff52-4424-884a-f431574ad90a', N'Mbappe', 1, CAST(N'2024-06-08T19:44:40.1133333' AS DateTime2), CAST(N'2024-06-08T19:44:40.1133333' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
GO
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'2a0ceae2-0e78-416a-0f4d-08dc8957960a', N'3a36be07-892f-45f6-b526-89e040dafeeb', N'd4a7839f-b255-4120-b3f3-282de9c67178', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'6fc463d8-eadf-40a7-0f4e-08dc8957960a', N'3a36be07-892f-45f6-b526-89e040dafeeb', N'8dc756cb-1437-4353-8c9f-2a1a663bf9b8', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'58c49327-0cc4-4ea7-0f4f-08dc8957960a', N'3a36be07-892f-45f6-b526-89e040dafeeb', N'7bafaad1-a2c0-4bc5-adc3-56f75a726c48', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'a27854ec-ad27-432a-eeca-08dc89587305', N'189d797e-b2be-401d-81eb-31f499376524', N'd4a7839f-b255-4120-b3f3-282de9c67178', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'3165721e-22e3-4b76-eecb-08dc89587305', N'189d797e-b2be-401d-81eb-31f499376524', N'8dc756cb-1437-4353-8c9f-2a1a663bf9b8', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'eea70805-bea7-4b46-eecc-08dc89587305', N'189d797e-b2be-401d-81eb-31f499376524', N'7bafaad1-a2c0-4bc5-adc3-56f75a726c48', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'1a8339d7-d68a-437d-509c-08dc896679d3', N'1e2f84c7-e718-459f-b293-25662d6dd51b', N'd4a7839f-b255-4120-b3f3-282de9c67178', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'4cc08632-0f24-49cd-509d-08dc896679d3', N'1e2f84c7-e718-459f-b293-25662d6dd51b', N'8dc756cb-1437-4353-8c9f-2a1a663bf9b8', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'99529013-b53c-49a9-509e-08dc896679d3', N'1e2f84c7-e718-459f-b293-25662d6dd51b', N'7bafaad1-a2c0-4bc5-adc3-56f75a726c48', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'6074ce0c-6a3f-4083-509f-08dc896679d3', N'1e2f84c7-e718-459f-b293-25662d6dd51b', N'1baa92cf-e253-41ce-81de-70c282a11f7e', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'72f2494f-2872-44fc-0540-08dc89d3ad9d', N'b4f26c1c-ed25-4ffd-8c70-55e4a05db493', N'3d846bf9-30b4-4869-05bb-08dc896ada32', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'071347e0-6d26-474a-0541-08dc89d3ad9d', N'b4f26c1c-ed25-4ffd-8c70-55e4a05db493', N'd4a7839f-b255-4120-b3f3-282de9c67178', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'f9fd866f-d192-4b47-0542-08dc89d3ad9d', N'b4f26c1c-ed25-4ffd-8c70-55e4a05db493', N'8dc756cb-1437-4353-8c9f-2a1a663bf9b8', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'6af0056c-965d-42cf-0543-08dc89d3ad9d', N'b4f26c1c-ed25-4ffd-8c70-55e4a05db493', N'7bafaad1-a2c0-4bc5-adc3-56f75a726c48', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'4078d294-6918-4223-0544-08dc89d3ad9d', N'b4f26c1c-ed25-4ffd-8c70-55e4a05db493', N'1baa92cf-e253-41ce-81de-70c282a11f7e', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'a508abe7-7db4-4221-bbed-4ecbcc9a45a7', N'6b2ff683-926d-402b-b49b-e9e700f182de', N'8dc756cb-1437-4353-8c9f-2a1a663bf9b8', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'82f98fc7-6e0a-47b6-add3-6c9fc51f76b4', N'54673e96-1f8c-4e9d-94d3-373acf0eb100', N'd4a7839f-b255-4120-b3f3-282de9c67178', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'd17d94d1-b53d-4df4-8d6a-8ada6ccd1cf1', N'ca8731a1-fd2c-49d6-ba25-f0c6e4d845a1', N'd4a7839f-b255-4120-b3f3-282de9c67178', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'929deede-bea5-434c-ac11-91442f5a805c', N'ca8731a1-fd2c-49d6-ba25-f0c6e4d845a1', N'7bafaad1-a2c0-4bc5-adc3-56f75a726c48', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'0900aa33-5a90-4cfd-98b5-96c16ca7b798', N'0a105ee4-ff52-4424-884a-f431574ad90a', N'd4a7839f-b255-4120-b3f3-282de9c67178', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'd76341e7-61a6-47e7-b21e-97984b4165a0', N'6b2ff683-926d-402b-b49b-e9e700f182de', N'1baa92cf-e253-41ce-81de-70c282a11f7e', 14)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'db8db05b-8fc4-4bdd-8155-9cedeedc86ab', N'6b2ff683-926d-402b-b49b-e9e700f182de', N'd4a7839f-b255-4120-b3f3-282de9c67178', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'8ae0072f-3e1f-435b-9f5d-b8dc2d5de21b', N'6b2ff683-926d-402b-b49b-e9e700f182de', N'7bafaad1-a2c0-4bc5-adc3-56f75a726c48', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'54ef422e-483b-4e46-8ed0-bf5bbe9a9169', N'ca8731a1-fd2c-49d6-ba25-f0c6e4d845a1', N'1baa92cf-e253-41ce-81de-70c282a11f7e', 15)
INSERT [dbo].[Permissions] ([Id], [PermissionGroupId], [ResourceId], [PermissionValue]) VALUES (N'b739a3d1-34b5-4c87-b266-f472c784e86c', N'ca8731a1-fd2c-49d6-ba25-f0c6e4d845a1', N'8dc756cb-1437-4353-8c9f-2a1a663bf9b8', 15)
GO
INSERT [dbo].[Resources] ([Id], [Name], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'3d846bf9-30b4-4869-05bb-08dc896ada32', N'VNPay', CAST(N'2024-06-10T23:32:00.0450803' AS DateTime2), CAST(N'2024-06-10T23:32:00.0629037' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
INSERT [dbo].[Resources] ([Id], [Name], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'd4a7839f-b255-4120-b3f3-282de9c67178', N'Product', CAST(N'2024-05-09T19:04:13.4433333' AS DateTime2), CAST(N'2024-05-09T19:04:13.4433333' AS DateTime2), N'5d1179aa-7385-4126-b5b6-eb6e1f06694c', N'5d1179aa-7385-4126-b5b6-eb6e1f06694c')
INSERT [dbo].[Resources] ([Id], [Name], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'8dc756cb-1437-4353-8c9f-2a1a663bf9b8', N'Category', CAST(N'2024-05-09T19:04:13.4133333' AS DateTime2), CAST(N'2024-05-09T19:04:13.4133333' AS DateTime2), N'5d1179aa-7385-4126-b5b6-eb6e1f06694c', N'5d1179aa-7385-4126-b5b6-eb6e1f06694c')
INSERT [dbo].[Resources] ([Id], [Name], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'7bafaad1-a2c0-4bc5-adc3-56f75a726c48', N'Campus', CAST(N'2024-05-09T19:04:13.4000000' AS DateTime2), CAST(N'2024-05-09T19:04:13.4000000' AS DateTime2), N'5d1179aa-7385-4126-b5b6-eb6e1f06694c', N'5d1179aa-7385-4126-b5b6-eb6e1f06694c')
INSERT [dbo].[Resources] ([Id], [Name], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'1baa92cf-e253-41ce-81de-70c282a11f7e', N'Admin', CAST(N'2024-06-10T22:44:32.6066667' AS DateTime2), CAST(N'2024-06-10T22:44:32.6066667' AS DateTime2), N'5d1179aa-7385-4126-b5b6-eb6e1f06694c', N'5d1179aa-7385-4126-b5b6-eb6e1f06694c')
GO
INSERT [dbo].[Tokens] ([Id], [AccessToken], [RefreshToken], [AccountId], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'b646af93-2ed6-4419-9c4b-062278f3fb96', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIwZjBhM2Q3Yy00MjhjLTRjZWEtOWFlMi1jNDllODZiOTEzOTkiLCJlbWFpbCI6InZ1bm10c2UxNjEyMDdAZnB0LmVkdS52biIsIm5iZiI6MTcxNzg1MzU5OSwiZXhwIjoxNzE4MDY5NTM5LCJpYXQiOjE3MTc4NTM1OTksImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTE4OCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTE4OCJ9.8Fo4OKVBYhdor9ViHQBdhZkCelM2QaMfeE4o4JU3R7w', N'Rgeaj2WKKM7dkad+5xzxzsOhs9fWijHstWsXhAhz7x0=', N'0f0a3d7c-428c-4cea-9ae2-c49e86b91399', CAST(N'2024-06-08T20:33:19.4836328' AS DateTime2), CAST(N'2024-06-08T20:33:19.4968714' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
INSERT [dbo].[Tokens] ([Id], [AccessToken], [RefreshToken], [AccountId], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'84789363-d788-4a78-bc7c-17afebcc6250', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIwZjBhM2Q3Yy00MjhjLTRjZWEtOWFlMi1jNDllODZiOTEzOTkiLCJlbWFpbCI6InZ1bm10c2UxNjEyMDdAZnB0LmVkdS52biIsIm5iZiI6MTcxNzg1Mzk1MywiZXhwIjoxNzE4MDY5ODkzLCJpYXQiOjE3MTc4NTM5NTMsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTE4OCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTE4OCJ9.EsTNo7XGgH8L_Lo3CCm6PYV_5B9t54jmEMHULt6WkNo', N'V8pk+smh5pomHxv0434giATi3+O5WTfwXql8QPFEHQY=', N'0f0a3d7c-428c-4cea-9ae2-c49e86b91399', CAST(N'2024-06-08T20:39:13.1636179' AS DateTime2), CAST(N'2024-06-08T20:39:13.1748965' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
INSERT [dbo].[Tokens] ([Id], [AccessToken], [RefreshToken], [AccountId], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'd3f4ff73-4a1a-40f8-9029-32ef637a6141', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIwZjBhM2Q3Yy00MjhjLTRjZWEtOWFlMi1jNDllODZiOTEzOTkiLCJlbWFpbCI6InZ1bm10c2UxNjEyMDdAZnB0LmVkdS52biIsIm5iZiI6MTcxNzg1MzcwMSwiZXhwIjoxNzE4MDY5NjQxLCJpYXQiOjE3MTc4NTM3MDEsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTE4OCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTE4OCJ9.EVD2SNRBLVPyQxFRZHlGKkByJahU0J9cnvzsJPhPtwg', N'+enIHkg+dB3RTAj/e8jH+4JYSWl6fVa703/q51PJfD4=', N'0f0a3d7c-428c-4cea-9ae2-c49e86b91399', CAST(N'2024-06-08T20:35:01.9312594' AS DateTime2), CAST(N'2024-06-08T20:35:01.9434982' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
INSERT [dbo].[Tokens] ([Id], [AccessToken], [RefreshToken], [AccountId], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'7c05a240-9bac-475f-bf7e-443d764f19ad', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIwZjBhM2Q3Yy00MjhjLTRjZWEtOWFlMi1jNDllODZiOTEzOTkiLCJlbWFpbCI6InZ1bm10c2UxNjEyMDdAZnB0LmVkdS52biIsIm5iZiI6MTcxODEwMzY3NywiZXhwIjoxNzE4MzE5NjE3LCJpYXQiOjE3MTgxMDM2NzcsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTE4OCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTE4OCJ9.lJbKWcK6prnC7B3KJPYZEiSln0ZHH3iUdt7dA-jv53E', N'2D4gjBOKSGY8KYUEXGMa6q9JghRlejHAsYE1sKs05Qk=', N'0f0a3d7c-428c-4cea-9ae2-c49e86b91399', CAST(N'2024-06-11T18:01:17.2922930' AS DateTime2), CAST(N'2024-06-11T18:01:17.3053231' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
INSERT [dbo].[Tokens] ([Id], [AccessToken], [RefreshToken], [AccountId], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'0f0a3d7c-428c-4cea-9ae2-c49e86b91399', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIwZjBhM2Q3Yy00MjhjLTRjZWEtOWFlMi1jNDllODZiOTEzOTkiLCJlbWFpbCI6InZ1bm10c2UxNjEyMDdAZnB0LmVkdS52biIsIm5iZiI6MTcxNzg1MDIzMSwiZXhwIjoxNzE4MDY2MTcxLCJpYXQiOjE3MTc4NTAyMzEsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTE4OCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTE4OCJ9.vwlJD01l0R8Shtj6XShMXwQUtHO3AG92Ys7BhQ70uaM', N'u3hVgwwN3nwb6i47sofEzIAASCLNZggzXggnfhV0zNk=', N'0f0a3d7c-428c-4cea-9ae2-c49e86b91399', CAST(N'2024-06-08T19:37:13.4998028' AS DateTime2), CAST(N'2024-06-08T19:37:13.5126455' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
INSERT [dbo].[Tokens] ([Id], [AccessToken], [RefreshToken], [AccountId], [CreatedOn], [ModifiedOn], [CreatedBy], [ModifiedBy]) VALUES (N'31310842-c393-46a8-9f6a-d5e3c6140538', N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIwZjBhM2Q3Yy00MjhjLTRjZWEtOWFlMi1jNDllODZiOTEzOTkiLCJlbWFpbCI6InZ1bm10c2UxNjEyMDdAZnB0LmVkdS52biIsIm5iZiI6MTcxNzg1NDYyNCwiZXhwIjoxNzE4MDcwNTY0LCJpYXQiOjE3MTc4NTQ2MjQsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTE4OCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTE4OCJ9.5jNMHBj0kks_vPARVZ0C7_co4heykpbmyOyktSY5b9k', N'G5U07v0fB4ixqMK4aJ00PB6ZeWG1wT6VDhlv4qVEY8o=', N'0f0a3d7c-428c-4cea-9ae2-c49e86b91399', CAST(N'2024-06-08T20:50:24.8310832' AS DateTime2), CAST(N'2024-06-08T20:50:24.8431533' AS DateTime2), N'00000000-0000-0000-0000-000000000000', N'00000000-0000-0000-0000-000000000000')
GO
/****** Object:  Index [IX_AccountPermissionGroup_PermissionGroupsId]    Script Date: 11/06/2024 19:49:29 ******/
CREATE NONCLUSTERED INDEX [IX_AccountPermissionGroup_PermissionGroupsId] ON [dbo].[AccountPermissionGroup]
(
	[PermissionGroupsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Accounts_CampusId]    Script Date: 11/06/2024 19:49:29 ******/
CREATE NONCLUSTERED INDEX [IX_Accounts_CampusId] ON [dbo].[Accounts]
(
	[CampusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CategoryProduct_ProductsId]    Script Date: 11/06/2024 19:49:29 ******/
CREATE NONCLUSTERED INDEX [IX_CategoryProduct_ProductsId] ON [dbo].[CategoryProduct]
(
	[ProductsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Comments_AccountId]    Script Date: 11/06/2024 19:49:29 ******/
CREATE NONCLUSTERED INDEX [IX_Comments_AccountId] ON [dbo].[Comments]
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Comments_ProductId]    Script Date: 11/06/2024 19:49:29 ******/
CREATE NONCLUSTERED INDEX [IX_Comments_ProductId] ON [dbo].[Comments]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_FinancialTickets_UserId]    Script Date: 11/06/2024 19:49:29 ******/
CREATE NONCLUSTERED INDEX [IX_FinancialTickets_UserId] ON [dbo].[FinancialTickets]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ImageProduct_ProductsId]    Script Date: 11/06/2024 19:49:29 ******/
CREATE NONCLUSTERED INDEX [IX_ImageProduct_ProductsId] ON [dbo].[ImageProduct]
(
	[ProductsId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Payments_UserId]    Script Date: 11/06/2024 19:49:29 ******/
CREATE NONCLUSTERED INDEX [IX_Payments_UserId] ON [dbo].[Payments]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Permissions_PermissionGroupId]    Script Date: 11/06/2024 19:49:29 ******/
CREATE NONCLUSTERED INDEX [IX_Permissions_PermissionGroupId] ON [dbo].[Permissions]
(
	[PermissionGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Permissions_ResourceId]    Script Date: 11/06/2024 19:49:29 ******/
CREATE NONCLUSTERED INDEX [IX_Permissions_ResourceId] ON [dbo].[Permissions]
(
	[ResourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PostTickets_ProductId]    Script Date: 11/06/2024 19:49:29 ******/
CREATE NONCLUSTERED INDEX [IX_PostTickets_ProductId] ON [dbo].[PostTickets]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PostTickets_UserId]    Script Date: 11/06/2024 19:49:29 ******/
CREATE NONCLUSTERED INDEX [IX_PostTickets_UserId] ON [dbo].[PostTickets]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PurchaseTickets_ProductId]    Script Date: 11/06/2024 19:49:29 ******/
CREATE NONCLUSTERED INDEX [IX_PurchaseTickets_ProductId] ON [dbo].[PurchaseTickets]
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_PurchaseTickets_UserId]    Script Date: 11/06/2024 19:49:29 ******/
CREATE NONCLUSTERED INDEX [IX_PurchaseTickets_UserId] ON [dbo].[PurchaseTickets]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Tokens_AccountId]    Script Date: 11/06/2024 19:49:29 ******/
CREATE NONCLUSTERED INDEX [IX_Tokens_AccountId] ON [dbo].[Tokens]
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_TransactionHistories_UserId]    Script Date: 11/06/2024 19:49:29 ******/
CREATE NONCLUSTERED INDEX [IX_TransactionHistories_UserId] ON [dbo].[TransactionHistories]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Accounts] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsActived]
GO
ALTER TABLE [dbo].[AccountPermissionGroup]  WITH CHECK ADD  CONSTRAINT [FK_AccountPermissionGroup_Accounts_AccountsId] FOREIGN KEY([AccountsId])
REFERENCES [dbo].[Accounts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AccountPermissionGroup] CHECK CONSTRAINT [FK_AccountPermissionGroup_Accounts_AccountsId]
GO
ALTER TABLE [dbo].[AccountPermissionGroup]  WITH CHECK ADD  CONSTRAINT [FK_AccountPermissionGroup_PermissionGroups_PermissionGroupsId] FOREIGN KEY([PermissionGroupsId])
REFERENCES [dbo].[PermissionGroups] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AccountPermissionGroup] CHECK CONSTRAINT [FK_AccountPermissionGroup_PermissionGroups_PermissionGroupsId]
GO
ALTER TABLE [dbo].[Accounts]  WITH CHECK ADD  CONSTRAINT [FK_Accounts_Campuses_CampusId] FOREIGN KEY([CampusId])
REFERENCES [dbo].[Campuses] ([Id])
GO
ALTER TABLE [dbo].[Accounts] CHECK CONSTRAINT [FK_Accounts_Campuses_CampusId]
GO
ALTER TABLE [dbo].[CategoryProduct]  WITH CHECK ADD  CONSTRAINT [FK_CategoryProduct_Categories_CategoriesId] FOREIGN KEY([CategoriesId])
REFERENCES [dbo].[Categories] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CategoryProduct] CHECK CONSTRAINT [FK_CategoryProduct_Categories_CategoriesId]
GO
ALTER TABLE [dbo].[CategoryProduct]  WITH CHECK ADD  CONSTRAINT [FK_CategoryProduct_Products_ProductsId] FOREIGN KEY([ProductsId])
REFERENCES [dbo].[Products] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CategoryProduct] CHECK CONSTRAINT [FK_CategoryProduct_Products_ProductsId]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Accounts_AccountId] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Accounts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Accounts_AccountId]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Products_ProductId]
GO
ALTER TABLE [dbo].[FinancialTickets]  WITH CHECK ADD  CONSTRAINT [FK_FinancialTickets_Accounts_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Accounts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[FinancialTickets] CHECK CONSTRAINT [FK_FinancialTickets_Accounts_UserId]
GO
ALTER TABLE [dbo].[ImageProduct]  WITH CHECK ADD  CONSTRAINT [FK_ImageProduct_Images_ImagesId] FOREIGN KEY([ImagesId])
REFERENCES [dbo].[Images] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ImageProduct] CHECK CONSTRAINT [FK_ImageProduct_Images_ImagesId]
GO
ALTER TABLE [dbo].[ImageProduct]  WITH CHECK ADD  CONSTRAINT [FK_ImageProduct_Products_ProductsId] FOREIGN KEY([ProductsId])
REFERENCES [dbo].[Products] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ImageProduct] CHECK CONSTRAINT [FK_ImageProduct_Products_ProductsId]
GO
ALTER TABLE [dbo].[Payments]  WITH CHECK ADD  CONSTRAINT [FK_Payments_Accounts_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Accounts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Payments] CHECK CONSTRAINT [FK_Payments_Accounts_UserId]
GO
ALTER TABLE [dbo].[Permissions]  WITH CHECK ADD  CONSTRAINT [FK_Permissions_PermissionGroups_PermissionGroupId] FOREIGN KEY([PermissionGroupId])
REFERENCES [dbo].[PermissionGroups] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Permissions] CHECK CONSTRAINT [FK_Permissions_PermissionGroups_PermissionGroupId]
GO
ALTER TABLE [dbo].[Permissions]  WITH CHECK ADD  CONSTRAINT [FK_Permissions_Resources_ResourceId] FOREIGN KEY([ResourceId])
REFERENCES [dbo].[Resources] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Permissions] CHECK CONSTRAINT [FK_Permissions_Resources_ResourceId]
GO
ALTER TABLE [dbo].[PostTickets]  WITH CHECK ADD  CONSTRAINT [FK_PostTickets_Accounts_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Accounts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PostTickets] CHECK CONSTRAINT [FK_PostTickets_Accounts_UserId]
GO
ALTER TABLE [dbo].[PostTickets]  WITH CHECK ADD  CONSTRAINT [FK_PostTickets_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PostTickets] CHECK CONSTRAINT [FK_PostTickets_Products_ProductId]
GO
ALTER TABLE [dbo].[PurchaseTickets]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseTickets_Accounts_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Accounts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PurchaseTickets] CHECK CONSTRAINT [FK_PurchaseTickets_Accounts_UserId]
GO
ALTER TABLE [dbo].[PurchaseTickets]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseTickets_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PurchaseTickets] CHECK CONSTRAINT [FK_PurchaseTickets_Products_ProductId]
GO
ALTER TABLE [dbo].[Ratings]  WITH CHECK ADD  CONSTRAINT [FK_Ratings_PurchaseTickets_PurchaseTicketId] FOREIGN KEY([PurchaseTicketId])
REFERENCES [dbo].[PurchaseTickets] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Ratings] CHECK CONSTRAINT [FK_Ratings_PurchaseTickets_PurchaseTicketId]
GO
ALTER TABLE [dbo].[Tokens]  WITH CHECK ADD  CONSTRAINT [FK_Tokens_Accounts_AccountId] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Accounts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Tokens] CHECK CONSTRAINT [FK_Tokens_Accounts_AccountId]
GO
ALTER TABLE [dbo].[TransactionHistories]  WITH CHECK ADD  CONSTRAINT [FK_TransactionHistories_Accounts_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Accounts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TransactionHistories] CHECK CONSTRAINT [FK_TransactionHistories_Accounts_UserId]
GO
ALTER TABLE [dbo].[UserBalances]  WITH CHECK ADD  CONSTRAINT [FK_UserBalances_Accounts_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Accounts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserBalances] CHECK CONSTRAINT [FK_UserBalances_Accounts_UserId]
GO
USE [master]
GO
ALTER DATABASE [ExchangeStuff] SET  READ_WRITE 
GO
