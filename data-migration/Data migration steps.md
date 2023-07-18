# Data migration steps

1. Run the SBEAP application to create the new SBEAP DB tables.
2. Run the scripts in the "0-setup" folder to create the functions and lookup table.
3. Run the "1-import\recreate-airbranch-table-structure.sql" script to recreate the airbranch table structure.
4. Export the existing airbranch data.
5. Import the existing data into the airbranch table structure in the SBEAP DB.
6. Run the migration scripts in the "2-migrate" folder.
7. Run the cleanup scripts in the "3-cleanup" folder to remove the temporary tables, columns, and functions.
