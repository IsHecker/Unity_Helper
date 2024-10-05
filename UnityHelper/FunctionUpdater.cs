using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityHelper
{
    public class FunctionUpdater
    {
        private static readonly Dictionary<Action, UpdateCancellationToken> functionDictionary = new();

        /// <summary>
        /// Updates <paramref name="function"/> Repeatidly Every Frame While <paramref name="stopCondition"/> is true. 
        /// </summary>
        /// <param name="function"> The Method to be updated (don't use Lamda Exp).</param>
        /// <param name="stopCondition"> The Condition to keep updating. </param>
        public static void Update(Action function, Func<bool> stopCondition, bool reset = false)
        {
            if (!functionDictionary.TryGetValue(function, out UpdateCancellationToken cancellationToken))
            {
                CreateUpdate(function, stopCondition);
                return;
            }

            if (!reset)
                return;

            Stop(function);
            _ = StartUpdate(function, stopCondition, cancellationToken);
        }

        public static void Stop(Action function)
        {
            functionDictionary[function].Cancel();
        }

        public static void StopAll()
        {
            foreach (var token in functionDictionary.Values)
            {
                token.Cancel();
            }
        }

        private static void CreateUpdate(Action function, Func<bool> stopCondition)
        {
            var cancellationToken = new UpdateCancellationToken();
            functionDictionary[function] = cancellationToken;

            _ = StartUpdate(function, stopCondition, cancellationToken);
        }

        private static async Task StartUpdate(Action function, Func<bool> stopCondition, UpdateCancellationToken cancellationToken)
        {
            cancellationToken.Reset();

            while (!stopCondition() && !cancellationToken.IsCanceled)
            {
                function.Invoke();
                await Task.Yield();
            }
        }


        private unsafe class BoolPtr
        {
            private readonly bool* ptr;
            public BoolPtr(bool* value)
            {
                ptr = value;
            }
            public void SetPtr(bool value) => *ptr = value;
            public bool GetPtr() => *ptr;
        }

        private class UpdateCancellationToken
        {
            public volatile bool IsCanceled = false;

            public void Cancel() => IsCanceled = true;

            public void Reset() => IsCanceled = false;
        }
    }
}
