CREATE TABLE [dbo].[CranRelQuestionTag]
(
	[Id]         INT            IDENTITY (1, 1) NOT NULL,
    [IdQuestion]  INT NOT NULL,
    [IdTag]       INT  NOT NULL,
	[InsertUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [InsertDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    [UpdateUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [UpdateDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_CranRelQuestionTag_CranQuestion] FOREIGN KEY ([IdQuestion]) REFERENCES [CranQuestion]([Id]), 
    CONSTRAINT [FK_CranRelQuestionTag_CranTag] FOREIGN KEY ([IdTag]) REFERENCES [CranTag]([Id])
)
