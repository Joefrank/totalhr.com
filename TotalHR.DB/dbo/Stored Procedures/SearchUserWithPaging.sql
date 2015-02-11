
CREATE proc [dbo].[SearchUserWithPaging]
	@id int = null,
	@name nvarchar(100) = null,
	@usertypeid int= null,
	@departmentid int = null,
	@email nvarchar(100) = null,
	@partialaddress nvarchar(200) = null,
	@town nvarchar(50) = null,
	@county nvarchar(50) = null,
	@postcode nvarchar(30) = null,
	@phone nvarchar(30) = null,
	@PageSize int = 10,
	@PageNumber int = 1,
	@ordercolumn varchar(50) = 'UserId',
	@sortorder varchar(5) = 'ASC',
	@viewinglanguageid int = 1
As
Begin
	declare @tempView as Table(RowNumber int identity(1,1), Total int, UserId int, DepartmentName nvarchar(50),
		GenderTerm nvarchar(100), TitleTerm nvarchar(100), PreferredLanguageTerm nvarchar(100))
	
	declare @higherbound int, @lowerbound int
	declare @colId varchar(20), @colFirstName varchar(20), @colSurname varchar(20),
		@colGender varchar(20), @colUsername varchar(20), @colEmail varchar(20), @colDepartment varchar(20),
		@colLastVisit varchar(20), @colNoOfVisits varchar(20)
		
	
	set @colId = 'UserId'; set  @colFirstName = 'FirstName'; set  @colSurname = 'SurName';
	set @colGender = 'Gender'; set  @colUsername = 'Username'; set  @colEmail = 'Email'; 
	set  @colDepartment = 'Department';	set @colLastVisit = 'LastVisit'; set  @colNoOfVisits = 'NoOfVisits'
	
	set @lowerbound = (@PageNumber  - 1) * @PageSize
	set @higherbound = @PageSize * @PageNumber
	
	insert into @tempView
	select COUNT(u.id) OVER() AS Total,	u.id , d.Name , gl2.Term , gl.Term , gl3.Term 	
	from [User] u		
		inner join Department d on d.id = u.departmentid
		left join Glossary gl on gl.RootId = u.title and gl.LanguageId = @viewinglanguageid and gl.GlossaryGroup = 'Title'
		left join Glossary gl2 on gl2.RootId = u.GenderId and gl2.LanguageId = @viewinglanguageid and gl2.GlossaryGroup = 'Gender'
		left join Glossary gl3 on gl3.RootId = u.preferedlanguageid and gl3.LanguageId = @viewinglanguageid and gl3.GlossaryGroup = 'language' 
		
	where (isnull(@name, '') = '' or (firstname like '%' + @name + '%') or (surname like '%' + @name + '%') or (othernames like '%' + @name + '%'))
		and (isnull(@id, 0) =0 or @id = u.id)
		and (isnull(@usertypeid, 0) = 0 or @usertypeid = u.typeid)
		and (isnull(@departmentid, 0) = 0 or @departmentid = u.departmentid)
		and (ISNULL(@email, '') = '' or (u.email like '%' + @email + '%'))	
		and (ISNULL(@partialaddress, '') = '' or (u.Address1 like '%' + @partialaddress + '%')or (u.Address2 like '%' + @partialaddress + '%') or (u.Address3 like '%' + @partialaddress + '%'))
		and (ISNULL(@town, '') = '' or (u.Town like '%' + @town + '%'))
		and (ISNULL(@county, '') = '' or (u.stateorcounty like '%' + @county + '%'))
		and (ISNULL(@postcode, '') = '' or (u.PostCode like '%' + @postcode + '%'))
		and (ISNULL(@phone, '') = '' or (u.Phone like '%' + @phone + '%'))
	ORDER BY 
    CASE WHEN @sortorder = 'ASC' THEN
      CASE @ordercolumn 
		WHEN '' then u.id
        WHEN @colId  THEN u.id 
        WHEN @colNoOfVisits THEN u.novisits 
      END
    END,
    CASE WHEN @sortorder = 'DESC' THEN
      CASE @ordercolumn 
        WHEN @colId   THEN u.id 
        WHEN @colNoOfVisits THEN u.novisits 
      END
    END DESC,
    CASE WHEN @sortorder = 'ASC' THEN
      CASE @ordercolumn 
        when  @colFirstName then u.firstname
		when  @colSurname then u.surname
		when  @colGender then gl2.Term
		When  @colUsername then u.username
		When  @colEmail then u.email
		When  @colDepartment then d.Name
      END
    END,
    CASE WHEN @sortorder = 'DESC' THEN
      CASE @ordercolumn 
       when  @colFirstName then u.firstname
		when  @colSurname then u.surname
		when  @colGender then gl2.Term
		When  @colUsername then u.username
		When  @colEmail then u.email
		When  @colDepartment then d.Name
      END
    END DESC,
    CASE WHEN @ordercolumn = @colLastVisit  
      AND @sortorder = 'ASC' THEN u.lastvisit 
    END,
    CASE WHEN @ordercolumn = @colLastVisit 
      AND @sortorder = 'DESC' THEN u.lastvisit 
    END DESC;		
	
	select tv.*, u.*	
	from @tempView tv
		inner join [User] u on u.id = tv.UserId		
		where @lowerbound < tv.RowNumber and tv.Rownumber <= @higherbound
	order by tv.RowNumber
End