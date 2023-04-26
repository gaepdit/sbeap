# Site Map

## Home Page
After `/Account/Login` page will redirect to:

- `/` (Home page {links employee access portals})
- `/Public` (Public Home page {links to public pages} )

These are search pages that will show results as a table at bottom of page.
> - `/Public` Public search page results will only show cases pertaining to that particular public user and will have links to only the user's Cases:
> 1. `/Case/{id}` (Specific Case)
---
> - `/` Internal search page results for internal users will show all cases and case info as determined by search criteria, and will link to specified pages as follows: 
> 1. `/Case/{id}` (Specific Case)
> 2. `/Customer/{id}` (All Cases for a specific customer - Customer refers to Client Data in Air DB)
> 3. `/Office/{id}` (All Cases for a specific office)
> 4. `/Contact/{id}` (All Cases for a specific Contact - Contact refers to Client Contact data in Air DB)

### Redirects

| Redirect                   | To                                           |
|----------------------------|----------------------------------------------|
| `/Public`                  | `/Public` (Public Users Only)                |
| `/`                        | `/Case/{id}`, `/Customer/{id}`, `/Office/{id}`, or `/Contact/{id}`|

# Public pages

* `/Public` (public search for Case, restricted to only cases pertaining to User)
* `/Case/Public/{id}` (public detail view of a specific Case, with list of links to Action Items) 
* `/ActionItem/Public/{id}` (public view of Action Item in a specific Case)
* `/Account/Edit/{id}` (public Edit page of own Customer Info)

### Redirects

| Redirect                   | To                   |
|----------------------------|----------------------|
| `/Public/{id}`             | `/Case/Public/{id}`  |
| (menu Bar)`AccountInfo`    | `/Account/Edit/{id}`  |


# Internal Pages

## Case Pages

* `/Case/Details/{id}` (Internal detail view of a Case with links to Customer, Office, Contact, List of Action Items)
* `/Case/Add` (Add New Case)
* `/Case/Edit/{id}` (Edit Case)

## Customer Pages *{Customer refers to Company Info}*

* `/Customer/Details/{id}` (Customer *{Company}* Info with link to Contact, and list of links to Cases)
* `/Customer/Add` (Add new Customer)
* `/Customer/Edit/{id}` (Edit Customer)

## Office Pages

* `/Office/Details/{id}` (List of cases handled by an Office. Links to Customers and Cases)

## Action Item Pages

* `/ActionItem/Details/{id}` (Details for Action Item. Link to Edit)
* `/ActionItem/Add` (Add New Action Item)
* `/ActionItem/Edit/{id}` (Edit Action Item)

## Maintenance Pages 
> Maintenance pages availble to internal management personal to modifiy Drop Down Menus

* `/Maintenance/Office` (List of Internal Offices to assign cases to. Can be removed from list here)
* `/Maintenance/Office/Edit/{id}` (Edit Office properties)

# User Account and Admin pages

### Account login and profile pages

* `/Account` - (view profile)
* `/Account/Login`
* `/Account/Edit` - (edit contact info)

### User management pages

* `/Admin/Users` (search)
* `/Admin/Users/Details/{id}`
* `/Admin/Users/Edit/{id}` (edit contact info)
* `/Admin/Users/EditRoles/{id}` (edit roles)
