use [sbeap-app]

-- Remove temporary columns

alter table Cases
    drop column AirBranchCaseId
go

alter table Cases
    drop column AirBranchCustomerId
go

alter table Contacts
    drop column AirBranchContactId
go

alter table Contacts
    drop column AirBranchCustomerId
go

alter table Customers
    drop column AirBranchCustomerId
go

