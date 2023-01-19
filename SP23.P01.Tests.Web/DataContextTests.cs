using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SP23.P01.Tests.Web.Helpers;

namespace SP23.P01.Tests.Web;

[TestClass]
public class DataContextTests
{
    private WebTestContext context = new();

    [TestInitialize]
    public void Init()
    {
        context = new WebTestContext();
    }

    [TestCleanup]
    public void Cleanup()
    {
        context.Dispose();
    }

    [TestMethod]
    public void DataContext_IsOneDeclared()
    {
        var type = typeof(Program).Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(DbContext))).ToList();
        Assert.IsTrue(type.Count > 0, "You don't have a DbContext declared yet");
        Assert.IsFalse(type.Count > 1, "You have more than one data context created");
        Assert.IsTrue(type[0].Name == "DataContext", "You need to call your DbContext class 'DataContext' not " + type[0].Name);
    }

    [TestMethod]
    public void DataContext_RegisteredInServices()
    {
        var type = typeof(Program).Assembly.GetTypes().FirstOrDefault(x => x.IsSubclassOf(typeof(DbContext)));
        if (type == null)
        {
            Assert.Fail("Not ready for this test");
            return;
        }

        using var scope = context.GetServices().CreateScope();
        var dbContext = GetDataContext(scope);

        Assert.IsNotNull(dbContext, "You need to register your DB context");
    }

    [TestMethod]
    public void DataContext_HasTrainStation()
    {
        using var scope = context.GetServices().CreateScope();
        var dbContext = GetDataContext(scope);
        if (dbContext == null)
        {
            Assert.Fail("Not ready for this test");
            return;
        }

        EnsureSet("TrainStation", dbContext);
    }

    private static void EnsureSet(string modelName, DbContext dataContext)
    {
        var entityType = GetEntityType(modelName);
        var method = dataContext.GetType().GetMethod("Set", BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.Any, new Type[0], null);
        var generic = method?.MakeGenericMethod(entityType);
        dynamic collection = generic?.Invoke(dataContext, null) ?? throw new Exception($"You don't have '{modelName}' registered in your DataContext");

        try
        {
            Enumerable.ToList(collection);
        }
        catch (InvalidOperationException e)
        {
            throw new Exception($"it doesn't look like you registered '{modelName}' on your DbContext", e);
        }
        catch (Exception e)
        {
            throw new Exception($"Attempting to call 'ToList' on your '{modelName}' set didn't work. Are you automatically migrating / seeding on startup?", e);
        }
    }

    private static Type GetEntityType(string entityTypeName)
    {
        try
        {
            var entityType = typeof(Program).Assembly.GetTypes().Single(x => x.Name == entityTypeName);
            return entityType;
        }
        catch (Exception e)
        {
            throw new Exception($"You don't have the entity '{entityTypeName}' declared at all", e);
        }
    }

    private static DbContext? GetDataContext(IServiceScope scope)
    {
        try
        {
            var type = typeof(Program).Assembly.GetTypes().Single(x => x.IsSubclassOf(typeof(DbContext)));
            var dataContext = (DbContext)scope.ServiceProvider.GetService(type)!;
            return dataContext;
        }
        catch
        {
            return null;
        }
    }
}
