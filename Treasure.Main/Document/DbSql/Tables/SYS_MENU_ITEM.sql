

CREATE TABLE [dbo].[SYS_MENU_ITEM](
	[ID] [nvarchar](32) NOT NULL,
	[NO] [nvarchar](50) NULL,
	[NAME] [nvarchar](128) NULL,
	[ID_INDEX] [int] IDENTITY(1,1) NOT NULL,
	[PARENT_ID] [nvarchar](32) NULL,
	[SYS_MENU_ITEM_TYPE_ID] [nvarchar](32) NULL,
	[PICTURE_URL] [varchar](255) NULL,
	[FILE_URL] [varchar](255) NULL,
	[BUTTON_NAME] [varchar](255) NULL,
	[SORT_INDEX] [int] NULL,
	[ENABLE] [bit] NULL,
	[IS_SYS] [bit] NOT NULL,
	[CREATE_USER_ID] [nvarchar](32) NULL,
	[CREATE_DATETIME] [datetime] NULL,
	[MODIFY_USER_ID] [nvarchar](32) NULL,
	[MODIFY_DATETIME] [datetime] NULL,
 CONSTRAINT [PK_MENU_ITEM] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SYS_MENU_ITEM] ADD  CONSTRAINT [DF_SYS_MENU_ITEM_IS_SYS]  DEFAULT ((0)) FOR [IS_SYS]
GO

ALTER TABLE [dbo].[SYS_MENU_ITEM]  WITH CHECK ADD  CONSTRAINT [FK_MENU_ITEM_MENU_ITEM_TYPE] FOREIGN KEY([SYS_MENU_ITEM_TYPE_ID])
REFERENCES [dbo].[SYS_MENU_ITEM_TYPE] ([ID])
GO

ALTER TABLE [dbo].[SYS_MENU_ITEM] CHECK CONSTRAINT [FK_MENU_ITEM_MENU_ITEM_TYPE]
GO

ALTER TABLE [dbo].[SYS_MENU_ITEM]  WITH CHECK ADD  CONSTRAINT [FK_MENU_ITEM_SYS_USER] FOREIGN KEY([CREATE_USER_ID])
REFERENCES [dbo].[SYS_USER] ([ID])
GO

ALTER TABLE [dbo].[SYS_MENU_ITEM] CHECK CONSTRAINT [FK_MENU_ITEM_SYS_USER]
GO

ALTER TABLE [dbo].[SYS_MENU_ITEM]  WITH CHECK ADD  CONSTRAINT [FK_MENU_ITEM_SYS_USER1] FOREIGN KEY([MODIFY_USER_ID])
REFERENCES [dbo].[SYS_USER] ([ID])
GO

ALTER TABLE [dbo].[SYS_MENU_ITEM] CHECK CONSTRAINT [FK_MENU_ITEM_SYS_USER1]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_MENU_ITEM', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_MENU_ITEM', @level2type=N'COLUMN',@level2name=N'NO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_MENU_ITEM', @level2type=N'COLUMN',@level2name=N'NAME'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父级ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_MENU_ITEM', @level2type=N'COLUMN',@level2name=N'PARENT_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单类型ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_MENU_ITEM', @level2type=N'COLUMN',@level2name=N'SYS_MENU_ITEM_TYPE_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图片URL(项目、功能、页面前面图片的URL)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_MENU_ITEM', @level2type=N'COLUMN',@level2name=N'PICTURE_URL'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'页面对应文件的URL' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_MENU_ITEM', @level2type=N'COLUMN',@level2name=N'FILE_URL'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'按钮名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_MENU_ITEM', @level2type=N'COLUMN',@level2name=N'BUTTON_NAME'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_MENU_ITEM', @level2type=N'COLUMN',@level2name=N'SORT_INDEX'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'启用的？' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_MENU_ITEM', @level2type=N'COLUMN',@level2name=N'ENABLE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否系统菜单 0.默认 1.是系统菜单' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_MENU_ITEM', @level2type=N'COLUMN',@level2name=N'IS_SYS'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_MENU_ITEM', @level2type=N'COLUMN',@level2name=N'CREATE_USER_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_MENU_ITEM', @level2type=N'COLUMN',@level2name=N'CREATE_DATETIME'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_MENU_ITEM', @level2type=N'COLUMN',@level2name=N'MODIFY_USER_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_MENU_ITEM', @level2type=N'COLUMN',@level2name=N'MODIFY_DATETIME'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单选项' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_MENU_ITEM'
GO


