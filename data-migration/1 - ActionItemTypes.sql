use [sbeap-app]

-- Data comes from the LOOKUPSBEAPCASEWORK table, modified based on conversations with the program.

-- insert into ActionItemTypes (Id, Name, Active, CreatedAt)

select newid() as [Id], 'CAP meetings' as [Name], convert(bit, 0) as [Active], sysdatetimeoffset() as [CreatedAt]
union
select newid(), 'Compliance Assistance', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'Drafting Small Bus Impact Memos', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'Email Sent/Received', convert(bit, 1), sysdatetimeoffset()
union
select newid(), 'Mass Mailing', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'Meeting/Conferences Attended', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'Other', convert(bit, 1), sysdatetimeoffset()
union
select newid(), 'Permit Assistance', convert(bit, 1), sysdatetimeoffset()
union
select newid(), 'Phone Call Made/Received', convert(bit, 1), sysdatetimeoffset()
union
select newid(), 'Publication/Document Sent', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'Site Visit', convert(bit, 1), sysdatetimeoffset()
union
select newid(), 'Stakeholder meetings', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'Workshops/Training Courses Conducted', convert(bit, 1), sysdatetimeoffset();
