CREATE TABLE [dbo].[CranUserCourseFavorite]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[IdCourse]     INT  NOT NULL,
	[IdUser]	INT NOT NULL,
	[InsertUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [InsertDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    [UpdateUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [UpdateDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT [FK_CranUserCourseFavorite_User] FOREIGN KEY ([IdUser]) REFERENCES [CranUser]([Id]), 
    CONSTRAINT [FK_CranUserCourseFavorite_Course] FOREIGN KEY ([IdCourse]) REFERENCES [CranCourse]([Id]),
)

GO

CREATE INDEX [IX_CranUserCourseFavorite_IdUser] ON [dbo].[CranUserCourseFavorite] ([IdUser])

GO

CREATE INDEX [IX_CranUserCourseFavorite_IdCourse] ON [dbo].[CranUserCourseFavorite] ([IdCourse])
