CREATE TABLE [dbo].[CranQuestion] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Title]      VARCHAR (1000) NOT NULL,
    [Text]       VARCHAR (MAX)  NOT NULL,
	[Explanation] VARCHAR(MAX) NULL, 
	[InsertUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [InsertDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    [UpdateUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [UpdateDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
   
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

