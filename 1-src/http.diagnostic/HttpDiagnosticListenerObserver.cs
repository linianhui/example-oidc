using System;
using System.Diagnostics;

namespace Http.Diagnostic
{
    public class HttpDiagnosticListenerObserver : IObserver<DiagnosticListener>
    {
        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(DiagnosticListener value)
        {
            value.Subscribe(new HttpDefaultObserver());
        }
    }
}