CREATE TABLE [dbo].[CranCourseInstanceQuestion]
(
	[Id]      INT           IDENTITY (1, 1) NOT NULL,
	[IdCourseInstance] INT NOT NULL, 
    [IdQuestion] INT NOT NULL, 
	[Correct] BIT NOT NULL,
	[InsertUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [InsertDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    [UpdateUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [UpdateDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),   
    PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_CranCourseInstanceQuestion_CranCourseInstance] FOREIGN KEY ([IdCourseInstance]) REFERENCES [CranCourseInstance]([Id]), 
    CONSTRAINT [FK_CranCourseInstanceQuestion_CranQuestion] FOREIGN KEY ([IdQuestion]) REFERENCES [CranQuestion]([Id])
)

GO

CREATE INDEX [IX_CranCourseInstanceQuestion_IdCourseInstance] ON [dbo].[CranCourseInstanceQuestion] ([IdCourseInstance])

GO

CREATE INDEX [IX_CranCourseInstanceQuestion_IdQuestion] ON [dbo].[CranCourseInstanceQuestion] ([IdQuestion])
