
use master

ALTER DATABASE totalhr
SET SINGLE_USER WITH
ROLLBACK AFTER 60 --this will give your current connections 60 seconds to complete

declare @backuplocation nvarchar(255), @sqlmdffile nvarchar(255), @sqlldffile nvarchar(255)
set @backuplocation = N'D:\Projects\MyGithub\totalhr.com\Backups\DatabaseBackups\totalhr_11082014.bak'
set @sqlmdffile = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESSPC1003\MSSQL\DATA\totalhr.mdf'
set @sqlldffile = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESSPC1003\MSSQL\DATA\totalhr.ldf'

--Do Actual Restore
RESTORE DATABASE totalhr
FROM DISK = @backuplocation
WITH MOVE 'totalhr' TO @sqlmdffile,
MOVE 'totalhr' TO @sqlldffile

/*If there is no error in statement before database will be in multiuser
mode.  If error occurs please execute following command it will convert
database in multi user.*/
ALTER DATABASE totalhr SET MULTI_USER
GO