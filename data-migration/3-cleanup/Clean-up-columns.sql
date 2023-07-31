USE [Sbeap]
GO

-- Remove temporary columns

alter table dbo.Cases
    drop column AirBranchCaseId
go

alter table dbo.Cases
    drop column AirBranchCustomerId
go

alter table dbo.Contacts
    drop column AirBranchContactId
go

alter table dbo.Contacts
    drop column AirBranchCustomerId
go

alter table dbo.Customers
    drop column AirBranchCustomerId
go
