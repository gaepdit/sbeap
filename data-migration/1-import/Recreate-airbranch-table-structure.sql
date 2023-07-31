USE [Sbeap]
GO

create table dbo.SBEAPACTIONLOG
(
    NUMACTIONID      float,
    NUMCASEID        float,
    NUMACTIONTYPE    float,
    NUMMODIFINGSTAFF float,
    DATMODIFINGDATE  datetime2(0),
    STRCREATINGSTAFF varchar(4000),
    DATCREATIONDATE  datetime2(0),
    DATACTIONOCCURED datetime2(0)
);

create table dbo.SBEAPCASELOG
(
    NUMCASEID                float,
    NUMSTAFFRESPONSIBLE      float,
    DATCASEOPENED            datetime2(0),
    STRCASESUMMARY           varchar(4000),
    CLIENTID                 float,
    DATCASECLOSED            datetime2(0),
    NUMMODIFINGSTAFF         float,
    DATMODIFINGDATE          datetime2(0),
    STRINTERAGENCY           varchar(4000),
    STRREFERRALCOMMENTS      varchar(4000),
    DATREFERRALDATE          datetime2(0),
    STRCOMPLAINTBASED        varchar(4000),
    STRCASECLOSURELETTERSENT varchar(5)
);

create table dbo.SBEAPCASELOGLINK
(
    NUMCASEID float,
    CLIENTID  float
);

create table dbo.SBEAPCLIENTCONTACTS
(
    CLIENTCONTACTID      float,
    STRCLIENTFIRSTNAME   varchar(250),
    STRCLIENTLASTNAME    varchar(250),
    STRCLIENTSALUTATION  varchar(100),
    STRCLIENTCREDENTIALS varchar(100),
    STRCLIENTTITLE       varchar(250),
    STRCLIENTPHONENUMBER varchar(4000),
    STRCLIENTCELLPHONE   varchar(4000),
    STRCLIENTFAX         varchar(4000),
    STRCLIENTEMAIL       varchar(4000),
    STRCLIENTADDRESS     varchar(4000),
    STRCLIENTCITY        varchar(4000),
    STRCLIENTSTATE       varchar(5),
    STRCLIENTZIPCODE     varchar(4000),
    STRCLIENTCREATOR     varchar(1000),
    DATCLIENTCREATED     datetime2(0),
    STRMODIFINGPERSON    varchar(1000),
    DATMODIFINGDATE      datetime2(0),
    STRCONTACTNOTES      varchar(4000)
);

create table dbo.SBEAPCLIENTDATA
(
    CLIENTID             float,
    STRCLIENTDESCRIPTION varchar(4000),
    STRCLIENTWEBSITE     varchar(4000),
    STRCLIENTSIC         varchar(4),
    STRCLIENTNAICS       varchar(6),
    STRCLIENTEMPLOYEES   float,
    STRAIRSNUMBER        varchar(12),
    STRAIRPROGRAMCODES   varchar(15),
    STRSTATEPROGRAMCODES varchar(5),
    STRMODIFINGPERSON    varchar(4000),
    DATMODIFINGDATE      datetime2(0),
    STRMODIFINGCOMMENTS  varchar(4000),
    STRAIRPERMITNUMBER   varchar(4000),
    STRSSCPENGINEER      varchar(4000),
    STRSSCPUNIT          varchar(4000),
    STRSSPPENGINEER      varchar(4000),
    STRSSPPUNIT          varchar(4000),
    STRISMPENGINEER      varchar(4000),
    STRISMPUNIT          varchar(4000),
    STRAIRDESCRIPTION    varchar(4000)
);

create table dbo.SBEAPCLIENTLINK
(
    CLIENTID        float,
    CLIENTCONTACTID float,
    STRMAINCONTACT  varchar(4000)
);

