CREATE TABLE [dbo].[CranQuestionOption] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [IdQuestion] INT            NOT NULL,
    [Text]       VARCHAR (MAX)  NOT NULL,
    [IsTrue]     BIT            NOT NULL,
	[InsertUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [InsertDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    [UpdateUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [UpdateDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
	PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_CranQuestionOption_CranQuestion] FOREIGN KEY ([IdQuestion]) REFERENCES [CranQuestion]([Id])
);


GO

CREATE INDEX [IX_CranQuestionOption_IdQuestion] ON [dbo].[CranQuestionOption] ([IdQuestion])
