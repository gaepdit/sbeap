using AutoMapper;
using FluentAssertions.Extensions;
using Sbeap.AppServices.AutoMapper;

namespace AppServicesTests;

[SetUpFixture]
public class AppServicesTestsSetup
{
    internal static IMapper? Mapper;
    internal static MapperConfiguration? MapperConfig;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        // AutoMapper profiles are added here.
        MapperConfig = new MapperConfiguration(c => c.AddProfile(new AutoMapperProfile()));
        Mapper = MapperConfig.CreateMapper();

        AssertionConfiguration.Current.Equivalency.Modify(options => options
            // Setting this option globally since our DTOs generally exclude properties, e.g., audit properties.
            // See: https://fluentassertions.com/objectgraphs/#matching-members
            .ExcludingMissingMembers()

            // DateTimeOffset comparison is often off by a few microseconds.
            .Using<DateTimeOffset>(
                context => context.Subject.Should().BeCloseTo(context.Expectation, 10.Milliseconds()))
            .WhenTypeIs<DateTimeOffset>()
        );
    }
}
