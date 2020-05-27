USE [master]
GO
/****** Object:  Database [coronaInformation]    Script Date: 5/27/2020 12:26:45 PM ******/
CREATE DATABASE [coronaInformation]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'coronaInformation', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER01\MSSQL\DATA\coronaInformation.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'coronaInformation_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER01\MSSQL\DATA\coronaInformation_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [coronaInformation] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [coronaInformation].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [coronaInformation] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [coronaInformation] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [coronaInformation] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [coronaInformation] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [coronaInformation] SET ARITHABORT OFF 
GO
ALTER DATABASE [coronaInformation] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [coronaInformation] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [coronaInformation] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [coronaInformation] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [coronaInformation] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [coronaInformation] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [coronaInformation] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [coronaInformation] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [coronaInformation] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [coronaInformation] SET  DISABLE_BROKER 
GO
ALTER DATABASE [coronaInformation] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [coronaInformation] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [coronaInformation] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [coronaInformation] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [coronaInformation] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [coronaInformation] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [coronaInformation] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [coronaInformation] SET RECOVERY FULL 
GO
ALTER DATABASE [coronaInformation] SET  MULTI_USER 
GO
ALTER DATABASE [coronaInformation] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [coronaInformation] SET DB_CHAINING OFF 
GO
ALTER DATABASE [coronaInformation] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [coronaInformation] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [coronaInformation] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'coronaInformation', N'ON'
GO
ALTER DATABASE [coronaInformation] SET QUERY_STORE = OFF
GO
USE [coronaInformation]
GO
/****** Object:  Table [dbo].[Paths]    Script Date: 5/27/2020 12:26:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Paths](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[City] [nvarchar](max) NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[EndDate] [datetime2](7) NOT NULL,
	[Location] [nvarchar](max) NULL,
	[PatientId] [int] NOT NULL,
 CONSTRAINT [PK_Paths] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Patients]    Script Date: 5/27/2020 12:26:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Patients](
	[PatientId] [int] NOT NULL,
	[Age] [int] NULL,
 CONSTRAINT [PK_Patients] PRIMARY KEY CLUSTERED 
(
	[PatientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Paths] ON 

INSERT [dbo].[Paths] ([Id], [City], [StartDate], [EndDate], [Location], [PatientId]) VALUES (1, N'Jerusalem', CAST(N'2015-06-01T00:00:00.0000000' AS DateTime2), CAST(N'2016-09-01T00:00:00.0000000' AS DateTime2), N'Library', 1)
INSERT [dbo].[Paths] ([Id], [City], [StartDate], [EndDate], [Location], [PatientId]) VALUES (2, N'Tel Aviv', CAST(N'2019-06-01T00:00:00.0000000' AS DateTime2), CAST(N'2019-12-01T00:00:00.0000000' AS DateTime2), N'Library', 2)
INSERT [dbo].[Paths] ([Id], [City], [StartDate], [EndDate], [Location], [PatientId]) VALUES (3, N'Ashdod', CAST(N'2015-06-01T00:00:00.0000000' AS DateTime2), CAST(N'2016-09-01T00:00:00.0000000' AS DateTime2), N'Garden', 1)
INSERT [dbo].[Paths] ([Id], [City], [StartDate], [EndDate], [Location], [PatientId]) VALUES (7, N'Ashdod', CAST(N'2020-06-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-09-01T00:00:00.0000000' AS DateTime2), N'Broadwlk', 8)
INSERT [dbo].[Paths] ([Id], [City], [StartDate], [EndDate], [Location], [PatientId]) VALUES (8, N'Tel Aviv', CAST(N'2020-06-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-09-01T00:00:00.0000000' AS DateTime2), N'Garden', 3)
INSERT [dbo].[Paths] ([Id], [City], [StartDate], [EndDate], [Location], [PatientId]) VALUES (9, N'Jerusalem', CAST(N'2020-06-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-09-01T00:00:00.0000000' AS DateTime2), N'Garden', 8)
INSERT [dbo].[Paths] ([Id], [City], [StartDate], [EndDate], [Location], [PatientId]) VALUES (10, N'Ashdod', CAST(N'2020-06-01T00:00:00.0000000' AS DateTime2), CAST(N'2020-09-01T00:00:00.0000000' AS DateTime2), N'Garden', 1)
INSERT [dbo].[Paths] ([Id], [City], [StartDate], [EndDate], [Location], [PatientId]) VALUES (18, N'Ashdod', CAST(N'2020-01-05T00:01:00.0000000' AS DateTime2), CAST(N'2020-01-06T00:01:00.0000000' AS DateTime2), N'School', 35)
INSERT [dbo].[Paths] ([Id], [City], [StartDate], [EndDate], [Location], [PatientId]) VALUES (54, N'Modiin', CAST(N'2020-01-05T00:19:00.0000000' AS DateTime2), CAST(N'2020-01-05T00:22:00.0000000' AS DateTime2), N'Park', 200)
INSERT [dbo].[Paths] ([Id], [City], [StartDate], [EndDate], [Location], [PatientId]) VALUES (77, N'Kiryat Sefer', CAST(N'2020-01-04T00:29:00.0000000' AS DateTime2), CAST(N'2020-01-05T00:29:00.0000000' AS DateTime2), N'Store', 300)
INSERT [dbo].[Paths] ([Id], [City], [StartDate], [EndDate], [Location], [PatientId]) VALUES (78, N'Bet Shemesh', CAST(N'2020-01-05T00:01:00.0000000' AS DateTime2), CAST(N'2020-01-05T00:01:00.0000000' AS DateTime2), N'park', 300)
INSERT [dbo].[Paths] ([Id], [City], [StartDate], [EndDate], [Location], [PatientId]) VALUES (83, N'Ashdod', CAST(N'2020-01-01T00:01:00.0000000' AS DateTime2), CAST(N'2020-01-01T00:01:00.0000000' AS DateTime2), N'Beach', 7)
INSERT [dbo].[Paths] ([Id], [City], [StartDate], [EndDate], [Location], [PatientId]) VALUES (84, N'Sderot', CAST(N'2020-01-01T00:01:00.0000000' AS DateTime2), CAST(N'2020-01-01T00:01:00.0000000' AS DateTime2), N'Garden', 7)
INSERT [dbo].[Paths] ([Id], [City], [StartDate], [EndDate], [Location], [PatientId]) VALUES (85, N'Bet Shemesh', CAST(N'2020-01-03T00:01:00.0000000' AS DateTime2), CAST(N'2020-01-05T00:01:00.0000000' AS DateTime2), N'Super Market', 7)
INSERT [dbo].[Paths] ([Id], [City], [StartDate], [EndDate], [Location], [PatientId]) VALUES (86, N'Ramat Gan', CAST(N'2020-01-05T00:01:00.0000000' AS DateTime2), CAST(N'2020-01-06T00:01:00.0000000' AS DateTime2), N'Work', 7)
SET IDENTITY_INSERT [dbo].[Paths] OFF
GO
INSERT [dbo].[Patients] ([PatientId], [Age]) VALUES (1, NULL)
INSERT [dbo].[Patients] ([PatientId], [Age]) VALUES (2, NULL)
INSERT [dbo].[Patients] ([PatientId], [Age]) VALUES (3, NULL)
INSERT [dbo].[Patients] ([PatientId], [Age]) VALUES (4, NULL)
INSERT [dbo].[Patients] ([PatientId], [Age]) VALUES (5, NULL)
INSERT [dbo].[Patients] ([PatientId], [Age]) VALUES (6, NULL)
INSERT [dbo].[Patients] ([PatientId], [Age]) VALUES (7, NULL)
INSERT [dbo].[Patients] ([PatientId], [Age]) VALUES (8, NULL)
INSERT [dbo].[Patients] ([PatientId], [Age]) VALUES (9, NULL)
INSERT [dbo].[Patients] ([PatientId], [Age]) VALUES (18, NULL)
INSERT [dbo].[Patients] ([PatientId], [Age]) VALUES (25, NULL)
INSERT [dbo].[Patients] ([PatientId], [Age]) VALUES (29, NULL)
INSERT [dbo].[Patients] ([PatientId], [Age]) VALUES (35, NULL)
INSERT [dbo].[Patients] ([PatientId], [Age]) VALUES (45, NULL)
INSERT [dbo].[Patients] ([PatientId], [Age]) VALUES (50, NULL)
INSERT [dbo].[Patients] ([PatientId], [Age]) VALUES (100, NULL)
INSERT [dbo].[Patients] ([PatientId], [Age]) VALUES (102, NULL)
INSERT [dbo].[Patients] ([PatientId], [Age]) VALUES (200, NULL)
INSERT [dbo].[Patients] ([PatientId], [Age]) VALUES (300, NULL)
GO
/****** Object:  Index [IX_Paths_PatientId]    Script Date: 5/27/2020 12:26:45 PM ******/
CREATE NONCLUSTERED INDEX [IX_Paths_PatientId] ON [dbo].[Paths]
(
	[PatientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Paths]  WITH CHECK ADD  CONSTRAINT [FK_Paths_Patients_PatientId] FOREIGN KEY([PatientId])
REFERENCES [dbo].[Patients] ([PatientId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Paths] CHECK CONSTRAINT [FK_Paths_Patients_PatientId]
GO
USE [master]
GO
ALTER DATABASE [coronaInformation] SET  READ_WRITE 
GO
