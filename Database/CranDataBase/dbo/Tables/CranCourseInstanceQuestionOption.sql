CREATE TABLE [dbo].[CranCourseInstanceQuestionOption]
(
	[Id]         INT            IDENTITY (1, 1) NOT NULL,
	[IdCourseInstanceQuestion] INT NOT NULL, 
	[IdQuestionOption] INT NOT NULL, 
    [Checked] BIT NOT NULL, 
    [Correct] BIT NOT NULL, 
	[InsertUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [InsertDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    [UpdateUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [UpdateDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),          
    PRIMARY KEY CLUSTERED ([Id] ASC), 
    CONSTRAINT [FK_CranCourseInstanceQuestionOption_CranQuestionOption] FOREIGN KEY ([IdQuestionOption]) REFERENCES [CranQuestionOption]([Id]), 
    CONSTRAINT [FK_CranCourseInstanceQuestionOption_CranCourseInstanceQuestion] FOREIGN KEY ( [IdCourseInstanceQuestion]) REFERENCES [CranCourseInstanceQuestion]([Id])
)
