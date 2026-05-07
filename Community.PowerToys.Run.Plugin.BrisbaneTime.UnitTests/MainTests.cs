using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.BrisbaneTime.UnitTests;

[TestClass]
public sealed class MainTests
{
    [TestMethod]
    public void Query_returns_conversion_results()
    {
        var main = new Main();

        var results = main.Query(new Query("10:30 PM ET"));

        Assert.IsTrue(results.Count > 0);
        StringAssert.Contains(results[0].Title, "Brisbane time");
    }

    [TestMethod]
    public void LoadContextMenus_returns_copy_action_for_conversion()
    {
        var main = new Main();
        var result = main.Query(new Query("10:30 PM ET")).First();

        var menus = main.LoadContextMenus(result);

        Assert.AreEqual(1, menus.Count);
    }
}
