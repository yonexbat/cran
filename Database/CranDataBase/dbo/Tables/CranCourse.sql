CREATE TABLE [dbo].[CranCourse] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
	[IdLanguage] INT NOT NULL DEFAULT 1, 
    [Title]       VARCHAR (MAX) NULL,
    [Description] VARCHAR (MAX) NULL,
	[NumQuestionsToAsk] INT NOT NULL DEFAULT 10, 
	[InsertUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [InsertDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    [UpdateUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [UpdateDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    
    PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_CranCourse_Language] FOREIGN KEY ([IdLanguage]) REFERENCES [CranLanguage]([Id])
);


GO

CREATE INDEX [IX_CranCourse_IdLanguage] ON [dbo].[CranCourse] ([IdLanguage])
