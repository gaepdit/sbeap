# Site Map

## Application Pages

### Home Page

- `/` (Home page/user dashboard)

### Customer Pages 

* `/Customers` (Customer search)
* `/Customers/Add` (Add new Customer with Contact)
* `/Customers/Details/{id}` (Customer details; list of Contacts; list of Cases with a new Case form)
* `/Customers/Edit/{id}` (Edit Customer details)
* `/Customers/Delete/{id}` (Delete Customer)
* `/Customers/Restore/{id}` (Restore Customer)
* `/Customers/Details/{id}/AddContact` (Add new Contact)
* `/Customers/EditContact/{contactId}` (Edit Contact)
* `/Customers/Details/{id}/DeleteContact/{contactId}` (Delete Contact)

### Case Pages

* `/Cases` (Case search)
* `/Cases/Details/{id}` (Case details with list of Action Items and a new Action Item form)
* `/Cases/Edit/{id}` (Edit Case details)
* `/Cases/Delete/{id}` (Delete Case)
* `/Cases/Restore/{id}` (Restore Case)
* `/Cases/Details/{id}/EditAction/{actionId}` (Edit Action Item)
* `/Cases/Details/{id}/DeleteAction/{actionId}` (Delete Action Item)

### Maintenance Pages 

Maintenance pages available to Site Admin personnel to modify lookup tables used for drop-down lists. Editable items comprise Action Item Types, Agencies, and Offices.

* `/Admin/Maintenance` (List of item types)
* `/Admin/Maintenance/[type]` (List of items of given type; can be removed from list here)
* `/Admin/Maintenance/[type]/Add` (Add new item)
* `/Admin/Maintenance/[type]/Edit/{id}` (Edit item)

## User Account and Admin pages

### Account Pages

* `/Account` (View profile)
* `/Account/Login`
* `/Account/Edit` (Edit contact info)

### User Management Pages

* `/Admin/Users` (User search)
* `/Admin/Users/Details/{id}`
* `/Admin/Users/Edit/{id}` (Edit contact info)
* `/Admin/Users/EditRoles/{id}` (Edit roles)

## Menu Bar

* Home (`/`)
* Customer search (`/Customers`)
* Case search (`/Cases`)
* Add Customer (`/Customers/Add`)
* More (Drop-down to show links to User Search and Site Maintenance pages: `/Admin/Users` or `/Admin/Maintenance`)
* Account (Drop-down to show links to Profile page or sign-out: `/Account`)
