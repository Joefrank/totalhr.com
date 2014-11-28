create proc GetUserProfileIds
	@userid int
as
begin
	select p.id
	from userprofile up
	inner join profile p
		on p.id = up.profileid
	where up.userid = @userid
end