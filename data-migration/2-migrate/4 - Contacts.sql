USE [sbeap-app]
GO

-- Data comes from the SBEAPCLIENTCONTACTS and SBEAPCLIENTLINK tables.
-- * Contacts without a link to a Customer (client) are ignored.
-- * All strings are trimmed.
-- * State names are substituted.
-- * Postal codes are formatted.
-- * Invalid states and postal codes are ignored.

-- insert into Contacts
--     (Id,
--      CustomerId,
--      Honorific,
--      GivenName,
--      FamilyName,
--      Title,
--      Email,
--      Notes,
--      Address_Street,
--      Address_City,
--      Address_State,
--      Address_PostalCode,
--      CreatedAt,
--      UpdatedAt,
--      IsDeleted,
--      AirBranchContactId,
--      AirBranchCustomerId)

select newid()                                                    as [Id],
       u.Id                                                       as [CustomerId],
       nullif(trim(c.STRCLIENTSALUTATION), '')                    as [Honorific],
       nullif(trim(c.STRCLIENTFIRSTNAME), '')                     as [GivenName],
       nullif(trim(c.STRCLIENTLASTNAME), '')                      as [FamilyName],
       nullif(trim(c.STRCLIENTTITLE), '')                         as [Title],
       nullif(trim(c.STRCLIENTEMAIL), '')                         as [Email],
       nullif(concat_ws(CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10),
                        nullif(trim(c.STRCLIENTCREDENTIALS), ''),
                        nullif(trim(c.STRCONTACTNOTES), '')), '') as [Notes],
       nullif(trim(c.STRCLIENTADDRESS), '')                       as [Address_Street],
       nullif(trim(c.STRCLIENTCITY), '')                          as [Address_City],
       case
           when c.STRCLIENTSTATE = 'AL' then 'Alabama'
           when c.STRCLIENTSTATE = 'CA' then 'California'
           when c.STRCLIENTSTATE = 'CO' then 'Colorado'
           when c.STRCLIENTSTATE = 'CT' then 'Connecticut'
           when c.STRCLIENTSTATE = 'DC' then 'District of Columbia'
           when c.STRCLIENTSTATE = 'FL' then 'Florida'
           when c.STRCLIENTSTATE = 'GA' then 'Georgia'
           when c.STRCLIENTSTATE = 'IL' then 'Illinois'
           when c.STRCLIENTSTATE = 'IN' then 'Indiana'
           when c.STRCLIENTSTATE = 'KY' then 'Kentucky'
           when c.STRCLIENTSTATE = 'LA' then 'Louisiana'
           when c.STRCLIENTSTATE = 'MA' then 'Massachusetts'
           when c.STRCLIENTSTATE = 'MI' then 'Michigan'
           when c.STRCLIENTSTATE = 'MN' then 'Minnesota'
           when c.STRCLIENTSTATE = 'MO' then 'Missouri'
           when c.STRCLIENTSTATE = 'NC' then 'North Carolina'
           when c.STRCLIENTSTATE = 'NV' then 'Nevada'
           when c.STRCLIENTSTATE = 'OH' then 'Ohio'
           when c.STRCLIENTSTATE = 'Ohio' then 'Ohio'
           when c.STRCLIENTSTATE = 'PA' then 'Pennsylvania'
           when c.STRCLIENTSTATE = 'SC' then 'South Carolina'
           when c.STRCLIENTSTATE = 'TN' then 'Tennessee'
           when c.STRCLIENTSTATE = 'TX' then 'Texas'
           when c.STRCLIENTSTATE = 'VA' then 'Virginia'
           when c.STRCLIENTSTATE = 'WI' then 'Wisconsin'
       end                                                        as [Address_State],
       dbo.FormatZipCode(trim(c.STRCLIENTZIPCODE))      as [Address_PostalCode],
       c.DATCLIENTCREATED at time zone 'Eastern Standard Time'    as [CreatedAt],
       c.DATMODIFINGDATE at time zone 'Eastern Standard Time'     as [UpdatedAt],
       convert(bit, 0)                                            as [IsDelete],
       convert(int, c.CLIENTCONTACTID)                            as [AirBranchContactId],
       convert(int, l.CLIENTID)                                   as [AirBranchCustomerId]

from dbo.SBEAPCLIENTCONTACTS c
    left join dbo.SBEAPCLIENTLINK l
    on c.CLIENTCONTACTID = l.CLIENTCONTACTID
    inner join Customers u
    on u.AirBranchCustomerId = l.CLIENTID
where l.CLIENTCONTACTID is not null
  and l.CLIENTID is not null

order by convert(int, c.CLIENTCONTACTID)
