
Create Proc PrepareCalendarEventScheduledReminder
 @SenderName nvarchar(100), -- send from current app
 @SenderEmail nvarchar(150), -- send from current app
 @RecipientListId int -- send from current app
as
begin

declare @EventId int
declare @associationid int
declare @AssociationValue nvarchar(2000)
declare @Eventtitle nvarchar(1000)
declare @Description nvarchar(4000)
declare @location nvarchar(50)
declare @startofevent datetime
declare @endofevent datetime
declare @reminderStartDate datetime
declare @remindertype int
declare @frequencytypeid int
declare @frequency int
declare @notificationtype int


declare @ReminderTempTab as Table(AssociationId int, EventId int, CalendarId int, CreatorId int, AssociationValue nvarchar(max),
	EventTitle nvarchar(100), [Description] nvarchar(500), Location nvarchar(50), StartofEvent datetime, EndofEvent datetime)

declare @tempxmltable as Table(val xml)
declare @temp varchar(2000)

insert into @ReminderTempTab	
select ca.EventAssociationId, ca.EventId, ce.CalendarId, ce.CreatedBy, ca.AssociationValue, 
	ce.Title, ce.[Description], ce.Location, ce.StartOfEvent, ce.EndOfEvent
from CalendarAssociation ca
inner join CalendarEvent ce on ce.id = ca.EventId
left join ScheduledNotifications sn on sn.ObjectId = ca.EventAssociationId
where AssociationTypeid = 2 and sn.ObjectId is null -- reminders only

	-- take next row from the pool
	while ((select COUNT(associationid) from @ReminderTempTab) > 0)
	Begin

		select top 1  @associationid = AssociationId ,@EventId = EventId,@AssociationValue = AssociationValue, @Eventtitle = EventTitle, @Description = [Description],
		@location = Location, @startofevent = StartofEvent, @endofevent = EndofEvent
		from @ReminderTempTab

		-- remove processed row
		delete from @ReminderTempTab where AssociationId = @associationid

		insert into @tempxmltable select CAST(@AssociationValue as XML)

		SELECT 
			@remindertype = val.value('(/reminder/type)[1]', 'int'), 
			@frequencytypeid = val.value('(/reminder/frequencytype)[1]', 'int'), 
			@frequency = val.value('(/reminder/frequency)[1]', 'int'),
			@notificationtype = val.value('(/reminder/notification)[1]', 'int')
		FROM
			@tempxmltable		    
		   
		   select    
		   @frequencytypeid = (case when @remindertype <> 16 then frequencytype else @frequencytypeid end) , 
		   @frequency = (case when @remindertype <> 16 then frequency else @frequency end) 
		   from CalEventReminderType
		   where id = @remindertype

		
		select @reminderStartDate =
		 case when @frequencytypeid = 1 then DateAdd(minute, -1 * @frequency, @startofevent) 
		 when @frequencytypeid = 2 then DateAdd(HOUR, -1 * @frequency, @startofevent)
		 when @frequencytypeid = 3 then DateAdd(DAY, -1 * @frequency, @startofevent)  
		 when @frequencytypeid = 4 then DateAdd(WEEK, -1 * @frequency, @startofevent)
		 when @frequencytypeid = 5 then DateAdd(MONTH, -1 * @frequency, @startofevent)
		 end

		-- insert schedules task
		insert into scheduledNotifications
		select 257, @notificationtype, @associationid, @Description, @Eventtitle, @RecipientListId, @reminderStartDate,1,GETDATE(),
			@SenderName, @SenderEmail

	End

End