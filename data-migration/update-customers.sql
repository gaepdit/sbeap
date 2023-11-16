use Sbeap;

update t
set t.SicCodeId = c.STRCLIENTSIC
-- select t.Id, c.CLIENTID, c.STRCLIENTSIC
from Sbeap.dbo.Customers t
    inner join AIRBRANCH.dbo.SBEAPCLIENTDATA c
    on t.AirBranchCustomerId = c.CLIENTID
        and c.CLIENTID not in
            (select c2.CLIENTID
             from AIRBRANCH.dbo.SBEAPCLIENTS c2
                 left join AIRBRANCH.dbo.SBEAPCASELOGLINK l2
                 on c2.CLIENTID = l2.CLIENTID
             where l2.CLIENTID is null
               and c2.CLIENTID not in
                   (select s2.CLIENTID
                    from AIRBRANCH.dbo.SBEAPCASELOG s2
                    where s2.CLIENTID is not null))
        and c.STRCLIENTSIC in
            (select SIC_CODE
             from AIRBRANCH.dbo.LK_SIC
             where ACTIVE = '1');
