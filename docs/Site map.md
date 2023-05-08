# Site Map

## Home Page
After `/Account/Login` page will redirect to:

- `/` (Home page {links employee access portals})

These are search pages that will show results as a table at bottom of the page determined by search criteria, and will link to specified pages.

# Pages

## Case Pages

* `/Case` Case search page
* `/Case/Details/{id}` (Detail view of a Case with links to Edit, Customer and Contact)
* `/Case/Add` (Add New Case)
* `/Case/Edit/{id}` (Edit Case)
* `/Case/Delete/{id}` (Delete Case)
* `/Case/Details/{id}/EditAction/{actionId}` (Edit Action by Case)
* `/Case/Details/{id}/DeleteAction/{actionId}` (Delete Action by Case)

## Customer Pages 

* `/Customer` Customer Search Page
* `/Customer/Details/{id}` (Customer Info with links to Edit and list of links to Cases)
* `/Customer/Add` (Add new Customer)
* `/Customer/Edit/{id}` (Edit Customer)
* `/Customer/Delete` (Delete Customer)
* `/Customer/Details/{id}/ContactEdit/{contactId}` (Edit Contact by Customer) 
* `/Customer/Details/{id}/ContactDelete/{contactId}` (Delete Contact by Customer)

## Maintenance Pages 
> Maintenance pages available to Site Admin personnel to modifiy Drop Down Menus

* `/Maintenance` (List of Items that can be modified on Site - Action Items, Agencies and Offices)

### Office Maintenance Pages

* `/Maintenance/Office` (List of Internal Offices to assign cases to. Can be removed from list here)
* `/Maintenance/Office/Add` (Add New Office)
* `/Maintenance/Office/Edit/{id}` (Edit Office properties)

### Action Item Maintenance Pages

* `/Maintenance/ActionItem` (List of Action Items. Can be removed from list here)
* `/Maintenance/ActionItem/Add` (Add New Action Item)
* `/Maintenance/ActionItem/Edit/{id}` (Edit Action Item)

### Agency Maintenance Pages

* `/Maintenance/Agency` (List of Agencies. Can be removed from list here)
* `/Maintenance/Agency/Add` (Add new Agency)
* `/Maintenance/Agency/Edit/{id}` (Edit Agency)

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

# Menu Bar

* Home (Redirect to Search Page: `/`)
* Account (Redirect to Account Edit Page: `/Account/Edit/{id}`)
* Add Customer (Redirect to Add Customer Page: `/Customer/Add`)
* More (Drop Down to show links to Users or Site Maintenance Pages: `/Admin/Users` or `/Maintenance`)