/****** Object:  StoredProcedure [dbo].[usp_DeleteProject]    Script Date: 3/14/2024 10:54:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
    Author: Karan Bhalodiya
    Description: This stored procedure marks a project as deleted by setting the isDeleted flag to 1 in the Projects table and related tables. 
                 It also sets the isDeleted flag to 1 in the EmployeeProjects and ProjectManager tables where the project is referenced.

    Parameters:
        @Id: INT - The ID of the project to be marked as deleted.

    Dependencies:
        Requires tables: projects, EmployeeProjects, ProjectManager
*/

ALTER procedure [dbo].[usp_DeleteProject] 
@Id int,
@UpdatedUID int = null
As
begin

declare @UpdateDate date = CAST(GETDATE() as date)
    
  update projects set isDeleted = 1,UpdatedDate=@UpdateDate,UpdatedUID=@UpdatedUID where Id = @Id
  update EmployeeProjects set isDeleted = 1,UpdatedDate=@UpdateDate,UpdatedUID=@UpdatedUID where ProjectId = @Id
  update ProjectManager set isDeleted = 1,UpdatedDate=@UpdateDate,UpdatedUID=@UpdatedUID where ProjectId = @Id
end

--exec usp_DeleteProject @Id = 1,@UpdatedUID=16

