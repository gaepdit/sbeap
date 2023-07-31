USE [AIRBRANCH]
GO

USE [Sbeap]
GO

select 'SBEAPACTIONLOG'            as [table_name],
       count(*)                    as [count],
       count(distinct NUMACTIONID) as [count_distinct]
from dbo.SBEAPACTIONLOG
union
select 'SBEAPCASELOG'            as [table_name],
       count(*)                  as [count],
       count(distinct NUMCASEID) as [count_distinct]
from dbo.SBEAPCASELOG
union
select 'SBEAPCASELOGLINK'                                  as [table_name],
       count(*)                                            as [count],
       count(distinct concat_ws('-', NUMCASEID, CLIENTID)) as [count_distinct]
from dbo.SBEAPCASELOGLINK
union
select 'SBEAPCLIENTCONTACTS'           as [table_name],
       count(*)                        as [count],
       count(distinct CLIENTCONTACTID) as [count_distinct]
from dbo.SBEAPCLIENTCONTACTS
union
select 'SBEAPCLIENTDATA'        as [table_name],
       count(*)                 as [count],
       count(distinct CLIENTID) as [count_distinct]
from dbo.SBEAPCLIENTDATA
union
select 'SBEAPCLIENTLINK'                                         as [table_name],
       count(*)                                                  as [count],
       count(distinct concat_ws('-', CLIENTID, CLIENTCONTACTID)) as [count_distinct]
from dbo.SBEAPCLIENTLINK
union
select 'SBEAPCLIENTS'           as [table_name],
       count(*)                 as [count],
       count(distinct CLIENTID) as [count_distinct]
from dbo.SBEAPCLIENTS
union
select 'SBEAPCOMPLIANCEASSIST'     as [table_name],
       count(*)                    as [count],
       count(distinct NUMACTIONID) as [count_distinct]
from dbo.SBEAPCOMPLIANCEASSIST
union
select 'SBEAPCONFERENCELOG'        as [table_name],
       count(*)                    as [count],
       count(distinct NUMACTIONID) as [count_distinct]
from dbo.SBEAPCONFERENCELOG
union
select 'SBEAPOTHERLOG'             as [table_name],
       count(*)                    as [count],
       count(distinct NUMACTIONID) as [count_distinct]
from dbo.SBEAPOTHERLOG
union
select 'SBEAPPHONELOG'             as [table_name],
       count(*)                    as [count],
       count(distinct NUMACTIONID) as [count_distinct]
from dbo.SBEAPPHONELOG
union
select 'SBEAPTECHNICALASSIST'      as [table_name],
       count(*)                    as [count],
       count(distinct NUMACTIONID) as [count_distinct]
from dbo.SBEAPTECHNICALASSIST


select 'ActionItems' as [table_name], count(*) as [count] from dbo.ActionItems
union
select 'ActionItemTypes' as [table_name], count(*) as [count] from dbo.ActionItemTypes
union
select 'Agencies' as [table_name], count(*) as [count] from dbo.Agencies
union
select 'AspNetRoleClaims' as [table_name], count(*) as [count] from dbo.AspNetRoleClaims
union
select 'AspNetRoles' as [table_name], count(*) as [count] from dbo.AspNetRoles
union
select 'AspNetUserClaims' as [table_name], count(*) as [count] from dbo.AspNetUserClaims
union
select 'AspNetUserLogins' as [table_name], count(*) as [count] from dbo.AspNetUserLogins
union
select 'AspNetUserRoles' as [table_name], count(*) as [count] from dbo.AspNetUserRoles
union
select 'AspNetUsers' as [table_name], count(*) as [count] from dbo.AspNetUsers
union
select 'AspNetUserTokens' as [table_name], count(*) as [count] from dbo.AspNetUserTokens
union
select 'Cases' as [table_name], count(*) as [count] from dbo.Cases
union
select 'Contacts' as [table_name], count(*) as [count] from dbo.Contacts
union
select 'Customers' as [table_name], count(*) as [count] from dbo.Customers
union
select 'Offices' as [table_name], count(*) as [count] from dbo.Offices
union
select 'PhoneNumber' as [table_name], count(*) as [count] from dbo.PhoneNumber
