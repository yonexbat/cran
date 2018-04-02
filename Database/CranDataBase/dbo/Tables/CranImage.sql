CREATE TABLE [dbo].[CranImage]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [IdBinary] INT NOT NULL, 
    [Height] INT NULL, 
    [Width] INT NULL, 
    [Full] BIT NOT NULL,
	[InsertUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [InsertDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    [UpdateUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [UpdateDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT [FK_CranImage_Binary] FOREIGN KEY ([IdBinary]) REFERENCES [CranBinary]([Id])
)

GO

CREATE INDEX [IX_CranImage_IdBinary] ON [dbo].[CranImage] ([IdBinary])
