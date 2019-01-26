# Assumptions

The Assumptions library provides a clean method to make implicit assumptions explicit in C#.

## Why?

Have you ever written code that has holes in it? Assumed that a reference was not null? A collection not null, and certainly not empty. A signed integer to contain a positive value...

Sure you have. We all have. Every time we write code we make assumptions. For the most part, our assumptions hold true and there's nothing to worry about.  But when they don't... We experience bugs, crashes, log messages, metrics, null reference exceptions, division by zero, exceptions in code that has no exception handling, and many more indications of the issue.

These indicators are the ***effect***, not the ***cause***.  The cause is almost always the same: Somewhere, someone made an ***incorrect, implicit assumption*** that just got triggered.

## The value of explicit assumptions

The *Assumptions* library allows you to transform implicit assumptions into explicit ones.

You do not have to explain yourself, much less create a new return code or exception class to handle your bizzarre edge case.

Simply describe your assumption, informing future developers of the edge case and trust that the assumption engine will throw an exception if the assumption fails.

## TODO

More documentation and features pending.

### Examples

```C#
using Assumptions;

...

void Foo(string bar)
{
    Assume.
        That(bar).
        Is.NotNull();
}

void Fu(int bar)
{
    Assume.
        That(bar).
        Is.GreaterThan(4);
}

void Blat()
{
    Assume.
        That(true).
        Is.True();
}
```
