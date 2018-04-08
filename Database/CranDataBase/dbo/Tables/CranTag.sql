CREATE TABLE [dbo].[CranTag] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
	[IdTagType] INT NOT NULL DEFAULT 1, 
    [Name]        VARCHAR (300)  NOT NULL,
    [Description] VARCHAR (MAX)  NULL,
	[ShortDescDe] VARCHAR(1000) NULL, 
    [ShortDescEn] VARCHAR(1000) NULL, 
	[InsertUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [InsertDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    [UpdateUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [UpdateDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),   
    PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_CranTag_CranTagType] FOREIGN KEY ([IdTagType]) REFERENCES [CranTagType]([Id])
);


GO

CREATE INDEX [IX_CranTag_IdTagType] ON [dbo].[CranTag] ([IdTagType])
