/****** Object:  StoredProcedure [dbo].[usp_ManageProject]    Script Date: 3/14/2024 10:06:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
    Author: Karan Bhalodiya
    Description: This stored procedure is responsible for managing projects. It allows adding new projects or updating existing ones, assigning employees and managers to projects, and retrieving project details.

    Parameters:
        @ProjectId: INT (optional) - The ID of the project to be managed. If provided, the existing project will be updated; otherwise, a new project will be added.
        @ProjectName: VARCHAR(50) - The name of the project.
        @Description: VARCHAR(500) - The description of the project.
        @StartDate: DATE - The start date of the project.
        @EndDate: DATE - The end date of the project.
        @EmployeeIds: VARCHAR(MAX) (optional) - Comma-separated list of employee IDs to be assigned to the project.
        @ManagerId: INT (optional) - The ID of the manager to be assigned to the project.

    Dependencies:
        Requires tables: Projects, EmployeeProjects, ProjectManager
        Depends on stored procedure: usp_GetProjectById
*/

ALTER PROCEDURE [dbo].[usp_ManageProject]
    @ProjectId Int = null,
    @ProjectName VARCHAR(50),
	@Description VARCHAR(500),
	@StartDate date,
	@EndDate date,
    @EmployeeIds VARCHAR(MAX)='', -- Assuming employee IDs are passed as comma-separated values
	@ManagerId int=null,
	@UID int=null
AS
BEGIN

 DECLARE @newProjectId INT
	Set @newProjectId = @ProjectId

	-- below date for when insert or update project
	declare @UpdateDate date = CAST(GETDATE() as date)

     -- Check manager already in project
	 DECLARE @isValid INT;

	IF @ManagerId is not null
    BEGIN
	IF EXISTS (select * from projectManager where ManagerId =@ManagerId and (isDeleted is null OR isDeleted =0))
    BEGIN
        PRINT 'Manager already assign in another project';
		SET @isValid = 0
	END
	 ELSE
    BEGIN
        
	  SET @isValid = 1
    END
	END
    ELSE
    BEGIN
        
	  SET @isValid = 1
    END
   
	--add project when Project id not given 
   IF @isValid =1
	BEGIN 
	IF @newProjectId is null
    BEGIN
	--Print 'new project added'
    -- Insert new project into the project table
	 IF @isValid =1 
	 BEGIN
    INSERT INTO Projects (Name,Description,StartDate,EndDate,InsetedDate,InsertedUID)
    VALUES (@ProjectName,@Description,@StartDate,@EndDate,@UpdateDate,@UID)
	END
    -- Get the ID of the newly inserted project
    SET @newProjectId = SCOPE_IDENTITY()
    END

	-- update Project data when project id is available 
    ELSE
    BEGIN
	   update Projects set Name =@ProjectName, Description = @Description, StartDate= @StartDate,EndDate=@EndDate,UpdatedDate=@UpdateDate,UpdatedUID=@UID where Id = @newProjectId
       update EmployeeProjects set isDeleted = 1,UpdatedDate=@UpdateDate,UpdatedUID=@UID where ProjectId = @newProjectId
	   update ProjectManager set isDeleted = 1,UpdatedDate=@UpdateDate,UpdatedUID=@UID where ProjectId = @newProjectId
	   --Print 'new project updates'
    END

	-- assign employees to project 
	IF @EmployeeIds != ''
    BEGIN
    DECLARE @value int;

    DECLARE sp_cursor CURSOR FOR
    SELECT CAST(value AS INT)
   FROM STRING_SPLIT(@EmployeeIds, ',');

   OPEN sp_cursor;
   FETCH NEXT FROM sp_cursor INTO @value;

   WHILE @@FETCH_STATUS = 0
   BEGIN
     insert into employeeProjects (ProjectId,EmployeeId,InsetedDate,InsertedUID) values (@newProjectId,@value,@UpdateDate,@UID)
     FETCH NEXT FROM sp_cursor INTO @value;
   END

   CLOSE sp_cursor;
  DEALLOCATE sp_cursor;
  END

	END
	
	-- assign manager to project
	 IF @isValid =1  and @ManagerId is not null
	 BEGIN
	 insert into projectManager (ManagerID,ProjectId,InsetedDate,InsertedUID) 
	 Values(@ManagerId,@newProjectId,@UpdateDate,@UId)
	 End

	-- return project details  
	--IF @isValid =1 
	-- BEGIN
	--exec usp_GetProjectById @Id = @newProjectId
	--END
	
END

-- EXEC usp_ManageProject  @ProjectId='12',@Description='description 12', @ProjectName = '12 project', @StartDate = '2024-01-01', @EndDate = '2024-12-31',@ManagerId = '5',@UID = 7

