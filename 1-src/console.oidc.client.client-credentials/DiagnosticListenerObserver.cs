using System;
using System.Diagnostics;

namespace ClientCredentials
{
    public class DiagnosticListenerObserver : IObserver<DiagnosticListener>
    {
        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(DiagnosticListener value)
        {
            value.Subscribe(new DefaultObserver());
        }
    }
}