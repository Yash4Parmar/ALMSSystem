/****** Object:  StoredProcedure [dbo].[usp_GetProjectById]    Script Date: 3/13/2024 3:29:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_GetProjectById]
    @Id INT
AS
BEGIN
    -- Selecting project details
    SELECT distinct
        p.id AS Id,
        p.name AS Name,
        p.Description,
        p.StartDate,
        p.EndDate,
		CASE 
		
		-- get manager in project
	    WHEN pm.isDeleted = 1 then null
        ELSE  (au.FirstName+' '+au.LastName)
       END AS Manager,
	   case 
	   when pm.isDeleted=1 then null
	   else au.EmployeeId
	   end  as ManagerId,
	   case 

	   -- get employee in project
	   when ep.isDeleted=1 then null
	   else (a.FirstName+' '+a.LastName)
	   end AS Employee,
	   case 
	   when ep.isDeleted=1 then null
	   else a.EmployeeId
	   end  as EmployeeId
    FROM 
        projects p
    LEFT JOIN 
        Projectmanager pm ON p.id = pm.ProjectId
    LEFT JOIN 
        aspNetUsers au ON au.EmployeeId = pm.managerID
    LEFT JOIN 
        employeeProjects ep ON p.id = ep.ProjectId
    LEFT JOIN 
        aspNetUsers a ON a.EmployeeId = ep.employeeID

    WHERE 
        (p.isDeleted = 0 OR p.isDeleted IS NULL) 
        AND p.id = @Id;
END

--exec [usp_GetProjectById] @Id = 1
GO
