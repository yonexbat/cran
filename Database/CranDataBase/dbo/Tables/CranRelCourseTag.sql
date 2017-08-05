CREATE TABLE [dbo].[CranRelCourseTag]
(
	[Id]         INT            IDENTITY (1, 1) NOT NULL,
    [IdCourse]     INT  NOT NULL,
    [IdTag]       INT  NOT NULL,
	[InsertUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [InsertDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    [UpdateUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [UpdateDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_CranRelCourseTag_CranTag] FOREIGN KEY ([IdTag]) REFERENCES [CranTag]([Id]), 
    CONSTRAINT [FK_CranRelCourseTag_CranCourse] FOREIGN KEY ([IdCourse]) REFERENCES [CranCourse]([Id])
)

GO

CREATE INDEX [IX_CranRelCourseTag_IdCourse] ON [dbo].[CranRelCourseTag] ([IdCourse])

GO

CREATE INDEX [IX_CranRelCourseTag_IdTag] ON [dbo].[CranRelCourseTag] ([IdTag])
