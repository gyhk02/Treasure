
CREATE TABLE [dbo].[SYS_MENU_ITEM_TYPE](
	[ID] [nvarchar](32) NOT NULL,
	[NO] [nvarchar](50) NULL,
	[NAME] [nvarchar](128) NOT NULL,
	[SORT_INDEX] [int] NOT NULL,
 CONSTRAINT [PK_MENU_ITEM_TYPE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SYS_MENU_ITEM_TYPE] ADD  CONSTRAINT [DF_MENU_ITEM_TYPE_SORT_INDEX]  DEFAULT ((0)) FOR [SORT_INDEX]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单类型ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_MENU_ITEM_TYPE', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_MENU_ITEM_TYPE', @level2type=N'COLUMN',@level2name=N'NO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_MENU_ITEM_TYPE', @level2type=N'COLUMN',@level2name=N'NAME'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_MENU_ITEM_TYPE'
GO


