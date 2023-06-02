-- use [sbeap-app]

-- Data comes from the SBEAPCASELOG and SBEAPCASELOGLINK tables.
-- * All strings are trimmed.

-- -- Add temporary columns to store the old IDs.
-- alter table Cases
--     add AirBranchCaseId int
-- go
-- alter table Cases
--     add AirBranchCustomerId int
-- go

-- insert into Cases
--     (Id,
--      CustomerId,
--      Description,
--      CaseOpenedDate,
--      CaseClosedDate,
--      ReferralAgencyId,
--      ReferralDate,
--      ReferralNotes,
--      UpdatedAt,
--      IsDeleted,
--      AirBranchCaseId,
--      AirBranchCustomerId)

select newid()                                 as [Id],
--        u.Id                                    as [CustomerId],
       isnull(trim(c.STRCASESUMMARY), '')      as [Description],
       convert(date, c.DATCASEOPENED)          as [CaseOpenedDate],
       convert(date, c.DATCASECLOSED)          as [CaseClosedDate],
       c.STRINTERAGENCY                        as [TempAgencyName],
--        a.Id                                    as [ReferralAgencyId],
       convert(date, c.DATREFERRALDATE)        as [ReferralDate],
       isnull(trim(c.STRREFERRALCOMMENTS), '') as [ReferralNotes],
       c.DATMODIFINGDATE at time zone 'Eastern Standard Time'
                                               as [UpdatedAt],
       convert(bit, 0)                         as [IsDeleted],
       convert(int, c.NUMCASEID)               as [AirBranchCaseId],
       convert(int, l.CLIENTID)                as [AirBranchCustomerId]

from AIRBRANCH.dbo.SBEAPCASELOG c
    left join AIRBRANCH.dbo.SBEAPCASELOGLINK l
    on c.NUMCASEID = l.NUMCASEID
--     inner join Customers u
--     on l.CLIENTID = u.AirBranchCustomerId
--     left join Agencies a
--     on c.STRINTERAGENCY = a.Name
where l.NUMCASEID is not null

union

select newid()                                 as [Id],
--        u.Id                                    as [CustomerId],
       isnull(trim(c.STRCASESUMMARY), '')      as [Description],
       convert(date, c.DATCASEOPENED)          as [CaseOpenedDate],
       convert(date, c.DATCASECLOSED)          as [CaseClosedDate],
       c.STRINTERAGENCY                        as [TempAgencyName],
--        a.Id                                    as [ReferralAgencyId],
       convert(date, c.DATREFERRALDATE)        as [ReferralDate],
       isnull(trim(c.STRREFERRALCOMMENTS), '') as [ReferralNotes],
       c.DATMODIFINGDATE                       as [UpdatedAt],
       convert(bit, 0)                         as [IsDeleted],
       convert(int, c.NUMCASEID)               as [AirBranchCaseId],
       convert(int, l.CLIENTID)                as [AirBranchCustomerId]

from AIRBRANCH.dbo.SBEAPCASELOG c
    left join AIRBRANCH.dbo.SBEAPCASELOGLINK l
    on c.NUMCASEID = l.NUMCASEID
--     inner join Customers u
--     on c.CLIENTID = u.AirBranchCustomerId
--     left join Agencies a
--     on c.STRINTERAGENCY = a.Name
where l.NUMCASEID is null
  and c.CLIENTID is not null

order by AirBranchCaseId
