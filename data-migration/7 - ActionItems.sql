-- use [sbeap-app]

-- Data comes from the SBEAPACTIONLOG and LOOKUPSBEAPCASEWORK tables.
-- * Notes come from the subtables:
--   * SBEAPCOMPLIANCEASSIST
--   * SBEAPCONFERENCELOG
--   * SBEAPOTHERLOG
--   * SBEAPPHONELOG
--   * SBEAPTECHNICALASSIST

-- insert into ActionItems
--     (Id,
--      CaseworkId,
--      ActionItemTypeId,
--      ActionDate,
--      Notes,
--      EnteredOn,
--      CreatedAt,
--      UpdatedAt,
--      IsDeleted)

-- Compliance Assistance
select newid(),
       a.NUMCASEID                                            as [TempCaseId],
--        c.Id                                                   as [CaseworkId],
       l.STRWORKDESCRIPTION                                   as [TempActionItemType],
--        t.Id                                                   as [ActionItemTypeId],
       convert(date, a.DATACTIONOCCURED)                      as [ActionDate],
       concat_ws
           (CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10),
            concat_ws
                (': ',
                 'Compliance Assistance',
                 nullif
                     (concat_ws
                          (', ',
                           iif(n.STRAIRASSIST = 'True', 'Air', null),
                           iif(n.STRSTORMWATERASSIST = 'True', 'Storm Water', null),
                           iif(n.STRHAZWASTEASSIST = 'True', 'Hazardous Waste', null),
                           iif(n.STRSOLIDWASTEASSIST = 'True', 'Solid Waste', null),
                           iif(n.STRUSTASSIST = 'True', 'UST', null),
                           iif(n.STRSCRAPTIREASSIST = 'True', 'Scrap Tire', null),
                           iif(n.STRLEADASSIST = 'True', 'Lead and Asbestos', null),
                           iif(n.STROTHERASSIST = 'True', 'Other', null)
                          ), ''
                     )
                ),
            nullif(trim(n.STRCOMMENTS), '')
           )                                                  as [Notes],
       a.DATCREATIONDATE at time zone 'Eastern Standard Time' as [EnteredOn],
       a.DATCREATIONDATE at time zone 'Eastern Standard Time' as [CreatedAt],
       n.DATMODIFINGDATE at time zone 'Eastern Standard Time' as [UpdatedAt],
       convert(bit, 0)                                        as [IsDelete]
from AIRBRANCH.dbo.SBEAPACTIONLOG a
    inner join AIRBRANCH.dbo.LOOKUPSBEAPCASEWORK l
    on a.NUMACTIONTYPE = l.NUMACTIONTYPE
    inner join AIRBRANCH.dbo.SBEAPCOMPLIANCEASSIST n
    on a.NUMACTIONID = n.NUMACTIONID
    --     inner join Cases c
--     on convert(int, a.NUMCASEID) = c.AirBranchCaseId
--     left join ActionItemTypes t
--     on t.Name = l.STRWORKDESCRIPTION

-- union

-- Conferences
select newid(),
       a.NUMCASEID                                            as [TempCaseId],
--        c.Id                                                   as [CaseworkId],
       l.STRWORKDESCRIPTION                                   as [TempActionItemType],
--        t.Id                                                   as [ActionItemTypeId],
       convert(date, a.DATACTIONOCCURED)                      as [ActionDate],
       concat_ws
           (CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10),
            iif(n.STRCONFERENCEATTENDED is null, null,
                'Meeting/Conference Attended: ' + CHAR(13) + CHAR(10) + n.STRCONFERENCEATTENDED),
            iif(n.STRCONFERENCELOCATION is null, null,
                'Location: ' + CHAR(13) + CHAR(10) + n.STRCONFERENCELOCATION),
            iif(n.STRCONFERENCETOPIC is null, null,
                'Meeting/Conference Topic: ' + CHAR(13) + CHAR(10) + n.STRCONFERENCETOPIC),
           -- TODO: If possible, include concatenated list of attending staff.
            nullif(concat_ws
                       (' ',
                        iif(n.DATCONFERENCESTARTED is null, null,
                            'Date Started: ' + format(n.DATCONFERENCESTARTED, 'd-MMM-yyyy') + '.'),
                        iif(n.DATCONFERENCEENDED is null, null,
                            'Date Ended: ' + format(n.DATCONFERENCEENDED, 'd-MMM-yyyy') + '.')
                       ), ''),
            iif(n.STRSBEAPPRESENTATION is null, null,
                'Presentation given by SBEAP: ' + iif(n.STRSBEAPPRESENTATION = 'True', 'Yes', 'No')),
            iif(n.STRATTENDEES is null, null, 'Number of attendees: ' + trim(n.STRATTENDEES)),
            iif(n.STRLISTOFBUSINESSSECTORS is null, null,
                'List of Business Sectors or Organizations Present: ' + CHAR(13) + CHAR(10) +
                n.STRLISTOFBUSINESSSECTORS),
            iif(n.STRCONFERENCEFOLLOWUP is null, null,
                'Description of follow-up activities as a result of this presentation: ' + CHAR(13) + CHAR(10) +
                n.STRCONFERENCEFOLLOWUP)
           )                                                  as [Notes],
       a.DATCREATIONDATE at time zone 'Eastern Standard Time' as [EnteredOn],
       a.DATCREATIONDATE at time zone 'Eastern Standard Time' as [CreatedAt],
       n.DATMODIFINGDATE at time zone 'Eastern Standard Time' as [UpdatedAt],
       convert(bit, 0)                                        as [IsDelete]
