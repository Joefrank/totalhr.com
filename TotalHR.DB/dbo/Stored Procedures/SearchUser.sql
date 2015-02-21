	
CREATE proc [dbo].[SearchUser]
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
	@viewinglanguageid int = 1
As
Begin
	select distinct gl2.Term as GenderTerm, gl.Term as TitleTerm, 
	d.Name as DepartmentName, gl3.Term as PreferredLanguageTerm, u.* 
	from [User] u
	inner join Glossary gl on gl.RootId = u.title and gl.LanguageId = @viewinglanguageid and gl.GlossaryGroup = 'Title'
	inner join Glossary gl2 on gl2.RootId = u.GenderId and gl2.LanguageId = @viewinglanguageid and gl2.GlossaryGroup = 'Gender'
	left join Glossary gl3 on gl3.RootId = u.preferedlanguageid and gl3.LanguageId = @viewinglanguageid and gl3.GlossaryGroup = 'language' 
	inner join Department d on d.id = u.departmentid
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
End