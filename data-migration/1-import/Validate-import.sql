-- USE [AIRBRANCH]
-- GO

USE [sbeap-app]
GO

select 'SBEAPACTIONLOG' as [table_name], count(*) as [count] from dbo.SBEAPACTIONLOG
union
select 'SBEAPCASELOG' as [table_name], count(*) as [count] from dbo.SBEAPCASELOG
union
select 'SBEAPCASELOGLINK' as [table_name], count(*) as [count] from dbo.SBEAPCASELOGLINK
union
select 'SBEAPCLIENTCONTACTS' as [table_name], count(*) as [count] from dbo.SBEAPCLIENTCONTACTS
union
select 'SBEAPCLIENTDATA' as [table_name], count(*) as [count] from dbo.SBEAPCLIENTDATA
union
select 'SBEAPCLIENTLINK' as [table_name], count(*) as [count] from dbo.SBEAPCLIENTLINK
union
select 'SBEAPCLIENTS' as [table_name], count(*) as [count] from dbo.SBEAPCLIENTS
union
select 'SBEAPCOMPLIANCEASSIST' as [table_name], count(*) as [count] from dbo.SBEAPCOMPLIANCEASSIST
union
select 'SBEAPCONFERENCELOG' as [table_name], count(*) as [count] from dbo.SBEAPCONFERENCELOG
union
select 'SBEAPOTHERLOG' as [table_name], count(*) as [count] from dbo.SBEAPOTHERLOG
union
select 'SBEAPPHONELOG' as [table_name], count(*) as [count] from dbo.SBEAPPHONELOG
union
select 'SBEAPTECHNICALASSIST' as [table_name], count(*) as [count] from dbo.SBEAPTECHNICALASSIST
