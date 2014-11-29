create proc GetUserRoleIds
	@userid int
as
begin
select id 
from userrole ur
inner join roles r 
	on r.id = ur.roleid
where ur.userid = @userid
end