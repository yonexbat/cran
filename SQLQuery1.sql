SELECT [x].[Title], [x].[Id], [x].[Status]
FROM [CranQuestion] AS [x]
ORDER BY [x].[Title]
OFFSET 65 ROWS FETCH NEXT 5 ROWS ONLY

SELECT [x].[Id]
FROM [CranQuestion] AS [x]
ORDER BY [x].[Title]
OFFSET 65 ROWS FETCH NEXT 5 ROWS ONLY

SELECT [rel].[IdTag] AS [TagId], [rel].[IdQuestion] AS [QuestionId], [rel.Tag].[Name] AS [TagName]
FROM [CranRelQuestionTag] AS [rel]
		INNER JOIN [CranTag] AS [rel.Tag] ON [rel].[IdTag] = [rel.Tag].[Id]
WHERE [rel].[IdQuestion] IN (
          SELECT [x].[Id]
          FROM [CranQuestion] AS [x]
          ORDER BY [x].[Title]
          OFFSET 65 ROWS FETCH NEXT 5 ROWS ONLY
)