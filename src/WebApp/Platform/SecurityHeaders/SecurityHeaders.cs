﻿using Sbeap.WebApp.Platform.Settings;

namespace Sbeap.WebApp.Platform.SecurityHeaders;

internal static class SecurityHeaders
{
    internal static void AddSecurityHeaderPolicies(this HeaderPolicyCollection policies)
    {
        policies
            .AddFrameOptionsDeny()
            .AddXssProtectionBlock()
            .AddContentTypeOptionsNoSniff()
            .AddReferrerPolicyStrictOriginWhenCrossOrigin()
            .RemoveServerHeader();

        if (!string.IsNullOrEmpty(ApplicationSettings.RaygunSettings.ApiKey))
            policies.AddReportingEndpoints(builder => builder.AddEndpoint("csp-endpoint",
                $"https://report-to-api.raygun.com/reports-csp?apikey={ApplicationSettings.RaygunSettings.ApiKey}"));
    }
}
