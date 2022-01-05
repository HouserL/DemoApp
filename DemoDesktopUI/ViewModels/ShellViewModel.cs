using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using DemoDesktopUI.EventModels;

namespace DemoDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private IEventAggregator _events;
        private SalesViewModel _salesVM;
        private SimpleContainer _container;

        public ShellViewModel(IEventAggregator events, SimpleContainer container, SalesViewModel salesVM)
        {
            _events = events;
            _salesVM = salesVM;
            _container = container;

            _events.SubscribeOnPublishedThread(this);
            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }

        public Task HandleAsync(LogOnEvent message, CancellationToken cancellationToken)
        {
            ActivateItemAsync(_salesVM);
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }

        public void SalesScreen()
        {
            ActivateItemAsync(_salesVM);            
        }

        public void LoginScreen()
        {
            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }
    }
}
