using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Community.PowerToys.Run.Plugin.BrisbaneTime.UnitTests;

[TestClass]
public sealed class BrisbaneTimeConverterTests
{
    private static readonly DateTimeOffset MayUtc = new(2026, 5, 7, 12, 0, 0, TimeSpan.Zero);
    private static readonly DateTimeOffset JanuaryUtc = new(2026, 1, 7, 12, 0, 0, TimeSpan.Zero);

    [TestMethod]
    public void Convert_handles_eastern_time_with_daylight_saving()
    {
        var results = BrisbaneTimeConverter.Convert("10:30 PM ET", MayUtc);

        Assert.AreEqual(1, results.Count);
        Assert.AreEqual("12:30 PM Brisbane time", results[0].Title);
        StringAssert.Contains(results[0].Subtitle, "Fri 8 May 2026");
    }

    [TestMethod]
    public void Convert_handles_fixed_est_offset()
    {
        var results = BrisbaneTimeConverter.Convert("22:30 EST", MayUtc);

        Assert.AreEqual(1, results.Count);
        Assert.AreEqual("1:30 PM Brisbane time", results[0].Title);
    }

    [TestMethod]
    public void Convert_returns_am_and_pm_for_ambiguous_word_times()
    {
        var results = BrisbaneTimeConverter.Convert("ten thirty CST", JanuaryUtc);

        Assert.AreEqual(2, results.Count);
        Assert.AreEqual("2:30 AM Brisbane time", results[0].Title);
        Assert.AreEqual("2:30 PM Brisbane time", results[1].Title);
    }

    [TestMethod]
    public void Convert_handles_quarter_past()
    {
        var results = BrisbaneTimeConverter.Convert("quarter past ten pm PT", MayUtc);

        Assert.AreEqual(1, results.Count);
        Assert.AreEqual("3:15 PM Brisbane time", results[0].Title);
    }

    [TestMethod]
    public void Convert_handles_twenty_two_thirty_words()
    {
        var results = BrisbaneTimeConverter.Convert("twenty two thirty EST", MayUtc);

        Assert.AreEqual(1, results.Count);
        Assert.AreEqual("1:30 PM Brisbane time", results[0].Title);
    }

    [TestMethod]
    public void Convert_handles_iso_date_before_time()
    {
        var results = BrisbaneTimeConverter.Convert("2026-05-10 22:30 EST", MayUtc);

        Assert.AreEqual(1, results.Count);
        Assert.AreEqual("1:30 PM Brisbane time", results[0].Title);
        StringAssert.Contains(results[0].Subtitle, "Mon 11 May 2026");
    }

    [TestMethod]
    public void Convert_handles_month_name_dates()
    {
        var results = BrisbaneTimeConverter.Convert("May 10 10:30 PM ET", MayUtc);

        Assert.AreEqual(1, results.Count);
        Assert.AreEqual("12:30 PM Brisbane time", results[0].Title);
        StringAssert.Contains(results[0].Subtitle, "Mon 11 May 2026");
    }

    [TestMethod]
    public void Convert_handles_day_month_dates()
    {
        var results = BrisbaneTimeConverter.Convert("10 May 10:30 PM ET", MayUtc);

        Assert.AreEqual(1, results.Count);
        Assert.AreEqual("12:30 PM Brisbane time", results[0].Title);
        StringAssert.Contains(results[0].Subtitle, "Mon 11 May 2026");
    }

    [TestMethod]
    public void Convert_handles_next_weekday()
    {
        var results = BrisbaneTimeConverter.Convert("next friday ten thirty pm ET", MayUtc);

        Assert.AreEqual(1, results.Count);
        Assert.AreEqual("12:30 PM Brisbane time", results[0].Title);
        StringAssert.Contains(results[0].Subtitle, "Sat 9 May 2026");
    }

    [TestMethod]
    public void Convert_handles_central_european_abbreviations()
    {
        var cet = BrisbaneTimeConverter.Convert("10 May 10:00 AM CET", MayUtc);
        var cest = BrisbaneTimeConverter.Convert("10 May 10:00 AM CEST", MayUtc);

        Assert.AreEqual(1, cet.Count);
        Assert.AreEqual("7:00 PM Brisbane time", cet[0].Title);
        Assert.AreEqual(1, cest.Count);
        Assert.AreEqual("6:00 PM Brisbane time", cest[0].Title);
    }

    [TestMethod]
    public void Convert_handles_named_european_time_with_daylight_saving()
    {
        var results = BrisbaneTimeConverter.Convert("10 May 10:00 AM Central European Time", MayUtc);

        Assert.AreEqual(1, results.Count);
        Assert.AreEqual("6:00 PM Brisbane time", results[0].Title);
    }

    [TestMethod]
    public void Convert_handles_utc_offset_timezones()
    {
        var results = BrisbaneTimeConverter.Convert("10 May 10:00 AM GMT-0530", MayUtc);

        Assert.AreEqual(1, results.Count);
        Assert.AreEqual("1:30 AM Brisbane time", results[0].Title);
        StringAssert.Contains(results[0].Subtitle, "Mon 11 May 2026");
    }

    [TestMethod]
    public void Convert_handles_weekday_absolute_date_before_time()
    {
        var results = BrisbaneTimeConverter.Convert("Saturday, 16 May 2026, at 15:00 CEST", MayUtc);

        Assert.AreEqual(1, results.Count);
        Assert.AreEqual("11:00 PM Brisbane time", results[0].Title);
        StringAssert.Contains(results[0].Subtitle, "Sat 16 May, 3:00 PM CEST");
    }

    [TestMethod]
    public void Convert_reports_missing_timezone()
    {
        var results = BrisbaneTimeConverter.Convert("10:30 PM", MayUtc);

        Assert.AreEqual(1, results.Count);
        Assert.IsFalse(results[0].Success);
        StringAssert.Contains(results[0].Subtitle, "source timezone");
    }
}
