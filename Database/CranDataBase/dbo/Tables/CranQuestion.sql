﻿CREATE TABLE [dbo].[CranQuestion] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
	[IdContainer] INT NOT NULL, 
	[IdUser] INT NOT NULL, 
	[IdLanguage] INT NOT NULL DEFAULT 1, 
	[IdQuestionCopySource] INT NULL, 
	[IdQuestionType] INT DEFAULT 1,
    [Title]      VARCHAR (1000) NOT NULL,
    [Text]       VARCHAR (MAX)  NOT NULL,
	[Explanation] VARCHAR(MAX) NULL,
	[Status] INT NOT NULL DEFAULT 0, 
	[ApprovalDate] DATETIME2 NULL, 	
	[InsertUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [InsertDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    [UpdateUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [UpdateDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),                  
   
    PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_CranQuestion_CranUser] FOREIGN KEY ([IdUser]) REFERENCES [CranUser]([Id]), 
    CONSTRAINT [FK_CranQuestion_Language] FOREIGN KEY ([IdLanguage]) REFERENCES [CranLanguage]([Id]), 
    CONSTRAINT [FK_CranQuestion_Question] FOREIGN KEY ([IdQuestionCopySource]) REFERENCES [CranQuestion]([Id]), 
    CONSTRAINT [FK_CranQuestion_Container] FOREIGN KEY ([IdContainer]) REFERENCES [CranContainer]([Id]),
	CONSTRAINT [FK_CranQuestion_QuestionType] FOREIGN KEY ([IdQuestionType]) REFERENCES [CranQuestionType]([Id])
);


GO

CREATE INDEX [IX_CranQuestion_IdUser] ON [dbo].[CranQuestion] ([IdUser])

GO

CREATE INDEX [IX_CranQuestion_IdLanguage] ON [dbo].[CranQuestion] ([IdLanguage])

GO


CREATE INDEX [IX_CranQuestion_IdQuestionCopySource] ON [dbo].[CranQuestion] ([IdQuestionCopySource])

GO

CREATE INDEX [IX_CranQuestion_IdContainer] ON [dbo].[CranQuestion] ([IdContainer])

GO

CREATE INDEX [IX_CranQuestion_IdQuestionType] ON [dbo].[CranQuestion] ([IdQuestionType])
