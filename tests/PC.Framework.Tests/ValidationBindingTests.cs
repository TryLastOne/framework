using System.ComponentModel.DataAnnotations;
namespace PC.Framework.Tests;


public class ValidationBindingTests
{
    // TODO: test localizationmanager
    // TODO: test localize
    // TODO: test blank message

    public void Test()
    {
        var services = new ServiceCollection();
        //services.AddGlobalCommandExceptionAction();        
        //services.AddConnectivity();
        //services.AddScoped<BaseServices>();

        var sp = services.BuildServiceProvider();
        var vm = sp.GetRequiredService<StandardViewModel>();

    }
}


public class StandardViewModel(BaseServices services) : ViewModel(services)
{
    [Reactive]
    [Required(AllowEmptyStrings = false)]
    [StringLength(5, MinimumLength = 3)]
    public string String { get; set; } = string.Empty;
}


public class LocalizedViewModel(BaseServices services) : ViewModel(services)
{
    [Reactive]
    [Required(AllowEmptyStrings = false, ErrorMessage = "localize:ErrorString")]
    public string String { get; set; } = string.Empty;
}