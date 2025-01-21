create database Commentsapp
go
use Commentsapp
go

CREATE TABLE [dbo].[SignUp](
	[Email] [varchar](50) NOT NULL,
	[Password] [varchar](50) NULL,
	[Secret] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
go

CREATE TABLE [dbo].[Comments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](50) NULL,
	[UserComment] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE PROCEDURE [dbo].[sp_ValidateUserSignIn]
(@UserEmail varchar(50),
@password varchar(50))
AS
BEGIN
	if (@UserEmail not in (select email from SignUp))
		begin
			select 0
		end
	else
		begin
			if (@password=(select password from SignUp where Email=@UserEmail))
				begin
					select 1
				end
			else
				begin 
					select 2
				end
		end

END
GO

