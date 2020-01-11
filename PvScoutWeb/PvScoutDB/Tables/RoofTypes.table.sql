
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO
if not exists (select * from sysobjects where name='rooftypes' and xtype='U')
begin
CREATE TABLE [dbo].[RoofTypes](
	[RoofTypeId] [varchar](250) NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[IsSelected] [bit] NOT NULL,
	[ImagePath] [varchar](250) NULL,
	[DisplayName] [varchar](250) NULL,
 CONSTRAINT [PK_RoofTypes] PRIMARY KEY CLUSTERED 
(
	[RoofTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[RoofTypes] ADD  CONSTRAINT [DF_RoofTypes_IsDefault]  DEFAULT ((0)) FOR [IsDefault]
ALTER TABLE [dbo].[RoofTypes] ADD  CONSTRAINT [DF_RoofTypes_IsSelected]  DEFAULT ((0)) FOR [IsSelected]
end
SET ANSI_PADDING OFF

