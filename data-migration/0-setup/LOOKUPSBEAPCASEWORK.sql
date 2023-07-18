USE [sbeap-app]
GO

create table LOOKUPSBEAPCASEWORK (
    NUMACTIONTYPE      numeric(22) not null,
    STRWORKDESCRIPTION varchar(4000) not null
)
go

INSERT INTO dbo.LOOKUPSBEAPCASEWORK (NUMACTIONTYPE, STRWORKDESCRIPTION) VALUES (1, N'CAP meetings');
INSERT INTO dbo.LOOKUPSBEAPCASEWORK (NUMACTIONTYPE, STRWORKDESCRIPTION) VALUES (2, N'Drafting Small Bus Impact Memos');
INSERT INTO dbo.LOOKUPSBEAPCASEWORK (NUMACTIONTYPE, STRWORKDESCRIPTION) VALUES (3, N'Email Sent/Received');
INSERT INTO dbo.LOOKUPSBEAPCASEWORK (NUMACTIONTYPE, STRWORKDESCRIPTION) VALUES (4, N'Meeting/Conferences Attended');
INSERT INTO dbo.LOOKUPSBEAPCASEWORK (NUMACTIONTYPE, STRWORKDESCRIPTION) VALUES (5, N'Other');
INSERT INTO dbo.LOOKUPSBEAPCASEWORK (NUMACTIONTYPE, STRWORKDESCRIPTION) VALUES (6, N'Phone Call Made/Received');
INSERT INTO dbo.LOOKUPSBEAPCASEWORK (NUMACTIONTYPE, STRWORKDESCRIPTION) VALUES (7, N'Publication/Document Sent');
INSERT INTO dbo.LOOKUPSBEAPCASEWORK (NUMACTIONTYPE, STRWORKDESCRIPTION) VALUES (8, N'Site Visit');
INSERT INTO dbo.LOOKUPSBEAPCASEWORK (NUMACTIONTYPE, STRWORKDESCRIPTION) VALUES (9, N'Stakeholder meetings');
INSERT INTO dbo.LOOKUPSBEAPCASEWORK (NUMACTIONTYPE, STRWORKDESCRIPTION) VALUES (10, N'Permit Assistance');
INSERT INTO dbo.LOOKUPSBEAPCASEWORK (NUMACTIONTYPE, STRWORKDESCRIPTION) VALUES (11, N'Workshops/Training Courses Conducted');
INSERT INTO dbo.LOOKUPSBEAPCASEWORK (NUMACTIONTYPE, STRWORKDESCRIPTION) VALUES (12, N'Mass Mailing');
INSERT INTO dbo.LOOKUPSBEAPCASEWORK (NUMACTIONTYPE, STRWORKDESCRIPTION) VALUES (13, N'Compliance Assistance');