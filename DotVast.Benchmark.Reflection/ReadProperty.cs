using System.Linq.Expressions;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace DotVast.Benchmark.Reflection;

[SimpleJob(runtimeMoniker: RuntimeMoniker.Net60)]
[SimpleJob(runtimeMoniker: RuntimeMoniker.Net70, baseline: true)]
[MemoryDiagnoser]
public class ReadProperty
{
    private readonly User _user = new();

    [Benchmark]
    public string ReadPub()
    {
        return _user.Pub;
    }

    ///////////////////////////////////////////////////////////////////////////////////
    /// Reflection

    [Benchmark]
    public string ReadPriByReflection()
    {
        var propertyInfo = typeof(User).GetProperty("Pri", BindingFlags.Instance | BindingFlags.NonPublic)!;
        return (string)propertyInfo.GetValue(_user)!;
    }

    static readonly PropertyInfo _propInfo = typeof(User).GetProperty("Pri", BindingFlags.Instance | BindingFlags.NonPublic)!;

    [Benchmark]
    public string ReadPriByCachedReflection()
    {
        return (string)_propInfo.GetValue(_user)!;
    }

    ///////////////////////////////////////////////////////////////////////////////////
    /// Delegate

    [Benchmark]
    public string ReadPriByDelegate()
    {
        var propertyInfo = typeof(User).GetProperty("Pri", BindingFlags.Instance | BindingFlags.NonPublic)!;
        var delFunc = (Func<User, string>)Delegate.CreateDelegate(typeof(Func<User, string>), propertyInfo.GetMethod!);
        return delFunc(_user);
    }

    static readonly Func<User, string> _delFunc = (Func<User, string>)Delegate.CreateDelegate(typeof(Func<User, string>), _propInfo.GetMethod!);

    [Benchmark]
    public string ReadPriByCachedDelegate()
    {
        return _delFunc(_user);
    }

    ///////////////////////////////////////////////////////////////////////////////////
    /// Expression

    [Benchmark]
    public string ReadPriByExpression()
    {
        var propertyInfo = typeof(User).GetProperty("Pri", BindingFlags.Instance | BindingFlags.NonPublic)!;
        var parameter = Expression.Parameter(typeof(User));
        var propertyExpression = Expression.Property(parameter, propertyInfo);
        var expFunc = Expression.Lambda<Func<User, string>>(propertyExpression, parameter).Compile();
        return expFunc(_user);
    }

    static readonly ParameterExpression _parameter = Expression.Parameter(typeof(User));
    static readonly MemberExpression _propExpression = Expression.Property(_parameter, _propInfo);
    static readonly Func<User, string> _expFunc = Expression.Lambda<Func<User, string>>(_propExpression, _parameter).Compile();

    [Benchmark]
    public string ReadPriByCachedExpression()
    {
        return _expFunc(_user);
    }
}
