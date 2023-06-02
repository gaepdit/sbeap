use [sbeap-app]

-- Remove temporary columns

alter table Customers
    drop column AirBranchCustomerId
go

