CREATE TABLE [dbo].[CranBinary]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[IdUser] INT NOT NULL, 
	[Data] VARBINARY(MAX) NULL, 

    [ContentType] VARCHAR(1000) NULL, 
	[ContentDisposition] VARCHAR(1000) NULL, 
	[Length] INT NULL,   
	[Name] VARCHAR(1000) NULL,     
    [FileName] VARCHAR(1000) NULL, 
   
	[InsertUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [InsertDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    [UpdateUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [UpdateDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT [FK_CranBinary_User] FOREIGN KEY ([IdUser]) REFERENCES [CranUser]([Id]), 
          
)

GO

CREATE INDEX [IX_CranBinary_IdUser] ON [dbo].[CranBinary] ([IdUser])
