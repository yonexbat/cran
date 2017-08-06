CREATE TABLE [dbo].[CranCourse] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Title]       VARCHAR (MAX) NULL,
    [Description] VARCHAR (MAX) NULL,
	[NumQuestionsToAsk] INT NOT NULL DEFAULT 10, 
	[InsertUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [InsertDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    [UpdateUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [UpdateDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