create table dbo.SBEAPCLIENTS
(
    CLIENTID            float,
    STRCOMPANYNAME      varchar(4000),
    DATSTARTDATE        datetime2(0),
    STRCOMPANYADDRESS   varchar(4000),
    STRCOMPANYADDRESS2  varchar(4000),
    STRCOMPANYCITY      varchar(4000),
    STRCOMPANYSTATE     varchar(5),
    STRCOMPANYZIPCODE   varchar(4000),
    STRCOMPANYCOUNTY    varchar(5),
    STRCOMPANYLATITUDE  varchar(8),
    STRCOMPANYLONGITUDE varchar(8),
    STRMAILINGADDRESS   varchar(4000),
    STRMAILINGADDRESS2  varchar(4000),
    STRMAILINGCITY      varchar(4000),
    STRMAILINGSTATE     varchar(5),
    STRMAILINGZIPCODE   varchar(4000),
    STRCOMPANYCREATOR   varchar(4000),
    DATCOMPANYCREATED   datetime2(0),
    STRMODIFINGPERSON   varchar(4000),
    DATMODIFINGDATE     datetime2(0),
    STRMODIFINGCOMMENTS varchar(4000)
);

create table dbo.SBEAPCOMPLIANCEASSIST
(
    NUMACTIONID         float not null,
    STRAIRASSIST        varchar(5),
    STRSTORMWATERASSIST varchar(5),
    STRHAZWASTEASSIST   varchar(5),
    STRSOLIDWASTEASSIST varchar(5),
    STRUSTASSIST        varchar(5),
    STRSCRAPTIREASSIST  varchar(5),
    STRLEADASSIST       varchar(5),
    STROTHERASSIST      varchar(5),
    STRCOMMENTS         varchar(4000),
    STRMODIFINGSTAFF    varchar(20),
    DATMODIFINGDATE     datetime2(0)
);

create table dbo.SBEAPCONFERENCELOG
(
    NUMACTIONID              float,
    STRCONFERENCEATTENDED    varchar(4000),
    STRCONFERENCELOCATION    varchar(4000),
    STRCONFERENCETOPIC       varchar(4000),
    STRATTENDEES             varchar(4000),
    DATCONFERENCESTARTED     datetime2(0),
    DATCONFERENCEENDED       datetime2(0),
    STRSBEAPPRESENTATION     varchar(4000),
    STRLISTOFBUSINESSSECTORS varchar(4000),
    STRCONFERENCEFOLLOWUP    varchar(4000),
    STRSTAFFATTENDING        varchar(4000),
    STRMODIFINGSTAFF         varchar(4000),
    DATMODIFINGDATE          datetime2(0)
);

create table dbo.SBEAPOTHERLOG
(
    NUMACTIONID      float,
    STRCASENOTES     varchar(4000),
    STRMODIFINGSTAFF varchar(4000),
    DATMODIFINGDATE  datetime2(0)
);

create table dbo.SBEAPPHONELOG
(
    NUMACTIONID          float,
    STRCALLERINFORMATION varchar(4000),
    NUMCALLERPHONENUMBER float,
    STRPHONELOGNOTES     varchar(4000),
    STRONETIMEASSIST     varchar(4000),
    STRFRONTDESKCALL     varchar(4000),
    STRMODIFINGSTAFF     varchar(4000),
    DATMODIFINGDATE      varchar(4000)
);

create table dbo.SBEAPTECHNICALASSIST
(
    NUMACTIONID             float,
    STRTECHNICALASSISTTYPE  varchar(4000),
    DATINITIALCONTACTDATE   datetime2(0),
    DATASSISTSTARTDATE      datetime2(0),
    DATASSISTENDDATE        datetime2(0),
    STRASSISTANCEREQUEST    varchar(4000),
    STRAIRSNUMBER           varchar(4000),
    STRTECHNICALASSISTNOTES varchar(4000),
    STRMODIFINGSTAFF        varchar(4000),
    DATMODIFINGDATE         datetime2(0)
);


