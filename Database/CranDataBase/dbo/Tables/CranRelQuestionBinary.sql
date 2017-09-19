CREATE TABLE [dbo].[CranRelQuestionBinary]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [IdBinary] INT NOT NULL, 
    [IdQuestion] INT NOT NULL,
	[InsertUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [InsertDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    [UpdateUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [UpdateDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT [FK_CranRelQuestionBinary_CranBinary] FOREIGN KEY ([IdBinary]) REFERENCES [CranBinary]([Id]), 
    CONSTRAINT [FK_CranRelQuestionBinary_CranQuestion] FOREIGN KEY ([IdQuestion]) REFERENCES [CranQuestion]([Id]),
)

GO

CREATE INDEX [IX_CranRelQuestionBinary_IdBinary] ON [dbo].[CranRelQuestionBinary] ([IdBinary])

GO

CREATE INDEX [IX_CranRelQuestionBinary_IdQuestion] ON [dbo].[CranRelQuestionBinary] ([IdQuestion])
