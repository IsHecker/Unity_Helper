using System;
using System.Collections;
using System.Collections.Generic;
using Unity_Helper.UnityHelper.Utilities;
using UnityEngine;
using UnityHelper.Utilities;

namespace UnityHelper
{
    public class UINavigator
    {
        private readonly Animator _animator;
        private readonly Dictionary<string, GameObject> _navigations;
        private readonly Stack<GameObject> _panelStack;

        public static Dictionary<string, object> Arguments { get; private set; }

        public UINavigator(Animator animator, Dictionary<string, GameObject> navigations)
        {
            _animator = animator;
            _navigations = navigations ?? throw new ArgumentNullException(nameof(navigations), "Navigation dictionary cannot be null.");
            _panelStack = new Stack<GameObject>();
        }

        public void PushNamed(string route, string animationName = "", bool useAnimation = true, Action uiSetup = null, Dictionary<string, object> args = null)
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

            _animator.SetFloat("direction", 1f);

            if (useAnimation)
                _animator.Play(string.IsNullOrEmpty(animationName) ? screen.name : animationName);

            uiSetup?.Invoke();

            var clipLength = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            UnityUtils.Invoke(() => _animator.SetFloat("direction", 0f), clipLength);
            Arguments = args;
        }

        public void Pop(string animationName = "", bool useAnimation = true, Action uiSetup = null, bool popAnimation = true, Dictionary<string, object> args = null)
        {
            if (_panelStack.Count == 0)
            {
                throw new InvalidOperationException("No routes to pop.");
            }

            CoroutineRunner.Instance.StartCoroutine(PopHandle(animationName, useAnimation, popAnimation, uiSetup));
            Arguments = args;
        }

        private IEnumerator PopHandle(string animationName = "", bool useAnimation = true, bool popAnimation = true, Action uiSetup = null)
        {
            GameObject poppedScreen = _panelStack.Pop();

            if (useAnimation)
            {
                _animator.SetFloat("direction", -1f);
                _animator.Play(string.IsNullOrEmpty(animationName) ? poppedScreen.name : animationName);
            }

            uiSetup?.Invoke();

            var clipLength = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

            yield return CoroutineUtils.WaitFor(clipLength);

            _animator.SetFloat("direction", 0f);

            poppedScreen.SetActive(false);

            if (_panelStack.Count > 0)
            {
                _panelStack.Peek().SetActive(true);
                if (popAnimation)
                {
                    _animator.SetFloat("direction", 1f);
                    _animator.Play(_panelStack.Peek().name);

                    clipLength = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

                    yield return CoroutineUtils.WaitFor(clipLength);

                    _animator.SetFloat("direction", 0f);
                }
            }
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
