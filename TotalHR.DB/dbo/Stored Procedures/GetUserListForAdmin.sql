

create proc [dbo].[GetUserListForAdmin]
	@showactive bit = null,
	@viewinglanguageid int 
as
begin
	select distinct gl2.Term as GenderTerm, gl.Term as TitleTerm, 
	d.Name as DepartmentName, gl3.Term as PreferredLanguageTerm, u.* 
	from [User] u
	inner join Glossary gl on gl.RootId = u.title and gl.LanguageId = @viewinglanguageid and gl.GlossaryGroup = 'Title'
	inner join Glossary gl2 on gl2.RootId = u.GenderId and gl2.LanguageId = @viewinglanguageid and gl2.GlossaryGroup = 'Gender'
	left join Glossary gl3 on gl3.RootId = u.preferedlanguageid and gl3.LanguageId = @viewinglanguageid and gl3.GlossaryGroup = 'language' 
	inner join Department d on d.id = u.departmentid
	where	(@showactive is null or active = @showactive)
end