from AIRBRANCH.dbo.SBEAPACTIONLOG a
    inner join AIRBRANCH.dbo.LOOKUPSBEAPCASEWORK l
    on a.NUMACTIONTYPE = l.NUMACTIONTYPE
    inner join AIRBRANCH.dbo.SBEAPCONFERENCELOG n
    on a.NUMACTIONID = n.NUMACTIONID
    --     inner join Cases c
--     on convert(int, a.NUMCASEID) = c.AirBranchCaseId
--     left join ActionItemTypes t
--     on t.Name = l.STRWORKDESCRIPTION

-- union

-- Phone Log
select newid(),
       a.NUMCASEID                                            as [TempCaseId],
--        c.Id                                                   as [CaseworkId],
       l.STRWORKDESCRIPTION                                   as [TempActionItemType],
--        t.Id                                                   as [ActionItemTypeId],
       convert(date, a.DATACTIONOCCURED)                      as [ActionDate],
       concat_ws
           (CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10),
            iif(n.STRCALLERINFORMATION is null, null, 'Caller: ' + n.STRCALLERINFORMATION),
            iif(n.NUMCALLERPHONENUMBER is null, null,
                'Phone #: ' + dbo.FormatOnlyValidPhoneNumber(str(n.NUMCALLERPHONENUMBER, 16, 0))),
            nullif(trim(n.STRPHONELOGNOTES), '')
           )                                                  as [Notes],
       a.DATCREATIONDATE at time zone 'Eastern Standard Time' as [EnteredOn],
       a.DATCREATIONDATE at time zone 'Eastern Standard Time' as [CreatedAt],
       convert(date, n.DATMODIFINGDATE)                       as [UpdatedAt],
       convert(bit, 0)                                        as [IsDelete]
from AIRBRANCH.dbo.SBEAPACTIONLOG a
    inner join AIRBRANCH.dbo.LOOKUPSBEAPCASEWORK l
    on a.NUMACTIONTYPE = l.NUMACTIONTYPE
    inner join AIRBRANCH.dbo.SBEAPPHONELOG n
    on a.NUMACTIONID = n.NUMACTIONID
    --     inner join Cases c
--     on convert(int, a.NUMCASEID) = c.AirBranchCaseId
--     left join ActionItemTypes t
--     on t.Name = l.STRWORKDESCRIPTION

union

-- Permit Assistance
select newid(),
       a.NUMCASEID                                            as [TempCaseId],
--        c.Id                                                   as [CaseworkId],
       l.STRWORKDESCRIPTION                                   as [TempActionItemType],
