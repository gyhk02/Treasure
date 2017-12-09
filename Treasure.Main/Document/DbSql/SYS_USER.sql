
CREATE TABLE [dbo].[SYS_USER](
	[ID] [nvarchar](32) NOT NULL,
	[NO] [nvarchar](50) NULL,
	[NAME] [nvarchar](128) NULL,
	[LOGIN_NAME] [nvarchar](30) NULL,
	[PASSWORD] [nvarchar](64) NULL,
	[SEX] [nvarchar](2) NULL,
	[EMAIL] [nvarchar](255) NULL,
	[EXPIRED_DATE] [datetime] NULL,
	[CREATE_USER_ID] [nvarchar](32) NULL,
	[CREATE_DATETIME] [datetime] NULL,
	[MODIFY_USER_ID] [nvarchar](32) NULL,
	[MODIFY_DATETIME] [datetime] NULL,
 CONSTRAINT [PK_SYS_USER] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SYS_USER] ADD  CONSTRAINT [DF_SYS_USER_CREATE_DATETIME]  DEFAULT (getdate()) FOR [CREATE_DATETIME]
GO

ALTER TABLE [dbo].[SYS_USER] ADD  CONSTRAINT [DF_SYS_USER_MODIFY_DATETIME]  DEFAULT (getdate()) FOR [MODIFY_DATETIME]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_USER', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_USER', @level2type=N'COLUMN',@level2name=N'NO'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_USER', @level2type=N'COLUMN',@level2name=N'NAME'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登陆名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_USER', @level2type=N'COLUMN',@level2name=N'LOGIN_NAME'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_USER', @level2type=N'COLUMN',@level2name=N'PASSWORD'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'性别' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_USER', @level2type=N'COLUMN',@level2name=N'SEX'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮件地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_USER', @level2type=N'COLUMN',@level2name=N'EMAIL'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账户过期日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_USER', @level2type=N'COLUMN',@level2name=N'EXPIRED_DATE'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_USER', @level2type=N'COLUMN',@level2name=N'CREATE_USER_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_USER', @level2type=N'COLUMN',@level2name=N'CREATE_DATETIME'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_USER', @level2type=N'COLUMN',@level2name=N'MODIFY_USER_ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_USER', @level2type=N'COLUMN',@level2name=N'MODIFY_DATETIME'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SYS_USER'
GO


