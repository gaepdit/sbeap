using Microsoft.AspNetCore.Mvc;
using Sbeap.WebApp.Platform.OrgNotifications;

namespace Sbeap.WebApp.Pages.Shared.Components.Notifications;

public class NotificationsViewComponent(IOrgNotifications orgNotifications) : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync() =>
        View("Default", await orgNotifications.GetOrgNotificationsAsync());
}
