using Autofac;
using EmberKernel.Plugins;
using EmberKernel.Plugins.Attributes;
using EmberKernel.Plugins.Components;
using EmberKernel.Services.UI.Mvvm.Extension;
using Statistic.WpfUI.UI.View;
using Statistic.WpfUI.UI.ViewModel;
using System.Threading.Tasks;

namespace Statistic.WpfUI
{
    [EmberPlugin(Author = "ZeroAsh", Name = "Statistic UI", Version = "1.0")]
    public class StatisticUI : Plugin
    {
        public override void BuildComponents(IComponentBuilder builder)
        {
            builder.ConfigureComponent<StatisticEditorViewModel>().AsSelf().As<IEditorContextViewModel>().SingleInstance().PropertiesAutowired();
            builder.ConfigureUIComponent<StatisticEditorTab>();
        }

        public override async ValueTask Initialize(ILifetimeScope scope)
        {
            await scope.InitializeUIComponent<StatisticEditorTab>();
        }

        public override async ValueTask Uninitialize(ILifetimeScope scope)
        {
            await scope.UninitializeUIComponent<StatisticEditorTab>();
        }
    }
}
