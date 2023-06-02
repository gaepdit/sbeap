-- use [sbeap-app]

-- Data comes from the SBEAPCLIENTS and SBEAPCLIENTDATA tables.
-- * All strings are trimmed.
-- * County names come from LOOKUPCOUNTYINFORMATION.
-- * State names are substituted.
-- * Postal codes are formatted.
-- * Invalid states and postal codes are ignored.

-- -- Add temporary column to store the old ID.
-- alter table Customers
--     add AirBranchCustomerId int
-- go

-- insert into Customers
--     (Id,
--      Name,
--      County,
--      Location_Street,
--      Location_Street2,
--      Location_City,
--      Location_State,
--      Location_PostalCode,
--      MailingAddress_Street,
--      MailingAddress_Street2,
--      MailingAddress_City,
--      MailingAddress_State,
--      MailingAddress_PostalCode,
--      Description,
--      WebSite,
--      CreatedAt,
--      UpdatedAt,
--      IsDeleted,
--      AirBranchCustomerId)

select newid()                                                     as [Id],
       nullif(trim(c.STRCOMPANYNAME), '')                          as [Name],
       nullif(trim(l.STRCOUNTYNAME), '')                           as [County],
       nullif(trim(c.STRCOMPANYADDRESS), '')                       as [Location_Street],
       nullif(trim(c.STRCOMPANYADDRESS2), '')                      as [Location_Street2],
       nullif(trim(c.STRCOMPANYCITY), '')                          as [Location_City],
       case
           when c.STRCOMPANYSTATE = 'AL' then 'Alabama'
           when c.STRCOMPANYSTATE = 'CA' then 'California'
           when c.STRCOMPANYSTATE = 'CO' then 'Colorado'
           when c.STRCOMPANYSTATE = 'CT' then 'Connecticut'
           when c.STRCOMPANYSTATE = 'DC' then 'District of Columbia'
           when c.STRCOMPANYSTATE = 'FL' then 'Florida'
           when c.STRCOMPANYSTATE = 'GA' then 'Georgia'
           when c.STRCOMPANYSTATE = 'IL' then 'Illinois'
           when c.STRCOMPANYSTATE = 'IN' then 'Indiana'
           when c.STRCOMPANYSTATE = 'KY' then 'Kentucky'
           when c.STRCOMPANYSTATE = 'LA' then 'Louisiana'
           when c.STRCOMPANYSTATE = 'MA' then 'Massachusetts'
           when c.STRCOMPANYSTATE = 'MI' then 'Michigan'
           when c.STRCOMPANYSTATE = 'MN' then 'Minnesota'
           when c.STRCOMPANYSTATE = 'MO' then 'Missouri'
           when c.STRCOMPANYSTATE = 'NC' then 'North Carolina'
           when c.STRCOMPANYSTATE = 'NV' then 'Nevada'
           when c.STRCOMPANYSTATE = 'OH' then 'Ohio'
           when c.STRCOMPANYSTATE = 'Ohio' then 'Ohio'
           when c.STRCOMPANYSTATE = 'PA' then 'Pennsylvania'
           when c.STRCOMPANYSTATE = 'SC' then 'South Carolina'
           when c.STRCOMPANYSTATE = 'TN' then 'Tennessee'
           when c.STRCOMPANYSTATE = 'TX' then 'Texas'
           when c.STRCOMPANYSTATE = 'VA' then 'Virginia'
           when c.STRCOMPANYSTATE = 'WI' then 'Wisconsin'
       end                                                         as [Location_State],
       IIF(len(trim(c.STRCOMPANYZIPCODE)) > 10, null,
           AIRBRANCH.dbo.FormatZipCode(trim(c.STRCOMPANYZIPCODE))) as [Location_PostalCode],
       nullif(trim(c.STRMAILINGADDRESS), '')                       as [MailingAddress_Street],
       nullif(trim(c.STRMAILINGADDRESS2), '')                      as [MailingAddress_Street2],
       nullif(trim(c.STRMAILINGCITY), '')                          as [MailingAddress_City],
       case
           when c.STRMAILINGSTATE = 'AL' then 'Alabama'
           when c.STRMAILINGSTATE = 'CA' then 'California'
           when c.STRMAILINGSTATE = 'CO' then 'Colorado'
           when c.STRMAILINGSTATE = 'CT' then 'Connecticut'
           when c.STRMAILINGSTATE = 'DC' then 'District of Columbia'
           when c.STRMAILINGSTATE = 'FL' then 'Florida'
           when c.STRMAILINGSTATE = 'GA' then 'Georgia'
           when c.STRMAILINGSTATE = 'IL' then 'Illinois'
           when c.STRMAILINGSTATE = 'IN' then 'Indiana'
           when c.STRMAILINGSTATE = 'KY' then 'Kentucky'
           when c.STRMAILINGSTATE = 'LA' then 'Louisiana'
           when c.STRMAILINGSTATE = 'MA' then 'Massachusetts'
           when c.STRMAILINGSTATE = 'MI' then 'Michigan'
           when c.STRMAILINGSTATE = 'MN' then 'Minnesota'
           when c.STRMAILINGSTATE = 'MO' then 'Missouri'
           when c.STRMAILINGSTATE = 'NC' then 'North Carolina'
           when c.STRMAILINGSTATE = 'NV' then 'Nevada'
           when c.STRMAILINGSTATE = 'OH' then 'Ohio'
           when c.STRMAILINGSTATE = 'Ohio' then 'Ohio'
           when c.STRMAILINGSTATE = 'PA' then 'Pennsylvania'
           when c.STRMAILINGSTATE = 'SC' then 'South Carolina'
           when c.STRMAILINGSTATE = 'TN' then 'Tennessee'
           when c.STRMAILINGSTATE = 'TX' then 'Texas'
           when c.STRMAILINGSTATE = 'VA' then 'Virginia'
           when c.STRMAILINGSTATE = 'WI' then 'Wisconsin'
       end                                                         as [MailingAddress_State],
       IIF(len(trim(c.STRMAILINGZIPCODE)) > 10, null,
           AIRBRANCH.dbo.FormatZipCode(trim(c.STRMAILINGZIPCODE))) as [MailingAddress_PostalCode],
       c.DATCOMPANYCREATED at time zone 'Eastern Standard Time'    as [CreatedAt],
       c.DATMODIFINGDATE at time zone 'Eastern Standard Time'      as [UpdatedAt],
       nullif(trim(d.STRCLIENTDESCRIPTION), '')                    as [Description],
       nullif(trim(d.STRCLIENTWEBSITE), '')                        as [WebSite],
       convert(bit, 0)                                             as [IsDeleted],
       convert(int, c.CLIENTID)                                    as [AirBranchCustomerId]

from AIRBRANCH.dbo.SBEAPCLIENTS c
    inner join AIRBRANCH.dbo.SBEAPCLIENTDATA d
    on c.CLIENTID = d.CLIENTID
    left join AIRBRANCH.dbo.LOOKUPCOUNTYINFORMATION l
    on l.STRCOUNTYCODE = c.STRCOMPANYCOUNTY

order by convert(int, c.CLIENTID)
