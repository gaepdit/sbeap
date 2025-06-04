global using AwesomeAssertions;
global using AwesomeAssertions.Execution;
global using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.All)]
[assembly: FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
