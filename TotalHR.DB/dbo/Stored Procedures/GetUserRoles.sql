CREATE proc [dbo].[GetUserRoles]
	@userid int
as
begin
select r.*
from userrole ur
inner join roles r 
	on r.id = ur.roleid
where ur.userid = @userid
end