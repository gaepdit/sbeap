USE [Sbeap]
GO

insert into dbo.Offices
    (Id,
     Name,
     Active,
     CreatedAt)

select newid()             as [Id],
       'SBEAP'             as [Name],
       convert(bit, 1)     as [Active],
       sysdatetimeoffset() as [CreatedAt];
