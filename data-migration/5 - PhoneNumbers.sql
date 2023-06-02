-- use [sbeap-app]

-- Data comes from the SBEAPCLIENTCONTACTS table.
-- * Phone numbers are reformatted.

-- insert into PhoneNumber
--     (ContactId,
--      Id,
--      Number,
--      Type)

select ContactId,
       row_number() over (partition by ContactId order by Sequence) as [Id],
       Number,
       Type

from ( --
    select CLIENTCONTACTID                                      as [ContactId],
--            s.AirBranchContactId,
           dbo.FormatOnlyValidPhoneNumber(STRCLIENTPHONENUMBER) as [Number],
           'Work'                                               as [Type],
           1                                                    as [Sequence]
    from AIRBRANCH.dbo.SBEAPCLIENTCONTACTS c
--         inner join Contacts s
--         on c.CLIENTCONTACTID = s.AirBranchContactId
    where c.STRCLIENTPHONENUMBER is not null

    union

    select CLIENTCONTACTID,
--            s.AirBranchContactId,
           dbo.FormatOnlyValidPhoneNumber(STRCLIENTCELLPHONE),
           'WorkCell',
           2
    from AIRBRANCH.dbo.SBEAPCLIENTCONTACTS c
--         inner join Contacts s
--         on c.CLIENTCONTACTID = s.AirBranchContactId
    where c.STRCLIENTCELLPHONE is not null

    union

    select CLIENTCONTACTID,
--            s.AirBranchContactId,
           dbo.FormatOnlyValidPhoneNumber(STRCLIENTFAX),
           'Fax',
           3
    from AIRBRANCH.dbo.SBEAPCLIENTCONTACTS c
--         inner join Contacts s
--         on c.CLIENTCONTACTID = s.AirBranchContactId
    where c.STRCLIENTFAX is not null
    --
) t

-- order by t.AirBranchContactId, Id
