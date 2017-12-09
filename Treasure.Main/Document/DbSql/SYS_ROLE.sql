
CREATE TABLE [dbo].[SYS_ROLE](
	[ID] [nvarchar](32) NOT NULL,
	[NO] [nvarchar](50) NULL,
	[NAME] [nvarchar](128) NULL,
	[CREATE_USER_ID] [nvarchar](32) NULL,
	[CREATE_DATETIME] [datetime] NULL,
	[MODIFY_USER_ID] [nvarchar](32) NULL,
	[MODIFY_DATETIME] [datetime] NULL,
 CONSTRAINT [PK_SYS_ROLE] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SYS_ROLE] ADD  CONSTRAINT [DF_SYS_ROLE_CREATE_DATETIME]  DEFAULT (getdate()) FOR [CREATE_DATETIME]
GO

ALTER TABLE [dbo].[SYS_ROLE] ADD  CONSTRAINT [DF_SYS_ROLE_MODIFY_DATETIME]  DEFAULT (getdate()) FOR [MODIFY_DATETIME]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_ROLE', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'编号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_ROLE', @level2type=N'COLUMN',@level2name=N'NO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_ROLE', @level2type=N'COLUMN',@level2name=N'NAME'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_ROLE', @level2type=N'COLUMN',@level2name=N'CREATE_USER_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_ROLE', @level2type=N'COLUMN',@level2name=N'CREATE_DATETIME'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_ROLE', @level2type=N'COLUMN',@level2name=N'MODIFY_USER_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_ROLE', @level2type=N'COLUMN',@level2name=N'MODIFY_DATETIME'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_ROLE'
GO


