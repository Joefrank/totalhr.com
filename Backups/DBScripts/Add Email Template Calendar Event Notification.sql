

Declare @newtemplateid int

if not exists(select 1 from EmailTemplate where Identifier ='CalendarEventNotification')
begin
	insert into EmailTemplate
	select 'Calendar Event Notification', 'Notify user when user invited for event', 'Event Notification: {0}',
	'  Dear {0},    You have been invited for the following event:   {1}.  Please check calendar by going to the Website at {2}    Regards,    {3}',
	-1, 1, 0, GETDATE(), 59, null, null, 'CalendarEventNotification'
	
	select @newtemplateid = @@IDENTITY
	
	if(@newtemplateid > 0)
	begin
		update EmailTemplate set RootId = id where RootId = -1
	end
end