--        t.Id                                                   as [ActionItemTypeId],
       convert(date, a.DATACTIONOCCURED)                      as [ActionDate],
       concat_ws
           (CHAR(13) + CHAR(10) + CHAR(13) + CHAR(10),
            'Permit Assistance',
            nullif(concat_ws
                       (CHAR(13) + CHAR(10),
                        iif(nullif(n.STRTECHNICALASSISTTYPE, '') is null, null,
                            'Assist Level: ' + n.STRTECHNICALASSISTTYPE),
                        iif(n.DATINITIALCONTACTDATE is null, null,
                            'Initial Contact Date: ' + format(n.DATINITIALCONTACTDATE, 'd-MMM-yyyy')),
                        iif(n.DATASSISTSTARTDATE is null, null,
                            'Assist Start Date: ' + format(n.DATASSISTSTARTDATE, 'd-MMM-yyyy')),
                        iif(n.DATASSISTENDDATE is null, null,
                            'Assist End Date: ' + format(n.DATASSISTENDDATE, 'd-MMM-yyyy'))
                       ), ''),
            'Assistance Request Type: ' + CHAR(13) + CHAR(10) + concat_ws
                (CHAR(13) + CHAR(10),
                 iif(n.STRASSISTANCEREQUEST = '00000000000000000000000000000', '* None selected', null),
                 iif(n.STRASSISTANCEREQUEST like '1%', '* Air Application Preparation', null),
                 iif(n.STRASSISTANCEREQUEST like '_1%', '* Air Emissions Inventory', null),
                 iif(n.STRASSISTANCEREQUEST like '__1%', '* Air Compliance Certification', null),
                 iif(n.STRASSISTANCEREQUEST like '___1%', '* Air Permit Assistance', null),
                 iif(n.STRASSISTANCEREQUEST like '____1%', '* Air Recordkeeping Assistanc', null),
                 iif(n.STRASSISTANCEREQUEST like '_____1%', '* Air Enforcement Assistance', null),
                 iif(n.STRASSISTANCEREQUEST like '______1%', '* Air Other', null),
                 iif(n.STRASSISTANCEREQUEST like '_______1%', '* Water Construction SWPPP', null),
                 iif(n.STRASSISTANCEREQUEST like '________1%', '* Water Industrial SWPPP', null),
                 iif(n.STRASSISTANCEREQUEST like '_________1%', '* Water SPCCC', null),
                 iif(n.STRASSISTANCEREQUEST like '__________1%', '* Water E & S', null),
                 iif(n.STRASSISTANCEREQUEST like '___________1%', '* Water NPDES', null),
                 iif(n.STRASSISTANCEREQUEST like '____________1%', '* Water POTW', null),
                 iif(n.STRASSISTANCEREQUEST like '_____________1%', '* Water Wetlands', null),
                 iif(n.STRASSISTANCEREQUEST like '______________1%', '* Water Other', null),
                 iif(n.STRASSISTANCEREQUEST like '_______________1%', '* Waste Form R', null),
                 iif(n.STRASSISTANCEREQUEST like '________________1%', '* Waste Tier 2', null),
                 iif(n.STRASSISTANCEREQUEST like '_________________1%', '* Waste Hazardous Waste', null),
                 iif(n.STRASSISTANCEREQUEST like '__________________1%', '* Waste Solid Waste', null),
                 iif(n.STRASSISTANCEREQUEST like '___________________1%', '* Waste UST', null),
                 iif(n.STRASSISTANCEREQUEST like '____________________1%', '* Waste AST', null),
                 iif(n.STRASSISTANCEREQUEST like '_____________________1%', '* Waste Other', null),
                 iif(n.STRASSISTANCEREQUEST like '______________________1%', '* Multimedia Compliance Audit', null),
                 iif(n.STRASSISTANCEREQUEST like '_______________________1%',
                     '* EMS Development / Implementation', null),
                 iif(n.STRASSISTANCEREQUEST like '________________________1%', '* Other', null),
                 iif(n.STRASSISTANCEREQUEST like '_________________________1%',
                     '* Pollution Prevention Energy Efficiency Assistance', null),
                 iif(n.STRASSISTANCEREQUEST like '__________________________1%',
                     '* Pollution Prevention Waste Minimization Assistance', null),
                 iif(n.STRASSISTANCEREQUEST like '___________________________1%',
                     '* Pollution Prevention Solvent Substitution Assistance', null),
                 iif(n.STRASSISTANCEREQUEST like '____________________________1%',
                     '* Pollution Prevention Water Minimization Assistance', null),
                 iif(n.STRASSISTANCEREQUEST like '_____________________________1%',
                     '* Pollution Prevention Other', null)
                ),
            nullif(trim(n.STRTECHNICALASSISTNOTES), '')
           )                                                  as [Notes],
       a.DATCREATIONDATE at time zone 'Eastern Standard Time' as [EnteredOn],
       a.DATCREATIONDATE at time zone 'Eastern Standard Time' as [CreatedAt],
       n.DATMODIFINGDATE at time zone 'Eastern Standard Time' as [UpdatedAt],
       convert(bit, 0)                                        as [IsDelete]
from AIRBRANCH.dbo.SBEAPACTIONLOG a
    inner join AIRBRANCH.dbo.LOOKUPSBEAPCASEWORK l
    on a.NUMACTIONTYPE = l.NUMACTIONTYPE
    inner join AIRBRANCH.dbo.SBEAPTECHNICALASSIST n
    on a.NUMACTIONID = n.NUMACTIONID
    --     inner join Cases c
--     on convert(int, a.NUMCASEID) = c.AirBranchCaseId
--     left join ActionItemTypes t
--     on t.Name = l.STRWORKDESCRIPTION

union

-- Other
select newid(),
       a.NUMCASEID                                            as [TempCaseId],
--        c.Id                                                   as [CaseworkId],
       l.STRWORKDESCRIPTION                                   as [TempActionItemType],
--        t.Id                                                   as [ActionItemTypeId],
       convert(date, a.DATACTIONOCCURED)                      as [ActionDate],
       isnull(trim(n.STRCASENOTES), '')                       as [Notes],
       a.DATCREATIONDATE at time zone 'Eastern Standard Time' as [EnteredOn],
       a.DATCREATIONDATE at time zone 'Eastern Standard Time' as [CreatedAt],
       n.DATMODIFINGDATE at time zone 'Eastern Standard Time' as [UpdatedAt],
       convert(bit, 0)                                        as [IsDelete]
from AIRBRANCH.dbo.SBEAPACTIONLOG a
    inner join AIRBRANCH.dbo.LOOKUPSBEAPCASEWORK l
    on a.NUMACTIONTYPE = l.NUMACTIONTYPE
    inner join AIRBRANCH.dbo.SBEAPOTHERLOG n
    on a.NUMACTIONID = n.NUMACTIONID
    --     inner join Cases c
--     on convert(int, a.NUMCASEID) = c.AirBranchCaseId
--     left join ActionItemTypes t
--     on t.Name = l.STRWORKDESCRIPTION
