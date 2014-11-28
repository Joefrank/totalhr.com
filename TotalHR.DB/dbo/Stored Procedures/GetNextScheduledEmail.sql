

CREATE proc GetNextScheduledEmail
 @timeoffset int
As
Begin
	declare @id int, @NotificationtypeId int, @Title nvarchar(1000),
	@body nvarchar(3000), @RecipientListId int, @ScheduledSendDate datetime,
	@SenderName nvarchar(100), @SenderEmail nvarchar(150), @DurationToStart int 
	
	select top 1 
	@id = Id, @NotificationtypeId = NotificationTypeId, @Title = MessageTitle,
	@body = MessageBody, @RecipientListId = RecipientListId, 
	@ScheduledSendDate = scheduledsenddate, @SenderName = sendername,
	@SenderEmail = SenderEmail, @DurationToStart = DATEDIFF(minute,GETDATE() ,scheduledsenddate ) 
	from ScheduledNotifications
	where DATEDIFF(minute, GETDATE(), scheduledsenddate) <= @timeoffset
	and StatusId = 1
	order by DATEDIFF(minute, GETDATE(), scheduledsenddate)
	
	select @id as scheduleid, @NotificationtypeId as notificationtype, 
		@Title as MessageTitle,@body as MessageBody, @RecipientListId as RecipientListId, 
		@ScheduledSendDate as SendDate, @SenderName as SenderName, @SenderEmail as SenderEmail, 
		@DurationToStart as DurationToStart
		
	select *
	from Recipient
	where RecipientListId = @RecipientListId

End