SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
if not exists (select * from sysobjects where name='Manufacturers' and xtype='U')
begin
CREATE TABLE [dbo].[Manufacturers](
	[ManufId] [varchar](250) NOT NULL,
	[ManufName] [varchar](250) NOT NULL,
	[Description] [varchar](500) NULL,
	[Address] [varchar](250) NULL,
 CONSTRAINT [PK_Manufacturer] PRIMARY KEY CLUSTERED 
(
	[ManufId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
end
GO
SET ANSI_PADDING OFF
GO