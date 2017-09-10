CREATE TABLE [dbo].[CranRating]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[IdUser] INT NOT NULL, 
    [IdQuestion] INT NOT NULL, 
	[QuestionRating] INT NOT NULL,   
	[InsertUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [InsertDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    [UpdateUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [UpdateDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT [FK_CranRating_CranUser] FOREIGN KEY ([IdUser]) REFERENCES [CranUser]([Id]), 
    CONSTRAINT [FK_CranRating_CranQuestion] FOREIGN KEY ([IdQuestion]) REFERENCES [CranQuestion]([Id]),    
   
)

GO

CREATE INDEX [IX_CranRating_IdUser] ON [dbo].[CranRating] ([IdUser])

GO

CREATE INDEX [IX_CranRating_IdQuestion] ON [dbo].[CranRating] ([IdQuestion])
