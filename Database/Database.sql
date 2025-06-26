
CREATE TABLE [dbo].[ComponentType](
	[Id] [int] primary key IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL
	)

CREATE TABLE [dbo].[User](
	[Id] [int] primary key IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[Phone] [nvarchar](50) NULL,
	[Username] [nvarchar](100) NOT NULL,
	[Role] [nvarchar](100) NOT NULL,
	[PwdHash] [nvarchar](256) NOT NULL,
	[PwdSalt] [nvarchar](256) NOT NULL
	)

CREATE TABLE [dbo].[CarComponent](
	[Id] [int] primary key IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](1024) NULL,
	[ImageBase64] [nvarchar](max) NULL,
	[ComponentTypeId] [int] REFERENCES ComponentType(Id)


	)
 

CREATE TABLE [dbo].[CarComponentCompatibility](
	[Id] [int] primary key IDENTITY(1,1) NOT NULL,
	[CarComponentId1] [int]  foreign key references CarComponent(Id),
	[CarComponentId2] [int]  foreign key references CarComponent(Id)
	)
 

CREATE TABLE [dbo].[Configuration](
	[Id] [int] primary key IDENTITY(1,1) NOT NULL,
	[CreationDate] [datetime2](7) NOT NULL,
	[UserId] [int]  foreign key references [User](Id)
	)


CREATE TABLE [dbo].[ConfigurationCarComponent](
	[Id] [int] primary key IDENTITY(1,1) NOT NULL,
	[ConfigurationId] [int] foreign key references [Configuration](Id),
	[CarComponentId] [int] foreign key references [CarComponent](Id)
	)


CREATE TABLE [dbo].[UserConfiguration](
	[Id] [int] primary key IDENTITY(1,1) NOT NULL,
	[UserId] [int]  foreign key references [User](Id),
	[CarComponentId] [int]  foreign key references [CarComponent](Id)
	)

CREATE TABLE [dbo].[UserRole](
	[Id] [int] primary key IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[Role] [nvarchar](100) NOT NULL
) 

