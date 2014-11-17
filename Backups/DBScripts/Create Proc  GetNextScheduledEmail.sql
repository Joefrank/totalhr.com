
create proc GetNextScheduledEmail
	@minuteoffset int
as
begin

declare @RecipientListid int, @NoMinutesBeforeSend int, @MessageTitle nvarchar(1000), @MessageBody nvarchar(3000),
@ScheduleSendDate datetime, @SenderEmail nvarchar(150), @SenderName nvarchar(100)

	select top 1 @NoMinutesBeforeSend = DATEDIFF(MINUTE, GETDATE(),scheduledsenddate), @MessageTitle =MessageTitle, 
	@MessageBody =MessageBody, @RecipientListid = RecipientListid,
	@ScheduleSendDate =ScheduledSendDate, @SenderEmail =SenderEmail, @SenderName =SenderName
	from ScheduledNotifications
	where DATEDIFF(MINUTE, GETDATE(),scheduledsenddate) between 0 and @minuteoffset
	order by scheduledsenddate

	if @RecipientListid > 0
	begin
		select recipientuserid, RecipientEmail/* , RecipientName to be added to table */, Recipientphone 
		from Recipient
		where RecipientListid = @RecipientListid
	end

end

