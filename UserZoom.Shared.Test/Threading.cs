using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Concurrent;
using System.Dynamic;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace UserZoom.Shared.Test
{
    public class Customer
    {
        public string Name { get; set; }
    }

    public class MyDynamicObject : DynamicObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = "hello world";

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(binder.Name));

            return base.TrySetMember(binder, value);
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            return base.TryGetIndex(binder, indexes, out result);
        }
    }

    public class Synchronized
    {
        private List<Customer> _customers = new List<Customer>();

        public List<Customer> Customers
        {
            get { return _customers; }
            set { _customers = value; }
        }

        public Synchronized DoX()
        {
            return this;
        }

        public void ThreadSynchronization()
        {
            List<Customer> customers = new List<Customer>();
            customers.AddRange(Customers);
            customers = Customers.OrderByDescending(c => c.Name).ToList();
            Interlocked.Exchange(ref _customers, customers);


            //AutoResetEvent resetEvent = new AutoResetEvent(true);
            //resetEvent.WaitOne();
            //try
            //{

            //    if (Customers.Count > 10)
            //    {
            //        Thread.Sleep(3000);
            //        Customers.Clear();
            //    }
            //}
            //finally
            //{
            //    resetEvent.Set();
            //}

            // Para sincronización entre procesos (apps) distintas)
            //Mutex mutex = new Mutex(false, "mutex1");

            //mutex.WaitOne();

            //try
            //{

            //    if (Customers.Count > 10)
            //    {
            //        Thread.Sleep(3000);
            //        Customers.Clear();
            //    }
            //}
            //finally
            //{
            //    mutex.ReleaseMutex();
            //}


            // Con Monitor
            //lock(Customers)
            //{
            //    if (Customers.Count > 10)
            //    {
            //        Thread.Sleep(3000);
            //        Customers.Clear();
            //    }
            //}


            // Monitor.Enter(Customers);
            //try
            //{
            //    // Código crítico
            //}
            //finally
            //{
            //    Monitor.Exit(Customers);
            //}

        }
    }

    [TestClass]
    public class Threading
    {
        [TestMethod]
        public void Dynamic()
        {
            dynamic myDyn = new MyDynamicObject();
            string abc = myDyn.meLoInvento;

            ((INotifyPropertyChanged)myDyn).PropertyChanged += (sender, e) =>
            {
                string addedProperty = e.PropertyName;
            };
            
            string jsonText = @"{ ""name"": ""matias"" }";
            dynamic someObject = JsonConvert.DeserializeObject<ExpandoObject>
            (
                jsonText, 
                new ExpandoObjectConverter()
            );

            Synchronized sync = new Synchronized();

            if(sync.GetType().GetProperty("x") != null)
            {

            }

            var a = "";
            dynamic b = new Synchronized();
            b.DoStuff11111111();

            b = 11;

            // Expando objects
            dynamic expando = new ExpandoObject();
            expando.text = "hola mundo";
            expando.number = 30;
            expando.doStuff = new Action(() =>
            {
                expando.x = 11;
            });
            expando.doStuff();

            IDictionary<string, object> dict = (IDictionary<string, object>)expando;

            if(dict.ContainsKey("doStuff"))
            {
                expando.doStuff();
            }


        }

        [TestMethod]
        public void TestMethod1()
        {
            Synchronized synchro = new Synchronized();

            Task.Run
            (
                () =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        synchro.Customers.Add(new Customer());
                    }
                }
            );

            for (int i = 0; i < 1000; i++)
            {
                Task.Run(() => synchro.ThreadSynchronization());
            }

            synchro.ThreadSynchronization();

        }

        [TestMethod]
        public void TaskApi()
        {
            // Task Asynchronous Pattern (TAP)
            // Task Parallel Library (TPL)
            Task t1 = new Task(() => Console.WriteLine("hello"));
            t1.Start();

            Task t2 = Task.Factory.StartNew(() => Console.WriteLine("hello"));

            // OPCIÓN RECOMENDADA
            Task<string> t3 = Task.Run(() => "hello");

            // NO UTILICÉIS ESTO JAMÁS
            t3.Wait();
            string result = t3.Result;
            //

            // Continuaciones CLOSURE
            t3.ContinueWith
            (
                t =>
                {
                    result = t.Result;
                }
            );

            ContinueTask(t3);

            Task.WhenAll(t1, t2, t3).ContinueWith(t => Console.WriteLine("han acabado todas"));
            Task.WhenAny(t1, t2, t3).ContinueWith(t => Console.WriteLine("ha acabado por lo menos 1"));

            Task timeoutTask = Task.Delay(1000);
            Task all = Task.WhenAll(t1, t2, t3).ContinueWith
            (
                t =>
                {
                    if (!timeoutTask.IsCompleted)
                    {
                        // Sigo
                    }
                }
            );

            timeoutTask.ContinueWith
            (
                t =>
                {
                    if (!all.IsCompleted)
                        throw new InvalidOperationException("timeout");
                }
            );

            // Código fire and forget
            Console.WriteLine("Ya he encolado todo");
        }

        public async Task<string> DoAsync()
        {
            return "hello world";
        }

        [TestMethod]
        public async Task TaskApiWithAsyncAwait()
        {
            // OPCIÓN RECOMENDADA
            Task<string> t1 = Task.Run(() => "hello");
            Task<string> t2 = Task.FromResult("hello world");

            TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
            tcs.SetResult("hello world 2");

            // Lo que sea

            string text = await t1;
            string text2 = await t2;
            string text3 = await tcs.Task;
            
            if(text.Length > 0)
            {

            }
        }

        [TestMethod]
        public void CancellingTasks()
        {
            Func<CancellationToken, Task> doStuff = cancellationToken =>
            {
                return Task.Run
                (
                    () =>
                    {
                        while (!cancellationToken.IsCancellationRequested) ;

                        // Esto sería para que, si la cancelación se pide, este thread
                        // acabe y también el thread padre y como el thread padre también
                        // acabaría, el resto de hijos también acabarían.
                        //while (true)
                        //    cancellationToken.ThrowIfCancellationRequested();
                    }
                );
            };

            CancellationTokenSource cancellationSource = new CancellationTokenSource();
            doStuff(cancellationSource.Token).ContinueWith
            (
                t =>
                {
                    if(t.Exception.InnerException is InvalidOperationException)
                    {

                    }
                },
                TaskContinuationOptions.OnlyOnFaulted
            ).ContinueWith
            (
                t =>
                {

                },
                TaskContinuationOptions.NotOnFaulted

            );
            Thread.Sleep(5000);

            cancellationSource.Cancel();
        }

        public void ContinueTask(Task<string> t1)
        {
            t1.ContinueWith(t => Console.WriteLine(t.Result));
        }
    }
}
