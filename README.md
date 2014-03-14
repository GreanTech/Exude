#Exude

An extension to [xUnit.net](https://xunit.codeplex.com), providing support for test cases as First-Class, programmatic citizens.

## First-Class Test Cases

Sometimes, writing [Parameterized Tests](http://xunitpatterns.com/Parameterized%20Test.html) using xUnit.net's `[Theory]` attribute can be troublesome, because the various options aren't compile-time safe, and it can be difficult to supply test case values that aren't constants (strings, integers, booleans, etc.)

Exude enables you to write test cases as **First-Class Citizens** by using the `[FirstClassTests]` attribute on a method that returns `IEnumerable<ITestCase>`:

```C#
[FirstClassTests]
public static IEnumerable<ITestCase> YieldFirstClassTests()
{
    yield return new TestCase(_ => Assert.Equal(1, 1));
    yield return new TestCase(_ => Assert.Equal(2, 2));
    yield return new TestCase(_ => Assert.Equal(3, 3));
}
```

In the above, very trivial example, three test cases are created and returned from the test method. When you run the tests, all three tests are executed and (in this case) pass:

> 3 passed, 0 failed, 0 skipped, took 0,06 seconds (xUnit.net 1.9.2 build 1705).

The flexible design of Exude gives you many opportunities for organising your test cases.

### Generic Test Cases

If you need to access the test class that contains the test methods, you can use the `TestCase<T>` class, which also implements `ITestCase`. The object passed into each `TestCase` instance's Action is an instance of the containing test class. If you use `TestCase<T>`, you don't have to perform the cast yourself.

Here's an example which is difficult to write with xUnit.net's built-in data sources:

```C#
public void AParameterizedTest(DateTimeOffset x, DateTimeOffset y)
{
    Assert.True(x < y);
}
```

The problem is that `DateTimeOffset` is a complex datatype, which has no representation through a *constant*, so you can't use the `[InlineData]` attribute. With the built-in data sources from xUnit.net, you'll have to use either `[ClassData]` or `[PropertyData]`, but it's easy to make mistakes with those, because they aren't type-safe, and have a weird API.

Instead, you can supply the data for the test method by simply *invoking* it:


```C#
[FirstClassTests]
public static TestCase<Scenario>[] RunAParameterizedTest()
{
    var testCases = new[] 
    {
        new 
        {
            x = new DateTimeOffset(2002, 10, 12, 18, 15, 0, TimeSpan.FromHours(1)),
            y = new DateTimeOffset(2007,  4, 21, 18, 15, 0, TimeSpan.FromHours(1))
        },
        new
        {
            x = new DateTimeOffset(1970, 11, 25, 16, 10, 0, TimeSpan.FromHours(1)),
            y = new DateTimeOffset(1972,  6,  6,  8,  5, 0, TimeSpan.FromHours(1))
        },
        new
        {
            x = new DateTimeOffset(2014, 3, 2, 17, 18, 45, TimeSpan.FromHours(1)),
            y = new DateTimeOffset(2014, 3, 2, 17, 18, 45, TimeSpan.FromHours(0))
        }
    };
    return testCases
        .Select(tc =>
            new TestCase<Scenario>(
                s => s.AParameterizedTest(tc.x, tc.y)))
        .ToArray();
}
```

In this example, notice that the Parameterized Test itself is an instance method, which doesn't have any attributes. Instead, the `RunAParameterizedTest` method creates three test cases, and converts them to an array of `TestCase<Scenario>` instances.

Each `TestCase<Scenario>` instance adapts an `Action<Scenario>`, which invokes the `AParameterizedTest` method in a type-safe manner.

## Get Exude

Obviously, the source code is available here on GitHub, and you can soon download the compiled library with NuGet.

### Versioning

Exude follows Semantic [Versioning 2.0.0](http://semver.org/spec/v2.0.0.html).

## Credits

Exude was inspired by Mauricio Scheffer's blog post [First-class tests in MbUnit](http://bugsquash.blogspot.dk/2012/05/first-class-tests-in-mbunit.html).