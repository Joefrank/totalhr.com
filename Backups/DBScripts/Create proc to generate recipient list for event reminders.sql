
alter proc BuildCalEventReminderRecipientList

 @eventid int,  -- feed as parameter from proc
 @companyid int, -- feed as param
 @RecipientListName nvarchar(250), 
 @description nvarchar(500),
 @CreatedBy int   
 
As
Begin

-- declaration
declare @eventtargexml as xml
declare @targetypeid int
declare @val nvarchar(max)
declare @creatorId int
declare @currentemail nvarchar(150)
declare @currentmobile nvarchar(20)
declare @recipientListId int


-- create recipient list first
if not exists(select 1 from RecipientList where eventid = @eventid)
begin

insert into RecipientList
select @RecipientListName, @description, GETDATE(), @createdby, null, null, @eventid

select @recipientListId = @@IDENTITY

select @creatorId = u.id, @currentemail = u.email, @currentmobile = ISNULL(u.Mobile, u.Phone) 
from CalendarEvent ca 
inner join [User]  u on ca.CreatedBy = u.id
where ca.id = @eventid 

select @eventtargexml = cast(AssociationValue as XML)
from CalendarAssociation
where EventId = @eventid

select @targetypeid = feed.x.value('.','int')
from @eventtargexml.nodes('//target/type') feed(x)

select @val = feed.x.value('.','nvarchar(max)')
from @eventtargexml.nodes('//target/value') feed(x)

select @targetypeid

if not @targetypeid is null
begin
	if @targetypeid = 251 -- company
	begin
		insert into Recipient
		select u.id, u.email, ISNULL(u.Mobile, u.Phone),  0, null, @recipientListId
		from [User]  u
		where u.CompanyId = @companyid and u.active = 1
	end
	else if @targetypeid = 252 -- department
	begin
		declare @tempdept as table(departmentid int)
		insert into @tempdept
		select * from [dbo].[SplitCSV](@val, ',')
		
		insert into Recipient
		select distinct u.id, u.email, ISNULL(u.Mobile, u.Phone),  0, null, @recipientListId
		from [User]  u
		inner join @tempdept tt on tt.departmentid = u.departmentid
		inner join Department dp on dp.id = tt.departmentid
		where u.active = 1 
		
		if not exists (select 1 from Recipient where RecipientUserId = @CreatedBy and RecipientListId = @recipientListId)
		begin
			insert into Recipient
			select u.id, u.email, ISNULL(u.Mobile, u.Phone),  0, null, @recipientListId
			from [User] u where id = @CreatedBy and active = 1
		end
		
	end
	else if @targetypeid = 253 -- User
	begin
		declare @temp as table(userid int)

		insert into @temp
		select * from [dbo].[SplitCSV](@val, ',')
		-- add creator to list
		insert into @temp
		select @CreatedBy
		
		insert into Recipient
		select distinct u.id, u.email, ISNULL(u.Mobile, u.Phone),  0, null, @recipientListId
		from [User]  u
		inner join @temp tt on tt.userid = u.id
		where u.active = 1 
		
	end
	else if @targetypeid = 254 -- MyselfOnly
	begin
		
		insert into Recipient
		select u.id, u.email, ISNULL(u.Mobile, u.Phone),  0, null, @recipientListId
		from [User]  u 
		where u.id = @CreatedBy and u.active = 1		
		
	end
	
	
end

End
	select @recipientListId as RecipientListId
	
end