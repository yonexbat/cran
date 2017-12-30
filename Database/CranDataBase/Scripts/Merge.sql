BEGIN TRANSACTION

DECLARE @MappInt TABLE  (
	IdQuestion	INT,
	IdContainer INT
)

MERGE dbo.CranContainer AS TARGET
USING (
	SELECT
	 Id,
	 IdContainer,
	 InsertUser,
	 UpdateUser
	FROM dbo.CranQuestion
) AS SOURCE 
	ON TARGET.Id = SOURCE.IdContainer
WHEN NOT MATCHED THEN
    INSERT (InsertUser, UpdateUser)
    VALUES (SOURCE.InsertUser, UpdateUser)
OUTPUT SOURCE.Id,  inserted.Id INTO @MappInt(IdQuestion, IdContainer);

UPDATE Q
	SET Q.IdContainer = T.IdContainer
FROM dbo.CranQuestion Q
	INNER JOIN @MappInt T ON T.IdQuestion = Q.Id

COMMIT TRANSACTION