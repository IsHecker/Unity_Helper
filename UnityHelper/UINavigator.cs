using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHelper.Utilities;

namespace UnityHelper
{
    public class UINavigator
    {
        private readonly Animator _animator;
        private readonly Dictionary<string, GameObject> _navigations;
        private readonly Stack<GameObject> _panelStack;
        private readonly string _defaultAnimationName;

        public static Dictionary<string, object> Arguments { get; private set; }

        public UINavigator(Animator animator, Dictionary<string, GameObject> navigations, string defaultAnimationName = "")
        {
            _animator = animator;
            _navigations = navigations ?? throw new ArgumentNullException(nameof(navigations), "Navigation dictionary cannot be null.");
            _panelStack = new Stack<GameObject>();
            _defaultAnimationName = defaultAnimationName;
        }

        public void PushNamed(string route, string animationName = "", Dictionary<string, object> args = null)
        {
            if (!_navigations.TryGetValue(route, out GameObject screen))
            {
                throw new KeyNotFoundException($"Route '{route}' doesn't exist.");
            }

            if (_panelStack.Count > 0)
            {
                _panelStack.Peek().SetActive(false);
            }

            screen.SetActive(true);
            _panelStack.Push(screen);

            _animator.SetFloat("direction", 1);
            _animator.Play(animationName != "" ? animationName : _defaultAnimationName);

            Arguments = args;
        }
        public void Pop(string animationName = "", Dictionary<string, object> args = null)
        {
            if (_panelStack.Count == 0)
            {
                throw new InvalidOperationException("No routes to pop.");
            }

            CoroutineRunner.Instance.StartCoroutine(PopHandle(animationName != "" ? animationName : _defaultAnimationName));
            Arguments = args;
        }
        private IEnumerator PopHandle(string animationName = "")
        {
            GameObject poppedScreen = _panelStack.Pop();
            _animator.SetFloat("direction", -1f);
            _animator.Play(animationName);

            var animatorState = _animator.GetCurrentAnimatorStateInfo(0);

            yield return UnityUtils.WaitFor(animatorState.length * animatorState.normalizedTime);
            poppedScreen.SetActive(false);

            yield return UnityUtils.WaitFor(10);
            if (_panelStack.Count > 0)
                _panelStack.Peek().SetActive(true);
        }
    }
    internal class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner instance;

        public static CoroutineRunner Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject gameObject = new GameObject("CoroutineRunner");
                    instance = gameObject.AddComponent<CoroutineRunner>();
                    DontDestroyOnLoad(gameObject);
                }
                return instance;
            }
        }
    }
}
