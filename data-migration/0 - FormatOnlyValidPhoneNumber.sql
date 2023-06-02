USE airbranch;
GO
SET ANSI_NULLS ON;
GO

CREATE OR ALTER FUNCTION dbo.FormatOnlyValidPhoneNumber(@phone nvarchar(25))
    RETURNS nvarchar(25)
AS

/**************************************************************************************************

Author:     Doug Waldron

Overview:   Formats a phone number for display. (SBEAP phone numbers are inconsistently formatted.)

  If the input phone number has any non-digit characters or if the number of digits is less than
  ten, then null is returned. Otherwise, it is formatted as '###-###-####' for the first ten
  digits, and any remainaing digits are returned as a phone number extension.

  (Compare with the `FormatPhoneNumber` function which returns invalid phone numbers unchanged.)

Input Parameters:
  @phone - The input phone number.

Returns:
  nvarchar(25) - The formatted phone number.

Modification History:
When        Who                 What
----------  ------------------  -------------------------------------------------------------------
2021-10-13  DWaldron            Initial dbo.FormatPhoneNumber (airbranch-db#14)
2023-06-02  DWaldron            Return null if number is not valid (sbeap#27)

***************************************************************************************************/

BEGIN

    if @phone is null
        RETURN null;

    declare @phoneTrimmed nvarchar(25) = replace(translate(@phone, '()- ', '####'), '#', '');

    if len(@phoneTrimmed) < 10
        RETURN null;

    if len(@phoneTrimmed) = 10
        RETURN concat_ws(N'-',
                         substring(@phoneTrimmed, 1, 3),
                         substring(@phoneTrimmed, 4, 3),
                         substring(@phoneTrimmed, 7, 4));

    RETURN concat_ws(' x ',
                     concat_ws(N'-',
                               substring(@phoneTrimmed, 1, 3),
                               substring(@phoneTrimmed, 4, 3),
                               substring(@phoneTrimmed, 7, 4)),
                     substring(@phoneTrimmed, 11, 25))

END
