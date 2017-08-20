CREATE TABLE [dbo].[CranComment]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [IdUser] INT NOT NULL, 
    [IdQuestion] INT NOT NULL, 
    [CommentText] VARCHAR(MAX) NOT NULL, 
	[InsertUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [InsertDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    [UpdateUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [UpdateDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),   
    CONSTRAINT [FK_CranComment_CranUser] FOREIGN KEY ([IdUser]) REFERENCES [CranUser]([Id]), 
    CONSTRAINT [FK_CranComment_CranQuestion] FOREIGN KEY ([IdQuestion]) REFERENCES [CranQuestion]([Id])
)

GO

CREATE INDEX [IX_CranComment_IdUser] ON [dbo].[CranComment] ([IdUser])

GO

CREATE INDEX [IX_CranComment_IdQuestion] ON [dbo].[CranComment] ([IdQuestion])
