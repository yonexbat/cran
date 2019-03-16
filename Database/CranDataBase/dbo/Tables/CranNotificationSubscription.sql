CREATE TABLE [dbo].[CranNotificationSubscription]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[IdUser] INT NOT NULL, 
    [Endpoint] VARCHAR(1000) NULL, 
    [ExpirationTime] DATETIME2 NULL, 
    [P256DiffHell] VARCHAR(1000) NULL, 
    [Auth] VARCHAR(1000) NULL, 
	[AsString] VARCHAR(MAX) NULL,
	[Active] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [FK_CranNotificationSubscription_CranUser] FOREIGN KEY ([IdUser]) REFERENCES [CranUser]([Id]), 
	[InsertUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [InsertDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),
    [UpdateUser]  VARCHAR (1000) NOT NULL DEFAULT SYSTEM_USER,
    [UpdateDate]  DATETIME2 (7)  NOT NULL DEFAULT GETDATE(),     
)

GO

CREATE INDEX [IX_CranNotificationSubscription_UserID] ON [dbo].[CranNotificationSubscription] ([IdUser])
