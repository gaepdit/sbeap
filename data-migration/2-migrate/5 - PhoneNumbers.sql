USE [sbeap-app]
GO

-- Data comes from the SBEAPCLIENTCONTACTS table.
-- * Phone numbers are reformatted.

-- insert into PhoneNumber
--     (ContactId,
--      Number,
--      Type)

select t.ContactId,
       t.Number,
       t.Type

from (
--
    select s.Id                                                 as [ContactId],
             dbo.FormatOnlyValidPhoneNumber(STRCLIENTPHONENUMBER) as [Number],
             'Work'                                               as [Type],
             s.AirBranchContactId,
             1                                                    as [TypeOrder]
      from dbo.SBEAPCLIENTCONTACTS c
          inner join Contacts s
          on c.CLIENTCONTACTID = s.AirBranchContactId
      where c.STRCLIENTPHONENUMBER is not null

      union

      select s.Id,
             dbo.FormatOnlyValidPhoneNumber(STRCLIENTCELLPHONE),
             'WorkCell',
             s.AirBranchContactId,
             2
      from dbo.SBEAPCLIENTCONTACTS c
          inner join Contacts s
          on c.CLIENTCONTACTID = s.AirBranchContactId
      where c.STRCLIENTCELLPHONE is not null

      union

      select s.Id,
             dbo.FormatOnlyValidPhoneNumber(STRCLIENTFAX),
             'Fax',
             s.AirBranchContactId,
             3
      from dbo.SBEAPCLIENTCONTACTS c
          inner join Contacts s
          on c.CLIENTCONTACTID = s.AirBranchContactId
      where c.STRCLIENTFAX is not null
      --
      ) t

order by t.AirBranchContactId, t.TypeOrder;
