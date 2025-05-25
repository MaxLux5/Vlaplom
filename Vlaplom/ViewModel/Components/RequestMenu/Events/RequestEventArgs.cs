using Vlaplom.ViewModel.Components.Helpers;

namespace Vlaplom.EventArguments
{
    public class RequestEventArgs : EventArgs
    {
        public RequestViewModel Request { get; private set; }

        public RequestEventArgs(RequestViewModel request)
        {
            Request = request;
        }
    }
}
