USE [Sbeap]
GO

-- Data comes from code in the IAIP and existing data in the SBEAPCASELOG.STRINTERAGENCY column, modified based on
-- conversations with the program.

insert into Agencies (Id, Name, Active, CreatedAt)

select newid(), 'Albany', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'APB', convert(bit, 1), sysdatetimeoffset()
union
select newid(), 'Athens', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'Atlanta District', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'Atlanta', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'Augusta', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'Brunswick District', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'Brunswick', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'Cartersville District', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'Cartersville', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'Coastal District (Brunswick)', convert(bit, 1), sysdatetimeoffset()
union
select newid(), 'Columbus', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'Directors Agency', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'East Central Dist (Augusta)', convert(bit, 1), sysdatetimeoffset()
union
select newid(), 'East Central District', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'EPA', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'EPD Air Nox', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'EPD Hazardous Waste', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'EPD NE District', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'GDED', convert(bit, 1), sysdatetimeoffset()
union
select newid(), 'GEFA', convert(bit, 1), sysdatetimeoffset()
union
select newid(), 'HWB', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'LBP', convert(bit, 1), sysdatetimeoffset()
union
select newid(), 'Macon', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'Main Agency', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'Mountain Dist (Cartersville)', convert(bit, 1), sysdatetimeoffset()
union
select newid(), 'Mountain District (Atlanta)', convert(bit, 1), sysdatetimeoffset()
union
select newid(), 'Northeast District (Athens)', convert(bit, 1), sysdatetimeoffset()
union
select newid(), 'Northeast District', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'NSBEAP', convert(bit, 1), sysdatetimeoffset()
union
select newid(), 'P2AD', convert(bit, 1), sysdatetimeoffset()
union
select newid(), 'Savannah', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'SBEAP', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'Southwest District (Albany)', convert(bit, 1), sysdatetimeoffset()
union
select newid(), 'Southwest District', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'Statewide', convert(bit, 1), sysdatetimeoffset()
union
select newid(), 'West Central District (Macon)', convert(bit, 1), sysdatetimeoffset()
union
select newid(), 'West Central District', convert(bit, 0), sysdatetimeoffset()
union
select newid(), 'WPB', convert(bit, 1), sysdatetimeoffset();
