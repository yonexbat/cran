DECLARE @CourseName VARCHAR(1000);
SET @CourseName = 'KOTLIN';


DELETE FROM OI
FROM  dbo.CranCourseInstanceQuestionOption OI
	INNER JOIN dbo.CranCourseInstanceQuestion QI ON QI.Id = OI.IdCourseInstanceQuestion
	INNER JOIN dbo.CranCourseInstance CI ON CI.Id = QI.IdCourseInstance
	INNER JOIN dbo.CranCourse C ON C.Id = CI.IdCourse
WHERE
	C.Title = @CourseName

DELETE FROM QI
FROM dbo.CranCourseInstanceQuestion QI
	INNER JOIN dbo.CranCourseInstance CI ON CI.Id = QI.IdCourseInstance
	INNER JOIN dbo.CranCourse C ON C.Id = CI.IdCourse
WHERE
	C.Title = @CourseName

DELETE FROM CI
FROM dbo.CranCourseInstance CI
	INNER JOIN dbo.CranCourse C ON C.Id = CI.IdCourse
WHERE
	C.Title = @CourseName
	

DELETE FROM C
FROM dbo.CranCourse C
WHERE 
	C.Title = @CourseName