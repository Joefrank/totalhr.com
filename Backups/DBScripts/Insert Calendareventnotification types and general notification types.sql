

-- use identity insert for this script to be sure of having 255 and 256 as ids.
-- these values will go unto notificationtypeid in Schedulednotifications and Generaltypeid will be to describe if it is
--a  calendar event reminder or any another notification

SET IDENTITY_INSERT Glossary ON

if not exists(select 1 from Glossary where Term ='ByEmail' and GlossaryGroup = 'CalendarEventNotificationType')
begin
	insert into Glossary(Id, Term, RootId, LanguageId, CreatedBy, CreatedOn, LastUpdatedOn, LastUpdatedBy,Obsolete, GlossaryGroup,GroupOrder)
	select 255, 'ByEmail', 255, 1, 59, GETDATE(), null,null, 0, 'CalendarEventNotificationType', 1
end

if not exists(select 1 from Glossary where Term ='ByTextPhone' and GlossaryGroup = 'CalendarEventNotificationType')
begin
	insert into Glossary(Id, Term, RootId, LanguageId, CreatedBy, CreatedOn, LastUpdatedOn, LastUpdatedBy,Obsolete, GlossaryGroup,GroupOrder)
	select 256, 'ByTextPhone', 256, 1, 59, GETDATE(), null,null, 0, 'CalendarEventNotificationType', 2
end

SET IDENTITY_INSERT Glossary OFF

-- generic notification types
if not exists(select 1 from Glossary where Term ='Calendar Event Reminder' and GlossaryGroup = 'GenericNotificationType')
begin
	insert into Glossary
	select  'Calendar Event Reminder', -1, 1, 59, GETDATE(), null,null, 0, 'GenericNotificationType', 1
end

-- Notification statuses
if not exists(select 1 from Glossary where Term ='New' and GlossaryGroup = 'NotificationStatus')
begin
	insert into Glossary
	select  'New', -1, 1, 59, GETDATE(), null,null, 0, 'NotificationStatus', 1
end

if not exists(select 1 from Glossary where Term ='Sent' and GlossaryGroup = 'NotificationStatus')
begin
	insert into Glossary
	select  'Sent', -1, 1, 59, GETDATE(), null,null, 0, 'NotificationStatus', 2
end

if not exists(select 1 from Glossary where Term ='Needs Resending' and GlossaryGroup = 'NotificationStatus')
begin
	insert into Glossary
	select  'Needs Resending', -1, 1, 59, GETDATE(), null,null, 0, 'NotificationStatus', 3
end

if not exists(select 1 from Glossary where Term ='Failed' and GlossaryGroup = 'NotificationStatus')
begin
	insert into Glossary
	select  'Failed', -1, 1, 59, GETDATE(), null,null, 0, 'NotificationStatus', 4
end

update Glossary set RootId = ID where RootId = -1

