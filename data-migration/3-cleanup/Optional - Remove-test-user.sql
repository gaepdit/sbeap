USE [Sbeap]
GO

-- Remove test user

delete r
from dbo.AspNetUserRoles r
    inner join AspNetUsers u
    on u.Id = r.UserId
where u.UserName = 'admin.user@example.net';

delete
from dbo.AspNetUsers
where UserName = 'admin.user@example.net';
