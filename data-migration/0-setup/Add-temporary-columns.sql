USE [sbeap-app]
GO

-- Add temporary columns to store the old IDs.

alter table dbo.Cases
    add AirBranchCaseId int
go

alter table dbo.Cases
    add AirBranchCustomerId int
go

alter table dbo.Contacts
    add AirBranchContactId int
go

alter table dbo.Contacts
    add AirBranchCustomerId int
go

alter table dbo.Customers
    add AirBranchCustomerId int
go
