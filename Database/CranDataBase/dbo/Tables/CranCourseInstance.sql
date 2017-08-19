CREATE TABLE [dbo].[CranCourseInstance]
(
	[Id]          INT            IDENTITY (1, 1) NOT NULL,
	[IdUser] INT NOT NULL, 
    [IdCourse] INT NOT NULL, 
	[StartedAt] DATETIME2 NULL, 
    [EndedAt] DATETIME2 NULL, 
	[InsertUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [InsertDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    [UpdateUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [UpdateDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),   
    PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_CranCourseInstance_CranCourse] FOREIGN KEY ([IdCourse]) REFERENCES [CranCourse]([Id]), 
    CONSTRAINT [FK_CranCourseInstance_CranUser] FOREIGN KEY ([IdUser]) REFERENCES [CranUser]([Id])
)

GO

CREATE INDEX [IX_CranCourseInstance_IdUser] ON [dbo].[CranCourseInstance] ([IdUser])

GO

CREATE INDEX [IX_CranCourseInstance_IdCourse] ON [dbo].[CranCourseInstance] ([IdCourse])
