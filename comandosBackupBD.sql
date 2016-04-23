BACKUP DATABASE FitocracyDB
TO DISK = 'C:\Users\karol\Desktop\FitocracyDBBackup\Copia_FitocracyDB.bak'
   WITH FORMAT;
GO

RESTORE FILELISTONLY
FROM DISK = 'C:\Users\karol\Desktop\FitocracyDBBackup\Copia_FitocracyDB.bak';


RESTORE DATABASE FitocracyDBBackup
   FROM DISK = 'C:\Users\karol\Desktop\FitocracyDBBackup\Copia_FitocracyDB.bak'
   WITH
   MOVE 'FitocracyDB' TO 'C:\Users\karol\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\ProjectsV12\FitocracyDB.mdf',
  MOVE 'FitocracyDB_log' TO 'C:\Users\karol\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\ProjectsV12\FitocracyDB.ldf'; 