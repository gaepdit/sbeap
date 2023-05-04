# Site Map

## Home Page
After `/Account/Login` page will redirect to:

- `/` (Home page {links employee access portals})

These are search pages that will show results as a table at bottom of each page determined by search criteria, and will link to specified pages as follows: 

> 1. `/Case/{id}` (Specific Case)
> 2. `/Customer/{id}` (All Cases for a specific customer - Customer refers to Client Data in Air DB)
> 4. `/Contact/{id}` (All Cases for a specific Program Contact)

### Redirects

| Redirect                   | To                                           |
|----------------------------|----------------------------------------------|
| `/`                        | `/Case/{id}`, `/Customer/{id}`, or `/Contact/{id}`|

# Pages

## Case Pages

* `/Case/Details/{id}` (Detail view of a Case with links to Edit, Customer and Contact)
* `/Case/Add` (Add New Case)
* `/Case/Edit/{id}` (Edit Case)

### Redirects

| Redirect      | To      |
|----------------|-------------|
| `/Case/Details/{id}` | `/Case/Edit/{id}`, `/Customer/Details/{id}`, `/Contact/Details/{id}`  |

## Customer Pages *{Customer refers to Company Info}*

* `/Customer/Details/{id}` (Customer *{Company}* Info with links to Edit and list of links to Cases)
* `/Customer/Add` (Add new Customer)
* `/Customer/Edit/{id}` (Edit Customer)

### Redirects

| Redirects   | To    |
|-------------|----------|
| `/Customer/Details/{id}`  | `/Customer/Edit/{id}`, `/Case/Details/{id}` |

## Contact Pages

* `/Contact/Details/{id}` (List of Customers handled by a Program Contact)

### Redirects

| Redirect     | To     |
|--------------|---------|
| `/Contact/Details/{id}`  | `/Customer/Details/{id}` |

### Redirects

| Redirects   | To   |
|---------------|----------|
| `/ActionItem/Details/{id}`  | `/Case/Details/{id}` |

## Maintenance Pages 
> Maintenance pages available to Site Admin personnel to modifiy Drop Down Menus

### Office Maintenance Pages

* `/Maintenance` (List of Items that can be modified on Site - Action Items and Offices)

### Office Maintenance Pages

* `/Maintenance/Office` (List of Internal Offices to assign cases to. Can be removed from list here)
* `/Maintenance/Office/Add` (Add New Office)
* `/Maintenance/Office/Edit/{id}` (Edit Office properties)

### Action Item Maintenance Pages

* `/Maintenance/ActionItem/Details/{id}` (Details for Action Item. Links to Edit)
* `/Maintenance/ActionItem/Add` (Add New Action Item)
* `/Maintenance/ActionItem/Edit/{id}` (Edit Action Item)

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
* Add Case (Redirect to Add Case Page: `/Case/Add`)
* More (Drop Down to show links to Users or Site Maintenance Pages: `/Admin/Users` or `/Maintenance`)