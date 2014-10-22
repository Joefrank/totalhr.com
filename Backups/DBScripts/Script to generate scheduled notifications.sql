


--GET the frequencytype of reminder

-- based on frequency, get duration

-- get event details

-- compute time to duration of reminder and start of event

-- drop entry into scheduled task table (check notification type)

-- service will then read from that table to send notification

-- create email template for reminders


declare @EventId int
declare @associationid int
declare @AssociationValue nvarchar(2000)
declare @Eventtitle nvarchar(1000)
declare @Description nvarchar(4000)
declare @location nvarchar(50)
declare @startofevent datetime
declare @endofevent datetime
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
where AssociationTypeid = 2 -- reminders only

-- take next row from the pool
select top 1  @associationid = AssociationId ,@EventId = EventId,@AssociationValue = AssociationValue, @Eventtitle = EventTitle, @Description = Description,
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
    
   
   select @remindertype, @frequencytypeid, @frequency, @notificationtype
   
   return;
   
--select 257,


--insert into scheduledNotifications
 -- <reminder><type>3</type><frequencytype>0</frequencytype><frequency>0</frequency><notification>1</notification></reminder>

--select * from Glossary
--select * from scheduledNotifications  
--select * from CalendarAssociation

--select * from Glossary
