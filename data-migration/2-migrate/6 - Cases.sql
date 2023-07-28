USE [sbeap-app]
GO

-- Data comes from the SBEAPCASELOG and SBEAPCASELOGLINK tables.
-- * All strings are trimmed.

insert into Cases
    (Id,
     CustomerId,
     Description,
     CaseOpenedDate,
     CaseClosedDate,
     ReferralAgencyId,
     ReferralDate,
     ReferralNotes,
     UpdatedAt,
     IsDeleted,
     AirBranchCaseId,
     AirBranchCustomerId)

select newid() as [Id],
       CustomerId,
       Description,
       CaseOpenedDate,
       CaseClosedDate,
       ReferralAgencyId,
       ReferralDate,
       ReferralNotes,
       UpdatedAt,
       IsDeleted,
       AirBranchCaseId,
       AirBranchCustomerId
from (select u.Id                                    as [CustomerId],
             isnull(trim(c.STRCASESUMMARY), '')      as [Description],
             convert(date, c.DATCASEOPENED)          as [CaseOpenedDate],
             convert(date, c.DATCASECLOSED)          as [CaseClosedDate],
             a.Id                                    as [ReferralAgencyId],
             convert(date, c.DATREFERRALDATE)        as [ReferralDate],
             isnull(trim(c.STRREFERRALCOMMENTS), '') as [ReferralNotes],
             c.DATMODIFINGDATE at time zone 'Eastern Standard Time'
                                                     as [UpdatedAt],
             convert(bit, 0)                         as [IsDeleted],
             convert(int, c.NUMCASEID)               as [AirBranchCaseId],
             convert(int, l.CLIENTID)                as [AirBranchCustomerId]

      from dbo.SBEAPCASELOG c
          left join dbo.SBEAPCASELOGLINK l
          on c.NUMCASEID = l.NUMCASEID
          inner join Customers u
          on l.CLIENTID = u.AirBranchCustomerId
          left join Agencies a
          on c.STRINTERAGENCY = a.Name
      where l.NUMCASEID is not null

      union

      select u.Id                                    as [CustomerId],
             isnull(trim(c.STRCASESUMMARY), '')      as [Description],
             convert(date, c.DATCASEOPENED)          as [CaseOpenedDate],
             convert(date, c.DATCASECLOSED)          as [CaseClosedDate],
             a.Id                                    as [ReferralAgencyId],
             convert(date, c.DATREFERRALDATE)        as [ReferralDate],
             isnull(trim(c.STRREFERRALCOMMENTS), '') as [ReferralNotes],
             c.DATMODIFINGDATE at time zone 'Eastern Standard Time'
                                                     as [UpdatedAt],
             convert(bit, 0)                         as [IsDeleted],
             convert(int, c.NUMCASEID)               as [AirBranchCaseId],
             convert(int, l.CLIENTID)                as [AirBranchCustomerId]

      from dbo.SBEAPCASELOG c
          left join dbo.SBEAPCASELOGLINK l
          on c.NUMCASEID = l.NUMCASEID
          inner join Customers u
          on c.CLIENTID = u.AirBranchCustomerId
          left join Agencies a
          on c.STRINTERAGENCY = a.Name
      where l.NUMCASEID is null
        and c.CLIENTID is not null) t

where t.AirBranchCaseId not in
      (20, 43, 44, 150, 163, 294, 323, 381, 492, 495, 498, 607, 1023, 1040, 1055, 1187, 1196, 1210, 1268, 1269, 1270,
       1271, 1272, 1273, 1274, 1275, 1276, 1285, 1352, 2035, 2184, 2207, 2232, 2378, 2384, 2389, 2488, 2490, 2491, 2650,
       2850, 2951, 2952, 2953, 3093, 3095, 3102, 3103, 3104, 3118, 3132, 3146, 3151, 3152, 3156, 3157, 3158, 3159, 3160,
       3164, 3191, 3211, 3218, 3220, 3223, 3238, 3255, 3256, 3258, 3260, 3264, 3273, 3294, 3296, 3333, 3334, 3342, 3347,
       3371, 3394, 3395, 3405, 3408, 3411, 3418, 3419, 3428, 3449, 3488, 3523, 3535, 3553, 3556, 3558, 3564, 3567, 3581,
       3582, 3583, 3607, 3618, 3632, 3633, 3713, 3736, 3737, 3738, 3771, 3779, 3780, 3810, 3827, 3828, 3829, 3830, 3831,
       3840, 3861, 3883, 3887, 3894, 3896, 3898)

order by AirBranchCaseId
