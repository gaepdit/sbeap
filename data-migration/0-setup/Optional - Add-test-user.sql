USE [sbeap-app]
GO

-- Add test user (if running locally without Azure AD)

insert into AspNetUsers
    (Id,
     GivenName,
     FamilyName,
     Active,
     UserName,
     NormalizedUserName,
     Email,
     NormalizedEmail,
     AccessFailedCount,
     EmailConfirmed,
     PhoneNumberConfirmed,
     LockoutEnabled,
     TwoFactorEnabled,
     SecurityStamp)
select newid(),
       'Admin',
       'User',
       convert(bit, 1),
       'admin.user@example.net',
       'ADMIN.USER@EXAMPLE.NET',
       'admin.user@example.net',
       'ADMIN.USER@EXAMPLE.NET',
       0,
       convert(bit, 0),
       convert(bit, 0),
       convert(bit, 1),
       convert(bit, 0),
       newid();

insert into AspNetUserRoles
    (UserId,
     RoleId)
select u.Id,
       r.Id
from AspNetUsers u
    outer apply AspNetRoles r
where u.UserName = 'admin.user@example.net';
