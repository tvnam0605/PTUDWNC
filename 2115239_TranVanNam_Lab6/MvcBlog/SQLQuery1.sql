create database MvcBlog
go
use MvcBlog
go

CREATE TABLE [dbo].[Blogs] ( 
    [BlogId] INT IDENTITY (1, 1) NOT NULL, 
    [Name] NVARCHAR (200) NULL, 
	[Description] NVARCHAR (200) NULL, 
	[Owner] NVARCHAR (50) NULL, 
    CONSTRAINT [PK_dbo.Blogs] PRIMARY KEY CLUSTERED ([BlogId] ASC) 
); 
  
CREATE TABLE [dbo].[Posts] ( 
    [PostId] INT IDENTITY (1, 1) NOT NULL, 
    [Title] NVARCHAR (200) NULL, 
    [Content] NTEXT NULL, 
    [BlogId] INT NOT NULL, 
	[CreateDate] Datetime,
    CONSTRAINT [PK_dbo.Posts] PRIMARY KEY CLUSTERED ([PostId] ASC), 
    CONSTRAINT [FK_dbo.Posts_dbo.Blogs_BlogId] FOREIGN KEY ([BlogId]) 
 
REFERENCES [dbo].[Blogs] ([BlogId]) ON DELETE CASCADE
);