
CREATE TABLE [dbo].[SYS_GENERATE_FILE](
	[ID] [nvarchar](32) NOT NULL,
	[NO] [nvarchar](50) NULL,
	[NAME] [nvarchar](128) NULL,
	[IS_DEFAULT_ADD] [bit] NOT NULL,
	[IS_DEFAULT_MODIFY] [bit] NOT NULL,
	[IS_DEFAULT_DELETE] [bit] NOT NULL,
	[IS_DEFAULT_EXPORT_EXCEL] [bit] NOT NULL,
	[CREATE_USER_ID] [nvarchar](32) NULL,
	[CREATE_DATETIME] [datetime] NULL,
	[MODIFY_USER_ID] [nvarchar](32) NULL,
	[MODIFY_DATETIME] [datetime] NULL,
 CONSTRAINT [PK_SYS_GENERATE_FILE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SYS_GENERATE_FILE] ADD  CONSTRAINT [DF_SYS_GENERATE_FILE_CREATE_DATETIME]  DEFAULT (getdate()) FOR [CREATE_DATETIME]
GO

ALTER TABLE [dbo].[SYS_GENERATE_FILE] ADD  CONSTRAINT [DF_SYS_GENERATE_FILE_MODIFY_DATETIME]  DEFAULT (getdate()) FOR [MODIFY_DATETIME]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'默认有新增功能 1有 0无' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_GENERATE_FILE', @level2type=N'COLUMN',@level2name=N'IS_DEFAULT_ADD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'默认有修改功能 1有 0无' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_GENERATE_FILE', @level2type=N'COLUMN',@level2name=N'IS_DEFAULT_MODIFY'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'默认有删除功能 1有 0无' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_GENERATE_FILE', @level2type=N'COLUMN',@level2name=N'IS_DEFAULT_DELETE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'默认导出Excel 1有 0无' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_GENERATE_FILE', @level2type=N'COLUMN',@level2name=N'IS_DEFAULT_EXPORT_EXCEL'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DESCRITIPTION', @value=N'生成文件系统设置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_GENERATE_FILE'
GO


