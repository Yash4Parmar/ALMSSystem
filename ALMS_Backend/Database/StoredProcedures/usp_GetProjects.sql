/****** Object:  StoredProcedure [dbo].[usp_GetProjects]    Script Date: 3/13/2024 3:29:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
    Author: Karan Bhalodiya
    Description: This stored procedure retrieves a list of projects based on various filters such as search keywords, sorting options, pagination, and specific employee or project IDs.

    Parameters:
      
        @sortColumn: NVARCHAR(50) - Column used for sorting projects (default is 'Id').
        @sortType: NVARCHAR(50) - Sorting order ('asc' for ascending, 'desc' for descending; default is 'asc').
        @pageNumber: BIGINT - Page number for pagination (default is 1).
        @pageSize: BIGINT - Number of projects per page (default is 10).
        @EmployeeIds: NVARCHAR(MAX) - Comma-separated list of employee IDs to filter projects by associated employees.
        @ProjectIds: NVARCHAR(MAX) - Comma-separated list of project IDs to filter projects.

    Returns:
        A list of projects with detailed information including project ID, name, description, start date, end date, associated employees, and project manager.

    Dependencies:
        Requires tables: projects, employeeProjects, aspNetUsers, projectManager
*/

CREATE PROCEDURE [dbo].[usp_GetProjects]
    @sortColumn nvarchar(50) = 'Id', --code
    @sortType nvarchar(50) = 'asc', -- asc
    @pageNumber bigint = 1,
    @pageSize bigint = 10,
	@EmployeeIds nvarchar(MAX) = '',
	@ProjectIds nvarchar(MAX) = ''
AS
BEGIN
   
    DROP TABLE IF EXISTS #tempProjectIds

	select p.Id
	into #tempProjectIds
	from projects p
	left join employeeProjects ep on p.id = ep.ProjectId
     left join aspNetUsers a on a.EmployeeId = ep.employeeID
	 left join Projectmanager pm on p.id = pm.ProjectId
     left join aspNetUsers au on au.EmployeeId = pm.ManagerId
	where 
	(@EmployeeIds='' or (ep.EmployeeId in  (SELECT value FROM STRING_SPLIT(@EmployeeIds, ',')) and (ep.IsDeleted is null OR ep.IsDeleted = 0))
	Or (pm.ManagerId in  (SELECT value FROM STRING_SPLIT(@EmployeeIds, ',')) and (pm.IsDeleted is null OR pm.IsDeleted = 0))
	)
	and (@ProjectIds='' or p.Id in  (SELECT value FROM STRING_SPLIT(@ProjectIds, ',')))
	and (p.IsDeleted is null or p.IsDeleted = 0)
	group by p.id

   select Count(*) as Count from #tempProjectIds

    SELECT 
    ROW_NUMBER() OVER (Order By 
     CASE
       WHEN @SortType <> 'asc' then ''
       WHEN @SortColumn = 'Id'  then p.Id
    END asc,

     CASE
       WHEN @SortType <> 'desc' then ''
       WHEN @SortColumn = 'Id' then p.Id
     END desc,

      CASE
       WHEN @SortType <> 'asc' then ''
       WHEN @SortColumn = 'Name' then p.Name
    END ASC,

     CASE
       WHEN @SortType <> 'desc' then ''
       WHEN @SortColumn = 'Name'  then p.Name
     END desc,

     CASE
       WHEN @SortType <> 'asc' then ''
       WHEN @SortColumn = 'StartDate' then p.StartDate
    END ASC,

     CASE
       WHEN @SortType <> 'desc' then ''
       WHEN @SortColumn = 'StartDate'  then p.StartDate
     END desc,

     CASE
       WHEN @SortType <> 'asc' then ''
       WHEN @SortColumn = 'EndDate' then p.EndDate
    END ASC,

     CASE
       WHEN @SortType <> 'desc' then ''
       WHEN @SortColumn = 'EndDate'  then p.EndDate
     END desc,
	 
     CASE
       WHEN @SortType <> 'asc' then ''
       WHEN @SortColumn = 'Manager' then au.FirstName+','+au.LastName
    END ASC,

     CASE
       WHEN @SortType <> 'desc' then ''
       WHEN @SortColumn = 'Manager'  then au.FirstName+','+au.LastName
     END desc
     
     )
     as Id,
     p.Id AS ProjectId,p.name as Name,p.StartDate,p.EndDate,
	 -- get List of employees work on this project
       STUFF((SELECT ', ' + (a.FirstName+' '+a.LastName) FROM employeeProjects ep Left JOIN aspNetUsers a ON a.EmployeeId = ep.employeeID
              WHERE ep.ProjectId = p.Id and (ep.IsDeleted is null or ep.isDeleted = 0)
	   FOR XML PATH('')), 1, 2, '') AS Employees,
   (au.FirstName+' '+au.LastName) as Manager

    FROM projects p 
	inner join #tempProjectIds tp on tp.Id = p.id
    left JOIN projectManager pm ON p.id = pm.ProjectId and (pm.IsDeleted is null OR pm.IsDeleted = 0)
    Left JOIN AspNEtUsers au ON pm.Managerid = au.EmployeeId

	-- get only those project whitch project id provided by #tempProjectIds
    WHERE 
	(p.IsDeleted is null OR p.IsDeleted = 0)
	
    ORDER BY Id
   
    OFFSET (@pageNumber-1) * @pageSize ROWS FETCH NEXT @pagesize ROWS ONLY

END


--exec [usp_GetProjects] @pageNumber = 1,@pageSize =20, @sortColumn = 'Id',@sortType ='asc',@ProjectIds = '',@EmployeeIds = ''

GO
