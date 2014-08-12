
if not exists (select 1 from caleventremindertype where name = N'R5MinsBefore')
begin
insert into caleventremindertype
select      'R5MinsBefore', 5, 1, GETDATE(), 59, null, null, false
end

if not exists (select 1 from caleventremindertype where name = N'R10MinsBefore')
begin
insert into caleventremindertype
select      'R10MinsBefore', 10, 1, GETDATE(), 59, null, null, false
end

if not exists (select 1 from caleventremindertype where name = N'R15MinsBefore')
begin
insert into caleventremindertype
select            'R15MinsBefore', 15, 1, GETDATE(), 59, null, null, false
end

if not exists (select 1 from caleventremindertype where name = N'R20MinsBefore')
begin
insert into caleventremindertype
select            'R20MinsBefore', 20, 1, GETDATE(), 59, null, null, false
end

if not exists (select 1 from caleventremindertype where name = N'R25MinsBefore')
begin
insert into caleventremindertype
select            'R25MinsBefore', 25, 1, GETDATE(), 59, null, null, false
end

if not exists (select 1 from caleventremindertype where name = N'R30MinsBefore')
begin
insert into caleventremindertype
select            'R30MinsBefore', 30 1, GETDATE(), 59, null, null, false
end

if not exists (select 1 from caleventremindertype where name = N'R45MinsBefore')
begin
insert into caleventremindertype
select            'R45MinsBefore', 45, 1, GETDATE(), 59, null, null, false
end

if not exists (select 1 from caleventremindertype where name = N'R1HourBefore')
begin
insert into caleventremindertype
select            'R1HourBefore', 1, 2, GETDATE(), 59, null, null, false
end

if not exists (select 1 from caleventremindertype where name = N'R2HoursBefore')
begin
insert into caleventremindertype
select            'R2HoursBefore', 2, 2, GETDATE(), 59, null, null, false
end

if not exists (select 1 from caleventremindertype where name = N'R4HoursBefore')
begin
insert into caleventremindertype
select            'R4HoursBefore', 4, 2, GETDATE(), 59, null, null, false
end

if not exists (select 1 from caleventremindertype where name = N'R1DayBefore')
begin
insert into caleventremindertype
select            'R1DayBefore', 1, 3, GETDATE(), 59, null, null, false
end

if not exists (select 1 from caleventremindertype where name = N'R2DaysBefore')
begin
insert into caleventremindertype
select            'R2DaysBefore', 2, 3, GETDATE(), 59, null, null, false
end

if not exists (select 1 from caleventremindertype where name = N'R1WeekBefore')
begin
insert into caleventremindertype
select            'R1WeekBefore', 1, 4, GETDATE(), 59, null, null, false
end

if not exists (select 1 from caleventremindertype where name = N'R2WeeksBefore')
begin
insert into caleventremindertype
select            'R2WeeksBefore', 2, 4, GETDATE(), 59, null, null, false
end

if not exists (select 1 from caleventremindertype where name = N'R1MonthBefore')
begin
insert into caleventremindertype
select            'R1MonthBefore', 1, 5, GETDATE(), 59, null, null, false
end

if not exists (select 1 from caleventremindertype where name = N'Customize')
begin
insert into caleventremindertype
select            'Customize', 0, 0, GETDATE(), 59, null, null, false
end        
            
           