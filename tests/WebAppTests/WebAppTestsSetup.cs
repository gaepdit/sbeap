using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebAppTests;

[SetUpFixture]
public static class WebAppTestsSetup
{
    internal static TempDataDictionary PageTempData() =>
        new(new DefaultHttpContext(), Substitute.For<ITempDataProvider>());
}
