
CREATE proc [dbo].[GetUserProfiles]
	@userid int
as
begin
	select p.*
	from userprofile up
	inner join [profile] p
		on p.id = up.profileid
	where up.userid = @userid
end