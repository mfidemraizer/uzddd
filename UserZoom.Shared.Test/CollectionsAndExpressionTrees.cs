using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using UserZoom.Domain;

namespace UserZoom.Shared.Test
{


    public class A
    {
        public A(string text, int number)
        {
            Text = text;
            Number = number;
        }

        public string Text { get; set; }
        public int Number { get; set; }

        [ContractInvariantMethod]
        private void Invariants()
        {
            Contract.Invariant(!string.IsNullOrEmpty(Text));
            Contract.Invariant(Number > 0);
        }

        public string DoStuff(string name, int value)
        {
            // Precondiciones
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(value > 0);
            // Postcondición
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            string result = null;

            // Fallaría el Ensures porque result no está garantizado que no sea nulo
            // o vacío
            return result;

        }
    }

    public class MemberSelector
    {
        public static object GetPropertyValue(object theObject, string propertyName)
        {
            return theObject.GetType().GetProperty(propertyName).GetValue(theObject);
        }

        public static TReturnValue GetPropertyValue<TReturnValue>(Func<TReturnValue> selector)
        {
            return selector();
        }

        public static string ConcatNames(IEnumerable<string> names)
        {
            return string.Join(", ", names);
        }

        // Quiero un método que reciba con árboles de expresiones 
        // nombres de propiedades para luego devolver esos nombres concatenados
        public static string ConcatNames<T>(params Expression<Func<T, object>>[] propertySelectors)
        {
            Contract.Requires(propertySelectors != null && propertySelectors.Length > 0);
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));

            StringBuilder namesBuilder = new StringBuilder();

            foreach (Expression<Func<T, object>> propertySelector in propertySelectors)
            {
                MemberExpression propertyExpr = propertySelector.Body as MemberExpression;
                Contract.Assert(propertyExpr != null);

                namesBuilder.Append($"{propertyExpr.Member.Name},");
            }

            return namesBuilder.ToString().TrimEnd(',');

            //return string.Join
            //(
            //    ",",
            //    propertySelectors.Select(s => ((MemberExpression)s.Body).Member.Name)
            //);
        }

        public static string ConcatNames(object some)
        {
            StringBuilder namesBuilder = new StringBuilder();

            foreach (PropertyInfo property in some.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                namesBuilder.Append($"{property.Name},");
            }

            return namesBuilder.ToString().TrimEnd(',');
        }
    }

    [TestClass]
    public class CollectionsAndExpressionTrees
    {
        [TestMethod]
        public void DelegatesAndExpressionTrees()
        {
            // Cómo generar un árbol de expresiones equivalente
            // a "a => a.Text"
            ParameterExpression param1Expr = Expression.Parameter(typeof(A), "a");
            MemberExpression propertyAccessExpr = Expression.Property
            (
                param1Expr,
                "Text"
            );

            Expression<Func<A, object>> expr2 = Expression.Lambda<Func<A, object>>
            (
                propertyAccessExpr,
                param1Expr
            );

            A a = new A("hello world", 2);
            a.Text = "hello world";

            string text = MemberSelector.GetPropertyValue(a, "Text") as string;

            text = MemberSelector.GetPropertyValue(() => a.Text);

            string concat = MemberSelector.ConcatNames
            (
                new[]
                {
                    "A.Text",
                    nameof(a.Number)
                }
            );

            string properties = "Name,Number";
            List<Expression<Func<A, object>>> propertySelectorList = new List<Expression<Func<A, object>>>();

            foreach (string property in properties.Split(','))
            {
                // a =>
                ParameterExpression param2Expr = Expression.Parameter(typeof(A), "a");

                // a.[Propiedad] (por ejemplo a.Text)
                MemberExpression propertyAccessExpr2 = Expression.Property
                (
                    param2Expr,
                    property
                );

                // Esto es el pegamento que une el parámetro de entrada con 
                // el cuerpo de la expresión
                Expression<Func<A, object>> expr3 = Expression.Lambda<Func<A, object>>
                (
                    propertyAccessExpr2,
                    param2Expr
                );

                propertySelectorList.Add(expr3);
            }

            Func<A, object> func3 = propertySelectorList[0].Compile();
            object result = func3(new A("asdsdaas", 3389));

            string text3 = MemberSelector.ConcatNames(propertySelectorList.ToArray());

            string text2 = MemberSelector.ConcatNames<A>
            (
                some => some.Text,
                some => some.Number
            );
        }

        private string X()
        {
            return "";
        }

        [TestMethod]
        public void Contracts()
        {
            A a = new A("a", 3893);
            a.DoStuff("839", 38903);
        }

        [TestMethod]
        public void TaskIsUnique()
        {
            UZTask t1 = new UZTask { Id = Guid.NewGuid() };
            UZTask t2 = new UZTask { Id = Guid.NewGuid() };
            UZTask t3 = new UZTask { Id = Guid.NewGuid() };
            UZTask t4 = new UZTask { Id = Guid.NewGuid() };

            HashSet<UZTask> tasksSet1 = new HashSet<UZTask>(new UZTask.IdComparer());
            tasksSet1.Add(t1);
            tasksSet1.Add(t2);
            tasksSet1.Add(t3);
            tasksSet1.Add(t4);

            HashSet<UZTask> tasksSet2 = new HashSet<UZTask>(new UZTask.IdComparer());
            tasksSet2.Add(t2);
            tasksSet2.Add(t4);

            IEnumerable<UZTask> tasks3 = tasksSet1.Intersect(tasksSet2, new UZTask.IdComparer());
            tasksSet1.IntersectWith(tasksSet2);

            Dictionary<UZTask, string> taskDictionary = new Dictionary<UZTask, string>(new UZTask.IdComparer());
            taskDictionary.Add(new UZTask { Id = Guid.NewGuid() }, "hello world");

            List<string> list = new List<string> { "hola", "adios" };
            IImmutableList<string> immutableList = list.ToImmutableList();
        }
    }
}
