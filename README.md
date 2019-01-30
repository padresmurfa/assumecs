# Assumptions

The Assumptions library provides a clean method to replace implicit assumptions with explicit ones in C#.

Assumptions are much akin to Asserts in unit tests, but differ in that they are intended to be part of production code.

## 1. Basic usage

An Assumption is typically a statement such as "assume that 'foo' is equal to 'blat'.

In fact, since the  Assumption library uses a the [fluent interface](https://en.wikipedia.org/wiki/Fluent_interface)  pattern, the syntax is pretty much just that:
```C#
Assume.That(foo).Is.Equal.To("blat", "Foo should have had the value 'blat'")
```

## 2. Starting an Assumption

Assumptions always start with one of the following:

| Starts with   | Description  |
| ------------- |:-------------|
| Assume.That(actual)      | Followed by a logical operation comparing the actual value to an expected value.|
| Assume.That(x => {}) | Followed by an expression regarding the expected execution state of 'x'|
| Assume.Unreachable("reason") | This expression is a special case, which will always raise an exception and does not support method chaining. |

The result of Assume.That will be an Assumption object, which should be further appended to via method chaining.

## 3. Stating the relationship

Whereas the first two words in an assumption will generally be Assume.That, declaring the actual value, the next word will typically state the relationship that should exist between the actual value and the expected value.  

The following relationships are supported:

| Relationship  | Description  |
| ------------- |:-------------|
| Equal<span>.</span>To(expected)      | actual == expected|
| Less<span>.</span>Than(expected)      | actual < expected|
| Greater<span>.</span>Than(expected)      | actual > expected |
| LessThanOrEqual<span>.</span>To(expected)      | actual <= expected |
| GreaterThanOrEqual<span>.</span>To(expected)      | actual >= expected|
| Empty() | string.IsNullOrEmpty(actual) for strings |
| Empty() |  !((ICollection)actual).Any() for collections |
| Empty() | false == ((IEnumerable)actual).GetEnumerator().MoveNext() for enumerables |
| Null() | actual == null |
| True() | actual == true |
| False() | actual == false |
| InstanceOf(expected) | typeof(expected).IsAssignableFrom(actual) if expected is a Type |
| InstanceOf(expected) | expected.Any(x => typeof(x).IsAssignableFrom(actual)) if expected is a Type[] |

## 4. Negating a relationship

Any relationship can be negated by preceeding it with a Not operator, e.g.

```C#
Assume.That(true).Is.Not.False();
```

Double-negatives are not supported.

## 5. Special case: the Completed relationship 

The 'completed' relationship can be used to test that a lambda completes without throwing an exception, e.g.:

```C#
Assume.That((a) => { throw new NotSupportedException(); }).Not.Completed();
```

## 6. Terminating words

Whereas the first two words in an assumption will generally be Assume.That, declaring the actual value, the last word will be a terminating word.  

When a terminating word is encountered, the expression will be evaluated as it stands at that point in time,  raising an AssumptionFailure if the actual value does not have the prescribed relationship to the expected value.

The following words terminate an assumption:

| Terminator  | Description  |
| ------------- |:-------------|
| Than(expected) | Terminates a comparison or equality relationship, such as Less or Equal |
| To(expected) | Synonymous with Than |
| Empty() | Tests a string or collection for emptiness |
| Null() | Synonymous with To(null) |
| True() | Synonymous with To(true) |
| False() | Synonymous with To(false) |
| InstanceOf(expected) | Terminates a type-check-relationship |

## 7. Chained assumptions

Assumptions can be chained together with the And operator.  The original actual value, declared in That(x), will stil apply, e.g.

```C#
Assume.That(x).Is.Greater.Than(y).And.Less.Than(z)
```

A new That statement may however be issued as well to change the actual value, e.g.

```C#
Assume.That(x).Is.Greater.Than(y).And.That(x + 100).Is.Less.Than(z)
```

## 8. Non-trivial words

Following 'That', every property/call will return a modified version of the Assumption instance returned from 'That'.  Trivial words will have effects or side-effects, while there are also some convenient non-trivial words that you may use with no other effect than to increase readability.

The non-trivial words supported by the Assumptions library are:

* Is, Be, An, Of, A, Been, Have, Has, With, Which, The, It

## 9. Explanations

By default, an AssumptionFailure will have a stock message explaining the condition that was amiss, e.g.

```C#
var x = 10;
var y = 12;

try
{
    Assume.That(x).Is.Greater.Than(y);
}
catch (AssumptionFailure ex)
{
    Assert.Equal("Expected '10' to be greater than '12'", ex.Message);
}
```

Optionally, an explanatory message can be added to the assumption's terminating word, e.g.

```C#
var x = 40;
var y = 42;

try
{
    Assume.That(x).Is.Equal.To(y, "The answer to the ultimate question is 42");
}
catch (AssumptionFailure ex)
{
    Assert.Equal("The answer to the ultimate question is 42. Expected '40' to be equal to '42'", ex.Message);
}
```

Note that each terminating word in a chained expression should provide its own explanation.

#### Example:

```C#
Assert.That(a).Has.Been.Less.Than(b);
```

# A. Detailed Topics

## A1. Implicit vs explicit assumptions

### Implicit assumptions

Implicit assumptions are made when results are not checked, branches are not included, exceptions are not caught, collections are assumed to contain elements, and so forth.

Let as explore a common example:

#### Example: Missing null checks

When a piece of code assumes that a reference is not null by deferencing without testing for validity, then an assumption is implicitly made that the reference is indeed valid.

Typically, this assumption may hold. If it does not however, then a NullReference will be thrown. 

While this may appear at face value to be less severe than an access violation, there is little difference in the eyes of the user who's application failed.


```C#
// pattern 1: a typical missing null check
var foo = Blat();
var splat = foo.Bar();
splat.Bark();
```

This code can of course be patched up by making the implicit explicit, and making the code more fault tolerant, e.g.:

```C#
// pattern 2: a typical fault tolerate null check addition
var foo = Blat();
if (foo != null)
{
    var splat = foo.Bar();
    if (splat != null)
    {
        splat.Bark();
    }
}
```

This gets pretty verbose pretty fast, so people tended to skip this pattern rather frequently.  Also there is now no indication that anything is amiss.  That may e.g. be remedied with logging:

```C#
// pattern 3: a typical fault tolerate null check addition, with logging
var foo = Blat();
if (foo != null)
{
    var splat = foo.Bar();
    if (splat != null)
    {
        splat.Bark();
    }
    else
    {
        log.Warn("splat was null!");
    }
}
else
{
    log.Warn("Foo was null!");
}
```

We could also have gone all-in on this code and made the holes more explicit.  This would be more informative when we sift through our logs, but gets extremely verbose extremely fast:

```C#
// pattern 3: a typical robust, explicit, fault intolerant solution
var foo = Blat();
if (foo == null)
{
    throw new BlatException("Foo was null!");
}

var splat = foo.Bar();
if (splat == null)
{
    throw new BlatBarException("Bar returned null.  Perhaps the code is fubar?");
}
splat.Bark();
```
This is certainly more robust, and can be considered more explanatory.  But now its hard for the caller to know whether the exceptions are plausible cases or highly abnormal.  And this is a bit repetitive, ugly, and verbose.  Its not really a very sustainable coding style, and it is costly both from a readability and writability perspective.

So typically code was written more frequently according to pattern 1.  So frequently in fact that C# has introduced syntactic sugar to make it easier to at least "level up" to pattern 2, where there is some level of fault tolerance.  Now were roughly back where we started, syntactically speaking, and we're not throwing cryptic NullReference exceptions to boot:

```C#
// pattern 4: a fault-tolerant implementation using the ?. shorthand.
var foo = Blat();
var splat = foo?.Bar();
splat?.Bark();
```

But now we also have to henceforth deal with the fact that foo and splat may indeed be null, and we may or may not have invoked foo.Bar() and/or splat.Bark().  Are we better off? Perhaps. Perhaps sometimes. Perhaps sometimes not.

 Every time we write code, we make assumptions. For the most part, our assumptions hold true.  But when they don't... We experience bugs, crashes, cryptic log messages, incomprehensible metrics, generic null reference exceptions, division by zero, exceptions in code that has no exception handling, and many more indications of the issue.

These indicators are the ***effect***, not the ***cause***.

The cause is typically the same: Somewhere, someone made an ***incorrect, implicit assumption***.

## Explicit assumptions

The *Assumptions* library allows you to transform implicit assumptions into explicit ones, offering an alternative solution.

```C#
// pattern 5: Explicit assumptions
var foo = Blat();
Assume.That(foo).Is.Not.Null("Foo was null, which was highly unexpected and implies that the Blat library is broken");

var splat = foo.Bar();
Assume.That(splat).Is.Not.Null("There was no splatter available for fubarring");

splat.Bark();
```

The added value in this code is that the reader knows that we have thought of the edge cases.  It may be verbose, but we have codified our knowledge, provided specific explanations which will presumably reach our logs. And we have expressed to the reader that these are not natural conditions which the caller is expected to provide exception handling for. 

### A few more examples

Here are a few more examples, for inspiration:

```C#

void Foo(string bar)
{
    Assume.That(bar).Is.Not.Null();

    Console.WriteLine($"Explanation: {bar}")
}

void Fu(int bar)
{
    Assume.That(bar).Is.Not.Equal.To(4);

    return fu / (bar - 4);
}

void Transfer(string fromAccount, string toAccount, int amount)
{
    // the client would not have issued this call naturally if this were not the case, but we still better check this, in case of hackers

    var owner = OwnerOf(fromAccount);
    var user = CurrentUser();
    Assume.That(user).Is.Equal.To(owner);

    Assume.That(fromAccount).Is.Not.Equal.To(toAccount);
    Assume.That(amount).Is.Greater.Than(0);
}
```
