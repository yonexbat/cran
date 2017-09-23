CREATE TABLE [dbo].[CranRelQuestionImage]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [IdQuestion] INT NOT NULL, 
    [IdImage] INT NOT NULL, 
	[InsertUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [InsertDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    [UpdateUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [UpdateDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [FK_CranRelQuestionImage_Question] FOREIGN KEY ([IdQuestion]) REFERENCES [CranQuestion]([Id]), 
    CONSTRAINT [FK_CranRelQuestionImage_Image] FOREIGN KEY ([IdImage]) REFERENCES [CranImage]([Id])	
)

GO

CREATE INDEX [IX_CranRelQuestionImage_IdQuestion] ON [dbo].[CranRelQuestionImage] ([IdQuestion])

GO

CREATE INDEX [IX_CranRelQuestionImage_IdImage] ON [dbo].[CranRelQuestionImage] ([IdImage])
