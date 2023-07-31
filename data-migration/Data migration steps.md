# Data migration steps

1. Run the SBEAP application to create the new SBEAP DB tables.

2. Run the scripts in the "0-setup" folder to create the functions and lookup table.

3. Import the existing airbranch `SBEAP*` tables.

    There are various ways to do this. The "1-import\recreate-airbranch-table-structure.sql" script is available if needed to recreate the airbranch table structure.

4. Run the migration scripts in the "2-migrate" folder.

5. Run the cleanup scripts in the "3-cleanup" folder to remove the temporary tables, columns, and functions.
