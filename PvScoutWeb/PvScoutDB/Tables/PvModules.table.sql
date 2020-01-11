SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
if not exists (select * from sysobjects where name='PvModules' and xtype='U')
begin
CREATE TABLE [dbo].[PvModules](
	[PVscoutArticleNumber] [varchar](250) NOT NULL,
	[ArticleNumber] [varchar](50) NOT NULL,
	[ManufId] [int] NOT NULL,
	[ImagePath] [varchar](250) NULL,
	[Length] [int] NOT NULL,
	[Width] [int] NOT NULL,
	[Thickness] [int] NOT NULL,
	[Weight] [float] NOT NULL,
	[CellTechnology] [varchar](250) NULL,
	[CellMaterial] [varchar](250) NULL,
 CONSTRAINT [PK_PvModules] PRIMARY KEY CLUSTERED 
(
	[PVscoutArticleNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
ALTER TABLE [dbo].[PvModules]  WITH CHECK ADD  CONSTRAINT [FK_PvModules_Manufacturers] FOREIGN KEY([ManufId])
REFERENCES [dbo].[Manufacturers] ([ManufId])
ALTER TABLE [dbo].[PvModules] CHECK CONSTRAINT [FK_PvModules_Manufacturers]
end
GO
SET ANSI_PADDING OFF
GO
/****** Object:  ForeignKey [FK_PvModules_Manufacturers]    Script Date: 04/04/2011 00:16:08 ******/
GO
