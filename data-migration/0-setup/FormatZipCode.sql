USE [Sbeap]
GO
SET ANSI_NULLS ON
GO

CREATE OR ALTER FUNCTION dbo.FormatZipCode(@zip nvarchar(10))
    RETURNS nvarchar(10)
AS

/*******************************************************************************

Author:     Doug Waldron

Overview:   Formats a ZIP Code for display. Input ZIP code may be 5 or 9 digits,
            with or without a hyphen. A ZIP code that does not match this
            pattern returns null.
            Validate Regex: https://regex101.com/r/7aJyGD/1

Input Parameters:
  @zip - The input ZIP code.

Modification History:
When        Who                 What
----------  ------------------  ------------------------------------------------
2021-08-16  DWaldron            Initial Version (geco#483)

*******************************************************************************/

BEGIN

    if @zip is null
        RETURN null;

    declare @zipTrimmed nvarchar(10) = replace(trim(@zip), '-', '');

    if @zipTrimmed in (N'00000', N'000000000')
        RETURN null;

    if len(@zipTrimmed) = 9
        RETURN concat_ws('-', left(@zipTrimmed, 5), right(@zipTrimmed, 4));

    if len(@zipTrimmed) = 5
        RETURN @zipTrimmed;

    RETURN null;

END