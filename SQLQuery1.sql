SELECT
	Q.Id,
	Q.IdQuestionSucessor
FROM dbo.CranQuestion Q
WHERE
	Q.Title LIKE '%XXX%'