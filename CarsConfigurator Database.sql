create database Cars

use Cars


-- 1-N: Component Type (e.g. Engine, Color, Package, etc.)
CREATE TABLE [dbo].[ComponentType](
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    CONSTRAINT [PK_ComponentType] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

-- Main entity: Car Components (e.g. Engine, Body Color, Special Feature)
CREATE TABLE [dbo].[CarComponent](
    [Id] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(256) NOT NULL,
    [Description] NVARCHAR(1024) NULL,
    [ComponentTypeId] INT NOT NULL, -- Foreign key to ComponentType
   
    CONSTRAINT [PK_CarComponent] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

ALTER TABLE [dbo].[CarComponent]
ADD [ImageBase64] NVARCHAR(MAX) NULL;

select * from [CarComponent]

-- M:N: Self-referencing relationship between CarComponents (which parts are compatible with others)
CREATE TABLE [dbo].[CarComponentCompatibility](
    [Id] INT IDENTITY(1,1) NOT NULL,
    [CarComponentId1] INT NOT NULL, -- First part
    [CarComponentId2] INT NOT NULL, -- Second part
    CONSTRAINT [PK_CarComponentCompatibility] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

-- User (customer who configures the car)
CREATE TABLE [dbo].[User]
(
    [Id] INT IDENTITY(1,1) NOT NULL,
    [FirstName] NVARCHAR(100) NOT NULL,
    [LastName] NVARCHAR(100) NOT NULL,
	[Username] NVARCHAR(100) NOT NULL,
    [Role] NVARCHAR(100) NOT NULL,
	[Email] NVARCHAR(256) NOT NULL,
    [PasswordHash] NVARCHAR(256) NOT NULL,
    [Phone] NVARCHAR(50) NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
ALTER TABLE [User]
ADD
    [Role] NVARCHAR(100) NOT NULL DEFAULT 'User';


	UPDATE [User]
SET Role = 'Admin'
WHERE Username = 'ncevid@algebra.hr';

select * from [User]

alter table [User]
drop column
[PasswordHash]-- Use to check password hash

alter TABLE [dbo].[User](
  [Id] [int] IDENTITY(1,1) NOT NULL,
  [Username] [nvarchar](50) NOT NULL, -- Use this for login
  [PwdHash] [nvarchar](256) NOT NULL, -- Use to check password hash
  [PwdSalt] [nvarchar](256) NOT NULL, -- Additional level of security (random string)
  [FirstName] [nvarchar](256) NOT NULL,
  [LastName] [nvarchar](256) NOT NULL,
  [Email] [nvarchar](256) NOT NULL,
  [Phone] [nvarchar](256) NULL,
  CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED (
    [Id] ASC
  )
)

select * from [User]
-- Configuration (the car configuration made by a user)
CREATE TABLE [dbo].[Configuration](
    [Id] INT IDENTITY(1,1) NOT NULL,
    [UserId] INT NOT NULL, -- Foreign key to User
    [CreationDate] DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [PK_Configuration] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

-- M:N: Relationship between Configuration and CarComponent (which components are selected for a configuration)
CREATE TABLE [dbo].[ConfigurationCarComponent](
    [Id] INT IDENTITY(1,1) NOT NULL,
    [ConfigurationId] INT NOT NULL, -- Foreign key to Configuration
    [CarComponentId] INT NOT NULL, -- Foreign key to CarComponent
    CONSTRAINT [PK_ConfigurationCarComponent] PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO

CREATE TABLE [dbo].[UserConfiguration] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserId] INT NOT NULL,
    [CarComponentId] INT NOT NULL,
    CONSTRAINT FK_UserConfiguration_User FOREIGN KEY (UserId) REFERENCES [dbo].[User](Id),
    CONSTRAINT FK_UserConfiguration_CarComponent FOREIGN KEY (CarComponentId) REFERENCES [dbo].[CarComponent](Id)
);


-- FOREIGN KEYS

-- CarComponent → ComponentType (1:N relationship)
ALTER TABLE [dbo].[CarComponent] WITH CHECK ADD  
    CONSTRAINT [FK_CarComponent_ComponentType] FOREIGN KEY([ComponentTypeId])
    REFERENCES [dbo].[ComponentType] ([Id])
GO

-- CarComponentCompatibility (Self-referencing M:N relationship)
ALTER TABLE [dbo].[CarComponentCompatibility] WITH CHECK ADD  
    CONSTRAINT [FK_CarComponentCompatibility_CarComponent1] FOREIGN KEY([CarComponentId1])
    REFERENCES [dbo].[CarComponent] ([Id])
GO

ALTER TABLE [dbo].[CarComponentCompatibility] WITH CHECK ADD  
    CONSTRAINT [FK_CarComponentCompatibility_CarComponent2] FOREIGN KEY([CarComponentId2])
    REFERENCES [dbo].[CarComponent] ([Id])
GO

-- Configuration → User (1:N relationship)
ALTER TABLE [dbo].[Configuration] WITH CHECK ADD  
    CONSTRAINT [FK_Configuration_User] FOREIGN KEY([UserId])
    REFERENCES [dbo].[User] ([Id])
GO

-- ConfigurationCarComponent → Configuration and CarComponent (M:N relationship)
ALTER TABLE [dbo].[ConfigurationCarComponent] WITH CHECK ADD  
    CONSTRAINT [FK_ConfigurationCarComponent_Configuration] FOREIGN KEY([ConfigurationId])
    REFERENCES [dbo].[Configuration] ([Id])
GO

ALTER TABLE [dbo].[ConfigurationCarComponent] WITH CHECK ADD  
    CONSTRAINT [FK_ConfigurationCarComponent_CarComponent] FOREIGN KEY([CarComponentId])
    REFERENCES [dbo].[CarComponent] ([Id])
GO



